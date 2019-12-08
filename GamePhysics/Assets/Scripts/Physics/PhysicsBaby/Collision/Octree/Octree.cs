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
    public int MaxObjecstPerOctant { get => maxObjectsPerOctant; private set { maxObjectsPerOctant = value; } }

    [SerializeField] private Vector3 centerPosition = Vector3.zero;
    public Vector3 CenterPosition { get => centerPosition; private set { centerPosition = value; } }

    [SerializeField] private int maxTreeDepth = 5;
    public int MaxTreeDepth { get => maxTreeDepth; private set { maxTreeDepth = value; } }

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
        Log("Updating Origin of Octree: " + octreeName);
        transform.position = centerPosition;
    }

    private void UpdateOctreeName()
    {
        transform.gameObject.name = octreeName;
    }

    private void GenerateOctree()
    {
        Log("Generating " + octreeName);
        if (RootNode == null)
        {
            RootNode = Octant.GenerateOctant(this, 0, null, CenterPosition, CubeRadius, null);
        }
    }

    private void Log(string s)
    {
        Debug.Log(debugPrefix + s);
    }
}
