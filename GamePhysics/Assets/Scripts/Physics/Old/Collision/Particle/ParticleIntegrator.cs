using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleIntegrator : ManagerBase<ParticleIntegrator>
{
    private List<Particle2D> particles = new List<Particle2D>();

    public void RegisterParticle(Particle2D particle)
    {
        particles.Add(particle);
    }

    public void UnRegisterParticle(Particle2D particle)
    {
        particles.Remove(particle);
    }

    private void FixedUpdate()
    {
        foreach (Particle2D p in particles)
        {
            p.Integrate(Time.fixedDeltaTime);
        }
    }
}
