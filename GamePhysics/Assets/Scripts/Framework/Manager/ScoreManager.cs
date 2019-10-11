using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : ManagerBase<ScoreManager>
{
	private int score = 0;
	[SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void AddScore(int amountAdded)
	{
		score += amountAdded;
		text.text = score.ToString();
	}

	public int GetScore()
	{
		return score;
	}
}
