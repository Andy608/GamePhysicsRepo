using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidBaby))]
public abstract class IShapeBaby : MonoBehaviour
{
    public enum ShapeType
    {
        SolidSphere,
        HollowSphere,
        SolidBox,
        HollowBox,
        SolidCube,
        HollowCube,
        SolidCylinder,
        SolidCone
    }

    protected RigidBaby rigidbaby;

    public Matrix4x4 Inertia { get; private set; }

    private void Awake()
    {
        rigidbaby = GetComponent<RigidBaby>();
        Inertia = CalculateInertia();
    }

    protected abstract Matrix4x4 CalculateInertia();
}
