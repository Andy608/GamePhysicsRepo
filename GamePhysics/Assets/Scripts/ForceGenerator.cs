using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator : MonoBehaviour
{
    public static Vector2 GenerateForce_Gravity(float mass, float gravitationalConstant, Vector2 worldUp)
    {
        return mass * gravitationalConstant * worldUp;
    }
}
