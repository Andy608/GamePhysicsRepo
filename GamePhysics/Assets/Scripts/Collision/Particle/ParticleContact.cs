using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleContact
{
    //Holds the particles that are involved in this isolated contact.
    //If we are dealing with a contact between an object and scenery,
    //the second partice in this array will be null.
    private Particle2D[] particles = new Particle2D[2];

    //The restitution value of the contact.
    private float restitution;

    //The contact normal, from the first objects perspective, in world coordinates.
    private Vector2 contactNormal;

    //Holds the depth of penetration
    //Negative = no interpenetration, Positive = interpenetration
    //Our goal is the get this the 0 if it is above 0.
    //The depth of penetration in the direction of the contact normal.
    public float Penetration { private set; get; }

    //Holds the amount each particle is moved during interpenetration.
    private Vector2[] particleMovement = new Vector2[2];

    public ParticleContact(Particle2D a, Particle2D b, float r, Vector2 n, float p)
    {
        particles[0] = a;
        particles[1] = b;
        restitution = r;
        contactNormal = n;
        Penetration = p;
    }

    //Resolves the contact for both velocity and interpenetration.
    public void Resolve(float deltaTime)
    {
        ResolveVelocity(deltaTime);
        ResolveInterpenetration(deltaTime);
    }

    //Calcuates the separating velocity at this contact.
    public float CalculateSeparatingVelocity()
    {
        Vector2 relativeVelocity = particles[0].Velocity;

        //If we have another particle
        if (particles[1])
        {
            relativeVelocity -= particles[1].Velocity;
        }

        return Vector2.Dot(relativeVelocity, contactNormal);
    }

    //Handles the impulse calculations for this collision.
    private void ResolveVelocity(float deltaTime)
    {
        //Find the velocity in the direction of the contact.
        float separatingVelocity = CalculateSeparatingVelocity();

        //Check if it needs to be resolved.
        if (separatingVelocity > 0)
        {
            //The contact is either separating or stationary
            //No impulse needed.
            return;
        }

        //Calculate the new separating velocity.
        float newSeparatingVelocity = -separatingVelocity * restitution;

        //Check the velocity buildup due to acceleration only.
        Vector2 accelerationCausedVelocity = particles[0].Acceleration;

        if (particles[1])
        {
            accelerationCausedVelocity -= particles[1].Acceleration;
        }

        float accelerationCausedSeperationVelocity = Vector2.Dot(accelerationCausedVelocity, contactNormal) * deltaTime;

        //If we've got a closing velocity due to acceleration buildup,
        //remove it from the new separating velocity.
        if (accelerationCausedSeperationVelocity < 0.0f)
        {
            newSeparatingVelocity += restitution * accelerationCausedSeperationVelocity;

            //Make sure we haven't removed more than was there to remove.
            if (newSeparatingVelocity < 0.0f)
            {
                newSeparatingVelocity = 0.0f;
            }
        }

        float deltaVelocity = newSeparatingVelocity - separatingVelocity;

        //Apply the change in velocity to each object in proportion to
        //their inverse mass. (Higher mass gets less change in velocity)
        float totalInverseMass = particles[0].MassInv;

        if (particles[1])
        {
            totalInverseMass += particles[1].MassInv;
        }

        //If all particles have infinite mass, then impules have no effect.
        if (totalInverseMass <= 0.0f)
        {
            return;
        }

        //Calculate the impule to apply.
        float impulse = deltaVelocity / totalInverseMass;

        //Find the amount of impulse per unit of inverse mass.
        Vector2 impulsePerIMass = impulse * contactNormal;

        //Apply impulses in the direction of the contact normal,
        //and are proportional to the inverse mass.
        particles[0].Velocity += (impulsePerIMass * particles[0].MassInv);

        if (particles[1])
        {
            particles[1].Velocity += (impulsePerIMass * -particles[1].MassInv);
        }
    }

    private void ResolveInterpenetration(float deltaTime)
    {
        //If we don't have any penetration, skip this step.
        if (Penetration <= 0)
        {
            return;
        }

        //The movement of each object is based on their inverse mass.
        float totalInverseMass = particles[0].MassInv;

        if (particles[1])
        {
            totalInverseMass += particles[1].MassInv;
        }

        //If all particles have infinite mass, then we do nothing.
        if (totalInverseMass <= 0)
        {
            return;
        }

        //Find the amount of penetration resoluion per unit of inverse mass.
        Vector2 movePerIMass = (Penetration / totalInverseMass) * contactNormal;

        //Calculate the movement amounts.
        particleMovement[0] = movePerIMass * particles[0].MassInv;

        if (particles[1])
        {
            particleMovement[1] = movePerIMass * -particles[1].MassInv;
        }
        else
        {
            particleMovement[1] = Vector2.zero;
        }

        //Apply the penetration resolution.
        particles[0].Position += particleMovement[0];

        if (particles[1])
        {
            particles[1].Position += particleMovement[1];
        }
    }
}
