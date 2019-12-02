using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGravityBaby : IForceBaby
{
    [SerializeField] private Vector3 gravity = Vector3.zero;
    public Vector3 Gravity { get; private set; } = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
       Init(gravity);
    }

    public void Init(Vector3 gravity)
    {
        Gravity = gravity;
    }

    public override void UpdateForce(RigidBaby rigidbaby, float deltaTime)
    {
        if (!rigidbaby.IsMassFinite())
        {
            return;
        }

        rigidbaby.AddForce(Gravity * rigidbaby.Mass);
    }
}
