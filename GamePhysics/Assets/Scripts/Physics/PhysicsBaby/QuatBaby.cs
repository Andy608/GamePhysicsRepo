﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatBaby
{
    public static QuatBaby Identity = new QuatBaby(true);

    public float w;
    public Vector3 v;

    /// <summary> QuatBaby constructor. </summary>
    /// <param name="identity"> True to return the identity. False (default) for true zero quaternion. </param>
    public QuatBaby(bool identity = false)
    {
        w = identity ? 1.0f : 0.0f;
        v = Vector3.zero;
    }

    /// <summary> QuatBaby constructor. </summary>
    /// <param name="axis"> The axis to rotate about </param>
    /// <param name="angle"> In degrees </param>
    public QuatBaby(Vector3 axis, float angle)
    {
        angle = (angle / 360.0f) * Mathf.PI * 2.0f;

        w = Mathf.Cos(angle / 2.0f);
        v = axis.normalized * Mathf.Sin(angle / 2.0f);
    }

    public void normalize()
    {
        //Squared dist / magnitude of the quat
        float d = GetSquaredMagnitude();

        if (d < double.Epsilon)
        {
            w = 1.0f;
            return;
        }

        d = 1.0f / Mathf.Sqrt(d);
        w *= d;
        v.x *= d;
        v.y *= d;
        v.z *= d;
    }

    /// <summary> Invert the quaternion. </summary>
    /// <returns> The inverted quaternion. </returns>
    public QuatBaby Inverted()
    {
        QuatBaby q = new QuatBaby();
        q.w = w;
        q.v = -q.v;
        return q;
    }

    /// <summary> Create new rotation from two quaternion rotations. </summary>
    /// <param name="q1"> Left hand Quatbaby. </param>
    /// <param name="q2"> Right hand Quatbaby. </param>
    /// <returns> A new QuatBaby object. </returns>
    public static QuatBaby operator*(QuatBaby q1, QuatBaby q2)
    {
        //1. Break quaternions into variables, vectors, and real
        Vector3 v1 = new Vector3(q1.v.x, q1.v.y, q1.v.z);
        Vector3 v2 = new Vector3(q2.v.x, q2.v.y, q2.v.z);
        float w1 = q1.w;
        float w2 = q2.w;

        //2. calculate real part
        //2.1 Get dot product of vectors
        float vDot = Vector3.Dot(v1, v2);

        float wFinal = w1 * w2 - vDot;

        //3. calculate vector component
        //3.1 Get cross of vectors
        Vector3 vCross = Vector3.Cross(v1, v2);

        Vector3 vFinal = w1 * v2 + w2 * v1 + vCross;

        QuatBaby qFinal = new QuatBaby();
        qFinal.v = vFinal;
        qFinal.w = wFinal;

        return qFinal;
    }

    public QuatBaby MultiplyByVec(Vector3 rot)
    {
        //1. calculate real part
        //1.1 Get dot product of vectors
        float vDot = Vector3.Dot(v, rot);
        float wFinal = -vDot;

        //2. calculate vector component
        //2.1 Get cross of vectors
        Vector3 vCross = Vector3.Cross(v, rot);
        Vector3 vFinal = w * rot + vCross;

        QuatBaby qFinal = new QuatBaby();
        qFinal.v = vFinal;
        qFinal.w = wFinal;

        return qFinal;
    }

    /// <summary> Scale the quaternion. </summary>
    /// <param name="scalar"> The amount to scale by. </param>
    /// <returns> A new Quatbaby. </returns>
    public QuatBaby Scale(float scalar)
    {
        QuatBaby q = new QuatBaby();
        q.v.x = v.x * scalar;
        q.v.y = v.y * scalar;
        q.v.z = v.z * scalar;
        q.w = w * scalar;
        return q;
    }

    /// <summary> Addition overload for quaterions. </summary>
    /// <param name="lhs"> The lefthand quaternion. </param>
    /// <param name="rhs"> The righthand quaternion. </param>
    /// <returns> A new QuatBaby. </returns>
    public static QuatBaby operator+(QuatBaby lhs, QuatBaby rhs)
    {
        QuatBaby q = new QuatBaby();
        q.v.x = lhs.v.x + rhs.v.x;
        q.v.y = lhs.v.y + rhs.v.y;
        q.v.z = lhs.v.z + rhs.v.z;
        q.w = lhs.w + rhs.w;
        return q;
    }

    /// <summary> Rotate a point around using the quaternion. </summary>
    /// <param name="vec"> The vector to rotate by the quaternion. </param>
    /// <returns> A new rotated vector. </returns>
    public Vector3 Rotate(Vector3 vec)
    {
        QuatBaby p = new QuatBaby();
        p.w = 0.0f;
        p.v = vec;

        Vector3 crossed = Vector3.Cross(v, vec);
        return vec + crossed * (2.0f * w) + Vector3.Cross(v, crossed) * 2.0f;
    }

    /// <summary> Slerps between two quaternions by t (0 - 1). </summary>
    /// <param name="other"> The target quaternion. </param>
    /// <param name="t"> The percentage between the original and target quaternions. </param>
    /// <returns> A slerped QuatBaby. </returns>
    public QuatBaby Slerp(QuatBaby other, float t)
    {
        QuatBaby r = other;
        return (r * Inverted() ^ t) * this;
    }

    /// <summary> ^ operator for quaterion. </summary>
    /// <param name="q"> The quaternion to ^. </param>
    /// <param name="t"> The time to scale by. </param>
    /// <returns> A new QuatBaby. </returns>
    public static QuatBaby operator^(QuatBaby q, float t)
    {
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;

        q.ToAxisAngle(ref axis, ref angle);

        float scaledAngle = angle * t;

        return new QuatBaby(axis, angle);
    }

    /// <summary> ToAxisAngle function for quaternion. </summary>
    /// <param name="axis"> The axis we want calculated. </param>
    /// <param name="angle"> The angle we want calculated. </param>
    public void ToAxisAngle(ref Vector3 axis, ref float angle)
    {
        if (v.sqrMagnitude < 0.0001f)
        {
            axis = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else
        {
            axis = v.normalized;
        }

        angle = Mathf.Acos(w) * 2.0f;

        angle *= 360.0f / Mathf.PI * 2.0f;
    }

    public Matrix4x4 ToMatrix()
    {
        //float aa = w * w, bb = v.x * v.x, cc = v.y * v.y, dd = v.z * v.z;
        //float ab = w * v.x, ac = w * v.y, ad = w * v.z;
        //float bc = v.x * v.y, bd = v.x * v.z;
        //float cd = v.y * v.z;
        //
        ////https://wikimedia.org/api/rest_v1/media/math/render/svg/b2b8eb5ce0c4bfe919d8f113aefbc09ab1a0b296
        //mat = new Matrix4x4(
        //    new Vector4(aa + bb - cc - dd, 2.0f * bc + 2.0f * ad, 2.0f * bd - 2.0f * ac, 0.0f),
        //    new Vector4(2.0f * bc - 2.0f * ad, aa - bb + cc - dd, 2.0f * cd + 2.0f * ab, 0.0f),
        //    new Vector4(2.0f * bd + 2.0f * ac, 2.0f * cd - 2.0f * ab, aa - bb - cc + dd, 0.0f),
        //    new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
        //);

        float qw = w, qx = v.x, qy = v.y, qz = v.z;
        normalize();

        //https://stackoverflow.com/questions/1556260/convert-quaternion-rotation-to-rotation-matrix
        return new Matrix4x4(
            new Vector4( 1.0f - 2.0f * qy * qy - 2.0f * qz * qz,        2.0f * qx * qy + 2.0f * qz * qw,        2.0f * qx * qz - 2.0f * qy * qw,      0.0f), 
            new Vector4(        2.0f * qx * qy - 2.0f * qz * qw, 1.0f - 2.0f * qx * qx - 2.0f * qz * qz,        2.0f * qy * qz + 2.0f * qx * qw,      0.0f), 
            new Vector4(        2.0f * qx * qz + 2.0f * qy * qw,        2.0f * qy * qz - 2.0f * qx * qw, 1.0f - 2.0f * qx * qx - 2.0f * qy * qy,      0.0f),
            new Vector4(                                   0.0f,                                   0.0f,                                   0.0f,      1.0f)
        );
    }

    /// <summary> Calculates the true magnitude of the quaternion. Caution: uses square root. </summary>
    /// <returns> The magnitude of the QuatBaby. </returns>
    public float GetMagnitude()
    {
        return Mathf.Sqrt(GetSquaredMagnitude());
    }

    /// <summary> Calculates the squared magnitude of the quaternion. </summary>
    /// <returns> The squared magnitude of the QuatBaby. </returns>
    public float GetSquaredMagnitude()
    {
        return v.x * v.x + v.y * v.y + v.z * v.z + w * w;
    }
    
    /// <summary> Converts QuatBaby to Unity's quaternion class. </summary>
    /// <returns> A Unity quaternion. </returns>
    public Quaternion ToUnityQuaternion()
    {
        return new Quaternion(v.x, v.y, v.z, w).normalized;
    }

    /// <summary> Converts Unity's quaternion to out QuatBaby class. </summary>
    /// <param name="q"> Unity quaternion. </param>
    /// <returns> A QuatBaby. </returns>
    public static QuatBaby QuaternionToQuatBaby(Quaternion q)
    {
        QuatBaby qBaby = new QuatBaby();
        qBaby.v.x = q.x;
        qBaby.v.y = q.y;
        qBaby.v.z = q.z;
        qBaby.w = q.w;
        return qBaby;
    }

    //https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
    public static QuatBaby MatrixToQuatBaby(Matrix4x4 m)
    {
        QuatBaby q = new QuatBaby();
        float trace = m[0] + m[5] + m[10];
        if (trace > 0.0f)
        {
            float s = 0.5f / Mathf.Sqrt(trace + 1.0f);
            q.w = 0.25f / s;
            q.v.x = (m[6] - m[9]) * s;
            q.v.y = (m[8] - m[2]) * s;
            q.v.z = (m[1] - m[4]) * s;
        }
        else
        {
            if (m[0] > m[5] && m[0] > m[10])
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + m[0] - m[5] - m[10]);
                q.w = (m[6] - m[9]) / s;
                q.v.x = 0.25f * s;
                q.v.y = (m[4] + m[1]) / s;
                q.v.z = (m[8] + m[2]) / s;
            }
            else if (m[5] > m[10])
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + m[5] - m[0] - m[10]);
                q.w = (m[8] - m[2]) / s;
                q.v.x = (m[4] + m[1]) / s;
                q.v.y = 0.25f * s;
                q.v.z = (m[9] + m[6]) / s;
            }
            else
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + m[10] - m[0] - m[5]);
                q.w = (m[1] - m[4]) / s;
                q.v.x = (m[8] + m[2]) / s;
                q.v.y = (m[9] + m[6]) / s;
                q.v.z = 0.25f * s;
            }
        }

        return q;
    }

    /// <summary> Returns a string with the axis, angle, and magnitude. </summary>
    /// <returns> A string with the axis, angle, and magnitude. </returns>
    public override string ToString()
    {
        return "Axis: (" + v.x + ", " + v.y + ", " + v.z + "), Angle: " + w + " Mag: " + GetMagnitude();
    }
}
