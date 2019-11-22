using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AndrickPlugin
{
    [DllImport("AndrickPlugin")]
    public static extern void CreateDefaultQuaternion(bool identity, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void CreateQuaternion(float[] vec3, float angle, bool isDegrees, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void Normalize(float[] quaternion, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void Inverted(float[] quaternion, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void Multiply(float[] q1, float[] q2, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void MultiplyWithVec(float[] q1, float[] vec3, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void Scale(float[] q1, float scalar, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void Add(float[] q1, float[] q2, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern void Rotate(float[] q1, float[] vec3, float[] r);

    [DllImport("AndrickPlugin")]
    public static extern float GetMagnitude(float[] quaternion);

    [DllImport("AndrickPlugin")]
    public static extern float GetMagnitudeSquared(float[] quaternion);
}
