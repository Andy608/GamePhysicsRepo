using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalQuatBaby
{
    public float w = 0.0f;
    public Vector3 v = new Vector3();
    private float[] quat = new float[4];

    private void Sync(float[] quaternion)
    {
        quat = quaternion;
        v.x = quat[0];
        v.y = quat[1];
        v.z = quat[2];
        w = quat[3];
    }

    /// <summary> ExternalQuatBaby constructor. </summary>
    /// <param name="identity"> True to return the identity. False (default) for true zero quaternion. </param>
    public ExternalQuatBaby(bool identity = false)
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.CreateDefaultQuaternion(identity, r);
        Sync(r);
    }

    /// <summary> ExternalQuatBaby constructor. </summary>
    /// <param name="axis"> The axis to rotate about </param>
    /// <param name="angle"> In degrees </param>
    public ExternalQuatBaby(Vector3 axis, float angle, bool isDegrees)
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.CreateQuaternion(new float[] { axis.x, axis.y, axis.z }, angle, isDegrees, r);
        Sync(r);
    }

    private ExternalQuatBaby(float[] quaternion)
    {
        Sync(quaternion);
    }

    public void Normalize()
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.Normalize(quat, r);
        Sync(r);
    }

    /// <summary> Invert the quaternion. </summary>
    /// <returns> The inverted quaternion. </returns>
    public ExternalQuatBaby Inverted()
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.Inverted(quat, r);
        return new ExternalQuatBaby(r);
    }

    /// <summary> Create new rotation from two quaternion rotations. </summary>
    /// <param name="q1"> Left hand Quatbaby. </param>
    /// <param name="q2"> Right hand Quatbaby. </param>
    /// <returns> A new ExternalQuatBaby object. </returns>
    public static ExternalQuatBaby operator *(ExternalQuatBaby q1, ExternalQuatBaby q2)
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.Multiply(q1.quat, q2.quat, r);
        return new ExternalQuatBaby(r);
    }

    public ExternalQuatBaby MultiplyByVec(Vector3 rot)
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.MultiplyWithVec(quat, new float[] { rot.x, rot.y, rot.z }, r);
        return new ExternalQuatBaby(r);
    }

    /// <summary> Scale the quaternion. </summary>
    /// <param name="scalar"> The amount to scale by. </param>
    /// <returns> A new Quatbaby. </returns>
    public ExternalQuatBaby Scale(float scalar)
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.Scale(quat, scalar, r);
        return new ExternalQuatBaby(r);
    }

    /// <summary> Addition overload for quaterions. </summary>
    /// <param name="lhs"> The lefthand quaternion. </param>
    /// <param name="rhs"> The righthand quaternion. </param>
    /// <returns> A new ExternalQuatBaby. </returns>
    public static ExternalQuatBaby operator +(ExternalQuatBaby lhs, ExternalQuatBaby rhs)
    {
        float[] r = new float[4] { 0, 0, 0, 0 };
        AndrickPlugin.Add(lhs.quat, rhs.quat, r);
        return new ExternalQuatBaby(r);
    }

    /// <summary> Rotate a point around using the quaternion. </summary>
    /// <param name="vec"> The vector to rotate by the quaternion. </param>
    /// <returns> A new rotated vector. </returns>
    public Vector3 Rotate(Vector3 vec)
    {
        float[] r = new float[3] { 0, 0, 0 };
        AndrickPlugin.Rotate(quat, new float[] { vec.x, vec.y, vec.z }, r);
        return new Vector3(r[0], r[1], r[2]);
    }

    public Matrix4x4 ToMatrix()
    {
        float qw = w, qx = v.x, qy = v.y, qz = v.z;
        Normalize();

        //https://stackoverflow.com/questions/1556260/convert-quaternion-rotation-to-rotation-matrix
        return new Matrix4x4(
            new Vector4(1.0f - 2.0f * qy * qy - 2.0f * qz * qz, 2.0f * qx * qy + 2.0f * qz * qw, 2.0f * qx * qz - 2.0f * qy * qw, 0.0f),
            new Vector4(2.0f * qx * qy - 2.0f * qz * qw, 1.0f - 2.0f * qx * qx - 2.0f * qz * qz, 2.0f * qy * qz + 2.0f * qx * qw, 0.0f),
            new Vector4(2.0f * qx * qz + 2.0f * qy * qw, 2.0f * qy * qz - 2.0f * qx * qw, 1.0f - 2.0f * qx * qx - 2.0f * qy * qy, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
        );
    }

    /// <summary> Calculates the true magnitude of the quaternion. Caution: uses square root. </summary>
    /// <returns> The magnitude of the ExternalQuatBaby. </returns>
    public float GetMagnitude()
    {
        return AndrickPlugin.GetMagnitude(quat);
    }

    /// <summary> Calculates the squared magnitude of the quaternion. </summary>
    /// <returns> The squared magnitude of the ExternalQuatBaby. </returns>
    public float GetSquaredMagnitude()
    {
        return AndrickPlugin.GetMagnitudeSquared(quat);
    }

    /// <summary> Converts ExternalQuatBaby to Unity's quaternion class. </summary>
    /// <returns> A Unity quaternion. </returns>
    public Quaternion ToUnityQuaternion()
    {
        return new Quaternion(v.x, v.y, v.z, w).normalized;
    }

    /// <summary> Converts Unity's quaternion to out ExternalQuatBaby class. </summary>
    /// <param name="q"> Unity quaternion. </param>
    /// <returns> A ExternalQuatBaby. </returns>
    public static ExternalQuatBaby QuaternionToExternalQuatBaby(Quaternion q)
    {
        ExternalQuatBaby b = new ExternalQuatBaby();
        b.v = new Vector3(q.x, q.y, q.z);
        b.w = q.w;

        return b;
    }

    //https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
    public static ExternalQuatBaby MatrixToExternalQuatBaby(Matrix4x4 m)
    {
        ExternalQuatBaby q = new ExternalQuatBaby();
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
