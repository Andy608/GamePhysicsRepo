using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octant : MonoBehaviour
{
    private string debugPrefix;

    public Octree Octree = null;
    public Octant ParentNode = null;
    public int Depth { get; private set; } = 0;
    public float CubeRadius { get; private set; } = 0.0f;
    public Vector3 CenterPosition { get; private set; } = Vector3.zero;
    public HashSet<CollisionHullBaby> Hulls { get; private set; } = null;
    public Octant[] ChildrenNodes { get; private set; } = null;
    private CollisionHullBabyAABB octantBounds;
    public int HullCount = 0;

    private LineRenderer octantRenderer = null;

    public static Octant GenerateOctant(Octree octree, int depth, Octant parent, Vector3 centerPosition, float cubeRadius, HashSet<CollisionHullBaby> hulls)
    {
        GameObject go = Instantiate(octree.OctantPrefab, parent ? parent.transform : octree.transform);
        Octant octant = go.AddComponent<Octant>();

        octant.octantRenderer = go.GetComponent<LineRenderer>();
        octant.octantRenderer.useWorldSpace = true;
        octant.octantRenderer.positionCount = 16;
        octant.octantRenderer.startWidth = 0.04f;
        octant.octantRenderer.endWidth = 0.04f;

        //This could be changed in the future.
        Color octantColor = Color.white;
        octant.octantRenderer.material.SetColor("_Color", octantColor);

        octant.Octree = octree;
        octant.ParentNode = parent;
        octant.Depth = depth;
        octant.CubeRadius = cubeRadius;
        octant.CenterPosition = centerPosition;
        octant.Hulls = new HashSet<CollisionHullBaby>();
        octant.ChildrenNodes = new Octant[8];
        octant.octantBounds = go.AddComponent<CollisionHullBabyAABB>();
        octant.octantBounds.IsTrigger = true;
        octant.transform.parent = parent ? parent.transform : octree.transform;
        octant.transform.localScale = new Vector3(cubeRadius * 2.0f, cubeRadius * 2.0f, cubeRadius * 2.0f);

        octant.transform.position = centerPosition;
        octant.name = "Octant: " + centerPosition.x + "," + centerPosition.y + "," + centerPosition.z;
        octant.debugPrefix = "[" + octant.name + "]";

        //When the octree is first created, hulls is an empty list.
        if (!ReferenceEquals(hulls, null))
        {
            //Iterate through all the hulls and try to insert them
            //recursively into their respective octants.
            foreach (CollisionHullBaby hull in hulls)
            {
                octant.TryInsertHull(hull);
            }
        }

        //Update the visualizer so we can see the octree in Unity
        //and return the newly created octree.
        octant.UpdateVisualizer();
        return octant;
    }

    public bool TryInsertHull(CollisionHullBaby hull)
    {
        if (IsHullInBounds(hull) && !Hulls.Contains(hull))
        {
            //Assumes the array is empty if the first index is null.
            //if the octant has no children.
            if (ReferenceEquals(ChildrenNodes[0], null))
            {
                InsertHull(hull);
                return true;
            }
            //Recursively traverse the octree until we hit a leaf 
            //octant that we can insert into.
            else
            {
                foreach (Octant child in ChildrenNodes)
                {
                    //if (child.TryInsertHull(hull))
                    //{
                        child.TryInsertHull(hull);
                    //}
                }
            }
        }

        //If we reach the bottom of the tree and the hull isn't
        //In the bounds, we cannot insert this hull into this
        //section of the octree.

        //This can happen if we try inserting a hull that is located
        //in a completely separate quadrant of the octree.

        return false;
    }

    private void InsertHull(CollisionHullBaby hull)
    {
        //Debug.Log(debugPrefix + " Insert the hull: " + hull.Type);

        //Insert the new hull if it is not already in the list.
        if (!Hulls.Contains(hull))
        {
            //hull.transform.SetParent(transform, true);
            Hulls.Add(hull);
            hull.parentOctants.Add(CenterPosition, this);
        }

        //If we have too many children, we must birth
        //more octants to take care of them!
        if (Hulls.Count > Octree.MaxObjectsPerOctant)
        {
            if ((Depth + 1) > Octree.MaxTreeDepth)
            {
                Debug.Log(debugPrefix + " We've reached the depth limit: " + Depth);
            }
            else
            {
                BirthOctantChildren();
            }
        }
    }

    public void UpdateOctant()
    {
        if (!ReferenceEquals(ChildrenNodes[0], null))
        {
            //Populate leavingHulls with all the objects that have moved out of 
            //all the octant children's bounds since last frame.
            HashSet<CollisionHullBaby> leavingHulls = new HashSet<CollisionHullBaby>();
            foreach (Octant child in ChildrenNodes)
            {
                if (!ReferenceEquals(child.ChildrenNodes[0], null))
                {
                    HullCount = Hulls.Count;
                    return;
                }

                foreach (CollisionHullBaby hull in child.Hulls)
                {
                    //Hull left octant, add to list.
                    if (!child.IsHullInBounds(hull))
                    {
                        //hull.transform.SetParent(null, true);
                        leavingHulls.Add(hull);
                    }
                }

                //Iterate through all the leaving hulls and remove this octant
                //from each's parentOctants dictionary.
                foreach (CollisionHullBaby hull in leavingHulls)
                {
                    if (child.Hulls.Remove(hull))
                    {
                        hull.parentOctants.Remove(child.CenterPosition);
                    }
                }
            }

            HashSet<CollisionHullBaby> totalHullsInChildren = new HashSet<CollisionHullBaby>();
            foreach (Octant child in ChildrenNodes)
            {
                //Create a list of all the hulls in all the child octants.
                foreach (CollisionHullBaby hull in child.Hulls)
                {
                    totalHullsInChildren.Add(hull);
                }
            }

            if (totalHullsInChildren.Count <= Octree.MaxObjectsPerOctant)
            {
                foreach (Octant child in ChildrenNodes)
                {
                    foreach (CollisionHullBaby hull in child.Hulls)
                    {
                        hull.parentOctants.Remove(child.CenterPosition);
                    }

                    child.Hulls.Clear();
                }

                foreach (Octant child in ChildrenNodes)
                {
                    DestroyImmediate(child.gameObject);
                }

                ChildrenNodes = new Octant[8];

                foreach (CollisionHullBaby hull in totalHullsInChildren)
                {
                    Octree.RootNode.TryInsertHull(hull);
                }

                //Iterate through all the leaving hulls and re-add each into
                //the octree.
                foreach (CollisionHullBaby hull in leavingHulls)
                {
                    Octree.RootNode.TryInsertHull(hull);
                }
            }
        }

        HullCount = Hulls.Count;

        //Iterate through all the leaving hulls and re-add each into
        //the octree.
        //foreach (CollisionHullBaby hull in leavingHulls)
        //{
        //    Octree.RootNode.TryInsertHull(hull);
        //}

        ////Check children octants to see if they can be removed

        ////Loop through all children nodes
        //HashSet<CollisionHullBaby> totalHullsInChildren = new HashSet<CollisionHullBaby>();
        //foreach (Octant child in ChildrenNodes)
        //{
        //    //This octant has no children or a child octant has children, so we can't collapse.
        //    if (ReferenceEquals(child, null) || !ReferenceEquals(child.ChildrenNodes[0], null))
        //    {
        //        HullCount = Hulls.Count;
        //        return;
        //    }
        //    else
        //    {
        //        //Create a list of all the hulls in all the child octants.
        //        foreach (CollisionHullBaby hull in child.Hulls)
        //        {
        //            totalHullsInChildren.Add(hull);
        //        }
        //    }
        //}

        //If there are less than max hulls in all of the children combined,
        //if (totalHullsInChildren.Count <= Octree.MaxObjectsPerOctant)
        //{
        //    //1. Loop through all children and clear all Hulls lists.
        //    //2. Loop through all of the hulls and remove octant as a parent.
        //    //3. DestroyImmediate all children octants.
        //    //4. Reset Children list in current octant.
        //    //5. Insert hulls into octant.
        //
        //    foreach (Octant child in ChildrenNodes)
        //    {
        //        foreach (CollisionHullBaby hull in Hulls)
        //        {
        //            hull.parentOctants.Remove(child.CenterPosition);
        //        }
        //
        //        child.Hulls.Clear();
        //    }
        //
        //    foreach (Octant child in ChildrenNodes)
        //    {
        //        DestroyImmediate(child.gameObject);
        //    }
        //
        //    ChildrenNodes = new Octant[8];
        //
        //    foreach (CollisionHullBaby hull in totalHullsInChildren)
        //    {
        //        TryInsertHull(hull);
        //    }
        //}
    }

    private bool IsHullInBounds(CollisionHullBaby hull)
    {
        List <RigidBabyContact> none = new List<RigidBabyContact>();
        if (CollisionHullBaby.TestCollision(octantBounds, hull, ref none))
        {
            //Debug.Log(debugPrefix + " Hull is inside my bounds.");
            return true;
        }
        else
        {
            //Debug.Log(debugPrefix + " Hull NOT inside my bounds.");
            return false;
        }
    }

    private void BirthOctantChildren()
    {
        //Loop through hulls and remove this octant from
        //each hull's parent octant list.
        foreach (CollisionHullBaby hull in Hulls)
        {
            hull.parentOctants.Remove(CenterPosition);
        }

        //Calculate the new centers of the child octants
        float childRadius = CubeRadius / 2.0f;
        Vector3 childOffsetPos = new Vector3(childRadius, childRadius, childRadius);

        //Construct child octants and add to children list
        for (int i = 0; i < 4; ++i)
        {
            ChildrenNodes[i] = GenerateOctant(Octree, Depth + 1, this, CenterPosition + childOffsetPos, childRadius, Hulls);
            childOffsetPos = Quaternion.Euler(0.0f, -90.0f, 0.0f) * childOffsetPos;
        }

        childOffsetPos = new Vector3(childRadius, -childRadius, childRadius);

        for (int i = 4; i < 8; ++i)
        {
            ChildrenNodes[i] = GenerateOctant(Octree, Depth + 1, this, CenterPosition + childOffsetPos, childRadius, Hulls);
            childOffsetPos = Quaternion.Euler(0.0f, -90.0f, 0.0f) * childOffsetPos;
        }

        //Clear hulls list
        Hulls.Clear();
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
        octantRenderer.SetPosition(0, octantCoordinates[0]);
        octantRenderer.SetPosition(1, octantCoordinates[1]);
        octantRenderer.SetPosition(2, octantCoordinates[2]);
        octantRenderer.SetPosition(3, octantCoordinates[3]);
        octantRenderer.SetPosition(4, octantCoordinates[0]);
        octantRenderer.SetPosition(5, octantCoordinates[4]);
        octantRenderer.SetPosition(6, octantCoordinates[5]);
        octantRenderer.SetPosition(7, octantCoordinates[1]);
        octantRenderer.SetPosition(8, octantCoordinates[5]);
        octantRenderer.SetPosition(9, octantCoordinates[6]);
        octantRenderer.SetPosition(10, octantCoordinates[2]);
        octantRenderer.SetPosition(11, octantCoordinates[6]);
        octantRenderer.SetPosition(12, octantCoordinates[7]);
        octantRenderer.SetPosition(13, octantCoordinates[3]);
        octantRenderer.SetPosition(14, octantCoordinates[7]);
        octantRenderer.SetPosition(15, octantCoordinates[4]);
    }
}
