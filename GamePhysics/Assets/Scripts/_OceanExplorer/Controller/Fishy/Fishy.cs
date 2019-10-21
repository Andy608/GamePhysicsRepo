using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class Fishy : MonoBehaviour
{
    public Particle2D FishyParticle { get; private set; }
	private Vector2 oldVel = Vector2.zero;

    [SerializeField] private SpriteRenderer fishSprite = null;
    [SerializeField] private int damageValue = 5;
    [SerializeField] private bool speedDamageDebug = false;
    private Vector2 fishSpriteSize;

    public bool SwimRight = false;

    [SerializeField] private float spawnRate = 10.0f;
    [SerializeField] private float fishSpeed = 5.0f;
    [SerializeField] private float fishSecondCounter = 0.0f;
    [SerializeField] private int minGroupSpawn = 1;
    [SerializeField] private int maxGroupSpawn = 10;
	[SerializeField] public int pointsAwarded = 10;
	[SerializeField] private int health = 1;

	private void Awake()
    {
        FishyParticle = GetComponent<Particle2D>();
        fishSecondCounter = 0.0f;

        fishSpriteSize = new Vector2(fishSprite.sprite.texture.width / fishSprite.sprite.pixelsPerUnit * 0.5f, fishSprite.sprite.texture.height / fishSprite.sprite.pixelsPerUnit * 0.5f);

		oldVel = FishyParticle.Velocity;
	}

	public int GetDamageValue()
    {
        return speedDamageDebug ? 100 : damageValue;
    }

    public void FlipX(bool flip)
    {
        Vector3 flippedScale = transform.localScale;
        flippedScale.x = (flip ? -1.0f : 1.0f);
        transform.localScale = flippedScale;
    }

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

		if (FishyParticle.Velocity.x > 0 && oldVel.x < 0)
		{
			FlipX(true);
		}
		if (FishyParticle.Velocity.x < 0 && oldVel.x > 0)
		{
			FlipX(false);
		}
		oldVel = FishyParticle.Velocity;

	}

	private void BoundsCheck()
    {
        float topBounds = Camera.main.orthographicSize + Camera.main.transform.position.y;
        float bottomBounds = -topBounds;

        float rightBounds = Camera.main.orthographicSize * Screen.width / Screen.height + Camera.main.transform.position.x;
        float leftBounds = -rightBounds;


        topBounds += fishSpriteSize.y;
        bottomBounds -= fishSpriteSize.y;

        rightBounds += fishSpriteSize.x;
        leftBounds -= fishSpriteSize.x;

        if (FishyParticle.Position.x < leftBounds && !SwimRight)
        {
            FishPool.Instance.DestroyFish(this);
        }
        else if (FishyParticle.Position.x > rightBounds && SwimRight)
        {
            FishPool.Instance.DestroyFish(this);
        }

        if (FishyParticle.Position.y < bottomBounds)
        {
            FishPool.Instance.DestroyFish(this);
        }
        else if (FishyParticle.Position.y > topBounds)
        {
            FishPool.Instance.DestroyFish(this);
        }
    }
}
