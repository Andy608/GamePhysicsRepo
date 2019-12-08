using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBaby))]
public abstract class CollisionHullBaby : MonoBehaviour
{
    private Octree parentOctree = null;
    public Dictionary<Vector3, Octant> parentOctants  = new Dictionary<Vector3, Octant>();

    public Color worldSpaceColor = new Color(0.9433962f, 0.6820406f, 0.0533998f, 1.0f);
    public Color localSpaceColor = new Color(1.0f, 0.0f, 0.5955915f, 1.0f);
    public Color otherSpaceColor = new Color(0.3540741f, 0.0f, 0.9433962f, 1.0f);
    public Color worldSpaceMegaBoxColor = new Color(0.3948f, 0.9983f, 0.02f, 1.0f);
    public Color otherSpaceMegaBoxColor = new Color(1.0f, 1.0f, 0.0533998f, 1.0f);
    private GameObject debugHolder = null;

    public bool IsTrigger = false;

    private GameObject vertex;
    private GameObject[] localSpaceVertices = new GameObject[8];
    private GameObject[] worldSpaceVertices = new GameObject[8];
    private GameObject[] verticesInOtherSpace = new GameObject[8];
    private GameObject[] megaBoxInOtherSpace = new GameObject[8];
    private GameObject[] worldSpaceMegaBoxVertices = new GameObject[8];
    private GameObject boxInOtherSpace, boxInLocalSpace;

    public bool inLocalSpace = false, inWorldSpace = false, inOtherSpace = false, megaInOtherSpace = false, megaInWorldSpace = false;

    public enum CollisionHullBabyType
    {
        Circle,
        AABB,
        OBB,
        Nispe
    }

    public CollisionHullBabyType Type { get; protected set; }
    protected RigidBaby rigidbaby;

    //public void UpdateParentOctants()
    //{
    //    parentOctree.RootNode.UpdateHull(this);
    //}

    //public void RefreshOctantOwners()
    //{
    //    parentOctree.RootNode.ProcessObject(this);

    //    List<OctreeNodeOld> currentParents = new List<OctreeNodeOld>();
    //    List<OctreeNodeOld> oldParents = new List<OctreeNodeOld>();

    //    foreach (OctreeNodeOld node in OwnerOctants)
    //    {
    //        if (!node.ContainsRigidBaby(this))
    //        {
    //            oldParents.Add(node);
    //        }
    //        else
    //        {
    //            currentParents.Add(node);
    //        }
    //    }

    //    OwnerOctants = currentParents;

    //    foreach (OctreeNodeOld node in oldParents)
    //    {
    //        //This object has left this octant.
    //        //If there are children octants, are there still enough objects in the octant for them to survive
    //        node.TryRemoveChildrenNodes(this);
    //    }
    //}

    protected virtual void Awake()
    {
        parentOctree = RBTestWorld.Instance.WorldOctree;

        rigidbaby = GetComponent<RigidBaby>();
        vertex = Resources.Load<GameObject>("Prefabs/Rigidbaby/Vertex");
        boxInOtherSpace = Resources.Load<GameObject>("Prefabs/Rigidbaby/Cube");

        debugHolder = new GameObject("Debug Holder");
        debugHolder.transform.parent = transform;

        boxInOtherSpace = Instantiate(Resources.Load<GameObject>("Prefabs/Rigidbaby/Cube"), debugHolder.transform);
        boxInOtherSpace.name = "In Other Space";
        otherSpaceColor.a = 0.2f;
        boxInOtherSpace.GetComponent<MeshRenderer>().material.color = otherSpaceColor;
        otherSpaceColor.a = 1.0f;

        boxInLocalSpace = Instantiate(Resources.Load<GameObject>("Prefabs/Rigidbaby/Cube"), debugHolder.transform);
        boxInLocalSpace.name = "In Local Space";
        localSpaceColor.a = 0.2f;
        boxInLocalSpace.GetComponent<MeshRenderer>().material.color = localSpaceColor;
        localSpaceColor.a = 1.0f;

        for (int i = 0; i < 8; ++i)
        {
            worldSpaceVertices[i] = Instantiate(vertex, debugHolder.transform);
            worldSpaceVertices[i].GetComponent<MeshRenderer>().material.color = worldSpaceColor;
            worldSpaceVertices[i].name = "World Space Vertex";
            worldSpaceVertices[i].tag = "World";

            localSpaceVertices[i] = Instantiate(vertex, debugHolder.transform);
            localSpaceVertices[i].GetComponent<MeshRenderer>().material.color = localSpaceColor;
            localSpaceVertices[i].name = "Local Space Vertex";
            localSpaceVertices[i].tag = "Local";

            verticesInOtherSpace[i] = Instantiate(vertex, debugHolder.transform);
            verticesInOtherSpace[i].GetComponent<MeshRenderer>().material.color = otherSpaceColor;
            verticesInOtherSpace[i].name = "Other Space Vertex";
            verticesInOtherSpace[i].tag = "Other";

            worldSpaceMegaBoxVertices[i] = Instantiate(vertex, debugHolder.transform);
            worldSpaceMegaBoxVertices[i].GetComponent<MeshRenderer>().material.color = worldSpaceMegaBoxColor;
            worldSpaceMegaBoxVertices[i].name = "World Space Mega Box Vertex";
            worldSpaceMegaBoxVertices[i].tag = "World MegaBox";

            megaBoxInOtherSpace[i] = Instantiate(vertex, debugHolder.transform);
            megaBoxInOtherSpace[i].GetComponent<MeshRenderer>().material.color = otherSpaceMegaBoxColor;
            megaBoxInOtherSpace[i].name = "Other Space Mega Box Vertex";
            megaBoxInOtherSpace[i].tag = "Other MegaBox";
        }
    }

    private void Start()
    {
        if (!IsTrigger)
        {
            RBTestWorld.Instance.WorldOctree?.RootNode?.TryInsertHull(this);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 8; ++i)
        {
            worldSpaceVertices[i].SetActive(inWorldSpace);
            localSpaceVertices[i].SetActive(inLocalSpace);
            verticesInOtherSpace[i].SetActive(inOtherSpace);
            megaBoxInOtherSpace[i].SetActive(megaInOtherSpace);
            worldSpaceMegaBoxVertices[i].SetActive(megaInWorldSpace);

            boxInLocalSpace.SetActive(inLocalSpace);
            boxInOtherSpace.SetActive(inOtherSpace);
        }

        if (!IsTrigger)
        {
            RBTestWorld.Instance.WorldOctree?.RootNode?.TryInsertHull(this);
        }
    }

    private void OnEnable()
    {
        //CollisionTesterBaby.Instance?.RegisterHull(this);
    }

    private void OnDisable()
    {
        //CollisionTesterBaby.Instance?.UnRegisterHull(this);
    }

    #region Abstact Functions
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
    #endregion

