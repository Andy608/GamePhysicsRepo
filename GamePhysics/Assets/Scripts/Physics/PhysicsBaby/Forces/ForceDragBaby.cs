using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDragBaby : IForceBaby
{
    [SerializeField] private float dragCo = 0.0f;
    public float DragCo { get => dragCo; private set { dragCo = value; } }
    public float DragCoSquared { get => dragCo * dragCo; }

    [SerializeField] private float rotDragCo = 0.0f;
    public float RotDragCo { get => rotDragCo; set { rotDragCo = value; } }
    public float RotDragCoSquared { get => rotDragCo * rotDragCo; set { rotDragCo = value; } }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UpdateForce(RigidBaby rigidbaby, float deltaTime)
    {
        //Vector3 rotForce = rigidbaby.RotVelocity;
        //float rotMag = rotForce.magnitude;
        //rotMag = RotDragCo * rotMag + RotDragCoSquared * rotMag * rotMag;
        //
        //rotForce.Normalize();
        //rotForce *= -rotMag;
        //
        //rigidbaby.ApplyTorque(rigidbaby.RotVelocity, rotForce);

        Vector3 force = rigidbaby.Velocity;
        float mag = force.magnitude;
        mag = DragCo * mag + DragCoSquared * mag * mag;
        
        force.Normalize();
        force *= -mag;

        rigidbaby.AddForce(force);
    }
}
