using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : SceneBase<TitleSceneManager>
{
    public void OnPlayClicked()
    {
        EventAnnouncer.OnRequestSceneChange(EnumScene.GAME, new TransitionEffect(1.0f, Color.white));
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
