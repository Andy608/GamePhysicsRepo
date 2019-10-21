using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMover : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private Vector3 repeatOffset;
    [SerializeField] private int wallAmount;
    [SerializeField] private float movementVelocity;

    private GameObject[] walls;

    private void Start()
    {
        walls = new GameObject[wallAmount];

        SpawnWalls();
    }

    private void SpawnWalls()
    {
        int i;
        for (i = 0; i < wallAmount; ++i)
        {
            walls[i] = Instantiate(wallPrefab, transform.position - (i * repeatOffset), Quaternion.identity, transform);
        }
    }

    private void FixedUpdate()
    {
        float topBounds = Camera.main.orthographicSize + Camera.main.transform.position.y;
        float bottomBounds = -topBounds;

        foreach (GameObject wall in walls)
        {
            Vector3 pos = wall.transform.position;
            pos.y += movementVelocity * Time.fixedDeltaTime;
            wall.transform.position = pos;

            if (movementVelocity < 0.0f && pos.y + (0.5f * repeatOffset.y) < bottomBounds)
            {
                wall.transform.position += (wallAmount * repeatOffset);
            }
            else if (movementVelocity > 0.0f && pos.y - (0.5f * repeatOffset.y) > topBounds)
            {
                wall.transform.position -= (wallAmount * repeatOffset);
            }
        }
    }
}
