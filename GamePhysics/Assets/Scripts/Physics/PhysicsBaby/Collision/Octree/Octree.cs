using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree : MonoBehaviour
{
    public static Octree GlobalDemoOctree = null;

    [SerializeField] private bool createOnStart = true;

    [SerializeField] private bool isGrowable = false;
    public bool IsGrowable { get; private set; }

    [SerializeField] private float cubeRadius = 10.0f;
    public float CubeRadius { get; private set; }

    [SerializeField] private int maxObjectsInOctant = 2;
    public int MaxObjectsInOctant { get; private set; }

    [SerializeField] private Vector3 centerPosition = Vector3.zero;
    public Vector3 CenterPosition { get; private set; }

    public OctreeNode RootNode { get; private set; }

    private void Awake()
    {
        Init();

        if (createOnStart)
        {
            GlobalDemoOctree = CreateOctree();
        }
    }

    private void Init()
    {
        IsGrowable = isGrowable;
        CubeRadius = cubeRadius;
        MaxObjectsInOctant = maxObjectsInOctant;
        CenterPosition = centerPosition;
    }

    public Octree CreateOctree()
    {
        if (RootNode == null)
        {
            RootNode = new OctreeNode(this, null, CenterPosition, CubeRadius, new List<RigidBaby>());
        }

        return this;
    }
}
