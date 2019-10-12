﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : ManagerBase<PersistentData>
{
    [SerializeField] private static float score;
	[SerializeField] private static int points;

    private void OnEnable()
    {
        EventAnnouncer.OnPlayerDied += SaveScore;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnPlayerDied -= SaveScore;
    }

    private void SaveScore(float s)
    {
        Debug.Log("HELLO: " + s);
        score = s;
		points = ScoreManager.Instance.GetScore();
    }

    public float GetScore()
    {
        Debug.Log("SCORE: " + score);
        return score;
    }
	public float GetPoints()
	{
		Debug.Log("POINTS: " + points);
		return points;
	}

}
