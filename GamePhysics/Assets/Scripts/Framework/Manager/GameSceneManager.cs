using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : ManagerBase<GameSceneManager>
{
    [SerializeField] private CanvasGroup transitionImage = null;

    public EnumScene CurrentScene { private set; get; }
    public EnumScene TargetScene { private set; get; }

    private SceneBundle sceneBundle = null;

    private void OnEnable()
    {
        EventAnnouncer.OnRequestSceneChange += RequestSceneChange;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnRequestSceneChange -= RequestSceneChange;
    }

    private bool RequestSceneChange(EnumScene targetScene, TransitionEffect transition = null)
    {
        if (!sceneBundle.IsTransitioning)
        {
            TargetScene = targetScene;

            if (targetScene != CurrentScene && (transition == null || transition.TransitionTime == 0.0f))
            {
                SetScene(targetScene);
                return true;
            }

            sceneBundle.Reset();
            sceneBundle.SetTargetScene(targetScene);
            sceneBundle.SetTransitionEffect(transition);

            if (sceneBundle.StartTransition())
            {
                StartCoroutine(ChangeScene());
                return true;
            }
        }

        return false;
    }

    private IEnumerator ChangeScene()
    {
        EventAnnouncer.OnStartSceneChange?.Invoke(CurrentScene, sceneBundle.TargetScene);

        transitionImage.gameObject.SetActive(true);
        transitionImage.GetComponent<Image>().color = sceneBundle.Effect.TransitionColor;

        float fadeOut = 0.0f, fadeIn = 0.0f;

        while (!sceneBundle.IsTransitionDone)
        {
            EventAnnouncer.OnSceneChanging?.Invoke(CurrentScene, sceneBundle.TargetScene);
            sceneBundle.Update(Time.deltaTime);

            if (fadeOut <= sceneBundle.Effect.TransitionTime)
            {
                Fade(fadeOut / sceneBundle.Effect.TransitionTime);
                fadeOut += Time.deltaTime;
            }
            else if (fadeIn == 0.0f)
            {
                SetScene(sceneBundle.TargetScene);
                fadeIn += Time.deltaTime;
            }
            else
            {
                Fade(1.0f - (fadeIn / sceneBundle.Effect.TransitionTime));
                fadeIn += Time.deltaTime;
            }

            yield return null;
        }

        sceneBundle.Reset();
        transitionImage.gameObject.SetActive(false);
        EventAnnouncer.OnEndSceneChange?.Invoke(CurrentScene);
    }

    private void Fade(float alpha)
    {
        transitionImage.alpha = alpha;
    }

    private void Awake()
    {
        transitionImage.gameObject.SetActive(false);

        int startupSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //The start up scene isn't a supported scene for this game's scene manager!
        if (startupSceneIndex >= (int)EnumScene.SIZE)
        {
            startupSceneIndex = 0;
        }

        CurrentScene = (EnumScene)startupSceneIndex;
        TargetScene = CurrentScene;

        sceneBundle = new SceneBundle(TargetScene, new TransitionEffect(1.0f, Color.black));
    }

    private void SetScene(EnumScene target)
    {
        SceneManager.LoadScene((int)target);
        CurrentScene = TargetScene;
        EventAnnouncer.OnSceneChanged?.Invoke(CurrentScene);
    }

    protected class SceneBundle
    {
        public TransitionEffect Effect { private set; get; }
        public EnumScene TargetScene { private set; get; }
        public bool IsTransitioning { private set; get; } = false;
        public bool IsTransitionDone { private set; get; } = false;

        private float timeCounter = 0.0f;

        public SceneBundle(EnumScene targetScene, TransitionEffect transition)
        {
            TargetScene = targetScene;
            Effect = transition;
            Reset();
        }

        public bool Reset()
        {
            if (!IsTransitioning)
            {
                IsTransitionDone = false;
                IsTransitioning = false;
                timeCounter = 0.0f;
                return true;
            }

            return false;
        }

        public bool SetTargetScene(EnumScene targetScene)
        {
            if (!IsTransitioning)
            {
                TargetScene = targetScene;
                return true;
            }

            return false;
        }

        public bool SetTransitionEffect(TransitionEffect transition)
        {
            if (!IsTransitioning)
            {
                Effect = transition;
                return true;
            }

            return false;
        }

        public bool StartTransition()
        {
            if (!IsTransitioning && !IsTransitionDone)
            {
                IsTransitionDone = false;
                IsTransitioning = true;
                return true;
            }

            return false;
        }

        public void Update(float deltaTime)
        {
            if (IsTransitioning && timeCounter >= (Effect.TransitionTime * 2.0f))
            {
                IsTransitioning = false;
                IsTransitionDone = true;
            }
            else
            {
                timeCounter += deltaTime;
            }
        }
    }
}

public class TransitionEffect
{
    public float TransitionTime { private set; get; } = 1.0f;
    public Color TransitionColor { private set; get; } = Color.black;

    public TransitionEffect(float time, Color color)
    {
        TransitionTime = time;
        TransitionColor = color;
    }
}

public enum EnumScene
{
    TITLE,
    GAME,
    RESULTS,
    SIZE
}
