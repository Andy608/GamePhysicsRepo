using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyOBB : CollisionHullBaby
{
    protected CollisionHullBabyOBB() : base(CollisionHullBabyType.OBB) {}

    public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
		//same as above but first...
		//multiply circle center by box inverse matrix 

		// 1. get circle center
		Vector4 circleCenter = new Vector4(other.transform.position.x, other.transform.position.y, other.transform.position.z, 1.0f);

		// 2. get box x bounds 
		float xMaxBound = transform.localScale.x * 0.5f;
		float xMinBound = -transform.localScale.x * 0.5f;
		// 3. get box y bounds

		float yMaxBound = transform.localScale.y * 0.5f;
		float yMinBound = -transform.localScale.y * 0.5f;

		// 3. get box y bounds
		float zMaxBound = transform.localScale.z * 0.5f;
		float zMinBound = -transform.localScale.z * 0.5f;


		// 4. multiply circle center world position by box world to local matrix
		circleCenter = transform.InverseTransformPoint(circleCenter);

		// 5. clamp circle center on box x bound
		float circleOnX = circleCenter.x;

		// 6. clamp circle center on box y bound
		float circleOnY = circleCenter.y;

		// 6. clamp circle center on box z bound
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

		// 7. use clamped point as closest point of box
		Vector3 closestPoint = new Vector3(circleOnX, circleOnY, circleOnz);

		Vector3 distance = closestPoint - new Vector3(circleCenter.x, circleCenter.y, circleCenter.z);
		float distSqr = Vector3.Dot(distance, distance);

		// 8. check if closest point of box is within the circle
		if (distSqr < other.radius * other.radius)
		{
			Debug.Log("There's a shape in mah boot!");
			return true;
		}

		return false;
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return CollisionTestAABBVsOBB(other, this, ref c);
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
		return false;
	}
}
