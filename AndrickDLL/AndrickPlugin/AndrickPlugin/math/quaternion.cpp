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
		return this;
	}

	const Quaternion& Quaternion::normalize()
	{
		float d = getSquaredMagnitude();

		if (d < EPSILON)
		{
			w = 1.0f;
			return *this;
		}

		d = 1.0f / sqrt(d);
		w *= d;
		v = d * v;

		return *this;
	}

	Quaternion Quaternion::normalized(const Quaternion& quat)
	{
		Quaternion newQuat = quat;
		return newQuat.normalize();
	}

	Quaternion Quaternion::inverted(const Quaternion& quat)
	{
		return -quat;
	}

	Quaternion Quaternion::operator-() const
	{
		Quaternion newQuat = this;
		newQuat.v = -newQuat.v;
		return newQuat;
	}

	Vector3 Quaternion::rotate(const Vector3& vec)
	{
		Quaternion p = &vec;
		Vector3 crossed = Vector3::cross(v, vec);
		return vec + crossed * (2.0f * w) + Vector3::cross(v, crossed) * 2.0f;
	}

	float Quaternion::getMagnitude() const
	{
		return sqrt(getSquaredMagnitude());
	}

	float Quaternion::getSquaredMagnitude() const
	{
		return v.x * v.x + v.y * v.y + v.z * v.z + w * w;
	}

	float* Quaternion::toFloatArray() const
	{
		float f[4] = { v.x, v.y, v.z, w };
		return f;
	}

	Quaternion Quaternion::toQuaternion(float* f)
	{
		Vector3 v = Vector3(f[0], f[1], f[2]);
		return Quaternion(v, f[3]);
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

	Quaternion operator*(const Quaternion& lhs, const Vector3& rhs)
	{
		float vDot = Vector3::dot(lhs.v, rhs);
		float wFinal = -vDot;

		Vector3 vCross = Vector3::cross(lhs.v, rhs);
		Vector3 vFinal = lhs.w * rhs + vCross;

		Quaternion qFinal = Quaternion(vFinal, wFinal);
		return qFinal;
	}

	Quaternion operator*(const float& scalar, const Quaternion& rhs)
	{
		Vector3 vFinal = scalar * rhs.v;
		float wFinal = scalar * rhs.w;
		return Quaternion(vFinal, wFinal);
	}

	Quaternion operator*(const Quaternion& lhs, const float& scalar)
	{
		Vector3 vFinal = scalar * lhs.v;
		float wFinal = scalar * lhs.w;
		return Quaternion(vFinal, wFinal);
	}

	Quaternion operator+(const Quaternion& lhs, const Quaternion& rhs)
	{
		Vector3 vFinal = lhs.v + rhs.v;
		float wFinal = lhs.w + rhs.w;
		return Quaternion(vFinal, wFinal);
	}
}