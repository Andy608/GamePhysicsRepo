using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree : MonoBehaviour
{
    private static string debugPrefix = "[Octree] ";

    public GameObject OctantPrefab { get; private set; } = null;

    [SerializeField] private string octreeName;
    public string OctreeName { get => octreeName; private set { octreeName = value; } }

    [SerializeField] private float octantRadius = 20.0f;
    public float CubeRadius { get => octantRadius; private set { octantRadius = value; } }

    [SerializeField] private int maxObjectsPerOctant = 2;
    public int MaxObjectsPerOctant { get => maxObjectsPerOctant; private set { maxObjectsPerOctant = value; } }

    [SerializeField] private Vector3 centerPosition = Vector3.zero;
    public Vector3 CenterPosition { get => centerPosition; private set { centerPosition = value; } }

    [SerializeField] private int maxTreeDepth = 5;
    public int MaxTreeDepth { get => maxTreeDepth; private set { maxTreeDepth = value; } }

    [SerializeField] private int maxCollisionIterations = 2;
    public int MaxCollisionIterations { get => maxCollisionIterations; private set { maxCollisionIterations = value; } }

    public Octant RootNode { get; private set; } = null;

    private void OnValidate()
    {
        UpdateOctreeName();
        UpdateOrigin();
    }

    private void Awake()
    {
        OctantPrefab = Resources.Load<GameObject>("Prefabs/Rigidbaby/Octant");
        UpdateOrigin();
        GenerateOctree();
    }

    private void UpdateOrigin()
    {
        Debug.Log(debugPrefix + " Updating Origin of Octree: " + octreeName);
        transform.position = centerPosition;
    }

    private void UpdateOctreeName()
    {
        transform.gameObject.name = octreeName;
    }

    private void GenerateOctree()
    {
        Debug.Log(debugPrefix + " Generating " + octreeName);
        if (RootNode == null)
        {
            RootNode = Octant.GenerateOctant(this, 0, null, CenterPosition, CubeRadius, null);
        }
    }

    private void FixedUpdate()
    {
        UpdateOctree(RootNode);
    }

    private void UpdateOctree(Octant octant)
    {
        octant.UpdateOctant();
        //If the octant has no children, we have reached a leaf node
        //so we can end the recursion.
        if (ReferenceEquals(octant.ChildrenNodes[0], null))
        {
            return;
        }

        foreach (Octant child in octant.ChildrenNodes)
        {
            //Recursively traverse the height of the octree.
            UpdateOctree(child);
        }
    }
}
