using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quat
{
    public float w;
    public Vector3 v;

    public Quat()
    {
        w = 0.0f;
        v = Vector3.zero;
    }

    public Quat(Vector3 axis, float angle)
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

    //Invert the quaternion.
    public Quat Inverted()
    {
        Quat q = new Quat();
        q.w = w;
        q.v = -q.v;
        return q;
    }

    //Create new rotation from two quaternion rotations.
    public static Quat operator*(Quat q1, Quat q2)
    {
        //1. Break quaternions into variables, vectors, and real
        Vector3 v1 = new Vector3(q1.v.x, q1.v.y, q1.v.z);
        Vector3 v2 = new Vector3(q2.v.x, q2.v.y, q2.v.z);
        float w1 = q1.w;
        float w2 = q2.w;

        //2. calculate real part
        //-2.1 Get dot product of vectors
        float vDot = Vector3.Dot(v1, v2);

        float wFinal = w1 * w2 - vDot;

        //3. calculate vector component
        //-3.1 Get cross of vectors
        Vector3 vCross = Vector3.Cross(v1, v2);

        Vector3 vFinal = w1 * v2 + w2 * v1 + vCross;

        Quat qFinal = new Quat();
        qFinal.v = vFinal;
        qFinal.w = wFinal;

        return qFinal;
    }

    public Quat Scale(float scalar)
    {
        Quat q = new Quat();
        q.v.x = v.x * scalar;
        q.v.y = v.y * scalar;
        q.v.z = v.z * scalar;
        q.w = w * scalar;
        return q;
    }

    public static Quat operator+(Quat lhs, Quat rhs)
    {
        Quat q = new Quat();
        q.v.x = lhs.v.x + rhs.v.x;
        q.v.y = lhs.v.y + rhs.v.y;
        q.v.z = lhs.v.z + rhs.v.z;
        q.w = lhs.w + rhs.w;
        return q;
    }

    //Rotate a point around using the quaternion.
    public Vector3 Rotate(Vector3 vec)
    {
        Quat p = new Quat();
        p.w = 0.0f;
        p.v = vec;

        Vector3 crossed = Vector3.Cross(v, vec);
        return vec + crossed * (2.0f * w) + Vector3.Cross(v, crossed) * 2.0f;
    }

    public Quat Slerp(Quat other, float t)
    {
        Quat r = other;
        return (r * Inverted() ^ t) * this;
    }

    public static Quat operator^(Quat q, float t)
    {
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;

        q.ToAxisAngle(ref axis, ref angle);

        float scaledAngle = angle * t;

        return new Quat(axis, angle);
    }

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

    public Quaternion ToUnityQuaternion()
    {
        return new Quaternion(v.x, v.y, v.z, w).normalized;
    }

    public override string ToString()
    {
        return "Axis: (" + v.x + ", " + v.y + ", " + v.z + "), Angle: " + w + " Mag: " + GetMagnitude();
    }
    
    public float GetMagnitude()
    {
        return Mathf.Sqrt(GetSquaredMagnitude());
    }

    public float GetSquaredMagnitude()
    {
        return Mathf.Abs(v.x * v.x + v.y * v.y + v.z + v.z + w * w);
    }
}
