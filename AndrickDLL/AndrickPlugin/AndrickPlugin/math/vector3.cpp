#include "vector3.h"
#include "math_util.h"

namespace ap
{
	Vector3::Vector3(const float& x, const float& y, const float& z) :
		x(x), y(y), z(z)
	{ };

	float Vector3::getMagnitude() const { return sqrt(getMagnitudeSquared()); };
	float Vector3::getMagnitudeSquared() const { return dot(*this, *this); };

	Vector3 Vector3::cross(const Vector3& lhs, const Vector3& rhs)
	{
		Vector3 vec;

		// a x b = <a2*b3 - a3*b2, a3*b1 - a1*b3, a1*b2 - a2*b1>

		vec.x = (lhs.y * rhs.z) - (lhs.z * rhs.y);
		vec.y = (lhs.z * rhs.x) - (lhs.x * rhs.z);
		vec.z = (lhs.x * rhs.y) - (lhs.y * rhs.x);

		return vec;
	}

	float Vector3::dot(const Vector3& lhs, const Vector3& rhs)
	{
		//dot = v1x * v2x + v1y * v2y + etc..
		float dot = lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;

		return dot;
	}

	const Vector3& Vector3::normalize()
	{
		Vector3 vecNorm = *this / dot(*this, *this);

		x = vecNorm.x;
		y = vecNorm.y;
		z = vecNorm.z;

		return *this;
	}

	Vector3 Vector3::normalized(const Vector3& vec)
	{
		// normV = v / |v|
		// |v| = pyfagorean

		Vector3 v = vec;

		v.x = v.x * v.x;
		v.y = v.y * v.y;
		v.z = v.z * v.z;

		float vecMag = v.x + v.y + v.z;
		vecMag = sqrt(vecMag);
		Vector3 vecNorm = vec / vecMag;

		return vecNorm;
	}

	Vector3 Vector3::operator-()
	{
		Vector3 result;
		result.x = -x;
		result.y = -y;
		result.z = -z;
		return result;
	}

	Vector3 operator+(const Vector3& lhs, const Vector3& rhs)
	{
		Vector3 result;
		result.x = lhs.x + rhs.x;
		result.y = lhs.y + rhs.y;
		result.z = lhs.z + rhs.z;
		return result;
	}

	Vector3 operator-(const Vector3& lhs, const Vector3& rhs)
	{
		Vector3 result;
		result.x = lhs.x - rhs.x;
		result.y = lhs.y - rhs.y;
		result.z = lhs.z - rhs.z;
		return result;
	}

	Vector3 operator*(const float& scalar, const Vector3& rhs)
	{
		Vector3 result;
		result.x = scalar * rhs.x;
		result.y = scalar * rhs.y;
		result.z = scalar * rhs.z;
		return result;
	}

	Vector3 operator*(const Vector3& lhs, const float& scalar)
	{
		Vector3 result;
		result.x = scalar * lhs.x;
		result.y = scalar * lhs.y;
		result.z = scalar * lhs.z;
		return result;
	}

	Vector3 operator/(const Vector3& lhs, const float& scalar)
	{
		Vector3 result;
		result.x = lhs.x / scalar;
		result.y = lhs.y / scalar;
		result.z = lhs.z / scalar;
		return result;
	}
}