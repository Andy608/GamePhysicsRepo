using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    private static Vector2 Gravity = new Vector2(0.0f, -10.0f);

    //[SerializeField] private GameObject testFloor = null;
    //[SerializeField] private Transform testSpringAnchor = null;

    [SerializeField] private PosIntegrationType positionType = PosIntegrationType.EulerExplicit;
    [SerializeField] private RotIntegrationType rotationType = RotIntegrationType.EulerExplicit;
    //[SerializeField] private ForceType forceType = ForceType.gravity;

    //[SerializeField] [Range(0.0f, 10.0f)] private float scaleX = 1.0f;
    //[SerializeField] [Range(-100.0f, 100.0f)] private float rotAccZ = 0.0f;
    //[SerializeField] [Range(0.0f, 1.0f)] private float frictionStatic = 0.75f;
    //[SerializeField] [Range(0.0f, 1.0f)] private float frictionKinetic = 0.75f;
    //[SerializeField] [Range(1.0f, 1.0f)] private float springRestingLength = 0.3f;
    //[SerializeField] [Range(0.0f, 8.0f)] private float springStrength = 1.0f;
    //[SerializeField] [Range(1.0f, 8.0f)] private float maxSpringLength = 1.0f;

    [SerializeField] private float startingMass = 1.0f;

    public float Mass { get { return mass; } private set { mass = value > 0.0f ? value : 0.0f; MassInv = mass > 0.0f ? 1.0f / mass : 0.0f; } }
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

    public void AddForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    private void Start()
    {
        Mass = startingMass;
        position = transform.position;

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
        position = Vector2.zero;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
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

        UpdateAngularAcceleration();
        UpdateAcceleration();

        transform.position = position;

        //Vector2 gravitationalForce = ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up);
        //Vector2 normalForce = ForceGenerator.GenerateForce_Normal(-gravitationalForce, testFloor.transform.up);
        //Vector2 slideForce = ForceGenerator.GenerateForce_Sliding(gravitationalForce, normalForce);
        //Vector2 frictionForce = ForceGenerator.GenerateForce_Friction(normalForce, slideForce, velocity, frictionStatic, frictionKinetic);
        //Vector2 dragForce = ForceGenerator.GenerateForce_Drag(velocity, new Vector2(0.2f, 0.0f), 10.0f, 10.0f, 4.0f);
        //Vector2 springForce = ForceGenerator.GenerateForce_Spring(transform.position, testSpringAnchor.position, springRestingLength, springStrength * springStrength);
        //Vector2 springDampForce = ForceGenerator.GenerateForce_SpringDamping(mass, velocity, springStrength, 5.0f);
        //Vector2 springMaxLengthForce = ForceGenerator.GenerateForce_SpringWithMax(transform.position, testSpringAnchor.position, springRestingLength, springStrength * springStrength, maxSpringLength);

        //switch (forceType)
        //{
            //case ForceType.gravity:
                //AddForce(gravitationalForce);
                //break;
            //case ForceType.normal:
                //AddForce(normalForce);
                //break;
            //case ForceType.slide:
                //AddForce(slideForce);
                //break;
            //case ForceType.friction:
                //AddForce(slideForce);
                //AddForce(frictionForce);
                //break;
            //case ForceType.drag:
                //AddForce(dragForce);
                //break;
            //case ForceType.spring:
                //AddForce(springForce);
                //break;
            //case ForceType.springDamping:
                //AddForce(springForce);
                //AddForce(springDampForce);
                //AddForce(gravitationalForce);
                //break;
            //case ForceType.springWithMaxLength:
                //AddForce(springMaxLengthForce);
                //AddForce(springDampForce);
                //AddForce(gravitationalForce);
                //break;
            //case ForceType.none:
                //Debug.Log("We ain't movin chief.");
                //break;
            //default:
                //AddForce(gravitationalForce);
                //break;
        //}

        ApplyTorque(pointApplied, forceApplied);


        //clamps rotation to 360
        SetRotation(rotation %= 360.0f);
        rotAcceleration = angularAccel;
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

    //lab3
    private void UpdateAngularAcceleration()
    {
		//angularAcceleration = Inertia^-1 * Torque
		angularAccel = momentOfInertiaInv * torque;
		torque = 0;
    }

	/// <summary>
	/// Adds torque to the aggrgate torque
	/// </summary>
	/// <param name="pointApplied"> Like a point, but applied! </param>
	/// <param name="forceApplied"></param>
    private void ApplyTorque(Vector2 pointAppliedWorld, Vector2 forceApplied)
    {
		//Torque = pointOfForceRelativeToCenterMass X forceApplied
		float miniTorque = 0;

        Vector2 pointAppliedLocal = pointAppliedWorld - centerOfMass;

        miniTorque = pointAppliedLocal.x * forceApplied.x - pointAppliedLocal.y * forceApplied.y;
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
