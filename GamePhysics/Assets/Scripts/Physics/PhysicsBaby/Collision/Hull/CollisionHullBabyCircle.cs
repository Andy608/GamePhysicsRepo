using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyCircle : CollisionHullBaby
{
    protected override void Awake()
    {
        Type = CollisionHullBabyType.Circle;
        base.Awake();
    }

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
        return TestCollisionCircleVsOBB(this, other, ref c);
    }
}
