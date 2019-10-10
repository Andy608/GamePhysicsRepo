using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public class Contact
        {
            public float Penetration { get; private set; }
            public Vector2 Point { get; private set; }
            public Vector2 Normal { get; private set; }
            public float CoefficientRestitution { get; private set; }

            public Contact(Vector2 p, Vector2 n, float coeffRes, float depPen)
            {
                Point = p;
                Normal = n;
                CoefficientRestitution = coeffRes;
				Penetration = depPen;
            }
        }

        public CollisionHull2D a = null, b = null;
        public bool status = false;
        public Contact[] contactPoints = new Contact[4];
        public int contactCount = 0;
        public float separatingVelocity = 0.0f;

        public Collision(CollisionHull2D hullA, CollisionHull2D hullB, Contact[] c, float separatingVel)
        {
            a = hullA;
            b = hullB;
            contactPoints = c;
            separatingVelocity = separatingVel;
        }

        public void Resolve()
        {
            ResolveVelocity();
            ResolveInterpenetration();
        }

        protected void ResolveInterpenetration()
        {
            foreach (Contact c in contactPoints)
            {
                if (c == null) break;

				if (c.Penetration <= 0)
				{
					Debug.Log("No pen");
					return;
				}

                float totalInverseMass = a.GetComponent<Particle2D>().MassInv;

                if (b != null)
                {
                    totalInverseMass += b.GetComponent<Particle2D>().MassInv;
                }

                if (totalInverseMass <= 0) return;

                Vector2 movePerIMass = c.Normal * (c.Penetration / totalInverseMass);
				Debug.Log(movePerIMass);
				Debug.Log(c.Penetration);
				Vector2 aMovement = movePerIMass * a.GetComponent<Particle2D>().MassInv;
				a.GetComponent<Particle2D>().SetPosition(a.GetComponent<Particle2D>().Position + aMovement);

				if (b != null)
                {
					Vector2 bMovement = movePerIMass * b.GetComponent<Particle2D>().MassInv;
					b.GetComponent<Particle2D>().SetPosition(b.GetComponent<Particle2D>().Position + bMovement);
				}

			}

        }

        protected void ResolveVelocity()
        {
            if (separatingVelocity > 0.0f)
            {
                //No impulse
                return;
            }

            foreach(Contact c in contactPoints)
            {
                if (c == null) break;

				//Calculate the new separating vel.
				float newSepVel = -separatingVelocity * c.CoefficientRestitution;
                float deltaVel = newSepVel - separatingVelocity;

                //We apply the change in velocity to each object in proportion to their inverse mass
                float totalInverseMass = a.GetComponent<Particle2D>().MassInv;

                if (b != null)
                {
                    totalInverseMass += b.GetComponent<Particle2D>().MassInv;
                }

                //Infinite mass
                if (totalInverseMass <= 0)
                {
					Debug.Log("Inf Mass");
                    return;
                }

                float impulse = deltaVel / totalInverseMass;

                //Find the amount of impulse per unit of inverse mass.
                Vector2 impulsePerIMass = c.Normal * impulse;
                a.GetComponent<Particle2D>().SetVelocity(a.GetComponent<Particle2D>().Velocity + impulsePerIMass * a.GetComponent<Particle2D>().MassInv);

                if (b != null)
                {
                    b.GetComponent<Particle2D>().SetVelocity(b.GetComponent<Particle2D>().Velocity + impulsePerIMass * -b.GetComponent<Particle2D>().MassInv);
                }
            }

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
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected Particle2D particle;

    private CollisionHullType2D type { get; }

    private List<CollisionHull2D> collidingWith = new List<CollisionHull2D>();

    public void SetColliding(bool colliding, CollisionHull2D otherObj)
    {
        if (colliding)
        {
            if (!IsColliding(otherObj))
            {
                collidingWith.Add(otherObj);
                //otherObj.collidingWith.Add(this, c);
            }
        }
        else
        {
            collidingWith.Remove(otherObj);
            //otherObj.collidingWith.Remove(this);
        }

        if (IsColliding())
        {
            if (material)
                material.color = Color.red;
            else
                spriteRenderer.color = Color.red;
        }
        else
        {
            if (material)
                material.color = Color.green;
            else
                spriteRenderer.color = Color.green;
        }
    }

    public bool IsColliding()
    {
        return collidingWith.Count != 0;
    }

    public bool IsColliding(CollisionHull2D other)
    {
        return collidingWith.Contains(other);
    }

    protected CollisionHull2D(CollisionHullType2D _type)
    {
        type = _type;
    }

    private void Awake()
    {
        particle = GetComponent<Particle2D>();

        material = GetComponent<Material>();

        CollisionManager.Instance?.RegisterObject(this);
    }

    private void OnDestroy()
    {
        CollisionManager.Instance?.UnRegisterObject(this);
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision col)
    {
        bool validCollision = false;

        if (b.type == CollisionHullType2D.circle)
        {
            validCollision = a.TestCollisionVsCircle((b as CircleCollisionHull2D), ref col);
        }
        else if (b.type == CollisionHullType2D.aabb)
        {
            validCollision = a.TestCollisionVsAABB((b as AxisAlignedBoundingBoxCollision2D), ref col);
        }
        else if (b.type == CollisionHullType2D.obb)
        {
            validCollision = a.TestCollisionVsObject((b as ObjectBoundingBoxCollisionHull2D), ref col);
        }
        else if (b.type == CollisionHullType2D.penis)
        {
            print("I N T E R P E N E T R A T I O N");
        }

        return validCollision;
    }

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other, ref Collision c);

    public abstract bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other, ref Collision c);

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
