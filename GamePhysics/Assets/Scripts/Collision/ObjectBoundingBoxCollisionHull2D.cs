using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollisionHull2D : CollisionHull2D
{
	public ObjectBoundingBoxCollisionHull2D() : base(CollisionHullType2D.obb) { }

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
		//see circle


		return false;
	}

	public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other)
	{
		//see AABB


		return false;
	}

	public override bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other)
	{
		//same as AABB-OBB part 2 twice
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
