using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For testing to see if the quaternion returns the correct values.
public class QuatBabyDriver : MonoBehaviour
{
    QuatBaby q1 = new QuatBaby(new Vector3(0, 1, 0), 90);
    QuatBaby q2 = new QuatBaby(new Vector3(1, 0, 0), 45);
    QuatBaby q3;

    QuatBaby qCurrent = new QuatBaby(new Vector3(0, 1, 0), 90);
    QuatBaby qNext = new QuatBaby();

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
