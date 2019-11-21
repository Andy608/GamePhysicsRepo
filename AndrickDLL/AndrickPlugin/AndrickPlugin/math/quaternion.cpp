#include "quaternion.h"
#include "math_util.h"

namespace ap
{
	Quaternion::Quaternion(bool identity)
	{
		w = identity ? 1.0f : 0.0f;
		v = Vector3();
	}

	Quaternion::Quaternion(Vector3& axis, float& angle, bool isDegrees)
	{
		if (isDegrees)
		{
			angle = (angle / 360.0f) * PI * 2.0f;
		}
	
		w = cos(0.5f * angle);
		v = axis.normalize() * sin(0.5f * angle);
	}

	Quaternion::Quaternion(const Quaternion& other) :
		w(other.w), v(other.v)
	{
		
	}

	Quaternion Quaternion::operator=(const Quaternion& other)
	{
		w = other.w;
		v = other.v;
	}

	const Quaternion& Quaternion::normalize()
	{
		float d = getSquaredMagnitude();

		if (d < EPSILON)
		{
			w = 1.0f;
			return;
		}

		d = 1.0f / sqrt(d);
		w *= d;
		v = d * v;

		return this;
	}

	Quaternion Quaternion::normalized(const Quaternion& quat)
	{
		Quaternion newQuat = quat;
		return newQuat.normalize();
	}

	Quaternion Quaternion::inverted(const Quaternion& quat)
	{
		Quaternion newQuat = quat;
		newQuat.v = -newQuat.v;
		return newQuat;
	}

	float Quaternion::getMagnitude()
	{
		return sqrt(getSquaredMagnitude());
	}

	float Quaternion::getSquaredMagnitude() const
	{
		return dot();
	}

	float Quaternion::dot() const
	{
		return v.x * v.x + v.y * v.y + v.z * v.z + w * w;
	}

	Quaternion operator*(const Quaternion& lhs, const Quaternion& rhs)
	{
		Vector3 v1 = lhs.v;
		Vector3 v2 = rhs.v;
		float w1 = lhs.w;
		float w2 = rhs.w;

		float vDot = Vector3::dot(v1, v2);
		float wFinal = w1 * w2 - vDot;

		Vector3 vCross = Vector3::cross(v1, v2);
		Vector3 vFinal = w1 * v2 + w2 * v1 + vCross;

		Quaternion qFinal = Quaternion(vFinal, wFinal);
		return qFinal;
	}
}