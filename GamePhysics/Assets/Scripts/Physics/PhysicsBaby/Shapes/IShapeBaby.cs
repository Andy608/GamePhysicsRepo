using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBaby))]
public abstract class IShapeBaby : MonoBehaviour
{
    protected RigidBaby rigidbaby;

    public Matrix4x4 Inertia { get; private set; }

    private void Awake()
    {
        rigidbaby = GetComponent<RigidBaby>();
        Inertia = CalculateInertia();
    }

    protected abstract Matrix4x4 CalculateInertia();
}
