using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBabyContact
{
    //Holds the rigidbabys that are involved in this isolated contact.
    //If we are dealing with a contact between an object and scenery,
    //the second rigidbaby in this array will be null.
    public RigidBaby[] Particles = new RigidBaby[2];

    //The restitution value of the contact.
    private float restitution;

    //The contact normal, from the first objects perspective, in world coordinates.
    public Vector3 ContactNormal;

    //Holds the depth of penetration
    //Negative = no interpenetration, Positive = interpenetration
    //Our goal is the get this the 0 if it is above 0.
    //The depth of penetration in the direction of the contact normal.
    public float Penetration;

    //Holds the amount each particle is moved during interpenetration.
    public Vector3[] ParticleMovement = new Vector3[2];

    public RigidBabyContact(RigidBaby a, RigidBaby b, float r, Vector3 n, float p)
    {
        Particles[0] = a;
        Particles[1] = b;
        restitution = r;
        ContactNormal = n;
        Penetration = p;
    }

    /// <summary>
    /// Resolves the contact for both velocity and interpenetration.
    /// </summary>
    /// <param name="deltaTime"> Delta Time. </param>
    public void Resolve(float deltaTime)
    {
        ResolveVelocity(deltaTime);
        ResolveInterpenetration(deltaTime);
    }

    /// <summary>
    /// Calcuates the separating velocity at this contact.
    /// </summary>
    /// <returns> The separating velocity of the two rigidbabys. </returns>
    public float CalculateSeparatingVelocity()
    {
        Vector3 relativeVelocity = Particles[0].GetVelocity();

        //If we have another particle
        if (Particles[1])
        {
            relativeVelocity -= Particles[1].GetVelocity();
        }

        return Vector3.Dot(relativeVelocity, ContactNormal);
    }

    /// <summary>
    /// Handles the impulse calculations for this collision.
    /// </summary>
    /// <param name="deltaTime"> Delta Time. </param>
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
        Vector3 accelerationCausedVelocity = Particles[0].GetAcceleration();

        if (Particles[1])
        {
            accelerationCausedVelocity -= Particles[1].GetAcceleration();
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
        float totalInverseMass = Particles[0].MassInverse;

        if (Particles[1])
        {
            totalInverseMass += Particles[1].MassInverse;
        }

        //If all particles have infinite mass, then impules have no effect.
        if (totalInverseMass <= 0.0f)
        {
            return;
        }

        //Calculate the impulse to apply.
        float impulse = deltaVelocity / totalInverseMass;

        //Find the amount of impulse per unit of inverse mass.
        Vector3 impulsePerIMass = ContactNormal * impulse;

        //Apply impulses in the direction of the contact normal,
        //and are proportional to the inverse mass.
        Particles[0].SetVelocity(Particles[0].GetVelocity() + impulsePerIMass * Particles[0].MassInverse);

        if (Particles[1])
        {
            Vector3 MomentArm = Particles[0].GetPosition() - Particles[1].GetPosition();

            Vector3 prevRot0 = Particles[0].RotVelocity;
            Vector3 prevRot1 = Particles[1].RotVelocity;

            Particles[0].AddAngularImpulse(MomentArm, Particles[0].Velocity);
            Particles[0].AddAngularImpulse(-MomentArm, Particles[1].Velocity);

            Particles[1].AddAngularImpulse(MomentArm, Particles[1].Velocity);
            Particles[1].AddAngularImpulse(-MomentArm, Particles[0].Velocity);

            Particles[0].AddRotVelocity(prevRot1);
            Particles[1].AddRotVelocity(prevRot0);

            //Particles[0].AddAngularImpulse(MomentArm, prevRot0);
            //Particles[0].AddAngularImpulse(-MomentArm, prevRot1);

            //Particles[1].AddAngularImpulse(MomentArm, prevRot1);
            //Particles[1].AddAngularImpulse(-MomentArm, prevRot0);

            Particles[1].SetVelocity(Particles[1].GetVelocity() + impulsePerIMass * -Particles[1].MassInverse);
        }

        EventAnnouncer.OnCollisionOccurredBaby?.Invoke(Particles[0], Particles[1]);
        EventAnnouncer.OnPlaySound?.Invoke(EnumSound.COLLISION);
    }

    /// <summary>
    /// Moves a rigidbaby's intersecting collider outside of the other rigidbaby's collider.
    /// </summary>
    /// <param name="deltaTime"> Delta Time. </param>
    private void ResolveInterpenetration(float deltaTime)
    {
        //If we don't have any penetration, skip this step.
        if (Penetration <= 0)
        {
            return;
        }

        //The movement of each object is based on their inverse mass.
        float totalInverseMass = Particles[0].MassInverse;

        if (Particles[1])
        {
            totalInverseMass += Particles[1].MassInverse;
        }

        //If all particles have infinite mass, then we do nothing.
        if (totalInverseMass <= 0)
        {
            return;
        }

        //Find the amount of penetration resoluion per unit of inverse mass.
        Vector3 movePerIMass = ContactNormal * (Penetration / totalInverseMass);

        //Calculate the movement amounts.
        ParticleMovement[0] = movePerIMass * Particles[0].MassInverse;

        if (Particles[1])
        {
            ParticleMovement[1] = movePerIMass * -Particles[1].MassInverse;
        }
        else
        {
            ParticleMovement[1] = Vector3.zero;
        }

		//Apply the penetration resolution.
		Particles[0].MovePosition(ParticleMovement[0]);//(Particles[0].GetPosition() + ParticleMovement[0]);

        if (Particles[1])
        {
			Particles[1].MovePosition(ParticleMovement[1]);//(Particles[1].GetPosition() + ParticleMovement[1]);
        }
    }
}
