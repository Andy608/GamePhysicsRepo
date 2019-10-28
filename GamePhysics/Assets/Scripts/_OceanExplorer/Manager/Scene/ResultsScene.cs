using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsScene : SceneBase<ResultsScene>
{
    [SerializeField] private TextMeshProUGUI score = null;
	[SerializeField] private TextMeshProUGUI points = null;

	private void OnEnable()
    {
        score.text = PersistentData.Instance.GetScore().ToString("n2") + " meters";
		points.text = PersistentData.Instance.GetPoints().ToString();
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
