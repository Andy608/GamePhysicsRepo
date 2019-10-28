using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody3D : MonoBehaviour
{
	private Vector3 force = Vector3.zero;
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
	[SerializeField] private PosIntegrationType positionType = PosIntegrationType.EulerExplicit;
	[SerializeField] private RotIntegrationType rotationType = RotIntegrationType.EulerExplicit;

	private Vector3 Position;
	//private Vector3 PrevPosition;
	//private Vector3 PosDiff = Vector3.zero;

	public Vector3 Velocity;
	[SerializeField] private Vector3 InitialVel = Vector3.zero;
	public Vector3 Acceleration;

	public Quaternion Rotation;
	private Quaternion helperRot;

	public Vector3 RotVelocity;
	public Vector3 RotAcceleration;

	[SerializeField] private float startingMass = 1.0f;

	public float Mass { get { return mass; } private set { mass = value > 0.0f ? value : 0.0f; MassInv = mass > 0.0f ? 1.0f / mass : 0.0f; } }
	private float mass = 1.0f;
	public float MassInv { get; private set; }


	/// <summary>
	/// Quick direct changes to Velocity.
	/// </summary>
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
		//PrevPosition = transform.position;

		Velocity = InitialVel;

	}

	// Update is called once per frame
	void Update()
    {
		switch (positionType)
		{
			case PosIntegrationType.EulerExplicit:
				UpdatePositionEulerExplicit(Time.deltaTime);
				break;
			default:
				UpdatePositionKinematic(Time.deltaTime);
				break;
		}

		switch (rotationType)
		{
			case RotIntegrationType.EulerExplicit:
				UpdateRotationEulerExplicit(Time.deltaTime);
				break;
			default:
				UpdateRotationKinematic(Time.deltaTime);
				break;
		}

		transform.position = Position;
		//SetRotation(Rotation);// %= 360.0f);//360 was for when it was float
		Quaternion thing = new Quaternion(1, 2, 3, 4);
		Vector3 thing2 = new Vector3(1, 2, 3);
		Debug.Log(thing * thing2);

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

	private void UpdateRotationEulerExplicit(float dt)
	{
		Vector3 oldAngle = transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler(RotateVector3(Rotation, oldAngle));
	}
	
	private void UpdateRotationKinematic(float dt)
	{

	}

	/// <summary>
	/// Sets rotation of euler angle without creating new vectors
	/// </summary>
	/// <param name="rot"></param>
	private void SetRotation(Quaternion rot)
	{
		helperRot = transform.rotation;
		helperRot = rot;
		transform.rotation = helperRot;
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
