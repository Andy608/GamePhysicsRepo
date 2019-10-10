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
}
