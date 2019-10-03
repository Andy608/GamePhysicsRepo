using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact
        {
            Vector2 point;
            Vector2 normal;
            float coefficientRestitution;

        }

        public CollisionHull2D a = null, b = null;
        public bool status = false;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public Vector2 closingVelocity = Vector2.zero;

        public Collision(CircleCollisionHull2D hullA, CircleCollisionHull2D hullB, Contact[] c, Vector2 closingVel)
        {
            a = hullA;
            b = hullB;
            contact = c;
            closingVelocity = closingVel;
        }
    }

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

    protected static float projOnAxis(Vector2 point, Vector2 axis)
    {
        return Vector2.Dot(point, axis);
    }

    protected static float projXAxis(Vector2 point)
    {
        return projOnAxis(point, Vector2.right);
    }

    protected static float projYAxis(Vector2 point)
    {
        return projOnAxis(point, Vector2.up);
    }

    protected static void CalculateAABB(Vector2 tr, Vector2 tl, Vector2 bl, Vector2 br, out float rb, out float tb, out float lb, out float bb)
    {
        Vector2 trProj = new Vector2(projXAxis(tr), projYAxis(tr));
        Vector2 tlProj = new Vector2(projXAxis(tl), projYAxis(tl));
        Vector2 blProj = new Vector2(projXAxis(bl), projYAxis(bl));
        Vector2 brProj = new Vector2(projXAxis(br), projYAxis(br));

        rb = Mathf.Max(Mathf.Max(Mathf.Max(trProj.x, tlProj.x), blProj.x), brProj.x);
        tb = Mathf.Max(Mathf.Max(Mathf.Max(trProj.y, tlProj.y), blProj.y), brProj.y);
        lb = Mathf.Min(Mathf.Min(Mathf.Min(trProj.x, tlProj.x), blProj.x), brProj.x);
        bb = Mathf.Min(Mathf.Min(Mathf.Min(trProj.y, tlProj.y), blProj.y), brProj.y);
    }

    protected static bool DoAABBCollisionTest(float rb, float tb, float lb, float bb,
        float rbOther, float tbOther, float lbOther, float bbOther)
    {

        if (rb < lbOther || tb < bbOther || rbOther < lb || tbOther < bb)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
