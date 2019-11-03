using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBaby : MonoBehaviour
{
	public enum ParticleShape
	{
		disk,
		ring,
		rectangle,
		thinRod,
		other
	}
	public ParticleShape particleShape = ParticleShape.ring;

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
	[SerializeField] private Matrix4x4 worldTransform, inverseWorldTransform;
	[SerializeField] private Vector3 localCenterOfMass, worldCenterofMass;
	[SerializeField] private Vector3 torque;
	[SerializeField] private Vector3 momentArm = Vector3.zero, forceApply = Vector3.zero;
	private Matrix4x4 localInertiaTensor, worldInertiaTensor; //these are actually 3x3 matricies
	private Vector3 angularAcceleration;

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

		//lab7
		switch (particleShape)
		{
			case ParticleShape.ring:
				//inertia = 0.5 * mass * (radiusOuter^2 + radiusInner^2)
				localInertiaTensor = new Matrix4x4(
					new Vector4((0.4f * mass * 4), 0, 0, 0),
					new Vector4(0, (0.4f * mass * 4), 0, 0),
					new Vector4(0, 0, (0.4f * mass * 4), 0),
					new Vector4(0, 0, 0, 1)
					);
				break;
			case ParticleShape.rectangle:
				//inertia = (0.083) * mass * (xLength^2 + yLength^2)
				//momentOfInertia = 0.083f * Mass * (xLength * xLength + yLength * yLength);
				localInertiaTensor = new Matrix4x4(
					new Vector4(0.083f * mass * 2, 0, 0,0),
					new Vector4(0, 0.083f * mass * 2, 0, 0),
					new Vector4(0, 0, 0.083f * mass * 2, 0),
					new Vector4(0, 0, 0, 1)
					);
				break;
			case ParticleShape.thinRod:
				//inertia = (0.083) * mass * length^2
				//momentOfInertia = 0.083f * Mass * (rodLength * rodLength);
				break;
			default:
			case ParticleShape.disk:
				//inertia = 0.5 * mass * (radius^2)
				//momentOfInertia = 0.5f * Mass * radius * radius;
				break;
		}
	}

    void Update()
    {
		//TODO: convert position and rotation into 3D homogeneous matricies, what I have is wrong
		Vector3 position = transform.position;
		worldTransform = new Matrix4x4(
			new Vector4(position.x, 0, 0, 0),
			new Vector4(0, position.y, 0, 0),
			new Vector4(0, 0, position.z, 0),
			new Vector4(0, 0, 0, 1)
			);
		//TODO: invert that bitch


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

		ApplyTorque(momentArm, forceApply);
		UpdateAngularAcceleration();
		RotAcceleration = angularAcceleration;
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
        //multiply the current Rot by the velocity to get half the derivative
        QuatBaby rotDeriv = Rotation.MultiplyByVec(RotVelocity);
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

		Vector4 torqueT = torque;

		//TODO: use our own local to whatever matricies, and don't call .inverse
		angularAcceleration = transform.localToWorldMatrix * localInertiaTensor.inverse * transform.worldToLocalMatrix * torqueT;

		////reset torque
		torque = Vector3.zero;
	}

	/// <summary>
	/// Adds torque to the aggrgate torque
	/// </summary>
	/// <param name="pointAppliedWorld"> The point the force is applied at world space </param>
	/// <param name="forceApplied"> Strength and direction of force applied. </param>
	public void ApplyTorque(Vector3 momentArm, Vector3 forceApplied)
	{
		//Transform world space coord to local space
		Vector3 pointAppliedLocal = momentArm - localCenterOfMass;

		//Torque = pointOfForceRelativeToCenterMass X forceApplied
		//Vector3 miniTorque = pointAppliedLocal * forceApplied - pointAppliedLocal * forceApplied;
		
		//is torque equal to moment arm vector cross force vector?
		Vector3 newMiniTorque = Vector3.Cross(momentArm, forceApplied);

		//Add it to the aggregate torque
		torque += newMiniTorque;
	}
}


