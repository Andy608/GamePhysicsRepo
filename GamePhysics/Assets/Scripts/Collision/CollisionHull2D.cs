using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//andy is big dweeb fuck him
public abstract class CollisionHull2D : MonoBehaviour
{
	public CollisionHull2D otherObj;

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

	protected static bool TestCollision(CollisionHull2D a, CollisionHull2D b)
	{

		return false;
	}

	private void Start()
	{
		particle = GetComponent<Particle2D>();
	}

	private void Update()
	{
		//for (int i = 0; i < 4; i++)
		//{
		if (otherObj.type == CollisionHullType2D.circle)
		{
			TestCollisionVsCircle(otherObj as CircleCollisionHull2D);
		}
		else if (otherObj.type == CollisionHullType2D.aabb)
		{
			TestCollisionVsAABB(otherObj as AxisAlignedBoundingBoxCollision2D);
		}
		//}
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
