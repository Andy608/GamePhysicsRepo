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
            return  -opposingForce * frictionForceMax / oppMag;
        }
    }

    // f_friction_k = -coeff*|f_normal| * unit(vel)
    public static Vector2 GenerateForce_FrictionKinetic(Vector2 normalForce, Vector2 particleVelocity, float materialCoefficientKinetic)
    {
        return -materialCoefficientKinetic * normalForce.magnitude * particleVelocity.normalized;
    }

    public static Vector2 GenerateForce_Friction(Vector2 normalForce, Vector2 opposingForce, Vector2 particleVelocity, 
        float materialCoefficientStatic, float materialCofficientKinetic)
    {
        Vector2 frictionForce = Vector2.zero;

        if (particleVelocity.SqrMagnitude() <= 0.0f)
        {
            frictionForce = GenerateForce_FrictionStatic(normalForce, opposingForce, materialCoefficientStatic);
        }
        else
        {
            frictionForce = GenerateForce_FrictionKinetic(normalForce, particleVelocity, materialCofficientKinetic);
        }

        return frictionForce;
    }

    // f_drag = (p * u^2 * area * coeff)/2
    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        return 0.5f * particleVelocity * fluidDensity * fluidVelocity * fluidVelocity * objectArea_crossSection * objectDragCoefficient;
    }

    // f_spring = -coeff*(spring length - spring resting length)
    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        Vector2 currentEndPos = (particlePosition - anchorPosition);

        float mag = currentEndPos.magnitude;
        float amount = -springStiffnessCoefficient * (mag - springRestingLength);
        Vector2 force = Vector2.zero;

        if (mag > 0.0f)
        {
            force = (currentEndPos * amount / mag);
        }

        return force;
    }

    public static Vector2 GenerateForce_SpringWithMax(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient, float maxSpringLength)
    {
        Vector2 currentEndPos = (particlePosition - anchorPosition);

        if (currentEndPos.magnitude > maxSpringLength)
        {
            springStiffnessCoefficient *= 0.3f;
        }

        float mag = currentEndPos.magnitude;
        float amount = -springStiffnessCoefficient * (mag - springRestingLength);
        Vector2 force = Vector2.zero;

        if (mag > 0.0f)
        {
            force = (currentEndPos * amount / mag);
        }

        return force;
    }

    public static Vector2 GenerateForce_SpringDamping(float mass, Vector2 velocity, float springStiffnessCoefficient, float damping)
    {
        //k is already square rooted!
        float c = 2 * mass * springStiffnessCoefficient;
        return (-c * velocity) / damping;
    }

    //public static Vector2 GenerateForce_DampSpring(float mass, Vector2 particlePosition, Vector2 anchorPosition, Vector2 particleVelocity, float springStiffnessCoefficient, float dragCoefficient)
    //{
    //    if (Time.time == 0.0f)
    //        return Vector2.zero;
    //
    //    Vector2 particleToAnchor = (particlePosition - anchorPosition);
    //
    //    float y = 0.5f * Mathf.Sqrt((4.0f * springStiffnessCoefficient) - (dragCoefficient * dragCoefficient));
    //
    //    if (y == 0)
    //        return Vector2.zero;
    //
    //    Vector2 c = (particleToAnchor * (dragCoefficient / (2.0f * y))) + (particleVelocity * (1.0f / y));
    //
    //    Vector2 target = (((particleToAnchor * Mathf.Cos(y * Time.time)) + (c * Mathf.Sin(y * Time.time)))) * Mathf.Exp(-0.5f * Time.time * dragCoefficient);
    //
    //    Vector2 acceleration = (target - particleToAnchor) * (1.0f / Time.time * Time.time) - (particleVelocity * Time.time);
    //
    //    return acceleration * mass;
    //}
}
