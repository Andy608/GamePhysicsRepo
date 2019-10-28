using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quat : MonoBehaviour
{
    private float w;
    private Vector3 v;

    public Quat()
    {
        w = 0.0f;
        v = Vector3.zero;
    }

    public Quat(Vector3 axis, float angle)
    {
        angle = (angle / 360.0f) * Mathf.PI * 2.0f;

        w = Mathf.Cos(angle / 2.0f);
        v = axis * Mathf.Sin(angle / 2.0f);
        normalize();
    }

    public void normalize()
    {
        float d = w * w + v.x * v.x + v.y * v.y + v.z * v.z;

        if (d < double.Epsilon)
        {
            w = 1.0f;
            return;
        }

        Debug.Log("D: " + d);
        d = (float)1.0 / Mathf.Sqrt(d);
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
    public static Quat operator*(Quat lhs, Quat rhs)
    {
        Quat rotated = new Quat();
        rotated.w = lhs.w * rhs.w + Vector3.Dot(lhs.v, rhs.v);
        rotated.v = lhs.v * rhs.w + rhs.v * lhs.w + Vector3.Cross(lhs.v, rhs.v);
        return rotated;
    }

    public Quat Scale(float scalar)
    {
        Quat q = this;
        q.v.x *= scalar;
        q.v.y *= scalar;
        q.v.z *= scalar;
        q.w *= scalar;
        return q;
    }

    public static Quat operator+(Quat lhs, Quat rhs)
    {
        Quat q = lhs;
        q.v.x += rhs.v.x;
        q.v.y += rhs.v.y;
        q.v.z += rhs.v.z;
        q.w += rhs.w;
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
        return new Quaternion(v.x, v.y, v.z, w);
    }

    public override string ToString()
    {
        return "Axis: (" + v.x + ", " + v.y + ", " + v.z + "), Angle: " + w;
    }
}
