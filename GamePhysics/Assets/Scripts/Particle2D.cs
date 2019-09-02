using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    private Vector2 position;
    private Vector2 velocity;

    private float rotation;
    private float rotVelocity;

    private Vector2 acceleration;
    private Vector2 rotAcceleration;
    [SerializeField][Range(0.0f, 10.0f)] private float xScale = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float yScale = 1.0f;

    [SerializeField] [Range(0.0f, 10.0f)] private float xRotAcc = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float yRotAcc = 1.0f;

    [SerializeField] private bool useFixedPosition = true;
    [SerializeField] private bool useFixedRotation = true;

    private void Start()
    {
        velocity.x = xScale;

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
        //rotation += rotation * dt;
        //rotVelocity += rotVelocity * dt;
    }

    private void UpdateRotationKinematic(float dt)
    {
        //rotation += rotVelocity * dt + 0.5f * rotAcceleration * dt * dt;
        //rotVelocity += rotAcceleration * dt;
    }

    private void FixedUpdate()
    {
        if (useFixedPosition)
        {
            UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        }
        else
        {
            UpdatePositionKinematic(Time.fixedDeltaTime);
        }

        if (useFixedRotation)
        {
            UpdateRotationEulerExplicit(Time.fixedDeltaTime);
        }
        else
        {
            UpdateRotationKinematic(Time.fixedDeltaTime);
        }
        
        transform.position = position;
        transform.eulerAngles = rotation;

        acceleration.x = xScale * -Mathf.Sin(Time.time);
        //acceleration.y = yScale * -Mathf.Cos(Time.time);

        rotAcceleration.x = xRotAcc;
        rotAcceleration.y = yRotAcc;
    }
}
