using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IShapeBaby))]
public class RigidBaby : MonoBehaviour
{
    private IShapeBaby shape;

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
    [SerializeField] private Matrix4x4 translationMat;
    [SerializeField] private Matrix4x4 rotationMat;
    [SerializeField] private Matrix4x4 scaleMat;
    
    [SerializeField] private Matrix4x4 transformationMat; //transformationMat = transform.localToWorldMatrix


    [SerializeField] private Vector3 localCenterOfMass, worldCenterofMass;
	[SerializeField] private Vector3 worldTorque;
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
        shape = GetComponent<IShapeBaby>();

        //lab7
        //switch (particleShape)
        //{
        //case ParticleShape.ring:
        //    inertia = 0.4f * mass * 4.0f; //inertia = 0.5 * mass * (radiusOuter^2 + radiusInner^2)
        //	break;
        //case ParticleShape.rectangle:
        //    inertia = 0.083f * mass * 2.0f; //inertia = (0.083) * mass * (xLength^2 + yLength^2)
        //	break;
        //default:
        //case ParticleShape.disk:
        //    inertia = 0.5f * mass * 4.0f; //inertia = 0.5 * mass * (radius^2)
        //	break;
        //}

        //localInertiaTensor = new Matrix4x4(
        //    new Vector4(inertia,    0.0f,    0.0f,    0.0f),
        //    new Vector4(   0.0f, inertia,    0.0f,    0.0f),
        //    new Vector4(   0.0f,    0.0f, inertia,    0.0f),
        //    new Vector4(   0.0f,    0.0f,    0.0f,    1.0f)
        //);

        localInertiaTensor = shape.Inertia;

        worldInertiaTensor = localInertiaTensor;
        worldInertiaTensor[0] = 1.0f / worldInertiaTensor[0];
        worldInertiaTensor[5] = 1.0f / worldInertiaTensor[5];
        worldInertiaTensor[10] = 1.0f / worldInertiaTensor[10];
    }

    void Update()
    {
		//TODO: convert position and rotation into 3D homogeneous matricies, what I have is wrong
		Vector3 position = transform.position;
        translationMat = new Matrix4x4(
            new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
            new Vector4(position.x, position.y, position.z, 1.0f)
        );

        Rotation.ToMatrix(ref rotationMat);
        //Debug.Log(Rotation + " | " + rotationMat);

        Vector3 scale = transform.lossyScale;
        scaleMat = new Matrix4x4(
            new Vector4(scale.x,    0.0f,    0.0f,    0.0f),
            new Vector4(   0.0f, scale.y,    0.0f,    0.0f),
            new Vector4(   0.0f,    0.0f, scale.z,    0.0f),
            new Vector4(   0.0f,    0.0f,    0.0f,    1.0f)
        );

        transformationMat = translationMat * rotationMat * scaleMat;
        worldCenterofMass = transformationMat * localCenterOfMass;

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
        //Unity version
        //angularAcceleration = transform.localToWorldMatrix * localInertiaTensor.inverse * transform.worldToLocalMatrix * torque4;
        
        //Our version v1.0
        angularAcceleration = transformationMat * worldInertiaTensor * worldToLocal(worldTorque, transformationMat);

        /* !! BUT if inertia tensor is uniform scale, the change of basis can cancel out. (Provides the same results as line above) !! */
        //Optimized v2.0
        //angularAcceleration = worldInertiaTensor * worldTorque;

        //Reset torque
        worldTorque = Vector3.zero;
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

		//Add it to the aggregate torque
		worldTorque += Vector3.Cross(pointAppliedLocal, forceApplied);
	}

    //Multiplies a vector by the inverse of a matrix -> when that matrix only comprises of translation and rotation.
    private Vector3 transformInverse(Vector3 vec, Matrix4x4 mat)
    {
        Vector3 temp = vec;
        temp.x -= mat[3];
        temp.y -= mat[7];
        temp.z -= mat[11];

        return new Vector3(
            temp.x * mat[0] + temp.y * mat[4] + temp.z * mat[8],
            temp.x * mat[1] + temp.y * mat[5] + temp.z * mat[9],
            temp.x * mat[2] + temp.y * mat[6] + temp.z * mat[10]
        );
    }

    private Vector3 worldToLocal(Vector3 world, Matrix4x4 transform)
    {
        return transformInverse(world, transform);
    }

    private Vector3 localToWorld(Vector3 local, Matrix4x4 transform)
    {
        return transform * local;
    }
}


