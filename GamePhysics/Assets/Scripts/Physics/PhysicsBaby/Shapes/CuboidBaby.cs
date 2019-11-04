using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboidBaby : IShapeBaby
{
    [SerializeField] private bool isHollow = false;
    [SerializeField] private Vector3 dimensions = Vector3.one;

    protected override Matrix4x4 CalculateInertia()
    {
        float xx = dimensions.x * dimensions.x;
        float yy = dimensions.y * dimensions.y;
        float zz = dimensions.z * dimensions.z;
        float yz = yy + zz;
        float xz = xx + zz;
        float xy = xx + yy;
        float scalar = (isHollow ? (5.0f / 3.0f) : (1.0f / 12.0f)) * rigidbaby.Mass;

        return new Matrix4x4(
            new Vector4(scalar * yz,        0.0f,        0.0f, 0.0f),
            new Vector4(       0.0f, scalar * xz,        0.0f, 0.0f),
            new Vector4(       0.0f,        0.0f, scalar * xy, 0.0f),
            new Vector4(       0.0f,        0.0f,        0.0f, 1.0f)
        );
    }
}
