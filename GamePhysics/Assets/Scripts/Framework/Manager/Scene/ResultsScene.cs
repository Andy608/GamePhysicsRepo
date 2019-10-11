using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsScene : SceneBase<ResultsScene>
{
    [SerializeField] private TextMeshProUGUI score;

    private void OnEnable()
    {
        score.text = PersistentData.Instance.GetScore().ToString("n2") + " meters";
    }

    public void OnMainMenuClicked()
    {
        EventAnnouncer.OnRequestSceneChange(EnumScene.TITLE, new TransitionEffect(1.0f, Color.white));
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
