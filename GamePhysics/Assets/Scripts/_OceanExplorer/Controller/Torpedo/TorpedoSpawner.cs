using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoSpawner : MonoBehaviour
{
    [SerializeField] private Torpedo torpedoPrefab = null;
    [SerializeField] private float torpedoSpeed = 10.0f;

    public void FireTorpedo(bool isSpriteFlipped)
    {
        Torpedo torpedo = Instantiate(torpedoPrefab, transform.position, Quaternion.identity);

        torpedo.TorpedoParticle.AddForce((isSpriteFlipped ? -transform.right : transform.right) * torpedoSpeed * 10000.0f);
    }
}
