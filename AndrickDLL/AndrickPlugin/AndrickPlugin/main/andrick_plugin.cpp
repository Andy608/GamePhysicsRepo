#include "andrick_plugin.h"
#include <iostream>
#include "../math/quaternion.h"
using namespace ap;

float* CreateQuaternion(bool identity)
{
	Quaternion q = Quaternion(identity);
	return q.toFloatArray();
}

float* CreateQuaternion(float* vec3, float angle, bool isDegrees)
{
	Vector3 v = Vector3(vec3[0], vec3[1], vec3[2]);
	Quaternion q = Quaternion(v, angle, isDegrees);
	return q.toFloatArray();
}

float* Normalize(float* quaternion)
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	q.normalize();
	return q.toFloatArray();
}

float* Inverted(float* quaternion)
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	q = -q;
	return q.toFloatArray();
}

float* Multiply(float* q1, float* q2)
{
	Quaternion q = (Quaternion::toQuaternion(q1) * Quaternion::toQuaternion(q2));
	return q.toFloatArray();
}

float* MultiplyWithVec(float* q1, float* vec3)
{
	Quaternion q = Quaternion::toQuaternion(q1);
	Vector3 v = Vector3::toVector(vec3);
	return (q * v).toFloatArray();
}

float* Scale(float* q1, float scalar)
{
	Quaternion q = Quaternion::toQuaternion(q1);
	return (q * scalar).toFloatArray();
}

float* Add(float* q1, float* q2)
{
	Quaternion q = (Quaternion::toQuaternion(q1) + Quaternion::toQuaternion(q2));
	return q.toFloatArray();
}

float* Rotate(float* q1, float* vec3)
{
	Quaternion q = Quaternion::toQuaternion(q1);
	Vector3 v = Vector3::toVector(vec3);
	return q.rotate(v).toFloatArray();
}

float GetMagnitude(float* quaternion)
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	return q.getMagnitude();
}

float GetMagnitudeSquared(float* quaternion)
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	return q.getSquaredMagnitude();
}

char* ToString(float* quaternion)
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	return q.toString();
}