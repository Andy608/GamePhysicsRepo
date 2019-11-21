#include "vector3.h"
#include "math_util.h"

namespace ap
{
	Vector3::Vector3(const float& x, const float& y, const float& z) :
		x(x), y(y), z(z)
	{ };

	Vector3 Vector3::cross(Vector3 lhs, Vector3 rhs)
	{
		Vector3 vec(0.0f, 0.0f, 0.0f);



		return vec;
	}

	Vector3 Vector3::dot(Vector3 lhs, Vector3 rhs)
	{
		Vector3 vec(0.0f, 0.0f, 0.0f);



		return vec;
	}

	Vector3 Vector3::normalize()
	{
		// normV = v / |v|
		// |v| = pyfagorean

		Vector3 vec = *this;
		float x = vec.x, y = vec.y, z = vec.z;

		x = x * x;
		y = y * y;
		z = z * z;

		float vecMag = x + y + z;
		vecMag = sqrt(vecMag);
		Vector3 vecNorm = vec / vecMag;

		return vecNorm;
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

	Vector3 operator/(const Vector3& lhs, const float& scalar)
	{
		Vector3 result;
		result.x = lhs.x / scalar;
		result.y = lhs.y / scalar;
		result.z = lhs.z / scalar;
		return result;
	}
}