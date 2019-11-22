using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AndrickPlugin
{
    [DllImport("AndrickPlugin")]
    public static extern float[] CreateQuaternion(bool identity);

    [DllImport("AndrickPlugin")]
    public static extern float[] CreateQuaternion(float[] vec3, float angle, bool isDegrees);

    [DllImport("AndrickPlugin")]
    public static extern float[] Normalize(float[] quaternion);

    [DllImport("AndrickPlugin")]
    public static extern float[] Inverted(float[] quaternion);

    [DllImport("AndrickPlugin")]
    public static extern float[] Multiply(float[] q1, float[] q2);

    [DllImport("AndrickPlugin")]
    public static extern float[] MultiplyWithVec(float[] q1, float[] vec3);

    [DllImport("AndrickPlugin")]
    public static extern float[] Scale(float[] q1, float scalar);

    [DllImport("AndrickPlugin")]
    public static extern float[] Add(float[] q1, float[] q2);

    [DllImport("AndrickPlugin")]
    public static extern float[] Rotate(float[] q1, float[] vec3);

    [DllImport("AndrickPlugin")]
    public static extern float GetMagnitude(float[] quaternion);

    [DllImport("AndrickPlugin")]
    public static extern float GetMagnitudeSquared(float[] quaternion);
}
