#include "andrick_plugin.h"
#include "../math/vector3.h"
#include <iostream>
#include "../math/quaternion.h"
using namespace ap;

void CreateDefaultQuaternion(bool identity, float ref[])
{
	Quaternion q = Quaternion(identity);
	
	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void CreateQuaternion(float vec3[], float angle, bool isDegrees, float ref[])
{
	Vector3 v = Vector3(vec3[0], vec3[1], vec3[2]);
	Quaternion q = Quaternion(v, angle, isDegrees);
	
	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void Normalize(float quaternion[], float ref[])
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	q.normalize();

	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void Inverted(float quaternion[], float ref[])
{
	Quaternion q = Quaternion::toQuaternion(quaternion);
	q = -q;

	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void Multiply(float q1[], float q2[], float ref[])
{
	Quaternion q = (Quaternion::toQuaternion(q1) * Quaternion::toQuaternion(q2));
	
	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void MultiplyWithVec(float q1[], float vec3[], float ref[])
{
	Quaternion q = Quaternion::toQuaternion(q1);
	Vector3 v = Vector3::toVector(vec3);
	q = (q * v);
	
	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void Scale(float q1[], float scalar, float ref[])
{
	Quaternion q = Quaternion::toQuaternion(q1);
	q = q * scalar;
	
	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void Add(float q1[], float q2[], float ref[])
{
	Quaternion q = (Quaternion::toQuaternion(q1) + Quaternion::toQuaternion(q2));
	
	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
	ref[3] = q.w;
}

void Rotate(float q1[], float vec3[], float ref[])
{
	Quaternion q = Quaternion::toQuaternion(q1);
	Vector3 v = Vector3::toVector(vec3);
	v = q.rotate(v);
	
	ref[0] = q.v.x;
	ref[1] = q.v.y;
	ref[2] = q.v.z;
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
