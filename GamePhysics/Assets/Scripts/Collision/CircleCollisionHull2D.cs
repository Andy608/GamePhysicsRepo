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

		return false;
	}

	public override bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other)
	{
		//same as above but first...
		//multiply circle center by box inverse matrix 

		return false;
	}
}
