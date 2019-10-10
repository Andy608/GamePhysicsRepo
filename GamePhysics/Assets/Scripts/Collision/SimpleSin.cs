using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSin : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 pos;

    public bool moveX = false;
    public bool moveY = false;

    public float xRange = 0.5f;
    public float yRange = 0.5f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        pos = startPos;

        if (moveX)
        {
            pos.x = startPos.x + (0.5f * Mathf.Sin(Time.time));
            transform.localPosition = pos;
        }

        if (moveY)
        {
            pos.y = startPos.y + (0.5f * Mathf.Cos(Time.time));
            transform.localPosition = pos;
        }
    }
}
