using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class Torpedo : MonoBehaviour
{
    public Particle2D TorpedoParticle;
    
    private void Awake()
    {
        TorpedoParticle = GetComponent<Particle2D>();
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

        if (TorpedoParticle.Position.x < leftBounds)
        {
            Destroy(gameObject);
        }
        else if (TorpedoParticle.Position.x > rightBounds)
        {
            Destroy(gameObject);
        }

        if (TorpedoParticle.Position.y < bottomBounds)
        {
            Destroy(gameObject);
        }
        else if (TorpedoParticle.Position.y > topBounds)
        {
            Destroy(gameObject);
        }
    }
}
