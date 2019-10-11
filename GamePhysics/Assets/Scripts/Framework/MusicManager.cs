using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumMusicState
{
    ON,
    OFF
}

public class MusicManager : ManagerBase<MusicManager>
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] titleSceneMusic;
    [SerializeField] private AudioClip[] gameSceneMusic;
    [SerializeField] private AudioClip[] resultSceneMusic;

    private EnumMusicState musicState = EnumMusicState.OFF;
    private int currentSongIndex = -1;

    public EnumMusicState CurrentMusicState { get { return musicState; } }

    public bool SetMusicState(EnumMusicState state)
    {
        if (musicState != state)
        {
            musicState = state;

            if (musicState == EnumMusicState.ON)
            {
                EventAnnouncer.OnPlayMusicRequested?.Invoke();
            }
            else
            {
                EventAnnouncer.OnStopMusicRequested?.Invoke();
            }

            return true;
        }

        return false;
    }

    private void Awake()
    {
        PlayMusic();
    }

    private void OnEnable()
    {
        EventAnnouncer.OnPlayMusicRequested += PlayMusic;
        EventAnnouncer.OnStopMusicRequested += StopMusic;
        EventAnnouncer.OnRequestSceneChange += ChangeMusic;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnPlayMusicRequested -= PlayMusic;
        EventAnnouncer.OnStopMusicRequested -= StopMusic;
        EventAnnouncer.OnRequestSceneChange -= ChangeMusic;
    }

    private bool ChangeMusic(EnumScene targetScene, TransitionEffect transition)
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(TransitionBetweenSongs(transition.TransitionTime));
            return true;
        }

        return false;
    }

    private IEnumerator TransitionBetweenSongs(float transitionTime)
    {
        StartCoroutine(VolumeManager.FadeOut(audioSource, transitionTime));

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        currentSongIndex = -1;

        AudioClip song = GetRandomSong();

        if (song && audioSource.isActiveAndEnabled)
        {
            audioSource.clip = song;
            audioSource.Play();

            musicState = EnumMusicState.ON;
        }
    }

    private void PlayMusic()
    {
        if (!audioSource.isPlaying && audioSource.isActiveAndEnabled)
        {
            AudioClip song = GetRandomSong();

            if (song)
            {
                audioSource.clip = song;
                audioSource.Play();

                musicState = EnumMusicState.ON;
            }
        }
    }

    private void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(VolumeManager.FadeOut(audioSource, 0.5f));

            musicState = EnumMusicState.OFF;
            currentSongIndex = -1;
        }
    }

    private void Update()
    {
        //The song ended, play a new song
        if (musicState == EnumMusicState.ON && !audioSource.isPlaying)
        {
            PlayMusic();
        }
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private AudioClip GetRandomSong()
    {
        AudioClip[] musicSelection;
        if (GameSceneManager.Instance.TargetScene == EnumScene.TITLE)
        {
            musicSelection = titleSceneMusic;
        }
        else if (GameSceneManager.Instance.TargetScene == EnumScene.GAME)
        {
            musicSelection = gameSceneMusic;
        }
        else
        {
            musicSelection = resultSceneMusic;
        }

        if (musicSelection.Length <= 0)
        {
            return null;
        }

        int index = Random.Range(0, musicSelection.Length);
        if (index == currentSongIndex)
        {
            index = ((currentSongIndex + 1) % musicSelection.Length);
        }

        currentSongIndex = index;
        return musicSelection[index];
    }
}
