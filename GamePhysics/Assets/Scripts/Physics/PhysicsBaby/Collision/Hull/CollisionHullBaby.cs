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

    protected static bool TestCollisionCircleVsCircle(CollisionHullBabyCircle circle1, CollisionHullBabyCircle circle2, ref List<RigidBabyContact> c)
    {
        //1. get two centers
        Vector3 circleCenterA = circle1.transform.position;
        Vector3 circleCenterB = circle2.transform.position;

        //2. get the distance between the two circles
        Vector3 distance = circleCenterB - circleCenterA;
        float distanceSquared = Vector3.Dot(distance, distance);

        //3. get the total distance of added radii
        float radiiSum = circle1.radius + circle2.radius;
        radiiSum *= radiiSum;

        //6. test if the distance between the circles is smaller than their total radii distance
        if (distanceSquared < radiiSum)
        {
            RigidBaby rigidBabyA = circle1.GetComponent<RigidBaby>();
            RigidBaby rigidBabyB = circle2.GetComponent<RigidBaby>();

            Vector3 normal = (rigidBabyA.GetPosition() - rigidBabyB.GetPosition()).normalized;
            float penetration = circle2.radius + circle1.radius - distance.magnitude;
            float restitution = 0.0f; // In the future we can store this in RigidBaby and then add up the two values from 1 and 2.

            RigidBabyContact contact = new RigidBabyContact(
                rigidBabyA, rigidBabyB, restitution, normal, penetration);

            c.Add(contact);

            Debug.Log("Circle and Circle Collision!");
            return true;
        }

        return false;
    }

    protected static bool TestCollisionAABBvsAABB(CollisionHullBabyAABB AABB1, CollisionHullBabyAABB AABB2, ref List<RigidBabyContact> c)
    {
        //0. Convenience Variables
        Vector3 positionAABB1 = AABB1.transform.position;
        Vector3 dimensionsAABB1 = AABB1.transform.localScale * 0.5f;

        Vector3 positionAABB2 = AABB2.transform.position;
        Vector3 dimensionsAABB2 = AABB2.transform.localScale * 0.5f;

        //1. get max and min bounds of AABB1 and AABB2  ->  Vector2(minBound, maxBound)
        Vector2 AABB1x = new Vector2(positionAABB1.x - dimensionsAABB1.x, positionAABB1.x + dimensionsAABB1.x);
        Vector2 AABB1y = new Vector2(positionAABB1.y - dimensionsAABB1.y, positionAABB1.y + dimensionsAABB1.y);
        Vector2 AABB1z = new Vector2(positionAABB1.z - dimensionsAABB1.z, positionAABB1.z + dimensionsAABB1.z);

        Vector2 AABB2x = new Vector2(positionAABB2.x - dimensionsAABB2.x, positionAABB2.x + dimensionsAABB2.x);
        Vector2 AABB2y = new Vector2(positionAABB2.y - dimensionsAABB2.y, positionAABB2.y + dimensionsAABB2.y);
        Vector2 AABB2z = new Vector2(positionAABB2.z - dimensionsAABB2.z, positionAABB2.z + dimensionsAABB2.z);

        if (DOAABBCollisionTest(AABB1x, AABB1y, AABB1z, AABB2x, AABB2y, AABB2z))
        {
            Debug.Log("AABB and AABB Collision!");

            //Do contact stuff here.

            return true;
        }
        else
        {
            return false;
        }
    }

    protected static bool TestCollisionAABBVsCircle(CollisionHullBabyAABB AABB, CollisionHullBabyCircle circle, ref List<RigidBabyContact> c)
    {
        //0. Convenience Variables
        Vector3 positionAABB = AABB.transform.position;
        Vector3 dimensionsAABB = AABB.transform.localScale * 0.5f;

        //1. get circle center
        Vector3 circleCenter = circle.transform.position;

        //2. get max and min bounds of the AABB  ->  Vector2(minBound, maxBound)
        Vector2 xBoundsAABB = new Vector2(positionAABB.x - dimensionsAABB.x, positionAABB.x + dimensionsAABB.x);
        Vector2 yBoundsAABB = new Vector2(positionAABB.y - dimensionsAABB.y, positionAABB.y + dimensionsAABB.y);
        Vector2 zBoundsAABB = new Vector2(positionAABB.z - dimensionsAABB.z, positionAABB.z + dimensionsAABB.z);

        //3. clamp Circle Center To AABB Bounds
        #region 

        Vector3 boundedCirclePosition = circleCenter;

        if (circleCenter.x > xBoundsAABB.y)
        {
            boundedCirclePosition.x = xBoundsAABB.y;
        }
        else if (circleCenter.x < xBoundsAABB.x)
        {
            boundedCirclePosition.x = xBoundsAABB.x;
        }

        if (circleCenter.y > yBoundsAABB.y)
        {
            boundedCirclePosition.y = yBoundsAABB.y;
        }
        else if (circleCenter.y < yBoundsAABB.x)
        {
            boundedCirclePosition.y = yBoundsAABB.x;
        }

        if (circleCenter.z > zBoundsAABB.y)
        {
            boundedCirclePosition.z = zBoundsAABB.y;
        }
        else if (circleCenter.z < zBoundsAABB.x)
        {
            boundedCirclePosition.z = zBoundsAABB.x;
        }
        #endregion

        //4. use the clamped point as the closest point to the AABB
        Vector3 distance = boundedCirclePosition - circleCenter;
        float distanceSquared = Vector3.Dot(distance, distance);

        //5. check if the bounded point is within the circle
        if (distanceSquared < circle.radius * circle.radius)
        {
            Debug.Log("AABB and Circle Collision!");

            //Do contact stuff here.

            return true;
        }

        return false;
    }

    protected static bool CollisionTestAABBVsOBB(CollisionHullBabyAABB AABB, CollisionHullBabyOBB OBB, ref List<RigidBabyContact> c)
    {
        //0. Convenience Variables
        Vector3 positionAABB = AABB.transform.position;
        Vector3 dimAABB = AABB.transform.lossyScale * 0.5f;

        Vector3 positionOBB = OBB.transform.position;
        Vector3 dimOBB = OBB.transform.lossyScale * 0.5f;

        RigidBaby AABBRigidBaby = AABB.GetComponent<RigidBaby>();
        RigidBaby OBBRigidBaby = OBB.GetComponent<RigidBaby>();

        //Matrix4x4 AABBTransformMat = AABBRigidBaby.TransformationMat;//transformationMat = transform.localToWorldMatrix//TransformationMat* inverseInertiaTensor *worldToLocal(worldTorque, TransformationMat);
        //Matrix4x4 OBBTransformMat = OBBRigidBaby.TransformationMat;

        Vector2 xBoundsAABB = new Vector2(-dimAABB.x, dimAABB.x);
        Vector2 yBoundsAABB = new Vector2(-dimAABB.y, dimAABB.y);
        Vector2 zBoundsAABB = new Vector2(-dimAABB.z, dimAABB.z);

        Vector2 xBoundsOBB = new Vector2(-dimOBB.x, dimOBB.x);
        Vector2 yBoundsOBB = new Vector2(-dimOBB.y, dimOBB.y);
        Vector2 zBoundsOBB = new Vector2(-dimOBB.z, dimOBB.z);

        // 2. Find corner points for both boxes.
        Vector3 WorldSpace_rightTopFront_AABB = AABB.transform.TransformPoint(new Vector3(dimAABB.x, dimAABB.y, dimAABB.z));
        Vector3 WorldSpace_leftTopFront_AABB = AABB.transform.TransformPoint(new Vector3(-dimAABB.x, dimAABB.y, dimAABB.z));
        Vector3 WorldSpace_rightTopBack_AABB = AABB.transform.TransformPoint(new Vector3(dimAABB.x, dimAABB.y, -dimAABB.z));
        Vector3 WorldSpace_leftTopBack_AABB = AABB.transform.TransformPoint(new Vector3(-dimAABB.x, dimAABB.y, -dimAABB.z));
        Vector3 WorldSpace_rightBottomFront_AABB = AABB.transform.TransformPoint(new Vector3(dimAABB.x, -dimAABB.y, dimAABB.z));
        Vector3 WorldSpace_leftBottomFront_AABB = AABB.transform.TransformPoint(new Vector3(-dimAABB.x, -dimAABB.y, dimAABB.z));
        Vector3 WorldSpace_rightBottomBack_AABB = AABB.transform.TransformPoint(new Vector3(dimAABB.x, -dimAABB.y, -dimAABB.z));
        Vector3 WorldSpace_leftBottomBack_AABB = AABB.transform.TransformPoint(new Vector3(-dimAABB.x, -dimAABB.y, -dimAABB.z));

        Vector3 WorldSpace_rightTopFront_OBB = OBB.transform.TransformPoint(new Vector3(dimOBB.x, dimOBB.y, dimOBB.z));
        Vector3 WorldSpace_leftTopFront_OBB = OBB.transform.TransformPoint(new Vector3(-dimOBB.x, dimOBB.y, dimOBB.z));
        Vector3 WorldSpace_rightTopBack_OBB = OBB.transform.TransformPoint(new Vector3(dimOBB.x, dimOBB.y, -dimOBB.z));
        Vector3 WorldSpace_leftTopBack_OBB = OBB.transform.TransformPoint(new Vector3(-dimOBB.x, dimOBB.y, -dimOBB.z));
        Vector3 WorldSpace_rightBottomFront_OBB = OBB.transform.TransformPoint(new Vector3(dimOBB.x, -dimOBB.y, dimOBB.z));
        Vector3 WorldSpace_leftBottomFront_OBB = OBB.transform.TransformPoint(new Vector3(-dimOBB.x, -dimOBB.y, dimOBB.z));
        Vector3 WorldSpace_rightBottomBack_OBB = OBB.transform.TransformPoint(new Vector3(dimOBB.x, -dimOBB.y, -dimOBB.z));
        Vector3 WorldSpace_leftBottomBack_OBB = OBB.transform.TransformPoint(new Vector3(-dimOBB.x, -dimOBB.y, -dimOBB.z));

        //3. Find AABBSpace points for OBB
        Vector3 AABBSpace_rightTopFront_OBB = AABB.transform.InverseTransformPoint(WorldSpace_rightTopFront_OBB);
        Vector3 AABBSpace_leftTopFront_OBB = AABB.transform.InverseTransformPoint(WorldSpace_leftTopFront_OBB);
        Vector3 AABBSpace_rightTopBack_OBB = AABB.transform.InverseTransformPoint(WorldSpace_rightTopBack_OBB);
        Vector3 AABBSpace_leftTopBack_OBB = AABB.transform.InverseTransformPoint(WorldSpace_leftTopBack_OBB);
        Vector3 AABBSpace_rightBottomFront_OBB = AABB.transform.InverseTransformPoint(WorldSpace_rightBottomFront_OBB);
        Vector3 AABBSpace_leftBottomFront_OBB = AABB.transform.InverseTransformPoint(WorldSpace_leftBottomFront_OBB);
        Vector3 AABBSpace_rightBottomBack_OBB = AABB.transform.InverseTransformPoint(WorldSpace_rightBottomBack_OBB);
        Vector3 AABBSpace_leftBottomBack_OBB = AABB.transform.InverseTransformPoint(WorldSpace_leftBottomBack_OBB);

        // 4. Calculate transformed OBB's AABB
        Vector2 AABBSpace_xBound_OBBs_AABB;
        Vector2 AABBSpace_yBound_OBBs_AABB;
        Vector2 AABBSpace_zBound_OBBs_AABB;

        CalculateAABB(AABBSpace_rightTopFront_OBB, AABBSpace_leftTopFront_OBB, AABBSpace_rightTopBack_OBB, AABBSpace_leftTopBack_OBB,
            AABBSpace_rightBottomFront_OBB, AABBSpace_leftBottomFront_OBB, AABBSpace_rightBottomBack_OBB, AABBSpace_leftBottomBack_OBB,
            out AABBSpace_xBound_OBBs_AABB, out AABBSpace_yBound_OBBs_AABB, out AABBSpace_zBound_OBBs_AABB);

        // 5. Do AABB collision test of AABB vs transformed OBB's ABB
        bool isTransformedOBBinAABB = DOAABBCollisionTest(xBoundsAABB, yBoundsAABB, zBoundsAABB, 
            AABBSpace_xBound_OBBs_AABB, AABBSpace_yBound_OBBs_AABB, AABBSpace_zBound_OBBs_AABB);

        // 6. If the test comes back neg, then return false because there is no way the boxes are colliding
        if (!isTransformedOBBinAABB)
        {
            return false;
        }

        // 7. Convert AABB into local OBB space.
        Vector3 OBBSpace_rightTopFront_AABB = OBB.transform.InverseTransformPoint(WorldSpace_rightTopFront_AABB);
        Vector3 OBBSpace_leftTopFront_AABB = OBB.transform.InverseTransformPoint(WorldSpace_leftTopFront_AABB);
        Vector3 OBBSpace_rightTopBack_AABB = OBB.transform.InverseTransformPoint(WorldSpace_rightTopBack_AABB);
        Vector3 OBBSpace_leftTopBack_AABB = OBB.transform.InverseTransformPoint(WorldSpace_leftTopBack_AABB);
        Vector3 OBBSpace_rightBottomFront_AABB = OBB.transform.InverseTransformPoint(WorldSpace_rightBottomFront_AABB);
        Vector3 OBBSpace_leftBottomFront_AABB = OBB.transform.InverseTransformPoint(WorldSpace_leftBottomFront_AABB);
        Vector3 OBBSpace_rightBottomBack_AABB = OBB.transform.InverseTransformPoint(WorldSpace_rightBottomBack_AABB);
        Vector3 OBBSpace_leftBottomBack_AABB = OBB.transform.InverseTransformPoint(WorldSpace_leftBottomBack_AABB);

        // 8. Calculate transformed AABB's AABB
        Vector2 OBBSpace_xBound_AABBs_AABB;
        Vector2 OBBSpace_yBound_AABBs_AABB;
        Vector2 OBBSpace_zBound_AABBs_AABB;

        CalculateAABB(OBBSpace_rightTopFront_AABB, OBBSpace_leftTopFront_AABB, OBBSpace_rightTopBack_AABB, OBBSpace_leftTopBack_AABB,
            OBBSpace_rightBottomFront_AABB, OBBSpace_leftBottomFront_AABB, OBBSpace_rightBottomBack_AABB, OBBSpace_leftBottomBack_AABB,
            out OBBSpace_xBound_AABBs_AABB, out OBBSpace_yBound_AABBs_AABB, out OBBSpace_zBound_AABBs_AABB);

        // 9. Do AABB collision test of OBB's local AABB vs transformed AABB's AABB
        bool isTransformedAABBinOBB = DOAABBCollisionTest(xBoundsOBB, yBoundsOBB, zBoundsOBB,
            OBBSpace_xBound_AABBs_AABB, OBBSpace_yBound_AABBs_AABB, OBBSpace_zBound_AABBs_AABB);

        if (isTransformedAABBinOBB)
        {
            Debug.Log("AABB and OBB Collision!");
            return true;
        }
        else
        {
            return false;
        }
    }

    protected static bool DOAABBCollisionTest(Vector2 AABB1x, Vector2 AABB1y, Vector2 AABB1z,
        Vector2 AABB2x, Vector2 AABB2y, Vector2 AABB2z)
    {
        float xMin1 = AABB1x.x, xMin2 = AABB2x.x;
        float xMax1 = AABB1x.y, xMax2 = AABB2x.y;
        float yMin1 = AABB1y.x, yMin2 = AABB2y.x;
        float yMax1 = AABB1y.y, yMax2 = AABB2y.y;
        float zMin1 = AABB1z.x, zMin2 = AABB2z.x;
        float zMax1 = AABB1z.y, zMax2 = AABB2z.y;

        if (xMax1 < xMin2 || xMin1 > xMax2 ||
            yMax1 < yMin2 || yMin1 > yMax2 ||
            zMax1 < zMin2 || zMin1 > zMax2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected static void CalculateAABB(
        Vector3 rft, Vector3 lft, Vector3 rbt, Vector3 lbt,
        Vector3 rfb, Vector3 lfb, Vector3 rbb, Vector3 lbb,
        out Vector2 xBound, out Vector2 yBound, out Vector2 zBound)
    {
        Vector3 rftProj = new Vector3(projXAxis(rft), projYAxis(rft), projZAxis(rft));
        Vector3 lftProj = new Vector3(projXAxis(lft), projYAxis(lft), projZAxis(lft));
        Vector3 rbtProj = new Vector3(projXAxis(rbt), projYAxis(rbt), projZAxis(rbt));
        Vector3 lbtProj = new Vector3(projXAxis(lbt), projYAxis(lbt), projZAxis(lbt));
        Vector3 rfbProj = new Vector3(projXAxis(rfb), projYAxis(rfb), projZAxis(rfb));
        Vector3 lfbProj = new Vector3(projXAxis(lfb), projYAxis(lfb), projZAxis(lfb));
        Vector3 rbbProj = new Vector3(projXAxis(rbb), projYAxis(rbb), projZAxis(rbb));
        Vector3 lbbProj = new Vector3(projXAxis(lbb), projYAxis(lbb), projZAxis(lbb));

        xBound.x = Mathf.Min(rftProj.x, lftProj.x, rbtProj.x, lbtProj.x, rfbProj.x, lfbProj.x, rbbProj.x, lbbProj.x);
        xBound.y = Mathf.Max(rftProj.x, lftProj.x, rbtProj.x, lbtProj.x, rfbProj.x, lfbProj.x, rbbProj.x, lbbProj.x);

        yBound.x = Mathf.Min(rftProj.y, lftProj.y, rbtProj.y, lbtProj.y, rfbProj.y, lfbProj.y, rbbProj.y, lbbProj.y);
        yBound.y = Mathf.Max(rftProj.y, lftProj.y, rbtProj.y, lbtProj.y, rfbProj.y, lfbProj.y, rbbProj.y, lbbProj.y);

        zBound.x = Mathf.Min(rftProj.z, lftProj.z, rbtProj.z, lbtProj.z, rfbProj.z, lfbProj.z, rbbProj.z, lbbProj.z);
        zBound.y = Mathf.Max(rftProj.z, lftProj.z, rbtProj.z, lbtProj.z, rfbProj.z, lfbProj.z, rbbProj.z, lbbProj.z);
    }

    protected static float projXAxis(Vector3 point)
    {
        return projOnAxis(point, Vector3.right);
    }

    protected static float projYAxis(Vector3 point)
    {
        return projOnAxis(point, Vector3.up);
    }

    protected static float projZAxis(Vector3 point)
    {
        return projOnAxis(point, Vector3.forward);
    }

    protected static float projOnAxis(Vector3 point, Vector3 axis)
    {
        return Vector3.Dot(point, axis);
    }
}
