using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OctreeNode
{
    public OctreeNode RootNode { get { return controller.RootNode; } }

    //The parent of the current octant.
    public OctreeNode ParentNode { get; private set; }

    //The 8 child nodes of the current octant, if it has any.
    public OctreeNode[] ChildrenNodes { get { return childrenNodes; } }
    private OctreeNode[] childrenNodes = new OctreeNode[8];

    //The octant's center position.
    public Vector3 CenterPosition { get; private set; }

    //The half the length of the octant's width.
    public float CubeRadius { get; private set; }

    //All of the game objects in the current octant.
    public List<RigidBaby> ObjectsInOctant { get; private set; } = new List<RigidBaby>();

    //Used to display the octant bounds in the game world.
    private GameObject octantVisualizer;
    private LineRenderer outlineRenderer;

    private Octree controller;

    //Only can be called by Octree class.
    public OctreeNode(Octree reference, OctreeNode parentNode, Vector3 centerPosition, float cubeRadius, List<RigidBaby> potentialObjects)
    {
        controller = reference;
        ParentNode = parentNode;
        CenterPosition = centerPosition;
        CubeRadius = cubeRadius;

        foreach (RigidBaby rigidbaby in potentialObjects)
        {
            //Checks to see if object is in this octant's bounds. If yes, then it belongs to this octant.
            ProcessObject(rigidbaby);
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
        childrenNodes = new OctreeNode[8];
    }

    public bool ProcessObject(RigidBaby rigidbaby)
    {
        if (ContainsRigidBaby(rigidbaby))
        {
            //Checks if memory address is the same so they literally have to be the same object.
            if (ReferenceEquals(ChildrenNodes[0], null))
            {
                StoreObject(rigidbaby);
                return true;
            }
            else
            {
                foreach (OctreeNode childNode in ChildrenNodes)
                {
                    if (childNode.ProcessObject(rigidbaby))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void StoreObject(RigidBaby rigidbaby)
    {
        if (!ObjectsInOctant.Contains(rigidbaby))
        {
            ObjectsInOctant.Add(rigidbaby);
            rigidbaby.OwnerOctants.Add(this);
        }

        if (ObjectsInOctant.Count > controller.MaxObjectsInOctant)
        {
            Split();
        }
    }

    private void Split()
    {
        foreach (RigidBaby rigidbaby in ObjectsInOctant)
        {
            rigidbaby.OwnerOctants.Remove(this);
        }

        float childRadius = CubeRadius / 2.0f;
        Vector3 childCenterPosition = new Vector3(childRadius, childRadius, childRadius);

        for (int i = 0; i < 4; ++i)
        {
            childrenNodes[i] = new OctreeNode(controller, this, CenterPosition + childCenterPosition, childRadius, ObjectsInOctant);
            childCenterPosition = Quaternion.Euler(0.0f, -90.0f, 0.0f) * childCenterPosition;
        }

        childCenterPosition = new Vector3(childRadius, -childRadius, childRadius);

        for (int i = 4; i < 8; ++i)
        {
            childrenNodes[i] = new OctreeNode(controller, this, CenterPosition + childCenterPosition, childRadius, ObjectsInOctant);
            childCenterPosition = Quaternion.Euler(0.0f, -90.0f, 0.0f) * childCenterPosition;
        }

        ObjectsInOctant.Clear();
    }

    private void Kill(OctreeNode[] siblingNodesToRemove)
    {
        foreach (RigidBaby rigidBaby in ObjectsInOctant)
        {
            rigidBaby.OwnerOctants = rigidBaby.OwnerOctants.Except(siblingNodesToRemove).ToList();
            rigidBaby.OwnerOctants.Remove(this);

            rigidBaby.OwnerOctants.Add(ParentNode);
            ParentNode.ObjectsInOctant.Add(rigidBaby);
        }

        foreach (OctreeNode sibling in siblingNodesToRemove)
        {
            GameObject.Destroy(sibling.octantVisualizer);
        }

        GameObject.Destroy(octantVisualizer);
    }

    public void TryRemoveChildrenNodes(RigidBaby escapedObject)
    {
        if (!ReferenceEquals(this, controller.RootNode) && !SiblingsHaveChildrenAndCanCollapse())
        {
            foreach (OctreeNode node in ParentNode.ChildrenNodes)
            {
                //Pass the 7 siblings as we kill the current node.
                node.Kill(ParentNode.ChildrenNodes.Where(i => !ReferenceEquals(i, this)).ToArray());
            }

            ParentNode.RemoveChildrenNodes();
        }
        else
        {
            ObjectsInOctant.Remove(escapedObject);
            escapedObject.OwnerOctants.Remove(this);
        }
    }

    //If we collapse the child will the parent's octant now be holding too many rigidbabies?
    private bool SiblingsHaveChildrenAndCanCollapse()
    {
        //The objects in transition between octants.
        List<RigidBaby> orphanObjects = new List<RigidBaby>();

        foreach (OctreeNode sibling in ParentNode.ChildrenNodes)
        {
            if (sibling == null)
            {
                Debug.Log("WHY?");
            }

            if (!ReferenceEquals(sibling.ChildrenNodes[0], null))
            {
                return true;
            }

            //Make sure not to add multiple objects to the list if a bounding box is in multiple octants at once.
            orphanObjects.AddRange(sibling.ObjectsInOctant.Where(i => !orphanObjects.Contains(i)));
        }

        if (orphanObjects.Count > controller.MaxObjectsInOctant + 1)
        {
            return true;
        }

        return false;
    }

    public bool ContainsRigidBaby(RigidBaby rigidbaby)
    {
        //CollisionHullBabyAABB maxBoundingBox = 
        Vector3 p = rigidbaby.Position;

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
