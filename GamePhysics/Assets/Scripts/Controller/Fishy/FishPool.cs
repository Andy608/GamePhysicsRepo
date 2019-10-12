using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPool : ManagerBase<FishPool>
{
    [SerializeField] private int maxFish = 50;
    public int fishCount = 0;

    public void SetMax(int max)
    {
        maxFish = max;
    }

    public Fishy CanSpawnFish(Fishy prefab, Vector2 pos)
    {
        if (fishCount < maxFish)
        {
            Fishy f = Instantiate(prefab, pos, Quaternion.identity);
            fishCount++;
            return f;
        }

        return null;
    }

    public void DestroyFish(Fishy fish)
    {
        fishCount--;
		Destroy(fish.gameObject);
    }
}
