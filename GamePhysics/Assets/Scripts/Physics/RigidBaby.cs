using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBaby : MonoBehaviour
{
	/// <summary> Enum for Integration Type. </summary>
	public enum IntegrationType
	{
		Kinematic,
		EulerExplicit
	}
	[SerializeField] private IntegrationType positionType = IntegrationType.EulerExplicit;
    [SerializeField] private IntegrationType rotationType = IntegrationType.EulerExplicit;

    [SerializeField] private Vector3 Position = Vector3.zero;
    [SerializeField] private Vector3 Velocity = Vector3.zero;
    [SerializeField] private Vector3 Acceleration = Vector3.zero;

    [SerializeField] private Vector3 RotVelocity = Vector3.zero;
    [SerializeField] private Vector3 RotAcceleration = Vector3.zero;
    private QuatBaby Rotation;

    [SerializeField] private Vector3 TotalForce = Vector3.zero;

    [SerializeField] private float startingMass = 1.0f;

	//lab 07
	Matrix4x4 worldTransform, inverseWorldTransform;
	Vector3 localCenterOfMass, worldCenterofMass;
	Matrix4x4 localInertiaTensor, worldInertiaTensor; //these are actually 3x3 matricies
	Vector3 torque;
	Vector3 angularAcceleration;

    public float Mass
    {
        get
        {
            return mass;
        }

        private set
        {
            if (value > 0.0f)
            {
                mass = value;
            }
            else
            {
                mass = 0.0f;
            }

            if (mass > 0.0f)
            {
                MassInverse = 1.0f / mass;
            }
            else
            {
                MassInverse = 0.0f;
            }
        }
    }

    private float mass = 1.0f;

    public float MassInverse
    {
        get;
        private set;
    }

    /// <summary> Quick direct changes to Velocity. </summary>
    /// <param name="v"> The velocity vector. </param>
    public void SetVelocity(Vector2 v)
    {
        Velocity = v;
    }

    /// <summary> Quick direct changes to Position. </summary>
    /// <param name="p"> The position vector. </param>
    public void SetPosition(Vector2 p)
    {
        Position = p;
    }

    /// <summary> Add force to total force. D'Alembert principle. </summary>
    /// <param name="newForce"> Additional force to keep track of. </param>
    public void AddForce(Vector3 newForce)
    {
        TotalForce += newForce;
    }

    void Start()
    {
        Mass = startingMass;
        Position = transform.position;
        Rotation = QuatBaby.QuaternionToQuatBaby(transform.rotation);
    }

    void Update()
    {
        switch (positionType)
        {
            case IntegrationType.EulerExplicit:
                UpdatePositionEulerExplicit(Time.deltaTime);
                break;
            default:
                UpdatePositionKinematic(Time.deltaTime);
                break;
        }

        switch (rotationType)
        {
            case IntegrationType.EulerExplicit:
                UpdateRotationEulerExplicit(Time.deltaTime);
                break;
            default:
                UpdateRotationKinematic(Time.deltaTime);
                break;
        }

        transform.position = Position;
        transform.rotation = Rotation.ToUnityQuaternion();
    }

    /// <summary> Integrates the particles position using the euler explicit formula. </summary>
    /// <param name="dt"> Delta time. </param>
    private void UpdatePositionEulerExplicit(float dt)
    {
        Position += Velocity * dt;
        Velocity += Acceleration * dt;
    }

    /// <summary> Integrates the particles position using the kinematic formula. </summary>
    /// <param name="dt"> Delta time. </param>
    private void UpdatePositionKinematic(float dt)
    {
        Position += Velocity * dt + 0.5f * Acceleration * dt * dt;
        Velocity += Acceleration * dt;
    }

    /// <summary> Integrates the particles rotation using the euler explicit formula. </summary>
    /// <param name="dt"> Delta Time. </param>
    private void UpdateRotationEulerExplicit(float dt)
    {
        //Turn the angular velocity into a quaternion with w = 0
        QuatBaby angularVelAsQuat = new QuatBaby();
        angularVelAsQuat.v = RotVelocity;

        //multiply the current Rot by the velocity to get half the derivative
        QuatBaby rotDeriv = angularVelAsQuat * Rotation;
        //complete the derivative by multiplying it by 1/2 delta time
        rotDeriv = rotDeriv.Scale(dt * 0.5f);

        //add the new derivative to the current rotation
        Rotation = Rotation + rotDeriv;
        //normalize to remove scaling
        Rotation.normalize();

        //update the velocity
        RotVelocity += RotAcceleration * dt;
    }

    #region UpdateRotationKinematic extra credit not implemented
    private void UpdateRotationKinematic(float dt)
    {
        //Rotation += RotVelocity * dt + 0.5f * RotAcceleration * dt * dt;
        //RotVelocity += RotAcceleration * dt;
    }
    #endregion

	private void UpdateAngularAcceleration()
	{
		//angularAcceleration = Inertia^-1 * Torque

		////Apply torque to angular accel using inverse of inertia
		//angularAccel = momentOfInertiaInv * torque;

		////reset torque
		//torque = 0;
	}
}

