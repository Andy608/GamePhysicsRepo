using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLLDriver : MonoBehaviour
{
    QuatBaby q, q5;
    Vector3 s;

    // Update is called once per frame
    void Start()
    {
        q = new QuatBaby();
        q.v = new Vector3(3, 4, 5);
        q.w = 90;

        q5 = new QuatBaby();
        q5.v = new Vector3(3, 7, 5);
        q5.w = 12;

        s = new Vector3(3, 7, 5);

        Debug.Log(q.GetMagnitude() + " | " + q.GetSquaredMagnitude());

        //float[] r = new float[4];
        //AndrickPlugin.CreateDefaultQuaternion(true, r);

        //float[] axis = new float[3] { 3, 4, 5 };
        //float angle = 90.0f;
        //float[] quaternion = new float[4];
        //AndrickPlugin.CreateQuaternion(axis, angle, true, quaternion);

        //float[] quat = new float[4] { 3, 4, 5, 90 };
        //float[] normalized = new float[4];
        //AndrickPlugin.Normalize(quat, normalized);

        //float[] quat = new float[4] { 3, 4, 5, 90 };
        //float[] inverted = new float[4];
        //AndrickPlugin.Inverted(quat, inverted);

        //float[] q1 = new float[4] { 3, 4, 5, 90 };
        //float[] q2 = new float[4] { 3, 7, 5, 12 };
        //float[] huh = new float[4];
        //AndrickPlugin.Multiply(q1, q2, huh);

        //float[] q1 = new float[4] { 3, 4, 5, 90 };
        //float[] v1 = new float[3] { 3, 7, 5 };
        //float[] huh = new float[4];
        //AndrickPlugin.MultiplyWithVec(q1, v1, huh);

        //float[] q1 = new float[4] { 3, 4, 5, 90 };
        //float[] huh = new float[4];
        //AndrickPlugin.Scale(q1, 0.5f, huh);

        //float[] q1 = new float[4] { 3, 4, 5, 90 };
        //float[] q2 = new float[4] { 3, 7, 5, 12 };
        //float[] huh = new float[4];
        //AndrickPlugin.Add(q1, q2, huh);

        //float[] q1 = new float[4] { 3, 4, 5, 90 };
        //float[] v4 = new float[3] { 3, 7, 5 };
        //float[] huh = new float[4];
        //AndrickPlugin.Rotate(q1, v4, huh);

        //float[] q1 = new float[4] { 3, 4, 5, 90 };
        //float[] v4 = new float[3] { 3, 7, 5 };
        //float[] huh = new float[4];
        //AndrickPlugin.GetMagnitude(q1);
        //QuatBaby gfrnjwgb = new QuatBaby();
        //gfrnjwgb.v = new Vector3(q1[0], q1[1], q1[2]);
        //gfrnjwgb.w = q1[3];

        //Debug.Log(gfrnjwgb + " | " + gfrnjwgb.GetSquaredMagnitude());
    }
}
