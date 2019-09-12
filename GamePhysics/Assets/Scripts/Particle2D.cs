using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    private Vector2 Gravity = new Vector2(0.0f, -10.0f);

    ////Used to swap what physics function is used
    //public delegate void MyDelegate(float dt);
    //[SerializeField] public MyDelegate myDelegate;

    public GameObject testFloor = null;

    [SerializeField] private PosIntegrationType positionType = PosIntegrationType.EulerExplicit;
    [SerializeField] private RotIntegrationType rotationType = RotIntegrationType.EulerExplicit;

    [SerializeField] [Range(0.0f, 10.0f)] private float scaleX = 1.0f;
    [SerializeField] [Range(-100.0f, 100.0f)] private float rotAccZ = 0.0f;

    [SerializeField] [Range(0.0f, 20.0f)] private float frictionStatic = 0.75f;
    [SerializeField] [Range(0.0f, 20.0f)] private float frictionKinetic = 0.75f;

    [SerializeField] private float startingMass = 1.0f;

    private float prevScaleX;

    public float Mass
    {
        get
        {
            return mass;
        }

        private set
        {
            mass = value > 0.0f ? value : 0.0f;
            MassInv = mass > 0.0f ? 1.0f / mass : 0.0f;
        }
    }

    private float mass = 1.0f;

    public float MassInv { get; private set; }
    private Vector2 force = Vector2.zero;

    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;

    private float rotation;
    private float rotVelocity;
    private float rotAcceleration;
    private Vector3 helperRot;

    public void AddForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    private void Start()
    {
        Mass = startingMass;
        //Reset();
    }

    /// <summary>
    /// Supposed to reset the particle to a default state.
    /// </summary>
    private void Init()
    {
        position = Vector2.zero;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        //velocity.x = scaleX;
        prevScaleX = scaleX;
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
            //Reset();
        }

        switch (positionType)
        {
            case PosIntegrationType.EulerExplicit:
                UpdatePositionEulerExplicit(Time.fixedDeltaTime);
                break;
            default:
                UpdatePositionKinematic(Time.fixedDeltaTime);
                break;
        }

        switch (rotationType)
        {
            case RotIntegrationType.EulerExplicit:
                UpdateRotationEulerExplicit(Time.fixedDeltaTime);
                break;
            default:
                UpdateRotationKinematic(Time.fixedDeltaTime);
                break;
        }

        UpdateAcceleration();

        transform.position = position;

        Vector2 gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up);
        Vector2 normalForce = ForceGenerator.GenerateForce_Normal(-gravitationalForce, testFloor.transform.up);
        Vector2 slideForce = ForceGenerator.GenerateForce_Sliding(gravitationalForce, normalForce);
        Vector2 frictionStaticForce = ForceGenerator.GenerateForce_FrictionStatic(normalForce, slideForce, frictionStatic);
        Vector2 frictionKineticForce = ForceGenerator.GenerateForce_FrictionKinetic(normalForce, velocity, frictionKinetic);
        //Debug.Log(frictionStaticForce);
        //AddForce(gravitationalForce);
        //AddForce(normalForce);

        AddForce(slideForce);
        AddForce(frictionStaticForce);
        //AddForce(frictionKineticForce);
        //AddForce(normalForce);
        //AddForce(gravitationalForce);

        //clamps rotation to 360
        SetRotation(rotation %= 360.0f);
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

    private void UpdateAcceleration()
    {
        acceleration = force * MassInv;
        force = Vector2.zero;
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
