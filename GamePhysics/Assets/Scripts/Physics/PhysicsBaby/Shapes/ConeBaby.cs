using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeBaby : IShapeBaby
{
    [SerializeField] private float radiusBase = 1.0f;
    [SerializeField] private float height = 1.0f;

    protected override Matrix4x4 CalculateInertia()
    {
        float hh = height * height;
        float rr = radiusBase * radiusBase;
        float mhh = rigidbaby.Mass * hh;
        float mrr = rigidbaby.Mass * rr;
        float inertiaX = ((3.0f / 80.0f) * mhh) + ((3.0f / 20.0f) * mrr);
        float inertiaY = (3.0f / 10.0f) * mrr;
        float inertiaZ = ((3.0f / 5.0f) * mhh) + ((3.0f / 20.0f) * mrr);

        return new Matrix4x4(
            new Vector4(inertiaX,     0.0f,     0.0f, 0.0f),
            new Vector4(    0.0f, inertiaY,     0.0f, 0.0f),
            new Vector4(    0.0f,     0.0f, inertiaZ, 0.0f),
            new Vector4(    0.0f,     0.0f,     0.0f, 1.0f)
        );
    }
}
