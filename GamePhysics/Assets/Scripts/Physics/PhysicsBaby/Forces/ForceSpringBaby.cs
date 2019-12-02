using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSpringBaby : IForceBaby
{
    [SerializeField] private Vector3 anchorPoint = Vector3.zero;
    [SerializeField] private float springConstant = 0.0f;
    [SerializeField] private float restLength = 0.0f;
    [SerializeField] private float damping = 0.0f;
    public Vector3 AnchorPoint { get; private set; } = Vector3.zero;
    public float SpringConstant { get; private set; } = 0.0f;
    public float RestLength { get; private set; } = 0.0f;
    public float Damping { get; private set; } = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        Init(anchorPoint, springConstant, restLength, damping);
    }

    public void Init(Vector3 anchorPoint, float springConstant, float restLength, float damping)
    {
        AnchorPoint = anchorPoint;
        SpringConstant = springConstant;
        RestLength = restLength;
        Damping = damping;
    }

    public override void UpdateForce(RigidBaby rigidbaby, float deltaTime)
    {
        Vector3 currentEndPos = (rigidbaby.Position - AnchorPoint);

        float mag = currentEndPos.magnitude;
        float amount = -(SpringConstant * SpringConstant) * (mag - RestLength);
        Vector3 force = Vector3.zero;

        if (mag > 0.0f)
        {
            force = (currentEndPos * amount / mag);
        }

        rigidbaby.AddForce(force);

        if (Damping > 0.0f)
        {
            DampSpring(rigidbaby, SpringConstant, Damping);
        }
    }

    private void DampSpring(RigidBaby rigidbaby, float springStrength, float damping)
    {
        float c = 2.0f * rigidbaby.Mass * springStrength;
        rigidbaby.AddForce((-c * rigidbaby.Velocity) / damping);
    }
}
