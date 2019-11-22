#include "andrick_plugin.h"
#include "../math/vector3.h"
#include <iostream>
#include "../math/quaternion.h"
using namespace ap;

void CreateDefaultQuaternion(bool identity, float ref[])
{
	Quaternion q = Quaternion(identity);
	q.toFloatArray(ref);
}

void CreateQuaternion(float vec3[], float angle, bool isDegrees, float ref[])
{
	Vector3 v = Vector3(vec3[0], vec3[1], vec3[2]);
	Quaternion q = Quaternion(v, angle, isDegrees);
	q.toFloatArray(ref);
}

void Normalize(float quaternion[], float ref[])
{
	Quaternion q = Quaternion(quaternion);
	q.normalize();

	q.toFloatArray(ref);
}

void Inverted(float quaternion[], float ref[])
{
	Quaternion q = Quaternion(quaternion);
	Quaternion::inverted(q).toFloatArray(ref);
}

void Multiply(float q1[], float q2[], float ref[])
{
	Quaternion q = (Quaternion(q1) * Quaternion(q2));
	q.toFloatArray(ref);
}

void MultiplyWithVec(float q1[], float vec3[], float ref[])
{
	Quaternion q = Quaternion(q1);
	Vector3 v = Vector3(vec3);
	q = (q * v);
	q.toFloatArray(ref);
}

void Scale(float q1[], float scalar, float ref[])
{
	Quaternion q = Quaternion(q1);
	q = q * scalar;
	q.toFloatArray(ref);
}

void Add(float q1[], float q2[], float ref[])
{
	Quaternion q = (Quaternion(q1) + Quaternion(q2));
	q.toFloatArray(ref);
}

void Rotate(float q1[], float vec3[], float ref[])
{
	Quaternion q = Quaternion(q1);
	Vector3 v = Vector3(vec3);
	v = q.rotate(v);
	v.toFloatArray(ref);
}

float GetMagnitude(float quaternion[])
{
	Quaternion q = Quaternion(quaternion);
	return q.getMagnitude();
}

float GetMagnitudeSquared(float quaternion[])
{
	Quaternion q = Quaternion(quaternion);
	return q.getSquaredMagnitude();
}

void AndrewTest(float nums[])
{
	Vector3 vec;
	vec.AndrewTest(nums);
}
