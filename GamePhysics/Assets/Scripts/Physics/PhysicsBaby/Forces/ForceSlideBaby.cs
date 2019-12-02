using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSlideBaby : IForceBaby
{
    [SerializeField] private Vector3 gravity = Vector3.zero;
    [SerializeField] private Vector3 surfaceNormal = Vector3.zero;
    public Vector3 Gravity { get; private set; } = Vector3.zero;
    public Vector3 SurfaceNormal { get; private set; } = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        Init(gravity, surfaceNormal);
    }

    public void Init(Vector3 gravity, Vector3 surfaceNormal)
    {
        Gravity = gravity;
        SurfaceNormal = surfaceNormal;
    }

    public override void UpdateForce(RigidBaby rigidbaby, float deltaTime)
    {
        rigidbaby.AddForce(Gravity + CalculateNormal(Gravity * rigidbaby.Mass, SurfaceNormal));
    }
}
