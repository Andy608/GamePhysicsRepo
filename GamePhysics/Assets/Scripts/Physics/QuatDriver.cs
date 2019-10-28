using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For testing to see if the quaternion returns the correct values.
public class QuatDriver : MonoBehaviour
{
    Quat q1 = new Quat(new Vector3(0, 1, 0), 90);
    Quat q2 = new Quat(new Vector3(1, 0, 0), 45);
    Quat q3;

    Quat qCurrent = new Quat(new Vector3(0, 1, 0), 90);
    Quat qNext = new Quat();

    Vector3 vRotated;

    private void Start()
    {
        q3 = q2 * q1;
        Debug.Log(q1);
        Debug.Log(q2);
        Debug.Log(q3);

        vRotated = q3.Rotate(new Vector3(1, 0, 0));
        Debug.Log(vRotated);

        vRotated = q1.Rotate(new Vector3(1, 0, 0));
        Debug.Log(vRotated);

        vRotated = q2.Rotate(vRotated);
        Debug.Log(vRotated);
    }
}
