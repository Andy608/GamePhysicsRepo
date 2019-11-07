using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyOBB : CollisionHullBaby
{
    protected CollisionHullBabyOBB() : base(CollisionHullBabyType.OBB) {}

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
