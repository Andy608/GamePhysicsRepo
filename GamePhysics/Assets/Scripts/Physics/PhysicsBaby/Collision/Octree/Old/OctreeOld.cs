using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeOld : MonoBehaviour
{
    [Header("Doesn't do anything right now.")]
    [SerializeField] private bool isGrowable = false;
    public bool IsGrowable { get; private set; }

    [SerializeField] private float cubeRadius = 20.0f;
    public float CubeRadius { get; private set; }

    [SerializeField] private int maxObjectsInOctant = 2;
    public int MaxObjectsInOctant { get; private set; }

    [SerializeField] private Vector3 centerPosition = Vector3.zero;
    public Vector3 CenterPosition { get; private set; }

    public OctreeNodeOld RootNode { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        IsGrowable = isGrowable;
        CubeRadius = cubeRadius;
        MaxObjectsInOctant = maxObjectsInOctant;
        CenterPosition = centerPosition;
    }

    public OctreeOld CreateOctree()
    {
        if (RootNode == null)
        {
            RootNode = new OctreeNodeOld(this, null, CenterPosition, CubeRadius, new List<CollisionHullBaby>());
        }

        return this;
    }
}
