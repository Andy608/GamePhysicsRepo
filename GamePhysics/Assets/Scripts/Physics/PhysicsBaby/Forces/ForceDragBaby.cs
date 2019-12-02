using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBaby : IForceBaby
{
    [SerializeField] private float dragCo = 0.0f;
    public float DragCo { get; private set; } = 0.0f;
    public float DragCoSquared { get; private set; } = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        Init(dragCo);
    }

    public void Init(float dragCo)
    {
        DragCo = dragCo;
        DragCoSquared = DragCo * DragCo;
    }

    public override void UpdateForce(RigidBaby rigidbaby, float deltaTime)
    {
        Vector3 force = rigidbaby.Velocity;
        float dragCo = force.magnitude;
        dragCo = DragCo * dragCo + DragCoSquared * dragCo * dragCo;
        
        force.Normalize();
        force *= -dragCo;

        rigidbaby.AddForce(force);
    }
}
