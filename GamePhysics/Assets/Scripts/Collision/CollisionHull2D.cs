using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public abstract class CollisionHull2D : MonoBehaviour
{
    public enum CollisionHullType2D
    {
        circle,
        aabb,
        obb,
        penis
    }

    private Material material;

    protected Particle2D particle;

    private CollisionHullType2D type { get; }

    private List<CollisionHull2D> collidingWith = new List<CollisionHull2D>();

    public void SetColliding(bool colliding, CollisionHull2D otherObj)
    {
        if (colliding)
        {
            if (!collidingWith.Contains(otherObj))
            {
                collidingWith.Add(otherObj);
            }
        }
        else
        {
            collidingWith.Remove(otherObj);
        }

        if (IsColliding())
        {
            material.color = Color.red;
        }
        else
        {
            material.color = Color.green;
        }
    }

    public bool IsColliding()
    {
        return collidingWith.Count != 0;
    }

    protected CollisionHull2D(CollisionHullType2D _type)
    {
        type = _type;
    }

    private void Awake()
    {
        particle = GetComponent<Particle2D>();
        material = GetComponent<MeshRenderer>().material;
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b)
    {
        if (b.type == CollisionHullType2D.circle)
        {
            return a.TestCollisionVsCircle(b as CircleCollisionHull2D);
        }
        else if (b.type == CollisionHullType2D.aabb)
        {
            return a.TestCollisionVsAABB(b as AxisAlignedBoundingBoxCollision2D);
        }
        else if (b.type == CollisionHullType2D.obb)
        {
            return a.TestCollisionVsObject(b as ObjectBoundingBoxCollisionHull2D);
        }
        else if (b.type == CollisionHullType2D.penis)
        {
            print("I N T E R P E N E T R A T I O N");
        }

        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other);

    public abstract bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other);
}
