using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleContact
{
    //Holds the particles that are involved in this isolated contact.
    //If we are dealing with a contact between an object and scenery,
    //the second partice in this array will be null.
    public Particle2D[] Particles = new Particle2D[2];

    //The restitution value of the contact.
    private float restitution;

    //The contact normal, from the first objects perspective, in world coordinates.
    public Vector2 ContactNormal;

    //Holds the depth of penetration
    //Negative = no interpenetration, Positive = interpenetration
    //Our goal is the get this the 0 if it is above 0.
    //The depth of penetration in the direction of the contact normal.
    public float Penetration;

    //Holds the amount each particle is moved during interpenetration.
    public Vector2[] ParticleMovement = new Vector2[2];

    public ParticleContact(Particle2D a, Particle2D b, float r, Vector2 n, float p)
    {
        Particles[0] = a;
        Particles[1] = b;
        restitution = r;
        ContactNormal = n;
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
        Vector2 relativeVelocity = Particles[0].Velocity;

        //If we have another particle
        if (Particles[1])
        {
            relativeVelocity -= Particles[1].Velocity;
        }

        return Vector2.Dot(relativeVelocity, ContactNormal);
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
        Vector2 accelerationCausedVelocity = Particles[0].Acceleration;

        if (Particles[1])
        {
            accelerationCausedVelocity -= Particles[1].Acceleration;
        }

        float accelerationCausedSeperationVelocity = Vector2.Dot(accelerationCausedVelocity, ContactNormal) * deltaTime;

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
        float totalInverseMass = Particles[0].MassInv;

        if (Particles[1])
        {
            totalInverseMass += Particles[1].MassInv;
        }

        //If all particles have infinite mass, then impules have no effect.
        if (totalInverseMass <= 0.0f)
        {
            return;
        }

        //Calculate the impulse to apply.
        float impulse = deltaVelocity / totalInverseMass;

        //Find the amount of impulse per unit of inverse mass.
        Vector2 impulsePerIMass = ContactNormal * impulse;

        //Apply impulses in the direction of the contact normal,
        //and are proportional to the inverse mass.
        Particles[0].Velocity = Particles[0].Velocity + impulsePerIMass * Particles[0].MassInv;

        if (Particles[1])
        {
            Particles[1].Velocity = Particles[1].Velocity + impulsePerIMass * -Particles[1].MassInv;
        }

        EventAnnouncer.OnCollisionOccurred?.Invoke(Particles[0], Particles[1]);
    }

    private void ResolveInterpenetration(float deltaTime)
    {
        //If we don't have any penetration, skip this step.
        if (Penetration <= 0)
        {
            return;
        }

        //The movement of each object is based on their inverse mass.
        float totalInverseMass = Particles[0].MassInv;

        if (Particles[1])
        {
            totalInverseMass += Particles[1].MassInv;
        }

        //If all particles have infinite mass, then we do nothing.
        if (totalInverseMass <= 0)
        {
            return;
        }

        //Find the amount of penetration resoluion per unit of inverse mass.
        Vector2 movePerIMass = ContactNormal * (Penetration / totalInverseMass);

        //Calculate the movement amounts.
        ParticleMovement[0] = movePerIMass * Particles[0].MassInv;

        if (Particles[1])
        {
            ParticleMovement[1] = movePerIMass * -Particles[1].MassInv;
        }
        else
        {
            ParticleMovement[1] = Vector2.zero;
        }

        //Apply the penetration resolution.
        Particles[0].Position = Particles[0].Position + ParticleMovement[0];

        if (Particles[1])
        {
            Particles[1].Position = Particles[1].Position + ParticleMovement[1];
        }
    }
}
