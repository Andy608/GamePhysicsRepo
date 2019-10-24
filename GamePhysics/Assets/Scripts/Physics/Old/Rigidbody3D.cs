using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody3D : MonoBehaviour
{
	private Vector3 force = Vector3.zero;

	public Vector3 Position;
	public Vector3 PrevPosition;
	private Vector3 PosDiff = Vector3.zero;

	public Vector3 Velocity;
	[SerializeField] private Vector2 InitialVel = Vector2.zero;
	public Vector3 Acceleration;

	public Quaternion Rotation;
	public Vector3 RotVelocity;
	public Vector3 RotAcceleration;
	public Vector3 helperRot;

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
		PrevPosition = transform.position;

		Velocity = InitialVel;

	}

	// Update is called once per frame
	void Update()
    {
        
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



}
