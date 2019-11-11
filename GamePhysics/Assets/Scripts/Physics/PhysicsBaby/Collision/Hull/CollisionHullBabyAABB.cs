using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyAABB : CollisionHullBaby
{
    protected CollisionHullBabyAABB() : base(CollisionHullBabyType.AABB) { }

	public Vector3 bounds;

	public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
		// 1. get circle center
		Vector3 circleCenter = other.transform.position;
		float xMaxBound = transform.position.x + transform.localScale.x * 0.5f;
		float xMinBound = transform.position.x - transform.localScale.x * 0.5f;

		// 3. get box y bounds
		float yMaxBound = transform.position.y + transform.localScale.y * 0.5f;
		float yMinBound = transform.position.y - transform.localScale.y * 0.5f;

		// 3. get box z bounds
		float zMaxBound = transform.position.z + transform.localScale.z * 0.5f;
		float zMinBound = transform.position.z - transform.localScale.z * 0.5f;


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
		if (distSqr < other.radius * other.radius)
		{
			Debug.Log("TOUCHE MOI");
			//Todo Add the contact data to the contact list.
			//Particle2D aParticle = GetComponent<Particle2D>();
			//Particle2D bParticle = other.GetComponent<Particle2D>();

			//Vector2 normal = Vector2.down;

			//float penetration = (aParticle.Position - bParticle.Position).magnitude - distance.magnitude;

			//ParticleContact contact = new ParticleContact(
			//    aParticle, bParticle, 0.0f, normal, penetration);

			//c.Add(contact);
			return true;
		}

		return false;
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return false;
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
        return false;
    }
}
