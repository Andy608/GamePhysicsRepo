using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxCollisionHull2D : CollisionHull2D
{
	public ObjectBoundingBoxCollisionHull2D() : base(CollisionHullType2D.obb) { }

    //[Range(0.0f, 720.0f)] public float rotationSpeed = 90.0f;

    //public void Update()
    //{
    //    //transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    //}

	public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
	{
        //same as above but first...
        //multiply circle center by box inverse matrix 

        // 1. get circle center
        Vector4 circleCenter = new Vector4(other.transform.position.x, other.transform.position.y, 0.0f, 1.0f);

        // 2. get box x bounds 
        float xMaxBound = transform.localScale.x * 0.5f;
        float xMinBound = -transform.localScale.x * 0.5f;
        // 3. get box y bounds

        float yMaxBound = transform.localScale.y * 0.5f;
        float yMinBound = -transform.localScale.y * 0.5f;

        // 4. multiply circle center world position by box world to local matrix
        circleCenter = transform.InverseTransformPoint(circleCenter);

        // 5. clamp circle center on box x bound
        float circleOnX = circleCenter.x;

        // 6. clamp circle center on box y bound
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

        // 7. use clamped point as closest point of box
        Vector2 closestPoint = new Vector2(circleOnX, circleOnY);

        Vector2 distance = closestPoint - new Vector2(circleCenter.x, circleCenter.y);
        float distSqr = Vector2.Dot(distance, distance);

        // 8. check if closest point of box is within the circle
        if (distSqr < other.radius * other.radius)
        {
            Debug.Log("OBB -> CIRCLE");
            return true;
        }

        // 9. do test (if in circle, true, else false)
        return false;
	}

	public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxCollision2D other)
	{
        // 1. Find width and height for both boxes.

        float widthAABB = other.transform.lossyScale.x * 0.5f;
        float heightAABB = other.transform.lossyScale.y * 0.5f;
        float widthOBB = transform.lossyScale.x * 0.5f;
        float heightOBB = transform.lossyScale.y * 0.5f;


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

        Vector2 trAABB_WORLD = other.transform.TransformPoint(new Vector2(widthAABB, heightAABB));
        Vector2 tlAABB_WORLD = other.transform.TransformPoint(new Vector2(-widthAABB, heightAABB));
        Vector2 blAABB_WORLD = other.transform.TransformPoint(new Vector2(-widthAABB, -heightAABB));
        Vector2 brAABB_WORLD = other.transform.TransformPoint(new Vector2(widthAABB, -heightAABB));

        Vector2 trOBB_WORLD = transform.TransformPoint(new Vector2(widthOBB, heightOBB));
        Vector2 tlOBB_WORLD = transform.TransformPoint(new Vector2(-widthOBB, heightOBB));
        Vector2 blOBB_WORLD = transform.TransformPoint(new Vector2(-widthOBB, -heightOBB));
        Vector2 brOBB_WORLD = transform.TransformPoint(new Vector2(widthOBB, -heightOBB));


        // 4. Convert OBB into local AABB space

        Vector2 trTranOBB = other.transform.InverseTransformPoint(trOBB_WORLD);
        Vector2 tlTranOBB = other.transform.InverseTransformPoint(tlOBB_WORLD);
        Vector2 blTranOBB = other.transform.InverseTransformPoint(blOBB_WORLD);
        Vector2 brTranOBB = other.transform.InverseTransformPoint(brOBB_WORLD);

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

        Vector2 trTranAABB = transform.InverseTransformPoint(trAABB_WORLD);
        Vector2 tlTranAABB = transform.InverseTransformPoint(tlAABB_WORLD);
        Vector2 blTranAABB = transform.InverseTransformPoint(blAABB_WORLD);
        Vector2 brTranAABB = transform.InverseTransformPoint(brAABB_WORLD);


        // 9. Calculate transformed AABB's AABB

        float rbTransAABB, tbTransAABB, lbTransAABB, bbTransAABB;
        CalculateAABB(trTranAABB, tlTranAABB, blTranAABB, brTranAABB, out rbTransAABB, out tbTransAABB, out lbTransAABB, out bbTransAABB);


        // 10. Do AABB collision test of OBB's local AABB vs transformed AABB's AABB

        bool isTransformedAABBinOBB = DoAABBCollisionTest(widthOBB, heightOBB, -widthOBB, -heightOBB, rbTransAABB, tbTransAABB, lbTransAABB, bbTransAABB);

        if (isTransformedAABBinOBB)
        {
            Debug.Log("OBB -> AABB");
            return true;
        }
        else
        {
            return false;
        }
    }

	public override bool TestCollisionVsObject(ObjectBoundingBoxCollisionHull2D other)
	{
        // 1. Find width and height for both boxes.

        float widthOBB1 = transform.lossyScale.x * 0.5f;
        float heightOBB1 = transform.lossyScale.y * 0.5f;
        float widthOBB2 = other.transform.lossyScale.x * 0.5f;
        float heightOBB2 = other.transform.lossyScale.y * 0.5f;


        // 2. Find local points for both boxes.

        Vector2 trOBB1_LOCAL = new Vector2(widthOBB1, heightOBB1);
        Vector2 tlOBB1_LOCAL = new Vector2(-widthOBB1, heightOBB1);
        Vector2 blOBB1_LOCAL = new Vector2(-widthOBB1, -heightOBB1);
        Vector2 brOBB1_LOCAL = new Vector2(widthOBB1, -heightOBB1);

        Vector2 trOBB2_LOCAL = new Vector2(widthOBB2, heightOBB2);
        Vector2 tlOBB2_LOCAL = new Vector2(-widthOBB2, heightOBB2);
        Vector2 blOBB2_LOCAL = new Vector2(-widthOBB2, -heightOBB2);
        Vector2 brOBB2_LOCAL = new Vector2(widthOBB2, -heightOBB2);


        // 3. Find world points for both boxes.

        Vector2 trOBB1_WORLD = transform.TransformPoint(new Vector2(widthOBB1, heightOBB1));
        Vector2 tlOBB1_WORLD = transform.TransformPoint(new Vector2(-widthOBB1, heightOBB1));
        Vector2 blOBB1_WORLD = transform.TransformPoint(new Vector2(-widthOBB1, -heightOBB1));
        Vector2 brOBB1_WORLD = transform.TransformPoint(new Vector2(widthOBB1, -heightOBB1));

        Vector2 trOBB2_WORLD = other.transform.TransformPoint(new Vector2(widthOBB2, heightOBB2));
        Vector2 tlOBB2_WORLD = other.transform.TransformPoint(new Vector2(-widthOBB2, heightOBB2));
        Vector2 blOBB2_WORLD = other.transform.TransformPoint(new Vector2(-widthOBB2, -heightOBB2));
        Vector2 brOBB2_WORLD = other.transform.TransformPoint(new Vector2(widthOBB2, -heightOBB2));


        // 4. Convert OBB2 into local OBB1 space

        Vector2 trTranOBB2 = transform.InverseTransformPoint(trOBB2_WORLD);
        Vector2 tlTranOBB2 = transform.InverseTransformPoint(tlOBB2_WORLD);
        Vector2 blTranOBB2 = transform.InverseTransformPoint(blOBB2_WORLD);
        Vector2 brTranOBB2 = transform.InverseTransformPoint(brOBB2_WORLD);

        // 5. Calculate transformed OBB2's AABB

        float rbTransOBB2, tbTransOBB2, lbTransOBB2, bbTransOBB2;
        CalculateAABB(trTranOBB2, tlTranOBB2, blTranOBB2, brTranOBB2, out rbTransOBB2, out tbTransOBB2, out lbTransOBB2, out bbTransOBB2);


        // 6. Do AABB collision test of OBB1 vs transformed OBB2's ABB

        bool isTransformedOBB2inOBB1 = DoAABBCollisionTest(widthOBB1, heightOBB1, -widthOBB1, -heightOBB1, rbTransOBB2, tbTransOBB2, lbTransOBB2, bbTransOBB2);


        // 7. If the test comes back neg, then return false because there is no way the boxes are colliding

        if (!isTransformedOBB2inOBB1)
        {
            return false;
        }


        // 8. Convert OBB1 into local OBB2 space.

        Vector2 trTranOBB1 = other.transform.InverseTransformPoint(trOBB1_WORLD);
        Vector2 tlTranOBB1 = other.transform.InverseTransformPoint(tlOBB1_WORLD);
        Vector2 blTranOBB1 = other.transform.InverseTransformPoint(blOBB1_WORLD);
        Vector2 brTranOBB1 = other.transform.InverseTransformPoint(brOBB1_WORLD);


        // 9. Calculate transformed OBB1's AABB

        float rbTransOBB1, tbTransOBB1, lbTransOBB1, bbTransOBB1;
        CalculateAABB(trTranOBB1, tlTranOBB1, blTranOBB1, brTranOBB1, out rbTransOBB1, out tbTransOBB1, out lbTransOBB1, out bbTransOBB1);


        // 10. Do AABB collision test of OBB2's local AABB vs transformed OBB1's AABB

        bool isTransformedOBB1inOBB2 = DoAABBCollisionTest(widthOBB2, heightOBB2, -widthOBB2, -heightOBB2, rbTransOBB1, tbTransOBB1, lbTransOBB1, bbTransOBB1);

        if (isTransformedOBB1inOBB2)
        {
            Debug.Log("OBB -> OBB");
            return true;
        }
        else
        {
            return false;
        }
    }
}
