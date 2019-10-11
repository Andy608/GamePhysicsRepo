using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //public float Mass { get { return mass; } private set { mass = value > 0.0f ? value : 0.0f; MassInv = mass > 0.0f ? 1.0f / mass : 0.0f; } }
    //private float mass = 1.0f;

    //public float MassInv { private set; get; }
    //public float Damping { private set; get; }

    //public Vector2 Position;
    //public Vector2 Velocity;
    //public Vector2 Acceleration;

    //private Vector2 forceAccumulator;

    //public void Integrate(float deltaTime)
    //{
    //    if (MassInv <= 0.0f)
    //    {
    //        return;
    //    }

    //    Position += Velocity * deltaTime;

    //    Vector2 resultingAcceleration = Acceleration;
    //    resultingAcceleration += forceAccumulator * MassInv;

    //    Velocity += resultingAcceleration * deltaTime;

    //    Velocity *= Mathf.Pow(Damping, deltaTime);

    //    ClearAccumulator();
    //}

    //public bool IsMassFinite()
    //{
    //    return MassInv >= 0.0f;
    //}

    //public void ClearAccumulator()
    //{
    //    forceAccumulator = Vector2.zero;
    //}

    //public void AddForce(Vector2 force)
    //{
    //    forceAccumulator += force;
    //}

    public Vector2 Gravity = new Vector2(0.0f, 9.8f);

    [SerializeField] private Vector2 InitialVel = Vector2.zero;

    [SerializeField] private GameObject testFloor = null;
    [SerializeField] private Transform testSpringAnchor = null;

    [SerializeField] private PosIntegrationType positionType = PosIntegrationType.EulerExplicit;
    [SerializeField] private RotIntegrationType rotationType = RotIntegrationType.EulerExplicit;
    [SerializeField] private ForceType forceType = ForceType.gravity;

    //[SerializeField] [Range(0.0f, 10.0f)] private float scaleX = 1.0f;
    //[SerializeField] [Range(-100.0f, 100.0f)] private float rotAccZ = 0.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float frictionStatic = 0.75f;
    [SerializeField] [Range(0.0f, 1.0f)] private float frictionKinetic = 0.75f;
    [SerializeField] [Range(1.0f, 1.0f)] private float springRestingLength = 0.3f;
    [SerializeField] [Range(0.0f, 8.0f)] private float springStrength = 1.0f;
    [SerializeField] [Range(1.0f, 8.0f)] private float maxSpringLength = 1.0f;

    [SerializeField] private float startingMass = 1.0f;

    public float Mass { get { return mass; } private set { mass = value > 0.0f ? value : 0.0f; MassInv = mass > 0.0f ? 1.0f / mass : 0.0f; } }
    private float mass = 1.0f;

    public float MassInv { get; private set; }
    private Vector2 force = Vector2.zero;

    public Vector2 Position;
    public Vector2 PrevPosition;
    private Vector3 PosDiff = Vector3.zero;

    public Vector2 Velocity;
    public Vector2 Acceleration;

    public float Rotation;
    public float RotVelocity;
    public float RotAcceleration;
    public Vector3 helperRot;

    //lab3
    private float momentOfInertia = 0;
    private float momentOfInertiaInv;

    private float torque = 0.0f;
    private float angularAccel = 0;

    private float radius = 0.5f;

    private float radiusOuter = 1.0f;
    private float radiusInner = 0.5f;

    private float xLength = 1.0f;
    private float yLength = 0.5f;

    private float rodLength = 1.0f;

    public Vector2 pointApplied = Vector2.zero, forceApplied = Vector2.zero;
    public Vector2 centerOfMass = Vector2.zero;

    public ParticleShape particleShape = ParticleShape.disk;
    public enum ParticleShape
    {
        disk,
        ring,
        rectangle,
        thinRod,
        other
    }

    private void OnEnable()
    {
        ParticleIntegrator.Instance?.RegisterParticle(this);
    }

    private void OnDisable()
    {
        ParticleIntegrator.Instance?.UnRegisterParticle(this);
    }

    public void SetVelocity(Vector2 v)
    {
        Velocity = v;
    }

    public void SetPosition(Vector2 p)
    {
        Position = p;
    }

    public void AddForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    private void Start()
    {
        Mass = startingMass;
        Position = transform.position;
        PrevPosition = transform.position;

        Velocity = InitialVel;

        //lab3
        switch (particleShape)
        {
            case ParticleShape.ring:
                //inertia = 0.5 * mass * (radiusOuter^2 + radiusInner^2)
                momentOfInertia = 0.5f * Mass * (radiusOuter * radiusOuter + radiusInner * radiusInner);
                break;
            case ParticleShape.rectangle:
                //inertia = (0.083) * mass * (xLength^2 + yLength^2)
                momentOfInertia = 0.083f * Mass * (xLength * xLength + yLength * yLength);
                break;
            case ParticleShape.thinRod:
                //inertia = (0.083) * mass * length^2
                momentOfInertia = 0.083f * Mass * (rodLength * rodLength);
                break;
            default:
            case ParticleShape.disk:
                //inertia = 0.5 * mass * (radius^2)
                momentOfInertia = 0.5f * Mass * radius * radius;
                break;
        }

        //momentOfInertia = momentOfInertia > 0.0f ? momentOfInertia : 0.0f;
        momentOfInertiaInv = momentOfInertia > 0.0f ? 1.0f / momentOfInertia : 0.0f;
    }

    /// <summary>
    /// Supposed to reset the particle to a default state.
    /// </summary>
    private void Init()
    {
        Position = Vector2.zero;
        PrevPosition = Vector2.zero;
        Acceleration = Vector2.zero;
        Velocity = Vector2.zero;
    }

    /// <summary>
    /// Integrates the particles rotation using the kinematic formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdateRotationKinematic(float dt)
    {
        Rotation += RotVelocity * dt + 0.5f * RotAcceleration * dt * dt;
        RotVelocity += RotAcceleration * dt;
    }

    public void Integrate(float deltaTime)
    {
        switch (positionType)
        {
            case PosIntegrationType.EulerExplicit:
                UpdatePositionEulerExplicit(deltaTime);
                break;
            default:
                UpdatePositionKinematic(deltaTime);
                break;
        }

        switch (rotationType)
        {
            case RotIntegrationType.EulerExplicit:
                UpdateRotationEulerExplicit(deltaTime);
                break;
            default:
                UpdateRotationKinematic(deltaTime);
                break;
        }

        //lab03
        UpdateAngularAcceleration();
        UpdateAcceleration();

        PosDiff.x = Position.x - PrevPosition.x;
        PosDiff.y = Position.y - PrevPosition.y;
        transform.position += PosDiff;
        PrevPosition = Position;

        Vector2 gravitationalForce;// = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
        Vector2 normalForce;// = ForceGenerator.GenerateForce_Normal(-gravitationalForce, testFloor.transform.up);
        Vector2 slideForce;// = ForceGenerator.GenerateForce_Sliding(gravitationalForce, normalForce);
        Vector2 frictionForce;// = ForceGenerator.GenerateForce_Friction(normalForce, slideForce, Velocity, frictionStatic, frictionKinetic);
        Vector2 dragForce;// = ForceGenerator.GenerateForce_Drag(Velocity, new Vector2(0.2f, 0.0f), 10.0f, 10.0f, 4.0f);
        Vector2 springForce;// = ForceGenerator.GenerateForce_Spring(transform.position, testSpringAnchor.position, springRestingLength, springStrength * springStrength);
        Vector2 springDampForce;// = ForceGenerator.GenerateForce_SpringDamping(mass, Velocity, springStrength, 5.0f);
        Vector2 springMaxLengthForce;// = ForceGenerator.GenerateForce_SpringWithMax(transform.position, testSpringAnchor.position, springRestingLength, springStrength * springStrength, maxSpringLength);

        switch (forceType)
        {
            case ForceType.gravity:
                gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
                AddForce(gravitationalForce);
                break;
            case ForceType.normal:
                gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
                normalForce = ForceGenerator.GenerateForce_Normal(-gravitationalForce, testFloor.transform.up);
                AddForce(normalForce);
                break;
            case ForceType.slide:
                gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
                normalForce = ForceGenerator.GenerateForce_Normal(-gravitationalForce, testFloor.transform.up);
                slideForce = ForceGenerator.GenerateForce_Sliding(gravitationalForce, normalForce);
                AddForce(slideForce);
                break;
            case ForceType.friction:
                gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
                normalForce = ForceGenerator.GenerateForce_Normal(-gravitationalForce, testFloor.transform.up);
                slideForce = ForceGenerator.GenerateForce_Sliding(gravitationalForce, normalForce);
                frictionForce = ForceGenerator.GenerateForce_Friction(normalForce, slideForce, Velocity, frictionStatic, frictionKinetic);
                AddForce(slideForce);
                AddForce(frictionForce);
                break;
            case ForceType.drag:
                dragForce = ForceGenerator.GenerateForce_Drag(Velocity, new Vector2(0.2f, 0.0f), 10.0f, 10.0f, 4.0f);
                AddForce(dragForce);
                break;
            case ForceType.spring:
                springForce = ForceGenerator.GenerateForce_Spring(transform.position, testSpringAnchor.position, springRestingLength, springStrength * springStrength);
                AddForce(springForce);
                break;
            case ForceType.springDamping:
                springForce = ForceGenerator.GenerateForce_Spring(transform.position, testSpringAnchor.position, springRestingLength, springStrength * springStrength);
                springDampForce = ForceGenerator.GenerateForce_SpringDamping(mass, Velocity, springStrength, 5.0f);
                gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
                AddForce(springForce);
                AddForce(springDampForce);
                AddForce(gravitationalForce);
                break;
            case ForceType.springWithMaxLength:
                springMaxLengthForce = ForceGenerator.GenerateForce_SpringWithMax(transform.position, testSpringAnchor.position, springRestingLength, springStrength * springStrength, maxSpringLength);
                springDampForce = ForceGenerator.GenerateForce_SpringDamping(mass, Velocity, springStrength, 5.0f);
                gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
                AddForce(springMaxLengthForce);
                AddForce(springDampForce);
                AddForce(gravitationalForce);
                break;
            case ForceType.none:
                //Debug.Log("We ain't movin chief.");
                break;
            default:
                gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -Gravity.y, Vector2.up);
                AddForce(gravitationalForce);
                break;
        }


        //lab03		
        ApplyTorque(pointApplied, forceApplied);

        //clamps rotation to 360
        SetRotation(Rotation %= 360.0f);
        RotAcceleration = angularAccel;
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
        Acceleration = force * MassInv;
        force = Vector2.zero;
    }

    /// <summary>
    /// Integrates the particles position using the euler explicit formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdatePositionEulerExplicit(float dt)
    {
        Position += Velocity * dt;
        Velocity += Acceleration * dt;
    }

    /// <summary>
    /// Integrates the particles position using the kinematic formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdatePositionKinematic(float dt)
    {
        Position += Velocity * dt + 0.5f * Acceleration * dt * dt;
        Velocity += Acceleration * dt;
    }

    /// <summary>
    /// Integrates the particles rotation using the euler explicit formula
    /// </summary>
    /// <param name="dt"></param>
    private void UpdateRotationEulerExplicit(float dt)
    {
        Rotation += RotVelocity * dt;
        RotVelocity += RotAcceleration * dt;
    }

    //lab3
    /// <summary>
    /// Update the Angular Accel from the torque
    /// </summary>
    private void UpdateAngularAcceleration()
    {
        //angularAcceleration = Inertia^-1 * Torque

        //Apply torque to angular accel using inverse of inertia
        angularAccel = momentOfInertiaInv * torque;

        //reset torque
        torque = 0;
    }

    /// <summary>
    /// Adds torque to the aggrgate torque
    /// </summary>
    /// <param name="pointAppliedWorld"> The point the force is applied at world space </param>
    /// <param name="forceApplied"> Strength and direction of force applied. </param>
    public void ApplyTorque(Vector2 pointAppliedWorld, Vector2 forceApplied)
    {
        //Transform world space coord to local space
        Vector2 pointAppliedLocal = pointAppliedWorld - centerOfMass;

        //Torque = pointOfForceRelativeToCenterMass X forceApplied
        float miniTorque = pointAppliedLocal.x * forceApplied.x - pointAppliedLocal.y * forceApplied.y;

        //Add it to the aggregate torque
        torque += miniTorque;
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

    public enum ForceType
    {
        gravity,
        normal,
        slide,
        friction,
        drag,
        spring,
        springDamping,
        springWithMaxLength,
        none
    }
}
