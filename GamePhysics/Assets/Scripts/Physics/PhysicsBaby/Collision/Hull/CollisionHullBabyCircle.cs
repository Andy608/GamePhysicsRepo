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
        return TestCollisionCircleVsCircle(this, other, ref c);
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return TestCollisionAABBVsCircle(other, this, ref c);
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
		//same as above but first...
		//multiply circle center by box inverse matrix 

		// 1. get circle center
		Vector4 circleCenter = new Vector4(transform.position.x, transform.position.y, transform.position.y, 1.0f);

		// 2. get box x bounds 
		float xMaxBound = other.transform.localScale.x * 0.5f;
		float xMinBound = -other.transform.localScale.x * 0.5f;
		// 3. get box y bounds
		float yMaxBound = other.transform.localScale.y * 0.5f;
		float yMinBound = -other.transform.localScale.y * 0.5f;

		// 3. get box y bounds
		float zMaxBound = other.transform.localScale.z * 0.5f;
		float zMinBound = -other.transform.localScale.z * 0.5f;


		// 4. multiply circle center world position by box world to local matrix
		circleCenter = other.transform.InverseTransformPoint(circleCenter);

		// 5. clamp circle center on box x bound
		float circleOnX = circleCenter.x;
		// 6. clamp circle center on box y bound
		float circleOnY = circleCenter.y;
		// 6. clamp circle center on box y bound
		float circleOnz = circleCenter.z;

		//ballBoxLocal.position = circleCenter;
		//boxLocal.position = other.transform.InverseTransformPoint(other.transform.position);

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

		// 7. use clamped point as closest point of box
		Vector3 closestPoint = new Vector3(circleOnX, circleOnY, circleOnz);

		Vector3 distance = closestPoint - new Vector3(circleCenter.x, circleCenter.y, circleCenter.z);
		float distSqr = Vector3.Dot(distance, distance);

		// 8. check if closest point of box is within the circle
		if (distSqr < radius * radius)
		{
			Debug.Log("There's a shape in mah boot!");
			//Todo Add the contact data to the contact list.
			return true;
		}

		// 9. do test (if in circle, true, else false)

		return false;
    }
}
