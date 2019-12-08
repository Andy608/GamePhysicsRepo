using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyAABB : CollisionHullBaby
{
    private Vector3 bounds;

    protected override void Awake()
    {
        Type = CollisionHullBabyType.AABB;
        bounds = transform.localScale;
        base.Awake();
    }

    public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
        return TestCollisionAABBVsCircle(this, other, ref c);
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return TestCollisionAABBVsAABB(this, other, ref c);
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
        return TestCollisionAABBVsOBB(this, other, ref c);
    }
}
