using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishySpawner : MonoBehaviour
{
    [SerializeField] public List<Fishy> fishPrefabs = null;

    private float maxHeightSpawn, minHeightSpawn;
    [SerializeField] private bool swimRight = false;

    private void Start()
    {
        maxHeightSpawn = Camera.main.orthographicSize;
        minHeightSpawn = -maxHeightSpawn;
    }

    private void Update()
    {
        foreach (Fishy fish in fishPrefabs)
        {
            fish.AddTime(Time.deltaTime);

            if (fish.GetFishSecondCounter() >= fish.GetSpawnRate())
            {
                fish.SetTime(Random.Range(0, fish.GetSpawnRate()));
                SpawnFish(fish);
            }
        }
    }

    private void SpawnFish(Fishy fish)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - Random.Range(minHeightSpawn, maxHeightSpawn), transform.position.z);
        int spawnAmount = Random.Range(fish.GetMinFishSpawn(), fish.GetMinFishSpawn());

        for (int i = 0; i < spawnAmount; ++i)
        {
            pos.x += Random.Range(-1, 1);
            pos.y += Random.Range(0.0f, 5.0f);

            Fishy fishy = Instantiate(fish, pos, Quaternion.identity);
            Vector2 acceleration = Vector2.right * Random.Range(fishy.GetFishSpeed() * 0.5f, fishy.GetFishSpeed()) * 100.0f;
            fishy.SwimRight = swimRight;

            if (swimRight)
            {
                fishy.FlipX(true);
                fishy.FishyParticle.AddForce(acceleration);
            }
            else
            {
                fishy.FishyParticle.AddForce(-acceleration);
            }
        }
    }
}
