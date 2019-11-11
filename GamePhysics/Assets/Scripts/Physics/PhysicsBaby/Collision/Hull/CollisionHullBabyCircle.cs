using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyCircle : CollisionHullBaby
{
    protected CollisionHullBabyCircle() : base(CollisionHullBabyType.Circle) {}

    public float radius = 5.0f;
    public float restitution = 0.0f;

    public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
        //collides if distance between centers is <= sum of radii
        //or if you like opptimizations
        //if (distance between center)^2 <= (sum of radii)^2
        // 1. get two centers
        Vector3 circleCenter1 = transform.position;
        Vector3 circleCenter2 = other.transform.position;
        // 2. take diference
        Vector3 distance = circleCenter2 - circleCenter1;
        // 3. distance squared = dot(diff, diff)
        float distSqr = Vector3.Dot(distance, distance);
        // 4. add the radii
        float radiiSum = other.radius + radius;
        // 5. square sum
        radiiSum *= radiiSum;
        // 6. do test. johnny test. it passes if distSq <= sumSq
        if (distSqr < radiiSum)
        {
            RigidBaby aParticle = GetComponent<RigidBaby>();
            RigidBaby bParticle = other.GetComponent<RigidBaby>();

            Vector2 normal = (aParticle.GetPosition() - bParticle.GetPosition()).normalized;
            float penetration = other.radius + radius - distance.magnitude;

            RigidBabyContact contact = new RigidBabyContact(
                aParticle, bParticle, restitution, normal, penetration);
            c.Add(contact);
			Debug.Log("It's touching ME!");
            return true;
        }

        return false;
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
		// find closest point to circle on box
		// done by clamping center of circle to be within box dimensions
		// if closest point is within circle, pass! (do circle vs point test)

		// 1. get circle center
		Vector3 circleCenter = transform.position;
		// 2. get box x bounds 
		float xMaxBound = other.transform.position.x + other.transform.localScale.x * 0.5f;
		float xMinBound = other.transform.position.x - other.transform.localScale.x * 0.5f;
		// 3. get box y bounds
		float yMaxBound = other.transform.position.y + other.transform.localScale.y * 0.5f;
		float yMinBound = other.transform.position.y - other.transform.localScale.y * 0.5f;
		// 3. get box z bounds
		float zMaxBound = other.transform.position.z + other.transform.localScale.z * 0.5f;
		float zMinBound = other.transform.position.z - other.transform.localScale.z * 0.5f;

		// 4. clamp circle center on box x bound
		// 5. clamp circle center on box y bound
		float circleOnX = circleCenter.x;
		float circleOnY = circleCenter.y;
		float circleOnz = circleCenter.z;

		if (circleCenter.x > xMaxBound)
		{
			circleOnX = xMaxBound;
		}
		else if (circleCenter.x < xMinBound)
		{
			circleOnX = xMinBound;
		}

		if (circleCenter.y > yMaxBound)
		{
			circleOnY = yMaxBound;
		}
		else if (circleCenter.y < yMinBound)
		{
			circleOnY = yMinBound;
		}

		if (circleCenter.z > zMaxBound)
		{
			circleOnz = zMaxBound;
		}
		else if (circleCenter.z < zMinBound)
		{
			circleOnz = zMinBound;
		}

		// 6. use clamped point as closest point of box
		Vector3 closestPoint = new Vector3(circleOnX, circleOnY, circleOnz);
		Vector3 distance = closestPoint - circleCenter;
		float distSqr = Vector3.Dot(distance, distance);

		// 7. check if closest point of box is within the circle
		// 8. do test (if in circle, true, else false)
		if (distSqr < radius * radius)
		{
			Debug.Log("TOUCHE MOI");
			//Todo Add the contact data to the contact list.
			//Particle2D aParticle = GetComponent<Particle2D>();
			//Particle2D bParticle = other.GetComponent<Particle2D>();

			//Vector2 normal = (aParticle.Position - bParticle.Position).normalized;

			//float penetration = (aParticle.Position - bParticle.Position).magnitude - distance.magnitude;

			//ParticleContact contact = new ParticleContact(
			//    aParticle, bParticle, 0.0f, normal, penetration);
			//c.Add(contact);
			return true;
		}

		return false;
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
        return false;
    }
}
