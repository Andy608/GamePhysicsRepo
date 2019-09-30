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

	public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other)
	{
		//pass if, for all axis, max extent of A is greater than min extent of B
		//

		// 1. get box A min extent
		// 2. get box A max extent
		// 3. get box B min extent
		// 4. get box B max extent
		// 5. check if max extent of A.x is greater than min extent B.x
		// 6. check if max extent of A.y is greater than min extent B.y
		// 7. check if max extent of B.x is greater than min extent A.x
		// 8. check if max extent of B.y is greater than min extent A.y
		// 9. only if all cases pass, collision is true

		return false;
	}

	public override bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other)
	{
		//same as above twice but first...
		//first, find max extent of OBB, do AABB vs this box
		// then, transform this box into OBBs space, find max extents, repeat

		// 1. Find OBB max extents on each Axis
		// 2. Perform AABB test with AABB bounds, and OBB found maxExtents
		// 3. Transform AABB box into OBB box's local space
		// 4. find max extents of AABB box in OBB local space
		// 5. perform AABB test again with AABB max extents and OOBBs bounds
		// 6. only if all cases pass, return true

		//DEPRECATED
		// 1. calculate box A normal (cos0, sin0) 0 = theta
		// 2. calculate box B normal (-sin0, cos0)
		// 3. calculate box A points
		// 4. calculate box B points
		// 5. project each box A point onto one of the box B normals (Apoint dot Bnormal^)Bnormal^
		// 6. project each box B point onto one of the box B normals (Bpoint dot Bnormal^)Bnormal^
		// 7. Do AABB with the points projected on the normal
		// 8. Repeat steps 5-7 for each normal. (4 total)
		// 9. only if all cases pass, collision is true

		return false;
	}
}
