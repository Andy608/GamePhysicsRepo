using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderBaby : IShapeBaby
{
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private float height = 1.0f;

    protected override Matrix4x4 CalculateInertia()
    {
        float hh = height * height;
        float rr = radius * radius;
        float mhh = rigidbaby.Mass * hh;
        float mrr = rigidbaby.Mass * rr;
        float inertiaXZ = ((1.0f / 12.0f) * mhh) + (0.25f * mrr);
        float inertiaY = 0.5f * mrr;

        return new Matrix4x4(
            new Vector4(inertiaXZ,     0.0f,      0.0f, 0.0f),
            new Vector4(     0.0f, inertiaY,      0.0f, 0.0f),
            new Vector4(     0.0f,     0.0f, inertiaXZ, 0.0f),
            new Vector4(     0.0f,     0.0f,      0.0f, 1.0f)
        );
    }
}
