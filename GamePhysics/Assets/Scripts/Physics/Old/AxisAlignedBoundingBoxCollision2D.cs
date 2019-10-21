using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxCollision2D : CollisionHull2D
{
	public AxisAlignedBoundingBoxCollision2D() : base(CollisionHullType2D.aabb) { }

    public Vector2 bounds;

    //private Transform OBJtrAABB, OBJtlAABB, OBJbrAABB, OBJblAABB;
    //private Transform OBJtrTranOBB, OBJtlTranOBB, OBJbrTranOBB, OBJblTranOBB;
    //private Transform OBJtrOBB, OBJtlOBB, OBJbrOBB, OBJblOBB;
    //private Transform OBJtrTranAABB, OBJtlTranAABB, OBJbrTranAABB, OBJblTranAABB;

	void Start()
    {
        bounds = new Vector2(transform.localScale.x, transform.localScale.y);

        //OBJtrAABB = Instantiate(testPoint);
        //OBJtlAABB = Instantiate(testPoint);
        //OBJbrAABB = Instantiate(testPoint);
        //OBJblAABB = Instantiate(testPoint);
        //
        //OBJtrAABB.GetComponent<MeshRenderer>().material.color = Color.blue;
        //OBJtlAABB.GetComponent<MeshRenderer>().material.color = Color.blue;
        //OBJbrAABB.GetComponent<MeshRenderer>().material.color = Color.blue;
        //OBJblAABB.GetComponent<MeshRenderer>().material.color = Color.blue;
        //
        //OBJtrTranOBB = Instantiate(testPoint);
        //OBJtlTranOBB = Instantiate(testPoint);
        //OBJbrTranOBB = Instantiate(testPoint);
        //OBJblTranOBB = Instantiate(testPoint);
        //
        //OBJtrTranOBB.GetComponent<MeshRenderer>().material.color = Color.yellow;
        //OBJtlTranOBB.GetComponent<MeshRenderer>().material.color = Color.yellow;
        //OBJbrTranOBB.GetComponent<MeshRenderer>().material.color = Color.yellow;
        //OBJblTranOBB.GetComponent<MeshRenderer>().material.color = Color.yellow;
        //
        //OBJtrTranAABB = Instantiate(testPoint);
        //OBJtlTranAABB = Instantiate(testPoint);
        //OBJbrTranAABB = Instantiate(testPoint);
        //OBJblTranAABB = Instantiate(testPoint);
        //
        //OBJtrOBB = Instantiate(testPoint);
        //OBJtlOBB = Instantiate(testPoint);
        //OBJbrOBB = Instantiate(testPoint);
        //OBJblOBB = Instantiate(testPoint);
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref List<ParticleContact> c)
	{
        //SEE CIRCLE

        // find closest point to circle on box
        // done by clamping center of circle to be within box dimensions
        // if closest point is within circle, pass! (do circle vs point test)


        // 1. get circle center
        Vector2 circleCenter = other.transform.position;
        float xMaxBound = transform.position.x + transform.localScale.x * 0.5f;
        float xMinBound = transform.position.x - transform.localScale.x * 0.5f;

        // 3. get box y bounds
        float yMaxBound = transform.position.y + transform.localScale.y * 0.5f;
        float yMinBound = transform.position.y - transform.localScale.y * 0.5f;

        // 4. clamp circle center on box x bound
        // 5. clamp circle center on box y bound

        float circleOnX = circleCenter.x;
        float circleOnY = circleCenter.y;

        if (circleCenter.x > xMaxBound)
        {
            circleOnX = xMaxBound;
        }
        else if (circleCenter.x < xMinBound)
        {
            circleOnX = xMinBound;
        }

        if (circleCenter.y > yMaxBound)
        {
            circleOnY = yMaxBound;
        }
        else if (circleCenter.y < yMinBound)
        {
            circleOnY = yMinBound;
        }

        // 6. use clamped point as closest point of box
        Vector2 closestPoint = new Vector2(circleOnX, circleOnY);
        Vector2 distance = closestPoint - circleCenter;
        float distSqr = Vector2.Dot(distance, distance);

        // 7. check if closest point of box is within the circle
        // 8. do test (if in circle, true, else false)
        if (distSqr < other.radius * other.radius)
        {
            Debug.Log("BOX -> CIRCLE");
            //Todo Add the contact data to the contact list.
            //Particle2D aParticle = GetComponent<Particle2D>();
            //Particle2D bParticle = other.GetComponent<Particle2D>();

            //Vector2 normal = Vector2.down;

            //float penetration = (aParticle.Position - bParticle.Position).magnitude - distance.magnitude;

            //ParticleContact contact = new ParticleContact(
            //    aParticle, bParticle, 0.0f, normal, penetration);

            //c.Add(contact);
            return true;
        }

        return false;
	}

	public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other, ref List<ParticleContact> col)
	{
        //pass if, for all axis, max extent of A is greater than min extent of B
        //

        // 1. get box A min extent
        float xMinExtentA = transform.position.x - transform.lossyScale.x * 0.5f;
        float yMinExtentA = transform.position.y - transform.lossyScale.y * 0.5f;
        // 2. get box A max extent
        float xMaxExtentA = transform.position.x + transform.lossyScale.x * 0.5f;
        float yMaxExtentA = transform.position.y + transform.lossyScale.y * 0.5f;
        // 3. get box B min extent
        float xMinExtentB = other.transform.position.x - other.transform.lossyScale.x * 0.5f;
        float yMinExtentB = other.transform.position.y - other.transform.lossyScale.y * 0.5f;
        // 4. get box B max extent
        float xMaxExtentB = other.transform.position.x + other.transform.lossyScale.x * 0.5f;
        float yMaxExtentB = other.transform.position.y + other.transform.lossyScale.y * 0.5f;

        if (DoAABBCollisionTest(xMaxExtentA, yMaxExtentA, xMinExtentA, yMinExtentA,
            xMaxExtentB, yMaxExtentB, xMinExtentB, yMinExtentB))
        {
            Debug.Log("AABB -> AABB");
            return true;
        }
        else
        {
            return false;
        }
        // 5. check if max extent of A.x is greater than min extent B.x
        //if (!(xMaxExtentA > xMinExtentB))
        //{
        //    return false;
        //}
        //// 6. check if max extent of A.y is greater than min extent B.y
        //if (!(yMaxExtentA > yMinExtentB))
        //{
        //    return false;
        //}
        //// 7. check if max extent of B.x is greater than min extent A.x
        //if (!(xMaxExtentB > xMinExtentA))
        //{
        //    return false;
        //}
        //// 8. check if max extent of B.y is greater than min extent A.y
        //if (!(yMaxExtentB > yMinExtentA))
        //{
        //    return false;
        //}
        // 9. only if all cases pass, collision is true

        //Debug.Log("Bammin Slammin, bootylicious");
	
		//return true;
	}

	public override bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other, ref List<ParticleContact> col)
	{
        // 1. Find width and height for both boxes.

        float widthAABB = transform.lossyScale.x * 0.5f;
        float heightAABB = transform.lossyScale.y * 0.5f;
        float widthOBB = other.transform.lossyScale.x * 0.5f;
        float heightOBB = other.transform.lossyScale.y * 0.5f;


        // 2. Find local points for both boxes.

        Vector2 trAABB_LOCAL = new Vector2(widthAABB, heightAABB);
        Vector2 tlAABB_LOCAL = new Vector2(-widthAABB, heightAABB);
        Vector2 blAABB_LOCAL = new Vector2(-widthAABB, -heightAABB);
        Vector2 brAABB_LOCAL = new Vector2(widthAABB, -heightAABB);

        Vector2 trOBB_LOCAL = new Vector2(widthOBB, heightOBB);
        Vector2 tlOBB_LOCAL = new Vector2(-widthOBB, heightOBB);
        Vector2 blOBB_LOCAL = new Vector2(-widthOBB, -heightOBB);
        Vector2 brOBB_LOCAL = new Vector2(widthOBB, -heightOBB);


        // 3. Find world points for both boxes.

        Vector2 trAABB_WORLD = transform.TransformPoint(new Vector2(widthAABB, heightAABB));
        Vector2 tlAABB_WORLD = transform.TransformPoint(new Vector2(-widthAABB, heightAABB));
        Vector2 blAABB_WORLD = transform.TransformPoint(new Vector2(-widthAABB, -heightAABB));
        Vector2 brAABB_WORLD = transform.TransformPoint(new Vector2(widthAABB, -heightAABB));

        Vector2 trOBB_WORLD = other.transform.TransformPoint(new Vector2(widthOBB, heightOBB));
        Vector2 tlOBB_WORLD = other.transform.TransformPoint(new Vector2(-widthOBB, heightOBB));
        Vector2 blOBB_WORLD = other.transform.TransformPoint(new Vector2(-widthOBB, -heightOBB));
        Vector2 brOBB_WORLD = other.transform.TransformPoint(new Vector2(widthOBB, -heightOBB));


        // 4. Convert OBB into local AABB space

        Vector2 trTranOBB = transform.InverseTransformPoint(trOBB_WORLD);
        Vector2 tlTranOBB = transform.InverseTransformPoint(tlOBB_WORLD);
        Vector2 blTranOBB = transform.InverseTransformPoint(blOBB_WORLD);
        Vector2 brTranOBB = transform.InverseTransformPoint(brOBB_WORLD);

        //OBJtrAABB.position = trAABB_LOCAL;
        //OBJtlAABB.position = tlAABB_LOCAL;
        //OBJblAABB.position = blAABB_LOCAL;
        //OBJbrAABB.position = brAABB_LOCAL;


        // 5. Calculate transformed OBB's AABB

        float rbTransOBB, tbTransOBB, lbTransOBB, bbTransOBB;
        CalculateAABB(trTranOBB, tlTranOBB, blTranOBB, brTranOBB, out rbTransOBB, out tbTransOBB, out lbTransOBB, out bbTransOBB);


        // 6. Do AABB collision test of AABB vs transformed OBB's ABB

        bool isTransformedOBBinAABB = DoAABBCollisionTest(widthAABB, heightAABB, -widthAABB, -heightAABB, rbTransOBB, tbTransOBB, lbTransOBB, bbTransOBB);


        // 7. If the test comes back neg, then return false because there is no way the boxes are colliding

        if (!isTransformedOBBinAABB)
        {
            return false;
        }


        // 8. Convert AABB into local OBB space.

        Vector2 trTranAABB = other.transform.InverseTransformPoint(trAABB_WORLD);
        Vector2 tlTranAABB = other.transform.InverseTransformPoint(tlAABB_WORLD);
        Vector2 blTranAABB = other.transform.InverseTransformPoint(blAABB_WORLD);
        Vector2 brTranAABB = other.transform.InverseTransformPoint(brAABB_WORLD);


        // 9. Calculate transformed AABB's AABB

        float rbTransAABB, tbTransAABB, lbTransAABB, bbTransAABB;
        CalculateAABB(trTranAABB, tlTranAABB, blTranAABB, brTranAABB, out rbTransAABB, out tbTransAABB, out lbTransAABB, out bbTransAABB);


        // 10. Do AABB collision test of OBB's local AABB vs transformed AABB's AABB

        bool isTransformedAABBinOBB = DoAABBCollisionTest(widthOBB, heightOBB, -widthOBB, -heightOBB, rbTransAABB, tbTransAABB, lbTransAABB, bbTransAABB);

        if (isTransformedAABBinOBB)
        {
            Debug.Log("AABB -> OBB");
            return true;
        }
        else
        {
            return false;
        }
    }
}
