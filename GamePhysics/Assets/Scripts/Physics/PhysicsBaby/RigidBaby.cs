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

    [SerializeField] private Vector3 position = Vector3.zero;
    public Vector3 PrevPosition { get; private set; }
    private Vector3 posDiff = Vector3.zero;

    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 acceleration = Vector3.zero;

    [SerializeField] private Vector3 rotVelocity = Vector3.zero;
    [SerializeField] private Vector3 rotAcceleration = Vector3.zero;
    private QuatBaby Rotation;

    [SerializeField] private Vector3 totalForce = Vector3.zero;

    [SerializeField] private float startingMass = 1.0f;

    //lab 07
    [SerializeField] private Matrix4x4 translationMat;
    [SerializeField] private Matrix4x4 rotationMat;
    [SerializeField] private Matrix4x4 scaleMat;
    
    [SerializeField] private Matrix4x4 transformationMat; //transformationMat = transform.localToWorldMatrix


    [SerializeField] private Vector3 localCenterOfMass, worldCenterofMass;
	[SerializeField] private Vector3 worldTorque;
	[SerializeField] private Vector3 momentArm = Vector3.zero, forceApply = Vector3.zero;
	private Matrix4x4 localInertiaTensor, inverseInertiaTensor; //these are actually 3x3 matricies
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

    private void OnEnable()
    {
        RigidBabyIntegrator.Instance?.RegisterRigidBaby(this);
    }

    private void OnDisable()
    {
        RigidBabyIntegrator.Instance?.UnRegisterRigidBaby(this);
    }

    /// <summary> Quick direct changes to Velocity. </summary>
    /// <param name="v"> The velocity vector. </param>
    public void SetVelocity(Vector3 v)
    {
        velocity = v;
    }

    /// <summary> Quick direct changes to Position. </summary>
    /// <param name="p"> The position vector. </param>
    public void SetPosition(Vector3 p)
    {
        position = p;
    }

    /// <summary> Add force to total force. D'Alembert principle. </summary>
    /// <param name="newForce"> Additional force to keep track of. </param>
    public void AddForce(Vector3 newForce)
    {
        totalForce += newForce;
    }

    /// <summary>
    /// Gets the velocity of the rigidbaby.
    /// </summary>
    /// <returns>The velocity.</returns>
    public Vector3 GetVelocity()
    {
        return velocity;
    }

    /// <summary>
    /// Gets the position of the rigidbaby.
    /// </summary>
    /// <returns>The position.</returns>
    public Vector3 GetPosition()
    {
        return position;
    }

    /// <summary>
    /// Gets the acceleration of the rigidbaby.
    /// </summary>
    /// <returns>The acceleration.</returns>
    public Vector3 GetAcceleration()
    {
        return acceleration;
    }

    void Start()
    {
        Mass = startingMass;
        position = transform.position;
        PrevPosition = transform.position;
        Rotation = QuatBaby.QuaternionToQuatBaby(transform.rotation);
        shape = GetComponent<IShapeBaby>();

        //lab7
        localInertiaTensor = shape.Inertia;

        inverseInertiaTensor = localInertiaTensor;
        inverseInertiaTensor[0]  = 1.0f / inverseInertiaTensor[0];
        inverseInertiaTensor[5]  = 1.0f / inverseInertiaTensor[5];
        inverseInertiaTensor[10] = 1.0f / inverseInertiaTensor[10];
    }

    public void Integrate()
    {
        switch (positionType)
        {
            case IntegrationType.EulerExplicit:
                UpdatePositionEulerExplicit(Time.fixedDeltaTime);
                break;
            default:
                UpdatePositionKinematic(Time.fixedDeltaTime);
                break;
        }

        switch (rotationType)
        {
            case IntegrationType.EulerExplicit:
                UpdateRotationEulerExplicit(Time.fixedDeltaTime);
                break;
            default:
                UpdateRotationKinematic(Time.fixedDeltaTime);
                break;
        }

        translationMat = new Matrix4x4(
            new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 1.0f, 0.0f),
            new Vector4(position.x, position.y, position.z, 1.0f)
        );

        UpdateAngularAcceleration();

        posDiff.x = position.x - PrevPosition.x;
        posDiff.y = position.y - PrevPosition.y;
        posDiff.z = position.z - PrevPosition.z;
        transform.position += posDiff;
        PrevPosition = position;

        transformationMat = translationMat * rotationMat;
        worldCenterofMass = transformationMat * localCenterOfMass;

        ApplyTorque(momentArm, forceApply);

        Rotation.ToMatrix(ref rotationMat);
        rotAcceleration = angularAcceleration;
        transform.rotation = Rotation.ToUnityQuaternion();
    }

    /// <summary> Integrates the particles position using the euler explicit formula. </summary>
    /// <param name="dt"> Delta time. </param>
    private void UpdatePositionEulerExplicit(float dt)
    {
        position += velocity * dt;
        velocity += acceleration * dt;
    }

    /// <summary> Integrates the particles position using the kinematic formula. </summary>
    /// <param name="dt"> Delta time. </param>
    private void UpdatePositionKinematic(float dt)
    {
        position += velocity * dt + 0.5f * acceleration * dt * dt;
        velocity += acceleration * dt;
    }

    /// <summary> Integrates the particles rotation using the euler explicit formula. </summary>
    /// <param name="dt"> Delta Time. </param>
    private void UpdateRotationEulerExplicit(float dt)
    {
        //multiply the current Rot by the velocity to get half the derivative
        QuatBaby rotDeriv = Rotation.MultiplyByVec(rotVelocity);
        //complete the derivative by multiplying it by 1/2 delta time
        rotDeriv = rotDeriv.Scale(dt * 0.5f);

        //add the new derivative to the current rotation
        Rotation = Rotation + rotDeriv;
        //normalize to remove scaling
        Rotation.normalize();

        //update the velocity
        rotVelocity += rotAcceleration * dt;
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
        angularAcceleration = transformationMat * inverseInertiaTensor * worldToLocal(worldTorque, transformationMat);

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


