using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBaby : IShapeBaby
{
    [SerializeField] private bool isHollow = false;
    [SerializeField] private float sphereRadius = 1.0f;

    protected override Matrix4x4 CalculateInertia()
    {
        float inertia = 2.0f / (isHollow ? 5.0f : 3.0f) * rigidbaby.Mass * (sphereRadius * sphereRadius);
        
        return new Matrix4x4(
            new Vector4(inertia,   0.0f,   0.0f, 0.0f),
            new Vector4(  0.0f, inertia,   0.0f, 0.0f),
            new Vector4(  0.0f,   0.0f, inertia, 0.0f),
            new Vector4(  0.0f,   0.0f,   0.0f,  1.0f)
        );
    }
}