using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody3D : MonoBehaviour
{
    /// <summary> Enum for Integration Type. </summary>
    public enum IntegrationType
    {
        Kinematic,
        EulerExplicit
    }

    private Vector3 force = Vector3.zero;

	[SerializeField] private IntegrationType positionType = IntegrationType.EulerExplicit;
	[SerializeField] private IntegrationType rotationType = IntegrationType.EulerExplicit;

	private Vector3 Position;

	public Vector3 Velocity;
	[SerializeField] private Vector3 InitialVel = Vector3.zero;

	public Vector3 Acceleration;

    //private Quaternion Rotation;
    private QuatBaby Rotation;

    public Vector3 RotVelocity = Vector3.zero;
	public Vector3 RotAcceleration;

	[SerializeField] private float startingMass = 1.0f;

	public float Mass { get { return mass; } private set { mass = value > 0.0f ? value : 0.0f; MassInv = mass > 0.0f ? 1.0f / mass : 0.0f; } }
	private float mass = 1.0f;
	public float MassInv { get; private set; }


	/// <summary> Quick direct changes to Velocity. </summary> 
    /// <param name="v"></param>
	public void SetVelocity(Vector2 v)
	{
		Velocity = v;
	}
	/// <summary>
	/// Quick direct changes to Position.
	/// </summary>
	/// <param name="v"></param>
	public void SetPosition(Vector2 p)
	{
		Position = p;
	}

	public void AddForce(Vector3 newForce)
	{
		//D'Alembert
		force += newForce;
	}

	// Start is called before the first frame update
	void Start()
    {
		Mass = startingMass;
		Position = transform.position;

		Velocity = InitialVel;
        Rotation = new QuatBaby(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z), transform.rotation.w);
	}

	// Update is called once per frame
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

        //Debug.Log("ROT: " + Rotation);
        transform.rotation = Rotation.ToUnityQuaternion();
        //transform.rotation = Rotation;

        //SetRotation(Rotation);// %= 360.0f);//360 was for when it was float
        Quaternion thing = new Quaternion(1, 2, 3, 4);
		Vector3 thing2 = new Vector3(1, 2, 3);
		//Debug.Log(thing * thing2);
	}

	//Pre: Implement operators for multiplying quaternion by scalar
	//Pre: Implement function for multiplying 3d vector by quaternion
	//1.Implement Euler for velocity and position
	//2.Implement Euler for rotation and angular velocity
	//3.Implement Kinematic for velocity and position 
	//Bonus: Implement Kinematic for rotation and angular velocity
	//Bonus: Implement our own quaternion class

	/// <summary> Integrates the particles position using the euler explicit formula </summary>
	/// <param name="dt"></param>
	private void UpdatePositionEulerExplicit(float dt)
	{
		Position += Velocity * dt;
		Velocity += Acceleration * dt;
	}
	/// <summary> Integrates the particles position using the kinematic formula </summary>
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
        //Vector3 oldAngle = transform.rotation.eulerAngles;
        //transform.rotation = Quaternion.Euler(RotateVector3(Rotation, oldAngle));

        //Quat qr = new Quat(RotVelocity, 0);
        //qr.normalize();
        //nextRot = currentRot; // + currentRot.Scale(dt * 0.5f) * qr;
        //Debug.Log(currentRot);
        //nextRot.normalize();

        QuatBaby angularVelAsQuat = new QuatBaby();
        angularVelAsQuat.v = RotVelocity;

        QuatBaby rotDeriv = angularVelAsQuat * Rotation;

        rotDeriv = rotDeriv.Scale(dt * 0.5f);
        Rotation = Rotation + rotDeriv;
        Rotation.normalize();

        RotVelocity += RotAcceleration * dt;

        //Turn the angular velocity into a quaternion with w = 0
        //Quaternion angularVelAsQuat = new Quaternion(RotVelocity.x, RotVelocity.y, RotVelocity.z, 0);

        //multiply the current Rot by the velocity to get half the derivative
        //Quaternion rotDeriv = angularVelAsQuat * Rotation;
        //complete the derivative by multiplying it by 1/2 delta time
        //rotDeriv = QuaternionExt.ScalarQuat(rotDeriv, dt * 0.5f);

        //add the new derivative to the current rotation
        //Rotation = new Quaternion(Rotation.x + rotDeriv.x, Rotation.y + rotDeriv.y, Rotation.z + rotDeriv.z, Rotation.w + rotDeriv.w);
        //normalize to remove scaling
        //Rotation = Quaternion.Normalize(Rotation);

        //float a = Mathf.Cos(RotVelocity.magnitude * Time.deltaTime / 2.0f);
        //Vector3 v = Mathf.Sin(RotVelocity.magnitude * Time.deltaTime / 2.0f) * RotVelocity / RotVelocity.magnitude;
        //nextRot = new Quat(v, a);
        //currentRot = currentRot * nextRot;
    }
	
	private void UpdateRotationKinematic(float dt)
	{
		//Rotation += RotVelocity * dt + 0.5f * RotAcceleration * dt * dt;
		//RotVelocity += RotAcceleration * dt;

	}

	private Vector3 RotateVector3(Quaternion q, Vector3 v)
	{
		Vector3 vFinal = Vector3.zero;

		Vector3 r = new Vector3(v.x, v.y, v.z);
		float w = q.w;

		Vector3 lhs = v + 2 * r;

		Vector3 rhs = (Vector3.Cross(r, v) + (w * v));

		vFinal = Vector3.Cross(lhs, rhs);

		return vFinal;
	}
}
