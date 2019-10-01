using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//andy is big dweeb fuck him
public abstract class CollisionHull2D : MonoBehaviour
{
	public enum CollisionHullType2D
	{
		circle,
		aabb,
		obb,
		penis
	}

	protected Particle2D particle;

	private CollisionHullType2D type { get; }

	protected CollisionHull2D(CollisionHullType2D _type)
	{
		type = _type;
	}

	private void Start()
	{
		particle = GetComponent<Particle2D>();
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

	public virtual bool TestCollisionVsCircle(CircleCollisionHull2D other)
	{
		return false;
	}

	public virtual bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other)
	{
		return false;
	}

	public virtual bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other)
	{
		return false;
	}
}
