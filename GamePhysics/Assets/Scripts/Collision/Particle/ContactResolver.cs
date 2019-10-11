using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactResolver
{
    //Holds the number of iterations allowed.
    private int iterations;

    //Performance tracking to see how many iterations used.
    private int iterationsUsed;

    public ContactResolver(int iter)
    {
        SetIterations(iter);
    }

    public void SetIterations(int iter)
    {
        iterations = iter;
    }

    //Chapter 16 optimizes this.
    public void ResolveContacts(ParticleContact[] contacts, float deltaTime)
    {
        int i;
        iterationsUsed = 0;

        while (iterationsUsed < iterations)
        {
            //Find the contact with the largest closing velocity.
            float max = float.MaxValue;
            int maxIndex = contacts.Length;
            for (i = 0; i < maxIndex; ++i)
            {
                float separationVelocity = contacts[i].CalculateSeparatingVelocity();
                if (separationVelocity < max && (separationVelocity < 0 || contacts[i].Penetration > 0.0f))
                {
                    max = separationVelocity;
                    maxIndex = i;
                }
            }

            //Do we have anything worth resolving?
            if (maxIndex == contacts.Length)
            {
                break;
            }

            //Resolve this contact.
            contacts[maxIndex].Resolve(deltaTime);
            ++iterationsUsed;
        }
    }
}
