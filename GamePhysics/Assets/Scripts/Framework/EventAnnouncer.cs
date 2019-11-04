using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnnouncer : ManagerBase<EventAnnouncer>
{
    #region Scene Change Events

    //Request the scene to change. Returns true if successful.
    public delegate bool RequestSceneChange(EnumScene targetScene, TransitionEffect transition = null);
    public static RequestSceneChange OnRequestSceneChange;

    //Locks the scene so it can't be changed while it is transitioning.
    public delegate void StartSceneChange(EnumScene currentScene, EnumScene targetScene);
    public static StartSceneChange OnStartSceneChange;

    //Continuously sends out this event while the scene is transitioning.
    public delegate void SceneChanging(EnumScene currentScene, EnumScene targetScene);
    public static SceneChanging OnSceneChanging;

    //Unlocks the scene so it can't be changed while it is transitioning.
    public delegate void EndSceneChange(EnumScene newScene);
    public static EndSceneChange OnEndSceneChange;

    public delegate void SceneChanged(EnumScene newScene);
    public static SceneChanged OnSceneChanged;

    #endregion

    #region Collision Events

    //Announce that a collision happened
    public delegate void CollisionOccurred(Particle2D a, Particle2D b);
    public static CollisionOccurred OnCollisionOccurred;

    public delegate void CollisionOccurredBaby(RigidBaby a, RigidBaby b);
    public static CollisionOccurredBaby OnCollisionOccurredBaby;

    #endregion

    #region Game Events

    //Announce that the player got damaged
    public delegate void PlayerDamaged(float damage, float health);
    public static PlayerDamaged OnPlayerDamaged;

    //Announce that the player's health reached 0
    public delegate void PlayerDied(float score);
    public static PlayerDied OnPlayerDied;

    //Announce that the player's health reached 0
    public delegate float RequestPlayerInvincible();
    public static RequestPlayerInvincible OnRequestPlayerInvincible;

    #endregion

    #region Audio Events

    public delegate void PlayMusicRequested();
    public static PlayMusicRequested OnPlayMusicRequested;

    public delegate void StopMusicRequested();
    public static StopMusicRequested OnStopMusicRequested;

    public delegate void MusicVolumeChanged(float volume);
    public static MusicVolumeChanged OnMusicVolumeChanged;

    public delegate void SoundVolumeChanged(float volume);
    public static SoundVolumeChanged OnSoundVolumeChanged;

    public delegate AudioSource PlaySound(EnumSound soundID);
    public static PlaySound OnPlaySound;

    #endregion
}
