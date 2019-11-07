using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBaby))]
public abstract class CollisionHullBaby : MonoBehaviour
{
    public enum CollisionHullBabyType
    {
        Circle,
        AABB,
        OBB,
        Nispe
    }

    public CollisionHullBabyType Type { get; private set; }
    protected RigidBaby rigidbaby;

    protected CollisionHullBaby(CollisionHullBabyType type)
    {
        Type = type;
    }

    private void Awake()
    {
        rigidbaby = GetComponent<RigidBaby>();
    }

    private void OnEnable()
    {
        CollisionTesterBaby.Instance?.RegisterHull(this);
    }

    private void OnDisable()
    {
        CollisionTesterBaby.Instance?.UnRegisterHull(this);
    }

    public static bool TestCollision(CollisionHullBaby a, CollisionHullBaby b, ref List<RigidBabyContact> contacts)
    {
        bool isValidCollision = false;

        if (b.Type == CollisionHullBabyType.Circle)
        {
            isValidCollision = a.TestCollisionVsCircle((b as CollisionHullBabyCircle), ref contacts);
        }
        else if (b.Type == CollisionHullBabyType.AABB)
        {
            isValidCollision = a.TestCollisionVsAABB((b as CollisionHullBabyAABB), ref contacts);
        }
        else if (b.Type == CollisionHullBabyType.OBB)
        {
            isValidCollision = a.TestCollisionVsObject((b as CollisionHullBabyOBB), ref contacts);
        }
        else if (b.Type == CollisionHullBabyType.Nispe)
        {
            print("I N T E R P E N E T R A T I O N");
        }

        return isValidCollision;
    }

    public abstract bool TestCollisionVsCircle(CollisionHullBabyCircle other, ref List<RigidBabyContact> c);

    public abstract bool TestCollisionVsAABB(CollisionHullBabyAABB other, ref List<RigidBabyContact> c);

    public abstract bool TestCollisionVsObject(CollisionHullBabyOBB other, ref List<RigidBabyContact> c);
}
