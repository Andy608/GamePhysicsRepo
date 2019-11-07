using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyCircle : CollisionHullBaby
{
    protected CollisionHullBabyCircle() : base(CollisionHullBabyType.Circle) {}

    public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
        return false;
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return false;
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
        return false;
    }
}
