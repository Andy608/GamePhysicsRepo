using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceNormalBaby : IForceBaby
{
    [SerializeField] private ForceGravityBaby gravity = null;
    [SerializeField] private Vector3 surfaceNormal = Vector3.zero;
    public ForceGravityBaby Gravity { get; private set; } = null;
    public Vector3 SurfaceNormal { get; private set; } = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        Init(gravity, surfaceNormal);
    }

    public void Init(ForceGravityBaby gravity, Vector3 surfaceNormal)
    {
        Gravity = gravity;
        SurfaceNormal = surfaceNormal;
    }

    public override void UpdateForce(RigidBaby rigidbaby, float deltaTime)
    {
        rigidbaby.AddForce(CalculateNormal(Gravity.Gravity * rigidbaby.Mass, SurfaceNormal));
    }
}
