using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHullBabyOBB : CollisionHullBaby
{
    protected CollisionHullBabyOBB() : base(CollisionHullBabyType.OBB) {}

    public override bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c)
    {
        return TestCollisionCircleVsOBB(other, this, ref c);
    }

    public override bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c)
    {
        return TestCollisionAABBVsOBB(other, this, ref c);
    }

    public override bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c)
    {
        return TestCollisionOBBVsOBB(other, this, ref c); ;
    }
}
