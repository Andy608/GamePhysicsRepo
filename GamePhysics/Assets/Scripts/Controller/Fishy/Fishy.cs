using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class Fishy : MonoBehaviour
{
    public Particle2D FishyParticle { get; private set; }

    [SerializeField] private SpriteRenderer fishSprite = null;
    private Vector2 fishSpriteSize;

    public bool SwimRight = false;

    [SerializeField] private float spawnRate = 10.0f;
    [SerializeField] private float fishSpeed = 1.0f;
    [SerializeField] private float fishSecondCounter = 0.0f;
    [SerializeField] private int minGroupSpawn = 1;
    [SerializeField] private int maxGroupSpawn = 10;
	[SerializeField] private int pointsAwarded = 10;
	[SerializeField] private int health = 1;

	private void Awake()
    {
        FishyParticle = GetComponent<Particle2D>();
        fishSecondCounter = 0.0f;

        fishSpriteSize = new Vector2(fishSprite.sprite.texture.width, fishSprite.sprite.texture.height);
    }

    public void FlipX(bool flip)
    {
        Vector3 flippedScale = transform.localScale;
        flippedScale.x = (flip ? -1.0f : 1.0f);
        transform.localScale = flippedScale;
    }

    //Do something with collison?

    public void AddTime(float t)
    {
        fishSecondCounter += t;
    }

    public void SetTime(float t)
    {
        fishSecondCounter = t;
    }

    public float GetSpawnRate()
    {
        return spawnRate;
    }

    public float GetFishSpeed()
    {
        return fishSpeed;
    }

    public float GetFishSecondCounter()
    {
        return fishSecondCounter;
    }

    public int GetMinFishSpawn()
    {
        return minGroupSpawn;
    }

    public int GetMaxFishSpawn()
    {
        return maxGroupSpawn;
    }

    private void Update()
    {
        BoundsCheck();
    }

    private void BoundsCheck()
    {
        float topBounds = Camera.main.orthographicSize + Camera.main.transform.position.y;
        float bottomBounds = -topBounds;

        float rightBounds = Camera.main.orthographicSize * Screen.width / Screen.height + Camera.main.transform.position.x;
        float leftBounds = -rightBounds;


        topBounds -= fishSpriteSize.y;
        bottomBounds += fishSpriteSize.y;

        rightBounds -= fishSpriteSize.x;
        leftBounds += fishSpriteSize.x;

        if (FishyParticle.Position.x < leftBounds && !SwimRight)
        {
            DestroyFish();
        }
        else if (FishyParticle.Position.x > rightBounds && SwimRight)
        {
            DestroyFish();
        }

        if (FishyParticle.Position.y < bottomBounds)
        {
            DestroyFish();
        }
        else if (FishyParticle.Position.y > topBounds)
        {
            DestroyFish();
        }
    }

    private void DestroyFish()
    {
        Destroy(gameObject);
    }

	//call when hit by a bubble! again, that collision response tag thing
	public void DamageFish(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			ScoreManager.Instance.AddScore(pointsAwarded);
			DestroyFish();
		}
	}
}
