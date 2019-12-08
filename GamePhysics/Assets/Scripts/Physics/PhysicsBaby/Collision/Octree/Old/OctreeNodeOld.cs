using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OctreeNodeOld
{
    public OctreeNodeOld RootNode { get { return controller.RootNode; } }

    //The parent of the current octant.
    public OctreeNodeOld ParentNode { get; private set; }

    //The 8 child nodes of the current octant, if it has any.
    public OctreeNodeOld[] ChildrenNodes { get; private set; } = new OctreeNodeOld[8];

    //The octant's center position.
    public Vector3 CenterPosition { get; private set; }

    //The half the length of the octant's width.
    public float CubeRadius { get; private set; }

    //All of the game objects in the current octant.
    public List<CollisionHullBaby> ObjectsInOctant { get; private set; } = new List<CollisionHullBaby>();

    //Used to display the octant bounds in the game world.
    private GameObject octantVisualizer;
    private LineRenderer outlineRenderer;

    private OctreeOld controller;

    //Only can be called by Octree class.
    public OctreeNodeOld(OctreeOld reference, OctreeNodeOld parentNode, Vector3 centerPosition, float cubeRadius, List<CollisionHullBaby> potentialObjects)
    {
        controller = reference;
        ParentNode = parentNode;
        CenterPosition = centerPosition;
        CubeRadius = cubeRadius;

        foreach (CollisionHullBaby hull in potentialObjects)
        {
            //Checks to see if object is in this octant's bounds. If yes, then it belongs to this octant.
            ProcessObject(hull);
        }

        octantVisualizer = new GameObject();
        octantVisualizer.hideFlags = HideFlags.HideInHierarchy;

        outlineRenderer = octantVisualizer.AddComponent<LineRenderer>();
        outlineRenderer.useWorldSpace = true;
        outlineRenderer.positionCount = 16;
        outlineRenderer.startWidth = 0.04f;
        outlineRenderer.endWidth = 0.04f;

        Color octantColor = Color.white;//new Color(Random.Range(0.4f, 1.0f), Random.Range(0.4f, 1.0f), Random.Range(0.4f, 1.0f));
        outlineRenderer.material.SetColor("_Color", octantColor);

        UpdateVisualizer();
    }

    public void RemoveChildrenNodes()
    {
        ChildrenNodes = new OctreeNodeOld[8];
    }

    public bool ProcessObject(CollisionHullBaby hull)
    {
        if (ContainsRigidBaby(hull))
        {
            //Checks if memory address is the same so they literally have to be the same object.
            //If ChildrenNodes is an empty array
            if (ReferenceEquals(ChildrenNodes[0], null))
            {
                StoreObject(hull);
                return true;
            }
            else
            {
                foreach (OctreeNodeOld childNode in ChildrenNodes)
                {
                    if (childNode.ProcessObject(hull))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void StoreObject(CollisionHullBaby hull)
    {
        if (!ObjectsInOctant.Contains(hull))
        {
            ObjectsInOctant.Add(hull);
            hull.OwnerOctants.Add(this);
        }

        if (ObjectsInOctant.Count > controller.MaxObjectsInOctant)
        {
            Split();
        }
    }

    private void Split()
    {
        foreach (CollisionHullBaby hull in ObjectsInOctant)
        {
            hull.OwnerOctants.Remove(this);
        }

        float childRadius = CubeRadius / 2.0f;
        Vector3 childCenterPosition = new Vector3(childRadius, childRadius, childRadius);

        for (int i = 0; i < 4; ++i)
        {
            ChildrenNodes[i] = new OctreeNodeOld(controller, this, CenterPosition + childCenterPosition, childRadius, ObjectsInOctant);
            childCenterPosition = Quaternion.Euler(0.0f, -90.0f, 0.0f) * childCenterPosition;
        }

        childCenterPosition = new Vector3(childRadius, -childRadius, childRadius);

        for (int i = 4; i < 8; ++i)
        {
            ChildrenNodes[i] = new OctreeNodeOld(controller, this, CenterPosition + childCenterPosition, childRadius, ObjectsInOctant);
            childCenterPosition = Quaternion.Euler(0.0f, -90.0f, 0.0f) * childCenterPosition;
        }

        ObjectsInOctant.Clear();
    }

    private void Kill(OctreeNodeOld[] siblingNodesToRemove)
    {
        foreach (CollisionHullBaby hull in ObjectsInOctant)
        {
            hull.OwnerOctants = hull.OwnerOctants.Except(siblingNodesToRemove).ToList();
            hull.OwnerOctants.Remove(this);

            hull.OwnerOctants.Add(ParentNode);
            ParentNode.ObjectsInOctant.Add(hull);
        }

        foreach (OctreeNodeOld sibling in siblingNodesToRemove)
        {
            GameObject.DestroyImmediate(sibling.octantVisualizer);
        }

        GameObject.DestroyImmediate(octantVisualizer);
    }

    public void TryRemoveChildrenNodes(CollisionHullBaby leavingObject)
    {
        //If not the root node, and there are less than max objects in child octants
        if (!ReferenceEquals(this, controller.RootNode) && !ObjectsInChildOctantsOrTooMany(leavingObject))
        {
            foreach (OctreeNodeOld childNode in ParentNode.ChildrenNodes)
            {
                //Pass the 7 siblings as we kill the current node.
                childNode.Kill(ParentNode.ChildrenNodes.Where(i => !ReferenceEquals(i, this)).ToArray());
            }

            ParentNode.RemoveChildrenNodes();
        }
        else
        {
            ObjectsInOctant.Remove(leavingObject);
            leavingObject.OwnerOctants.Remove(this);
        }
    }

    private bool ObjectsInChildOctantsOrTooMany(CollisionHullBaby leavingObject)
    {
        //The objects in transition between octants.
        List<CollisionHullBaby> allObjectsInOctant = new List<CollisionHullBaby>();

        foreach (OctreeNodeOld sibling in ParentNode.ChildrenNodes)
        {
            if (!ReferenceEquals(sibling.ChildrenNodes[0], null))
            {
                Debug.Log("We have too many children.");
                return true;
            }

            allObjectsInOctant.AddRange(sibling.ObjectsInOctant.Where(i => !allObjectsInOctant.Contains(i) && !ReferenceEquals(i, leavingObject)));
        }

        if (allObjectsInOctant.Count > controller.MaxObjectsInOctant)
        {
            Debug.Log("We have too many items: " + allObjectsInOctant.Count);
            return true;
        }

        return false;
    }

    public bool ContainsRigidBaby(CollisionHullBaby hull)
    {
        //CollisionHullBabyAABB maxBoundingBox = 
        //For now this only uses the objects center position. Will change to collision bounding box later.
        Vector3 p = hull.transform.position;

        if (p.x > CenterPosition.x + CubeRadius || p.x < CenterPosition.x - CubeRadius)
        {
            return false;
        }
        else if (p.y > CenterPosition.y + CubeRadius || p.y < CenterPosition.y - CubeRadius)
        {
            return false;
        }
        else if (p.z > CenterPosition.z + CubeRadius || p.z < CenterPosition.z - CubeRadius)
        {
            return false;
        }

        return true;
    }

    private void UpdateVisualizer()
    {
        Vector3[] octantCoordinates = new Vector3[8];

        Vector3 corner = new Vector3(CubeRadius, CubeRadius, CubeRadius);
        
        for (int i = 0; i < 4; ++i)
        {
            octantCoordinates[i] = CenterPosition + corner;
            corner = Quaternion.Euler(0.0f, 90.0f, 0.0f) * corner;
        }

        corner = new Vector3(CubeRadius, -CubeRadius, CubeRadius);

        for (int i = 4; i < 8; ++i)
        {
            octantCoordinates[i] = CenterPosition + corner;
            corner = Quaternion.Euler(0.0f, 90.0f, 0.0f) * corner;
        }

        //Draw cube outline
        outlineRenderer.SetPosition(0,  octantCoordinates[0]);
        outlineRenderer.SetPosition(1,  octantCoordinates[1]);
        outlineRenderer.SetPosition(2,  octantCoordinates[2]);
        outlineRenderer.SetPosition(3,  octantCoordinates[3]);
        outlineRenderer.SetPosition(4,  octantCoordinates[0]);
        outlineRenderer.SetPosition(5,  octantCoordinates[4]);
        outlineRenderer.SetPosition(6,  octantCoordinates[5]);
        outlineRenderer.SetPosition(7,  octantCoordinates[1]);
        outlineRenderer.SetPosition(8,  octantCoordinates[5]);
        outlineRenderer.SetPosition(9,  octantCoordinates[6]);
        outlineRenderer.SetPosition(10, octantCoordinates[2]);
        outlineRenderer.SetPosition(11, octantCoordinates[6]);
        outlineRenderer.SetPosition(12, octantCoordinates[7]);
        outlineRenderer.SetPosition(13, octantCoordinates[3]);
        outlineRenderer.SetPosition(14, octantCoordinates[7]);
        outlineRenderer.SetPosition(15, octantCoordinates[4]);
    }
}
