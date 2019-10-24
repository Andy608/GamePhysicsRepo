using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuaternionExt
{
	public static Quaternion ScalarQuat(Quaternion quat, float scalar)
	{
		quat.x *= scalar;
		quat.y *= scalar;
		quat.z *= scalar;
		quat.w *= scalar;
		return quat;
	}

	public static Quaternion MultQuat(Quaternion q1, Quaternion q2)
	{
		//1. Break quaternions into variables, vectors, and real
		Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
		Vector3 v2 = new Vector3(q2.x, q2.y, q2.z);
		float w1 = q1.w;
		float w2 = q2.w;

		//2. calculate real part
		//-2.1 Get dot product of vectors
		float vDot = Vector3.Dot(v1, v2);

		float wFinal = w1 * w2 - vDot;

		//3. calculate vector component
		//-3.1 Get cross of vectors
		Vector3 vCross = Vector3.Cross(v1, v2);

		Vector3 vFinal = w1 * v2 + w2 * v1 + vCross;

		Quaternion qFinal = new Quaternion(vFinal.x, vFinal.y, vFinal.z, wFinal);

		return qFinal;
	}
}
