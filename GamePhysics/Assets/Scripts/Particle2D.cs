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

    private void Reset()
    {
        position = Vector2.zero;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        velocity.x = scaleX;
        prevScaleX = scaleX;
    }

    private void UpdatePositionEulerExplicit(float dt)
    {
        position += velocity * dt;
        velocity += acceleration * dt;
    }

    private void UpdatePositionKinematic(float dt)
    {
        position += velocity * dt + 0.5f * acceleration * dt * dt;
        velocity += acceleration * dt;
    }

    private void UpdateRotationEulerExplicit(float dt)
    {
        rotation += rotVelocity * dt;
        rotVelocity += rotAcceleration * dt;
    }

    private void UpdateRotationKinematic(float dt)
    {
        rotation += rotVelocity * dt + 0.5f * rotAcceleration * dt * dt;
        rotVelocity += rotAcceleration * dt;
    }

    private void FixedUpdate()
    {
        if (scaleX != prevScaleX)
        {
            //WHY DOESN'T THIS RESET THE SIN WAVE?
            Reset();
        }

        if (physPos == PosIntegrationType.EulerExplicit)
        {
            myDelegate = UpdatePositionEulerExplicit;
        }
        else if (physPos == PosIntegrationType.Kinematic)
        {
            myDelegate = UpdatePositionKinematic;
        }
        myDelegate(Time.fixedDeltaTime);

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
        SetRotation(rotation %= 360.0f);

        acceleration.x = scaleX * -Mathf.Sin(Time.time);

        rotAcceleration = rotAccZ;
    }

    private void SetRotation(float rot)
    {
        helperRot = transform.eulerAngles;
        helperRot.z = rot;
        transform.eulerAngles = helperRot;
    }

    public enum PosIntegrationType
    {
        Kinematic,
        EulerExplicit
    }

    public enum RotIntegrationType
    {
        Kinematic,
        EulerExplicit
    }
}
