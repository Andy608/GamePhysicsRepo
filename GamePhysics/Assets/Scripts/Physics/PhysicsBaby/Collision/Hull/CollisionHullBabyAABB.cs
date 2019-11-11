using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyAABB : CollisionHullBaby
{
    protected CollisionHullBabyAABB() : base(CollisionHullBabyType.AABB) { }

    private Vector3 bounds;

    private void Start()
    {
        bounds = transform.localScale;
    }

    public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
        return TestCollisionAABBVsCircle(this, other, ref c);
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return TestCollisionAABBvsAABB(this, other, ref c);
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
        return CollisionTestAABBVsOBB(this, other, ref c);
    }
}
