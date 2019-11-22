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
			angle = (angle / 180.0f) * PI;
		}
	
		w = cos(angle / 2.0f);
		v = axis.normalize() * sin(angle / 2.0f);
	}

	Quaternion::Quaternion(float values[])
	{
		v.x = values[0];
		v.y = values[1];
		v.z = values[2];
		w = values[3];
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
		v.x *= d;
		v.y *= d;
		v.z *= d;

		return *this;
	}

	Quaternion Quaternion::normalized(const Quaternion& quat)
	{
		Quaternion newQuat = Quaternion(quat);
		return newQuat.normalize();
	}

	Quaternion Quaternion::inverted(const Quaternion& quat)
	{
		Quaternion newQuat = Quaternion();
		Vector3 vec = quat.v;
		newQuat.v = -vec;
		newQuat.w = quat.w;
		return newQuat;
	}

	Vector3 Quaternion::rotate(const Vector3& vec)
	{
		Quaternion p = new Quaternion();
		p.w = 0.0f;
		p.v = vec;

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

	void Quaternion::toFloatArray(float f[]) const
	{
		f[0] = v.x;
		f[1] = v.y;
		f[2] = v.z;
		f[3] = w;
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

		Quaternion qFinal = Quaternion();
		qFinal.v = vFinal;
		qFinal.w = wFinal;
		return qFinal;
	}

	Quaternion operator*(const Quaternion& lhs, const Vector3& rhs)
	{
		float vDot = Vector3::dot(lhs.v, rhs);
		float wFinal = -vDot;

		Vector3 vCross = Vector3::cross(lhs.v, rhs);
		Vector3 vFinal = lhs.w * rhs + vCross;

		Quaternion qFinal = Quaternion();
		qFinal.v = vFinal;
		qFinal.w = wFinal;
		return qFinal;
	}

	Quaternion operator*(const float& scalar, const Quaternion& rhs)
	{
		Vector3 vFinal = scalar * rhs.v;
		float wFinal = scalar * rhs.w;
		Quaternion q = Quaternion();
		q.v = vFinal;
		q.w = wFinal;
		return q;
	}

	Quaternion operator*(const Quaternion& lhs, const float& scalar)
	{
		Vector3 vFinal = scalar * lhs.v;
		float wFinal = scalar * lhs.w;
		Quaternion q = Quaternion();
		q.v = vFinal;
		q.w = wFinal;
		return q;
	}

	Quaternion operator+(const Quaternion& lhs, const Quaternion& rhs)
	{
		Vector3 vFinal = lhs.v + rhs.v;
		float wFinal = lhs.w + rhs.w;
		Quaternion q = Quaternion();
		q.v = vFinal;
		q.w = wFinal;
		return q;
	}
}