    #region Shape vs Shape
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
            if (!circle1.IsTrigger && !circle2.IsTrigger)
            {
                RigidBaby rigidBabyA = circle1.GetComponent<RigidBaby>();
                RigidBaby rigidBabyB = circle2.GetComponent<RigidBaby>();

                Vector3 normal = (rigidBabyA.GetPosition() - rigidBabyB.GetPosition()).normalized;
                float penetration = circle2.radius + circle1.radius - distance.magnitude;
                float restitution = 0.0f; // In the future we can store this in RigidBaby and then add up the two values from 1 and 2.

                RigidBabyContact contact = new RigidBabyContact(
                rigidBabyA, rigidBabyB, restitution, normal, penetration);

                c.Add(contact);
            }

            Debug.Log("Circle and Circle Collision!");
            return true;
        }

        return false;
    }

    public static bool TestCollisionAABBVsAABB(CollisionHullBabyAABB AABB1, CollisionHullBabyAABB AABB2, ref List<RigidBabyContact> c)
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
            if (!AABB1.IsTrigger && !AABB2.IsTrigger)
            {
                //Do contact stuff here.
                Debug.Log("AABB and AABB Collision!");
            }

            return true;
        }
        else
        {
            return false;
        }
    }
    protected static bool TestCollisionOBBVsOBB(CollisionHullBabyOBB OBB1, CollisionHullBabyOBB OBB2, ref List<RigidBabyContact> c)
    {
        Vector3 positionOBB1 = OBB1.transform.position;
        Vector3 dimOBB1 = OBB1.transform.lossyScale * 0.5f;
        Vector3 positionOBB2 = OBB2.transform.position;
        Vector3 dimOBB2 = OBB2.transform.lossyScale * 0.5f;

        RigidBaby OBB1RigidBaby = OBB1.GetComponent<RigidBaby>();
        RigidBaby OBB2RigidBaby = OBB2.GetComponent<RigidBaby>();
        Matrix4x4 OBB1TransformMat = OBB1RigidBaby.TransformationMat;//transformationMat = transform.localToWorldMatrix//TransformationMat* inverseInertiaTensor *worldToLocal(worldTorque, TransformationMat);
        Matrix4x4 OBB2TransformMat = OBB2RigidBaby.TransformationMat;

        Vector2 xOBB1_BOUNDS_LOCAL = new Vector2(-dimOBB1.x, dimOBB1.x);
        Vector2 yOBB1_BOUNDS_LOCAL = new Vector2(-dimOBB1.y, dimOBB1.y);
        Vector2 zOBB1_BOUNDS_LOCAL = new Vector2(-dimOBB1.z, dimOBB1.z);

        Vector2 xOBB2_BOUNDS_LOCAL = new Vector2(-dimOBB2.x, dimOBB2.x);
        Vector2 yOBB2_BOUNDS_LOCAL = new Vector2(-dimOBB2.y, dimOBB2.y);
        Vector2 zOBB2_BOUNDS_LOCAL = new Vector2(-dimOBB2.z, dimOBB2.z);

        //Local OBB1 debug vertices.
        OBB1.localSpaceVertices[0].transform.position = new Vector3(dimOBB1.x, dimOBB1.y, dimOBB1.z);
        OBB1.localSpaceVertices[1].transform.position = new Vector3(dimOBB1.x, dimOBB1.y, -dimOBB1.z);
        OBB1.localSpaceVertices[2].transform.position = new Vector3(dimOBB1.x, -dimOBB1.y, dimOBB1.z);
        OBB1.localSpaceVertices[3].transform.position = new Vector3(dimOBB1.x, -dimOBB1.y, -dimOBB1.z);
        OBB1.localSpaceVertices[4].transform.position = new Vector3(-dimOBB1.x, dimOBB1.y, dimOBB1.z);
        OBB1.localSpaceVertices[5].transform.position = new Vector3(-dimOBB1.x, dimOBB1.y, -dimOBB1.z);
        OBB1.localSpaceVertices[6].transform.position = new Vector3(-dimOBB1.x, -dimOBB1.y, dimOBB1.z);
        OBB1.localSpaceVertices[7].transform.position = new Vector3(-dimOBB1.x, -dimOBB1.y, -dimOBB1.z);

        //Local OBB2 debug vertices.
        OBB2.localSpaceVertices[0].transform.position = new Vector3(dimOBB2.x, dimOBB2.y, dimOBB2.z);
        OBB2.localSpaceVertices[1].transform.position = new Vector3(dimOBB2.x, dimOBB2.y, -dimOBB2.z);
        OBB2.localSpaceVertices[2].transform.position = new Vector3(dimOBB2.x, -dimOBB2.y, dimOBB2.z);
        OBB2.localSpaceVertices[3].transform.position = new Vector3(dimOBB2.x, -dimOBB2.y, -dimOBB2.z);
        OBB2.localSpaceVertices[4].transform.position = new Vector3(-dimOBB2.x, dimOBB2.y, dimOBB2.z);
        OBB2.localSpaceVertices[5].transform.position = new Vector3(-dimOBB2.x, dimOBB2.y, -dimOBB2.z);
        OBB2.localSpaceVertices[6].transform.position = new Vector3(-dimOBB2.x, -dimOBB2.y, dimOBB2.z);
        OBB2.localSpaceVertices[7].transform.position = new Vector3(-dimOBB2.x, -dimOBB2.y, -dimOBB2.z);

        // 2. Find world corner points for OBB1
        //Right/Left Top/Bottom Front/Back -> rtf / lbb
        Vector3 rtf_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(dimOBB1.x, dimOBB1.y, dimOBB1.z, 1.0f);
        Vector3 ltf_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(dimOBB1.x, dimOBB1.y, -dimOBB1.z, 1.0f);
        Vector3 rtb_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(dimOBB1.x, -dimOBB1.y, dimOBB1.z, 1.0f);
        Vector3 ltb_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(dimOBB1.x, -dimOBB1.y, -dimOBB1.z, 1.0f);
        Vector3 rbf_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(-dimOBB1.x, dimOBB1.y, dimOBB1.z, 1.0f);
        Vector3 lbf_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(-dimOBB1.x, dimOBB1.y, -dimOBB1.z, 1.0f);
        Vector3 rbb_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(-dimOBB1.x, -dimOBB1.y, dimOBB1.z, 1.0f);
        Vector3 lbb_OBB1_IN_WORLD_SPACE = OBB1TransformMat * new Vector4(-dimOBB1.x, -dimOBB1.y, -dimOBB1.z, 1.0f);
        OBB1.worldSpaceVertices[0].transform.position = rtf_OBB1_IN_WORLD_SPACE;
        OBB1.worldSpaceVertices[1].transform.position = ltf_OBB1_IN_WORLD_SPACE;
        OBB1.worldSpaceVertices[2].transform.position = rtb_OBB1_IN_WORLD_SPACE;
        OBB1.worldSpaceVertices[3].transform.position = ltb_OBB1_IN_WORLD_SPACE;
        OBB1.worldSpaceVertices[4].transform.position = rbf_OBB1_IN_WORLD_SPACE;
        OBB1.worldSpaceVertices[5].transform.position = lbf_OBB1_IN_WORLD_SPACE;
        OBB1.worldSpaceVertices[6].transform.position = rbb_OBB1_IN_WORLD_SPACE;
        OBB1.worldSpaceVertices[7].transform.position = lbb_OBB1_IN_WORLD_SPACE;

        // 2. Find world corner points for OBB2
        Vector3 rtf_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(dimOBB2.x, dimOBB2.y, dimOBB2.z, 1.0f);
        Vector3 ltf_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(dimOBB2.x, dimOBB2.y, -dimOBB2.z, 1.0f);
        Vector3 rtb_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(dimOBB2.x, -dimOBB2.y, dimOBB2.z, 1.0f);
        Vector3 ltb_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(dimOBB2.x, -dimOBB2.y, -dimOBB2.z, 1.0f);
        Vector3 rbf_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(-dimOBB2.x, dimOBB2.y, dimOBB2.z, 1.0f);
        Vector3 lbf_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(-dimOBB2.x, dimOBB2.y, -dimOBB2.z, 1.0f);
        Vector3 rbb_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(-dimOBB2.x, -dimOBB2.y, dimOBB2.z, 1.0f);
        Vector3 lbb_OBB2_IN_WORLD_SPACE = OBB2TransformMat * new Vector4(-dimOBB2.x, -dimOBB2.y, -dimOBB2.z, 1.0f);
        OBB2.worldSpaceVertices[0].transform.position = rtf_OBB2_IN_WORLD_SPACE;
        OBB2.worldSpaceVertices[1].transform.position = ltf_OBB2_IN_WORLD_SPACE;
        OBB2.worldSpaceVertices[2].transform.position = rtb_OBB2_IN_WORLD_SPACE;
        OBB2.worldSpaceVertices[3].transform.position = ltb_OBB2_IN_WORLD_SPACE;
        OBB2.worldSpaceVertices[4].transform.position = rbf_OBB2_IN_WORLD_SPACE;
        OBB2.worldSpaceVertices[5].transform.position = lbf_OBB2_IN_WORLD_SPACE;
        OBB2.worldSpaceVertices[6].transform.position = rbb_OBB2_IN_WORLD_SPACE;
        OBB2.worldSpaceVertices[7].transform.position = lbb_OBB2_IN_WORLD_SPACE;

        //3. Find OBB1Space points for OBB2

        Vector3 rtf_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(rtf_OBB2_IN_WORLD_SPACE);
        Vector3 ltf_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(ltf_OBB2_IN_WORLD_SPACE);
        Vector3 rtb_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(rtb_OBB2_IN_WORLD_SPACE);
        Vector3 ltb_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(ltb_OBB2_IN_WORLD_SPACE);
        Vector3 rbf_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(rbf_OBB2_IN_WORLD_SPACE);
        Vector3 lbf_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(lbf_OBB2_IN_WORLD_SPACE);
        Vector3 rbb_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(rbb_OBB2_IN_WORLD_SPACE);
        Vector3 lbb_OBB2_IN_OBB1_SPACE = OBB1TransformMat.inverse * ToVec4(lbb_OBB2_IN_WORLD_SPACE);
        OBB2.verticesInOtherSpace[0].transform.position = rtf_OBB2_IN_OBB1_SPACE;
        OBB2.verticesInOtherSpace[1].transform.position = ltf_OBB2_IN_OBB1_SPACE;
        OBB2.verticesInOtherSpace[2].transform.position = rtb_OBB2_IN_OBB1_SPACE;
        OBB2.verticesInOtherSpace[3].transform.position = ltb_OBB2_IN_OBB1_SPACE;
        OBB2.verticesInOtherSpace[4].transform.position = rbf_OBB2_IN_OBB1_SPACE;
        OBB2.verticesInOtherSpace[5].transform.position = lbf_OBB2_IN_OBB1_SPACE;
        OBB2.verticesInOtherSpace[6].transform.position = rbb_OBB2_IN_OBB1_SPACE;
        OBB2.verticesInOtherSpace[7].transform.position = lbb_OBB2_IN_OBB1_SPACE;

        // 7. Convert OBB1 into local OBB2 space.
        Vector3 rtf_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(rtf_OBB1_IN_WORLD_SPACE);
        Vector3 ltf_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(ltf_OBB1_IN_WORLD_SPACE);
        Vector3 rtb_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(rtb_OBB1_IN_WORLD_SPACE);
        Vector3 ltb_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(ltb_OBB1_IN_WORLD_SPACE);
        Vector3 rbf_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(rbf_OBB1_IN_WORLD_SPACE);
        Vector3 lbf_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(lbf_OBB1_IN_WORLD_SPACE);
        Vector3 rbb_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(rbb_OBB1_IN_WORLD_SPACE);
        Vector3 lbb_OBB1_IN_OBB2_SPACE = OBB2TransformMat.inverse * ToVec4(lbb_OBB1_IN_WORLD_SPACE);
        OBB1.verticesInOtherSpace[0].transform.position = rtf_OBB1_IN_OBB2_SPACE;
        OBB1.verticesInOtherSpace[1].transform.position = ltf_OBB1_IN_OBB2_SPACE;
        OBB1.verticesInOtherSpace[2].transform.position = rtb_OBB1_IN_OBB2_SPACE;
        OBB1.verticesInOtherSpace[3].transform.position = ltb_OBB1_IN_OBB2_SPACE;
        OBB1.verticesInOtherSpace[4].transform.position = rbf_OBB1_IN_OBB2_SPACE;
        OBB1.verticesInOtherSpace[5].transform.position = lbf_OBB1_IN_OBB2_SPACE;
        OBB1.verticesInOtherSpace[6].transform.position = rbb_OBB1_IN_OBB2_SPACE;
        OBB1.verticesInOtherSpace[7].transform.position = lbb_OBB1_IN_OBB2_SPACE;

        OBB1.boxInLocalSpace.transform.position = Vector3.zero;
        OBB1.boxInLocalSpace.transform.rotation = Quaternion.identity;
        OBB2.boxInLocalSpace.transform.position = Vector3.zero;
        OBB2.boxInLocalSpace.transform.rotation = Quaternion.identity;

        OBB1.boxInOtherSpace.transform.position = OBB2TransformMat.inverse * ToVec4(OBB1.transform.position);
        OBB1.boxInOtherSpace.transform.rotation = QuatBaby.MatrixToQuatBaby(OBB2TransformMat.inverse * OBB1RigidBaby.Rotation.ToMatrix()).ToUnityQuaternion();

        OBB2.boxInOtherSpace.transform.position = OBB1TransformMat.inverse * ToVec4(OBB2.transform.position);
        OBB2.boxInOtherSpace.transform.rotation = QuatBaby.MatrixToQuatBaby(OBB1TransformMat.inverse * OBB2RigidBaby.Rotation.ToMatrix()).ToUnityQuaternion();

        // 4. Calculate OBB2's Mega Bounds in World Space
        Vector2 xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE;

        CalculateAABB(
            rtf_OBB2_IN_WORLD_SPACE,
            ltf_OBB2_IN_WORLD_SPACE,
            rtb_OBB2_IN_WORLD_SPACE,
            ltb_OBB2_IN_WORLD_SPACE,
            rbf_OBB2_IN_WORLD_SPACE,
            lbf_OBB2_IN_WORLD_SPACE,
            rbb_OBB2_IN_WORLD_SPACE,
            lbb_OBB2_IN_WORLD_SPACE,
            out xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE,
            out yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE,
            out zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE
        );

        OBB2.worldSpaceMegaBoxVertices[0].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB2.worldSpaceMegaBoxVertices[1].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB2.worldSpaceMegaBoxVertices[2].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB2.worldSpaceMegaBoxVertices[3].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB2.worldSpaceMegaBoxVertices[4].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB2.worldSpaceMegaBoxVertices[5].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB2.worldSpaceMegaBoxVertices[6].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB2.worldSpaceMegaBoxVertices[7].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);

        // 4. Calculate OBB2's Mega Bounds in OBB1 Space
        Vector2 xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE;
        Vector2 yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE;
        Vector2 zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE;

        CalculateAABB(
            rtf_OBB2_IN_OBB1_SPACE,
            ltf_OBB2_IN_OBB1_SPACE,
            rtb_OBB2_IN_OBB1_SPACE,
            ltb_OBB2_IN_OBB1_SPACE,
            rbf_OBB2_IN_OBB1_SPACE,
            lbf_OBB2_IN_OBB1_SPACE,
            rbb_OBB2_IN_OBB1_SPACE,
            lbb_OBB2_IN_OBB1_SPACE,
            out xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE,
            out yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE,
            out zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE
        );

        OBB2.megaBoxInOtherSpace[0].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x);
        OBB2.megaBoxInOtherSpace[1].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y);
        OBB2.megaBoxInOtherSpace[2].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x);
        OBB2.megaBoxInOtherSpace[3].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y);
        OBB2.megaBoxInOtherSpace[4].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x);
        OBB2.megaBoxInOtherSpace[5].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y);
        OBB2.megaBoxInOtherSpace[6].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.x);
        OBB2.megaBoxInOtherSpace[7].transform.position = new Vector3(xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y, zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE.y);

        // 5. Do OBB1 collision test with - OBB1 and OBB2 in OBB1 Space
        bool isTransformedOBB2inOBB1 =
            DOAABBCollisionTest(
                xOBB1_BOUNDS_LOCAL,
                yOBB1_BOUNDS_LOCAL,
                zOBB1_BOUNDS_LOCAL,
                xOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE,
                yOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE,
                zOBB2_MIN_MAX_BOUNDS_IN_OBB1_SPACE
        );

        Vector2 xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE;

        CalculateAABB(
            rtf_OBB1_IN_WORLD_SPACE,
            ltf_OBB1_IN_WORLD_SPACE,
            rtb_OBB1_IN_WORLD_SPACE,
            ltb_OBB1_IN_WORLD_SPACE,
            rbf_OBB1_IN_WORLD_SPACE,
            lbf_OBB1_IN_WORLD_SPACE,
            rbb_OBB1_IN_WORLD_SPACE,
            lbb_OBB1_IN_WORLD_SPACE,
            out xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE,
            out yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE,
            out zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE
        );

        OBB1.worldSpaceMegaBoxVertices[0].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB1.worldSpaceMegaBoxVertices[1].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB1.worldSpaceMegaBoxVertices[2].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB1.worldSpaceMegaBoxVertices[3].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB1.worldSpaceMegaBoxVertices[4].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB1.worldSpaceMegaBoxVertices[5].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB1.worldSpaceMegaBoxVertices[6].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB1.worldSpaceMegaBoxVertices[7].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);

        // 8. Calculate OBB1's Mega Bounds in OBB2 Space
        Vector2 xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE;
        Vector2 yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE;
        Vector2 zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE;

        CalculateAABB(
            rtf_OBB1_IN_OBB2_SPACE,
            ltf_OBB1_IN_OBB2_SPACE,
            rtb_OBB1_IN_OBB2_SPACE,
            ltb_OBB1_IN_OBB2_SPACE,
            rbf_OBB1_IN_OBB2_SPACE,
            lbf_OBB1_IN_OBB2_SPACE,
            rbb_OBB1_IN_OBB2_SPACE,
            lbb_OBB1_IN_OBB2_SPACE,
            out xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE,
            out yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE,
            out zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE
        );

        OBB1.megaBoxInOtherSpace[0].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x);
        OBB1.megaBoxInOtherSpace[1].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y);
        OBB1.megaBoxInOtherSpace[2].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x);
        OBB1.megaBoxInOtherSpace[3].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y);
        OBB1.megaBoxInOtherSpace[4].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x);
        OBB1.megaBoxInOtherSpace[5].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y);
        OBB1.megaBoxInOtherSpace[6].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.x);
        OBB1.megaBoxInOtherSpace[7].transform.position = new Vector3(xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y, zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE.y);

        //6.If the test comes back neg, then return false because there is no way the boxes are colliding
        if (!isTransformedOBB2inOBB1)
        {
            return false;
        }

        // 9. Do OBB1 collision test of OBB2's local OBB1 vs transformed OBB1's OBB1
        bool isTransformedOBB1inOBB2 =
            DOAABBCollisionTest(
                xOBB2_BOUNDS_LOCAL,
                yOBB2_BOUNDS_LOCAL,
                zOBB2_BOUNDS_LOCAL,
                xOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE,
                yOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE,
                zOBB1_MIN_MAX_BOUNDS_IN_OBB2_SPACE
            );

        if (isTransformedOBB1inOBB2)
        {
            if (!OBB1.IsTrigger && !OBB2.IsTrigger)
            {
                RigidBaby rigidBabyA = OBB1.GetComponent<RigidBaby>();
                RigidBaby rigidBabyB = OBB2.GetComponent<RigidBaby>();

                Vector3 normal = (rigidBabyA.GetPosition() - rigidBabyB.GetPosition()).normalized;

                //float penetration = circle2.radius + circle1.radius - distance.magnitude;
                float toCenter = Vector3.Magnitude(rigidBabyA.GetPosition() - rigidBabyB.GetPosition());

                //Vertex Face Penetration check first
                float penetration = 0.0f;

                float restitution = 0.0f; // In the future we can store this in RigidBaby and then add up the two values from 1 and 2.

                RigidBabyContact contact = new RigidBabyContact(
                    rigidBabyA, rigidBabyB, restitution, normal, penetration);

                c.Add(contact);

                Debug.Log("OBB1 and OBB2 Collision!");
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private float PenOnAxis(float halfLengthOfBox1, float halfLengthOfBox2, float distBetweenBoxes)
    {
        //1. Calculate Half length of each box along a specific axis
        //2. Calculate the distance between the two boxes along said axis
        //3. Return the overlap

        float yDist1 = halfLengthOfBox1;
        float yDist2 = halfLengthOfBox2;

        float distance1 = Mathf.Abs(distBetweenBoxes * yDist1);
        float distance2 = Mathf.Abs(distBetweenBoxes * yDist2);

        return 0;
    }
    #endregion

    #region Shape vs Other Shape
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
            if (!AABB.IsTrigger && !circle.IsTrigger)
            {
                //Do contact stuff here.
                RigidBaby rigidBabyA = circle.GetComponent<RigidBaby>();
                RigidBaby rigidBabyB = AABB.GetComponent<RigidBaby>();


                Vector3 normal = (rigidBabyA.GetPosition() - rigidBabyB.GetPosition()).normalized;
                float penetration = circle.radius - distance.magnitude;
                float restitution = 0.0f; // In the future we can store this in RigidBaby and then add up the two values from 1 and 2.

                RigidBabyContact contact = new RigidBabyContact(
                    rigidBabyA, rigidBabyB, restitution, normal, penetration);

                c.Add(contact);
			    Debug.Log("AABB and Circle Collision!");
            }

			return true;
        }

        return false;
    }
    protected static bool TestCollisionAABBVsOBB(CollisionHullBabyAABB AABB, CollisionHullBabyOBB OBB, ref List<RigidBabyContact> c)
    {
        Vector3 positionAABB        = AABB.transform.position;
        Vector3 dimAABB             = AABB.transform.lossyScale * 0.5f;
        Vector3 positionOBB         = OBB.transform.position;
        Vector3 dimOBB              = OBB.transform.lossyScale * 0.5f;

        RigidBaby AABBRigidBaby     = AABB.GetComponent<RigidBaby>();
        RigidBaby OBBRigidBaby      = OBB.GetComponent<RigidBaby>();
        Matrix4x4 AABBTransformMat  = AABBRigidBaby.TransformationMat;//transformationMat = transform.localToWorldMatrix//TransformationMat* inverseInertiaTensor *worldToLocal(worldTorque, TransformationMat);
        Matrix4x4 OBBTransformMat   = OBBRigidBaby.TransformationMat;

        Vector2 xAABB_BOUNDS_LOCAL       = new Vector2(-dimAABB.x, dimAABB.x);
        Vector2 yAABB_BOUNDS_LOCAL       = new Vector2(-dimAABB.y, dimAABB.y);
        Vector2 zAABB_BOUNDS_LOCAL       = new Vector2(-dimAABB.z, dimAABB.z);

        Vector2 xOBB_BOUNDS_LOCAL        = new Vector2(-dimOBB.x, dimOBB.x);
        Vector2 yOBB_BOUNDS_LOCAL        = new Vector2(-dimOBB.y, dimOBB.y);
        Vector2 zOBB_BOUNDS_LOCAL        = new Vector2(-dimOBB.z, dimOBB.z);

        //Local AABB debug vertices.
        AABB.localSpaceVertices[0].transform.position   = new Vector3( dimAABB.x,  dimAABB.y,  dimAABB.z);
        AABB.localSpaceVertices[1].transform.position   = new Vector3( dimAABB.x,  dimAABB.y, -dimAABB.z);
        AABB.localSpaceVertices[2].transform.position   = new Vector3( dimAABB.x, -dimAABB.y,  dimAABB.z);
        AABB.localSpaceVertices[3].transform.position   = new Vector3( dimAABB.x, -dimAABB.y, -dimAABB.z);
        AABB.localSpaceVertices[4].transform.position   = new Vector3(-dimAABB.x,  dimAABB.y,  dimAABB.z);
        AABB.localSpaceVertices[5].transform.position   = new Vector3(-dimAABB.x,  dimAABB.y, -dimAABB.z);
        AABB.localSpaceVertices[6].transform.position   = new Vector3(-dimAABB.x, -dimAABB.y,  dimAABB.z);
        AABB.localSpaceVertices[7].transform.position   = new Vector3(-dimAABB.x, -dimAABB.y, -dimAABB.z);

        //Local OBB debug vertices.
        OBB.localSpaceVertices[0].transform.position    = new Vector3( dimOBB.x,  dimOBB.y,  dimOBB.z);
        OBB.localSpaceVertices[1].transform.position    = new Vector3( dimOBB.x,  dimOBB.y, -dimOBB.z);
        OBB.localSpaceVertices[2].transform.position    = new Vector3( dimOBB.x, -dimOBB.y,  dimOBB.z);
        OBB.localSpaceVertices[3].transform.position    = new Vector3( dimOBB.x, -dimOBB.y, -dimOBB.z);
        OBB.localSpaceVertices[4].transform.position    = new Vector3(-dimOBB.x,  dimOBB.y,  dimOBB.z);
        OBB.localSpaceVertices[5].transform.position    = new Vector3(-dimOBB.x,  dimOBB.y, -dimOBB.z);
        OBB.localSpaceVertices[6].transform.position    = new Vector3(-dimOBB.x, -dimOBB.y,  dimOBB.z);
        OBB.localSpaceVertices[7].transform.position    = new Vector3(-dimOBB.x, -dimOBB.y, -dimOBB.z);

        // 2. Find world corner points for AABB
        //Right/Left Top/Bottom Front/Back -> rtf / lbb
        Vector3 rtf_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4( dimAABB.x,  dimAABB.y,  dimAABB.z, 1.0f);
        Vector3 ltf_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4( dimAABB.x,  dimAABB.y, -dimAABB.z, 1.0f);
        Vector3 rtb_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4( dimAABB.x, -dimAABB.y,  dimAABB.z, 1.0f);
        Vector3 ltb_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4( dimAABB.x, -dimAABB.y, -dimAABB.z, 1.0f);
        Vector3 rbf_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4(-dimAABB.x,  dimAABB.y,  dimAABB.z, 1.0f);
        Vector3 lbf_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4(-dimAABB.x,  dimAABB.y, -dimAABB.z, 1.0f);
        Vector3 rbb_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4(-dimAABB.x, -dimAABB.y,  dimAABB.z, 1.0f);
        Vector3 lbb_AABB_IN_WORLD_SPACE                 = AABBTransformMat * new Vector4(-dimAABB.x, -dimAABB.y, -dimAABB.z, 1.0f);
        AABB.worldSpaceVertices[0].transform.position   = rtf_AABB_IN_WORLD_SPACE;
        AABB.worldSpaceVertices[1].transform.position   = ltf_AABB_IN_WORLD_SPACE;
        AABB.worldSpaceVertices[2].transform.position   = rtb_AABB_IN_WORLD_SPACE;
        AABB.worldSpaceVertices[3].transform.position   = ltb_AABB_IN_WORLD_SPACE;
        AABB.worldSpaceVertices[4].transform.position   = rbf_AABB_IN_WORLD_SPACE;
        AABB.worldSpaceVertices[5].transform.position   = lbf_AABB_IN_WORLD_SPACE;
        AABB.worldSpaceVertices[6].transform.position   = rbb_AABB_IN_WORLD_SPACE;
        AABB.worldSpaceVertices[7].transform.position   = lbb_AABB_IN_WORLD_SPACE;

        // 2. Find world corner points for OBB
        Vector3 rtf_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4( dimOBB.x,  dimOBB.y,  dimOBB.z, 1.0f);
        Vector3 ltf_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4( dimOBB.x,  dimOBB.y, -dimOBB.z, 1.0f);
        Vector3 rtb_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4( dimOBB.x, -dimOBB.y,  dimOBB.z, 1.0f);
        Vector3 ltb_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4( dimOBB.x, -dimOBB.y, -dimOBB.z, 1.0f);
        Vector3 rbf_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4(-dimOBB.x,  dimOBB.y,  dimOBB.z, 1.0f);
        Vector3 lbf_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4(-dimOBB.x,  dimOBB.y, -dimOBB.z, 1.0f);
        Vector3 rbb_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4(-dimOBB.x, -dimOBB.y,  dimOBB.z, 1.0f);
        Vector3 lbb_OBB_IN_WORLD_SPACE                  = OBBTransformMat * new Vector4(-dimOBB.x, -dimOBB.y, -dimOBB.z, 1.0f);
        OBB.worldSpaceVertices[0].transform.position    = rtf_OBB_IN_WORLD_SPACE;
        OBB.worldSpaceVertices[1].transform.position    = ltf_OBB_IN_WORLD_SPACE;
        OBB.worldSpaceVertices[2].transform.position    = rtb_OBB_IN_WORLD_SPACE;
        OBB.worldSpaceVertices[3].transform.position    = ltb_OBB_IN_WORLD_SPACE;
        OBB.worldSpaceVertices[4].transform.position    = rbf_OBB_IN_WORLD_SPACE;
        OBB.worldSpaceVertices[5].transform.position    = lbf_OBB_IN_WORLD_SPACE;
        OBB.worldSpaceVertices[6].transform.position    = rbb_OBB_IN_WORLD_SPACE;
        OBB.worldSpaceVertices[7].transform.position    = lbb_OBB_IN_WORLD_SPACE;

        //3. Find AABBSpace points for OBB

        Vector3 rtf_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(rtf_OBB_IN_WORLD_SPACE);
        Vector3 ltf_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(ltf_OBB_IN_WORLD_SPACE);
        Vector3 rtb_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(rtb_OBB_IN_WORLD_SPACE);
        Vector3 ltb_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(ltb_OBB_IN_WORLD_SPACE);
        Vector3 rbf_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(rbf_OBB_IN_WORLD_SPACE);
        Vector3 lbf_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(lbf_OBB_IN_WORLD_SPACE);
        Vector3 rbb_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(rbb_OBB_IN_WORLD_SPACE);
        Vector3 lbb_OBB_IN_AABB_SPACE                   = AABBTransformMat.inverse * ToVec4(lbb_OBB_IN_WORLD_SPACE);
        OBB.verticesInOtherSpace[0].transform.position   = rtf_OBB_IN_AABB_SPACE;
        OBB.verticesInOtherSpace[1].transform.position   = ltf_OBB_IN_AABB_SPACE;
        OBB.verticesInOtherSpace[2].transform.position   = rtb_OBB_IN_AABB_SPACE;
        OBB.verticesInOtherSpace[3].transform.position   = ltb_OBB_IN_AABB_SPACE;
        OBB.verticesInOtherSpace[4].transform.position   = rbf_OBB_IN_AABB_SPACE;
        OBB.verticesInOtherSpace[5].transform.position   = lbf_OBB_IN_AABB_SPACE;
        OBB.verticesInOtherSpace[6].transform.position   = rbb_OBB_IN_AABB_SPACE;
        OBB.verticesInOtherSpace[7].transform.position   = lbb_OBB_IN_AABB_SPACE;

        // 7. Convert AABB into local OBB space.
        Vector3 rtf_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(rtf_AABB_IN_WORLD_SPACE);
        Vector3 ltf_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(ltf_AABB_IN_WORLD_SPACE);
        Vector3 rtb_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(rtb_AABB_IN_WORLD_SPACE);
        Vector3 ltb_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(ltb_AABB_IN_WORLD_SPACE);
        Vector3 rbf_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(rbf_AABB_IN_WORLD_SPACE);
        Vector3 lbf_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(lbf_AABB_IN_WORLD_SPACE);
        Vector3 rbb_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(rbb_AABB_IN_WORLD_SPACE);
        Vector3 lbb_AABB_IN_OBB_SPACE                       = OBBTransformMat.inverse * ToVec4(lbb_AABB_IN_WORLD_SPACE);
        AABB.verticesInOtherSpace[0].transform.position     = rtf_AABB_IN_OBB_SPACE; 
        AABB.verticesInOtherSpace[1].transform.position     = ltf_AABB_IN_OBB_SPACE;
        AABB.verticesInOtherSpace[2].transform.position     = rtb_AABB_IN_OBB_SPACE;
        AABB.verticesInOtherSpace[3].transform.position     = ltb_AABB_IN_OBB_SPACE;
        AABB.verticesInOtherSpace[4].transform.position     = rbf_AABB_IN_OBB_SPACE;
        AABB.verticesInOtherSpace[5].transform.position     = lbf_AABB_IN_OBB_SPACE;
        AABB.verticesInOtherSpace[6].transform.position     = rbb_AABB_IN_OBB_SPACE;
        AABB.verticesInOtherSpace[7].transform.position     = lbb_AABB_IN_OBB_SPACE;

        AABB.boxInLocalSpace.transform.position = Vector3.zero;
        AABB.boxInLocalSpace.transform.rotation = Quaternion.identity;
        OBB.boxInLocalSpace.transform.position = Vector3.zero;
        OBB.boxInLocalSpace.transform.rotation = Quaternion.identity;

        AABB.boxInOtherSpace.transform.position = OBBTransformMat.inverse * ToVec4(AABB.transform.position);
        AABB.boxInOtherSpace.transform.rotation = QuatBaby.MatrixToQuatBaby(OBBTransformMat.inverse * AABBRigidBaby.Rotation.ToMatrix()).ToUnityQuaternion();

        OBB.boxInOtherSpace.transform.position = AABBTransformMat.inverse * ToVec4(OBB.transform.position);
        OBB.boxInOtherSpace.transform.rotation = QuatBaby.MatrixToQuatBaby(AABBTransformMat.inverse * OBBRigidBaby.Rotation.ToMatrix()).ToUnityQuaternion();

        // 4. Calculate OBB's Mega Bounds in World Space
        Vector2 xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE;

        CalculateAABB(
            rtf_OBB_IN_WORLD_SPACE,
            ltf_OBB_IN_WORLD_SPACE,
            rtb_OBB_IN_WORLD_SPACE,
            ltb_OBB_IN_WORLD_SPACE,
            rbf_OBB_IN_WORLD_SPACE, 
            lbf_OBB_IN_WORLD_SPACE,
            rbb_OBB_IN_WORLD_SPACE,
            lbb_OBB_IN_WORLD_SPACE,
            out xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE, 
            out yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE, 
            out zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE
        );

        OBB.worldSpaceMegaBoxVertices[0].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB.worldSpaceMegaBoxVertices[1].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB.worldSpaceMegaBoxVertices[2].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB.worldSpaceMegaBoxVertices[3].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB.worldSpaceMegaBoxVertices[4].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB.worldSpaceMegaBoxVertices[5].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        OBB.worldSpaceMegaBoxVertices[6].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        OBB.worldSpaceMegaBoxVertices[7].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);

        // 4. Calculate OBB's Mega Bounds in AABB Space
        Vector2 xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE;
        Vector2 yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE;
        Vector2 zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE;

        CalculateAABB(
            rtf_OBB_IN_AABB_SPACE,
            ltf_OBB_IN_AABB_SPACE,
            rtb_OBB_IN_AABB_SPACE,
            ltb_OBB_IN_AABB_SPACE,
            rbf_OBB_IN_AABB_SPACE,
            lbf_OBB_IN_AABB_SPACE,
            rbb_OBB_IN_AABB_SPACE,
            lbb_OBB_IN_AABB_SPACE,
            out xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE, 
            out yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE, 
            out zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE
        );

        OBB.megaBoxInOtherSpace[0].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x);
        OBB.megaBoxInOtherSpace[1].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y);
        OBB.megaBoxInOtherSpace[2].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x);
        OBB.megaBoxInOtherSpace[3].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y);
        OBB.megaBoxInOtherSpace[4].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x);
        OBB.megaBoxInOtherSpace[5].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y);
        OBB.megaBoxInOtherSpace[6].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.x);
        OBB.megaBoxInOtherSpace[7].transform.position = new Vector3(xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y, zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE.y);

        // 5. Do AABB collision test with - AABB and OBB in AABB Space
        bool isTransformedOBBinAABB = 
            DOAABBCollisionTest(
                xAABB_BOUNDS_LOCAL,
                yAABB_BOUNDS_LOCAL,
                zAABB_BOUNDS_LOCAL, 
                xOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE,
                yOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE,
                zOBB_MIN_MAX_BOUNDS_IN_AABB_SPACE
        );

        Vector2 xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE;
        Vector2 zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE;

        CalculateAABB(
            rtf_AABB_IN_WORLD_SPACE,
            ltf_AABB_IN_WORLD_SPACE,
            rtb_AABB_IN_WORLD_SPACE,
            ltb_AABB_IN_WORLD_SPACE,
            rbf_AABB_IN_WORLD_SPACE,
            lbf_AABB_IN_WORLD_SPACE, 
            rbb_AABB_IN_WORLD_SPACE,
            lbb_AABB_IN_WORLD_SPACE,
            out xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE, 
            out yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE, 
            out zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE
        );

        AABB.worldSpaceMegaBoxVertices[0].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        AABB.worldSpaceMegaBoxVertices[1].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        AABB.worldSpaceMegaBoxVertices[2].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        AABB.worldSpaceMegaBoxVertices[3].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        AABB.worldSpaceMegaBoxVertices[4].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        AABB.worldSpaceMegaBoxVertices[5].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);
        AABB.worldSpaceMegaBoxVertices[6].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.x);
        AABB.worldSpaceMegaBoxVertices[7].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_WORLD_SPACE.y);

        // 8. Calculate AABB's Mega Bounds in OBB Space
        Vector2 xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE;
        Vector2 yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE;
        Vector2 zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE;

        CalculateAABB(
            rtf_AABB_IN_OBB_SPACE,
            ltf_AABB_IN_OBB_SPACE,
            rtb_AABB_IN_OBB_SPACE,
            ltb_AABB_IN_OBB_SPACE,
            rbf_AABB_IN_OBB_SPACE, 
            lbf_AABB_IN_OBB_SPACE,
            rbb_AABB_IN_OBB_SPACE,
            lbb_AABB_IN_OBB_SPACE,
            out xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE, 
            out yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE, 
            out zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE
        );

        AABB.megaBoxInOtherSpace[0].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x);
        AABB.megaBoxInOtherSpace[1].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y);
        AABB.megaBoxInOtherSpace[2].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x);
        AABB.megaBoxInOtherSpace[3].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y);
        AABB.megaBoxInOtherSpace[4].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x);
        AABB.megaBoxInOtherSpace[5].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y);
        AABB.megaBoxInOtherSpace[6].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.x);
        AABB.megaBoxInOtherSpace[7].transform.position = new Vector3(xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y, zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE.y);

        //6.If the test comes back neg, then return false because there is no way the boxes are colliding
        if (!isTransformedOBBinAABB)
        {
            return false;
        }

        // 9. Do AABB collision test of OBB's local AABB vs transformed AABB's AABB
        bool isTransformedAABBinOBB = 
            DOAABBCollisionTest(
                xOBB_BOUNDS_LOCAL,
                yOBB_BOUNDS_LOCAL,
                zOBB_BOUNDS_LOCAL,
                xAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE,
                yAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE,
                zAABB_MIN_MAX_BOUNDS_IN_OBB_SPACE
            );

		if (isTransformedAABBinOBB)
		{
            if (!AABB.IsTrigger && !OBB.IsTrigger)
            {
                //Do collision stuff here
                Debug.Log("AABB and OBB Collision!");
            }

			return true;
        }
        else
        {
            return false;
        }
    }
    protected static bool TestCollisionCircleVsOBB(CollisionHullBabyCircle Circle, CollisionHullBabyOBB OBB, ref List<RigidBabyContact> c)
    {
        //same as above but first...
        //multiply circle center by box inverse matrix 

        // 1. get circle center
        Vector4 circleCenter = new Vector4(Circle.transform.position.x, Circle.transform.position.y, Circle.transform.position.z, 1.0f);

        // 2. get box x bounds 
        float xMaxBound = OBB.transform.localScale.x * 0.5f;
        float xMinBound = -OBB.transform.localScale.x * 0.5f;
        // 3. get box y bounds
        float yMaxBound = OBB.transform.localScale.y * 0.5f;
        float yMinBound = -OBB.transform.localScale.y * 0.5f;

        // 3. get box z bounds
        float zMaxBound = OBB.transform.localScale.z * 0.5f;
        float zMinBound = -OBB.transform.localScale.z * 0.5f;


        // 4. multiply circle center world position by box world to local matrix
        circleCenter = OBB.transform.InverseTransformPoint(circleCenter);

        // 5. clamp circle center on box x bound
        float circleOnX = circleCenter.x;
        // 6. clamp circle center on box y bound
        float circleOnY = circleCenter.y;
        // 6. clamp circle center on box y bound
        float circleOnz = circleCenter.z;

        //ballBoxLocal.position = circleCenter;
        //boxLocal.position = other.transform.InverseTransformPoint(other.transform.position);

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

        if (circleCenter.z > zMaxBound)
        {
            circleOnz = zMaxBound;
        }
        else if (circleCenter.z < zMinBound)
        {
            circleOnz = zMinBound;
        }

        // 7. use clamped point as closest point of box
        Vector3 closestPoint = new Vector3(circleOnX, circleOnY, circleOnz);

        Vector3 distance = closestPoint - new Vector3(circleCenter.x, circleCenter.y, circleCenter.z);
        float distSqr = Vector3.Dot(distance, distance);

        // 8. check if closest point of box is within the circle
        if (distSqr < Circle.radius * Circle.radius)
        {
            if (!OBB.IsTrigger && !Circle.IsTrigger)
            {
                //Todo Add the contact data to the contact list.
                Debug.Log("There's a shape in mah boot!");
            }

            return true;
        }

        // 9. do test (if in circle, true, else false)

        return false;
    }

    #endregion

    #region Helper Functions
    public static bool DOAABBCollisionTest(Vector2 AABB1x, Vector2 AABB1y, Vector2 AABB1z,
        Vector2 AABB2x, Vector2 AABB2y, Vector2 AABB2z)
    {
        float xMin1 = AABB1x.x, xMin2 = AABB2x.x;
        float yMin1 = AABB1y.x, yMin2 = AABB2y.x;
        float zMin1 = AABB1z.x, zMin2 = AABB2z.x;

        float xMax1 = AABB1x.y, xMax2 = AABB2x.y;
        float yMax1 = AABB1y.y, yMax2 = AABB2y.y;
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

    public static void CalculateAABB(
        Vector3 rft, Vector3 lft, Vector3 rbt, Vector3 lbt,
        Vector3 rfb, Vector3 lfb, Vector3 rbb, Vector3 lbb,
        out Vector2 xBound, out Vector2 yBound, out Vector2 zBound)
    {
        Vector3 rftProj = new Vector3(ProjXAxis(rft), ProjYAxis(rft), ProjZAxis(rft));
        Vector3 lftProj = new Vector3(ProjXAxis(lft), ProjYAxis(lft), ProjZAxis(lft));
        Vector3 rbtProj = new Vector3(ProjXAxis(rbt), ProjYAxis(rbt), ProjZAxis(rbt));
        Vector3 lbtProj = new Vector3(ProjXAxis(lbt), ProjYAxis(lbt), ProjZAxis(lbt));
        Vector3 rfbProj = new Vector3(ProjXAxis(rfb), ProjYAxis(rfb), ProjZAxis(rfb));
        Vector3 lfbProj = new Vector3(ProjXAxis(lfb), ProjYAxis(lfb), ProjZAxis(lfb));
        Vector3 rbbProj = new Vector3(ProjXAxis(rbb), ProjYAxis(rbb), ProjZAxis(rbb));
        Vector3 lbbProj = new Vector3(ProjXAxis(lbb), ProjYAxis(lbb), ProjZAxis(lbb));

        xBound.x = Mathf.Min(rftProj.x, lftProj.x, rbtProj.x, lbtProj.x, rfbProj.x, lfbProj.x, rbbProj.x, lbbProj.x);
        xBound.y = Mathf.Max(rftProj.x, lftProj.x, rbtProj.x, lbtProj.x, rfbProj.x, lfbProj.x, rbbProj.x, lbbProj.x);

        yBound.x = Mathf.Min(rftProj.y, lftProj.y, rbtProj.y, lbtProj.y, rfbProj.y, lfbProj.y, rbbProj.y, lbbProj.y);
        yBound.y = Mathf.Max(rftProj.y, lftProj.y, rbtProj.y, lbtProj.y, rfbProj.y, lfbProj.y, rbbProj.y, lbbProj.y);

        zBound.x = Mathf.Min(rftProj.z, lftProj.z, rbtProj.z, lbtProj.z, rfbProj.z, lfbProj.z, rbbProj.z, lbbProj.z);
        zBound.y = Mathf.Max(rftProj.z, lftProj.z, rbtProj.z, lbtProj.z, rfbProj.z, lfbProj.z, rbbProj.z, lbbProj.z);
    }

    protected static float ProjXAxis(Vector3 point)
    {
        return ProjOnAxis(point, Vector3.right);
    }

    protected static float ProjYAxis(Vector3 point)
    {
        return ProjOnAxis(point, Vector3.up);
    }

    protected static float ProjZAxis(Vector3 point)
    {
        return ProjOnAxis(point, Vector3.forward);
    }

    protected static float ProjOnAxis(Vector3 point, Vector3 axis)
    {
        return Vector3.Dot(point, axis);
    }

    private static Vector4 ToVec4(Vector3 v, float w = 1.0f)
    {
        return new Vector4(v.x, v.y, v.z, w);
    }

    public static void CalculateMaxExtents(CollisionHullBaby hull, out Vector2 xBounds, out Vector2 yBounds, out Vector2 zBounds)
    {
        Vector3 hullPosition = hull.transform.position;
        Vector3 radius = hull.transform.lossyScale * 0.5f;

        RigidBaby rb = hull.GetComponent<RigidBaby>();
        Matrix4x4 transformMat = rb.TransformationMat;

        Vector3 rtf = transformMat * new Vector4(radius.x, radius.y, radius.z, 1.0f);
        Vector3 ltf = transformMat * new Vector4(radius.x, radius.y, -radius.z, 1.0f);
        Vector3 rtb = transformMat * new Vector4(radius.x, -radius.y, radius.z, 1.0f);
        Vector3 ltb = transformMat * new Vector4(radius.x, -radius.y, -radius.z, 1.0f);
        Vector3 rbf = transformMat * new Vector4(-radius.x, radius.y, radius.z, 1.0f);
        Vector3 lbf = transformMat * new Vector4(-radius.x, radius.y, -radius.z, 1.0f);
        Vector3 rbb = transformMat * new Vector4(-radius.x, -radius.y, radius.z, 1.0f);
        Vector3 lbb = transformMat * new Vector4(-radius.x, -radius.y, -radius.z, 1.0f);

        CalculateAABB(rtf, ltf, rtb, ltb, rbf, lbf, rbb, lbb,
            out xBounds, out yBounds, out zBounds
        );
    }
    #endregion
}
