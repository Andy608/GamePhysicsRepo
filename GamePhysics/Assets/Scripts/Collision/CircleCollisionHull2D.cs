using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
	public CircleCollisionHull2D() : base(CollisionHullType2D.circle) { }

	[Range(0.0f, 69.0f)] public float radius = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
	{
		//collides if distance between centers is <= sum of radii
		//or if you like opptimizations
		//if (distance between center)^2 <= (sum of radii)^2
		// 1. get two centers
		Vector2 circleCenter1;
		Vector2 circleCenter2;
		// 2. take diference
		// 3. distance squared = dot(diff, diff)
		// 4. add the radii
		// 5. square sum
		// 6. do test. johnny test. it passes if distSq <= sumSq

		return false;
	}

	public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other)
	{
		// find closest point to circle on box
		// done by clamping center of circle to be within box dimensions
		// if closest point is within circle, pass! (do circle vs point test)

		// 1. get circle center
		// 2. get box x bounds 
		// 3. get box y bounds
		// 4. clamp circle center on box x bound
		// 5. clamp circle center on box y bound
		// 6. use clamped point as closest point of box
		// 7. check if closest point of box is within the circle
		// 8. do test (if in circle, true, else false)

		return false;
	}

	public override bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other)
	{
		//same as above but first...
		//multiply circle center by box inverse matrix 


		// 1. get circle center
		// 2. get box x bounds 
		// 3. get box y bounds
		// 4. multiply circle center world position by box world to local matrix
		// 5. clamp circle center on box x bound
		// 6. clamp circle center on box y bound
		// 7. use clamped point as closest point of box
		// 8. check if closest point of box is within the circle
		// 9. do test (if in circle, true, else false)

		return false;
	}
}
