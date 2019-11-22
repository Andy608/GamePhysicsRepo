#include "andrick_plugin.h"
#include "../math/vector3.h"
#include <iostream>
#include "../math/quaternion.h"
using namespace ap;

static float* helper = new float[4]();

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
	Quaternion q = Quaternion::toQuaternion(quaternion);
	q.normalize();

	q.toFloatArray(ref);
}

void Inverted(float quaternion[], float ref[])
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	q = -q;

	q.toFloatArray(ref);
}

void Multiply(float q1[], float q2[], float ref[])
{
	Quaternion q = (Quaternion::toQuaternion(q1) * Quaternion::toQuaternion(q2));
	q.toFloatArray(ref);
}

void MultiplyWithVec(float q1[], float vec3[], float ref[])
{
	Quaternion q = Quaternion::toQuaternion(q1);
	Vector3 v = Vector3::toVector(vec3);
	q = (q * v);
	q.toFloatArray(ref);
}

void Scale(float q1[], float scalar, float ref[])
{
	Quaternion q = Quaternion::toQuaternion(q1);
	q = q * scalar;
	q.toFloatArray(ref);
}

void Add(float q1[], float q2[], float ref[])
{
	Quaternion q = (Quaternion::toQuaternion(q1) + Quaternion::toQuaternion(q2));
	q.toFloatArray(ref);
}

void Rotate(float q1[], float vec3[], float ref[])
{
	Quaternion q = Quaternion::toQuaternion(q1);
	Vector3 v = Vector3::toVector(vec3);
	v = q.rotate(v);
	q.toFloatArray(ref);
}

float GetMagnitude(float quaternion[])
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	return q.getMagnitude();
}

float GetMagnitudeSquared(float quaternion[])
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	return q.getSquaredMagnitude();
}

void AndrewTest(float nums[])
{
	Vector3 vec;
	vec.AndrewTest(nums);
}
