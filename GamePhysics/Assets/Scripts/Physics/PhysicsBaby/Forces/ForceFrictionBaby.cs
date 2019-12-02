using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFrictionBaby : IForceBaby
{
    [SerializeField] private Vector3 normalForce = Vector3.zero;
    [SerializeField] private Vector3 pushingForce = Vector3.zero;
    [SerializeField] private float staticCo = 0.0f;
    [SerializeField] private float kineticCo = 0.0f;

    public Vector3 NormalForce { get; private set; } = Vector3.zero;
    public Vector3 PushingForce { get; private set; } = Vector3.zero;

    public float StaticCo { get; private set; } = 0.0f;
    public float KineticCo { get; private set; } = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        Init(normalForce, pushingForce, staticCo, kineticCo);
    }

    public void Init(Vector3 normalForce, Vector3 pushingForce, float staticCo, float kineticCo)
    {
        NormalForce = normalForce;
        PushingForce = pushingForce;
        StaticCo = staticCo;
        KineticCo = kineticCo;
    }

    public override void UpdateForce(RigidBaby rigidbaby, float deltaTime)
    {
        Vector3 frictionForce = Vector3.zero;
        if (rigidbaby.Velocity.sqrMagnitude <= 0.0f)
        {
            frictionForce = CalculateFrictionStatic(NormalForce, PushingForce, StaticCo);
        }
        else
        {
            frictionForce = CalculateFrictionKinectic(NormalForce, rigidbaby.Velocity, KineticCo);
        }

        rigidbaby.AddForce(frictionForce);
    }

    private Vector3 CalculateFrictionStatic(Vector3 normal, Vector3 pushingForce, float staticCo)
    {
        float tippingPoint = staticCo * normal.magnitude;

        float pushingForceMag = pushingForce.magnitude;

        if (pushingForceMag < tippingPoint)
        {
            return -pushingForce;
        }
        else
        {
            return -pushingForce * tippingPoint / pushingForceMag;
        }
    }

    private Vector3 CalculateFrictionKinectic(Vector3 normal, Vector3 velocity, float kineticCo)
    {
        return -kineticCo * normal.magnitude * velocity.normalized;
    }
}
