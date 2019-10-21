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
	public Vector3 Acceleration;

	public Quaternion Rotation;
	public Quaternion RotVelocity;
	public Quaternion RotAcceleration;
	public Vector3 helperRot;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
