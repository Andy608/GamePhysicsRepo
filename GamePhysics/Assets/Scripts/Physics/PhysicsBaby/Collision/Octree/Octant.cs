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
    public List<CollisionHullBaby> Hulls { get; private set; } = null;
    public Octant[] ChildrenNodes { get; private set; } = null;
    private CollisionHullBabyOBB octantBounds;

    private LineRenderer octantRenderer = null;

    public static Octant GenerateOctant(Octree octree, int depth, Octant parent, Vector3 centerPosition, float cubeRadius, List<CollisionHullBaby> hulls)
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
        octant.ChildrenNodes = new Octant[8];
        octant.octantBounds = go.AddComponent<CollisionHullBabyOBB>();
        octant.octantBounds.IsTrigger = true;
        octant.transform.parent = parent ? parent.transform : octree.transform;
        octant.transform.localScale = new Vector3(cubeRadius * 2.0f, cubeRadius * 2.0f, cubeRadius * 2.0f);

        octant.transform.position = centerPosition;
        octant.name = "Octant: " + centerPosition.x + "," + centerPosition.y + "," + centerPosition.z;
        octant.debugPrefix = "[" + octant.name + "]";

        if (hulls != null)
        {
            foreach (CollisionHullBaby hull in hulls)
            {
                if (octant.IsHullInBounds(hull))
                {
                    octant.InsertHull(hull);
                }
            }
        }

        octant.UpdateVisualizer();
        return octant;
    }

    private bool IsHullInBounds(CollisionHullBaby hull)
    {
        List <RigidBabyContact> none = new List<RigidBabyContact>();
        if (CollisionHullBaby.TestCollision(octantBounds, hull, ref none))
        {
            Log("Hull is inside my bounds.");
            return true;
        }
        else
        {
            Log("Hull NOT inside my bounds.");
            return false;
        }
    }

    private void InsertHull(CollisionHullBaby hull)
    {

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

    private void Log(string s)
    {
        Debug.Log(debugPrefix + s);
    }
}
