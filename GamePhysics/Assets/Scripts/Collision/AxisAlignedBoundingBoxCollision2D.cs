using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxCollision2D : CollisionHull2D
{
	public AxisAlignedBoundingBoxCollision2D() : base(CollisionHullType2D.aabb) { }

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
		//SEE CIRCLE

		return false;
	}

	public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other)
	{
		//pass if, for all axis, max extent of A is greater than min extent of B
		//

		return false;
	}

	public override bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other)
	{
		//same as above twice but first...
		//first, find max extent of OBB, do AABB vs this box
		// then, transform this box into OBBs space, find max extents, repeat

		return false;
	}
}
