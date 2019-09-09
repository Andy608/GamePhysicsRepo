using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //Used to swap what physics function is used
    public delegate void MyDelegate(float dt);
    [SerializeField] public MyDelegate myDelegate;

    [SerializeField]private PosIntegrationType physPos;
    [SerializeField] private RotIntegrationType physRot;
    [SerializeField][Range(0.0f, 10.0f)] private float scaleX = 1.0f;
    [SerializeField][Range(-100.0f, 100.0f)] private float rotAccZ = 0.0f;

    private float prevScaleX;

    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;

    private float rotation;
    private float rotVelocity;
    private float rotAcceleration;
    private Vector3 helperRot;

    private void Start()
    {
        Reset();
    }

    /// <summary>
    /// Supposed to reset the particle to a default state.
    /// </summary>
    private void Reset()
    {
        position = Vector2.zero;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        velocity.x = scaleX;
        prevScaleX = scaleX;
    }

    /// <summary>
    /// Integrates the particles position using the euler explicit formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdatePositionEulerExplicit(float dt)
    {
        position += velocity * dt;
        velocity += acceleration * dt;
    }

    /// <summary>
    /// Integrates the particles position using the kinematic formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdatePositionKinematic(float dt)
    {
        position += velocity * dt + 0.5f * acceleration * dt * dt;
        velocity += acceleration * dt;
    }

    /// <summary>
    /// Integrates the particles rotation using the euler explicit formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdateRotationEulerExplicit(float dt)
    {
        rotation += rotVelocity * dt;
        rotVelocity += rotAcceleration * dt;
    }

    /// <summary>
    /// Integrates the particles rotation using the kinematic formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdateRotationKinematic(float dt)
    {
        rotation += rotVelocity * dt + 0.5f * rotAcceleration * dt * dt;
        rotVelocity += rotAcceleration * dt;
    }

    private void FixedUpdate()
    {
        //TODO: Reset to default if the scaleX slider was changed, currently doesn't work
        if (scaleX != prevScaleX)
        {
            //WHY DOESN'T THIS RESET THE SIN WAVE?
            Reset();
        }

        //Uses the selected integration method to use for position
        if (physPos == PosIntegrationType.EulerExplicit)
        {
            myDelegate = UpdatePositionEulerExplicit;
        }
        else if (physPos == PosIntegrationType.Kinematic)
        {
            myDelegate = UpdatePositionKinematic;
        }
        myDelegate(Time.fixedDeltaTime);

        //Uses the selected integration method to use for rotation
        if (physRot == RotIntegrationType.EulerExplicit)
        {
            myDelegate = UpdateRotationEulerExplicit;
        }
        else  if (physRot == RotIntegrationType.Kinematic)
        {
            myDelegate = UpdateRotationKinematic;
        }
        myDelegate(Time.fixedDeltaTime);
        
        transform.position = position;

        //clamps rotation to 360
        SetRotation(rotation %= 360.0f);

        acceleration.x = scaleX * -Mathf.Sin(Time.time);

        rotAcceleration = rotAccZ;
    }

    /// <summary>
    /// Sets rotation of euler angle without creating new vectors
    /// </summary>
    /// <param name="rot"></param>
    private void SetRotation(float rot)
    {
        helperRot = transform.eulerAngles;
        helperRot.z = rot;
        transform.eulerAngles = helperRot;
    }

    /// <summary>
    /// Enum for Position Integration Type
    /// </summary>
    public enum PosIntegrationType
    {
        Kinematic,
        EulerExplicit
    }

    /// <summary>
    /// Enum for Rotation Integration Type
    /// </summary>
    public enum RotIntegrationType
    {
        Kinematic,
        EulerExplicit
    }
}
