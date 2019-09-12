using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator : MonoBehaviour
{
    public static Vector2 GenerateForce_Gravity(float mass, float gravitationalConstant, Vector2 worldUp)
    {
        return mass * gravitationalConstant * worldUp;
    }

    public static Vector2 GenerateForce_Normal(Vector2 gravitationalForce, Vector2 surfaceNormal)
    {
        return Vector3.Project(gravitationalForce, surfaceNormal);
    }

    public static Vector2 GenerateForce_Sliding(Vector2 gravitationalForce, Vector2 normalForce)
    {
        return gravitationalForce + normalForce;
    }

    // f_friction_s = -f_opposing if less than coeff*|f_normal|, else -coeff*|f_normal| (max amount is )
    public static Vector2 GenerateForce_FrictionStatic(Vector2 normalForce, Vector2 opposingForce, float materialCoefficientStatic)
    {
        float frictionForceMax = materialCoefficientStatic * normalForce.magnitude;

        float oppMag = opposingForce.magnitude;

        if (oppMag < frictionForceMax)
        {
            return -opposingForce;
        }
        else
        {
            return  -opposingForce * frictionForceMax/oppMag;
        }
    }

    // f_friction_k = -coeff*|f_normal| * unit(vel)
    public static Vector2 GenerateForce_FrictionKinetic(Vector2 normalForce, Vector2 particleVelocity, float materialCoefficientKinetic)
    {
        return -materialCoefficientKinetic * normalForce.magnitude * particleVelocity.normalized;
    }

    // f_drag = (p * u^2 * area * coeff)/2
    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        return Vector2.zero;
    }

    // f_spring = -coeff*(spring length - spring resting length)
    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        return Vector2.zero;
    }
}
