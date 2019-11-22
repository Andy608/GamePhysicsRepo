#include "vector4.h"
#include "math_util.h"

namespace ap
{
	Vector4::Vector4(const float& x, const float& y, const float& z, const float& w) :
		x(x), y(y), z(z), w(w)
	{ };

	float Vector4::getMagnitude() const 
	{ 
		return sqrt(getMagnitudeSquared()); 
	}

	float Vector4::getMagnitudeSquared() const 
	{ 
		return dot(*this, *this); 
	}

	//TODO: stil needs implementation
	Vector4 Vector4::cross(const Vector4& lhs, const Vector4& rhs)
	{
		Vector4 vec;

		// a x b = <a2*b3 - a3*b2, a3*b1 - a1*b3, a1*b2 - a2*b1>

		vec.x = (lhs.y * rhs.z) - (lhs.z * rhs.y);
		vec.y = (lhs.z * rhs.x) - (lhs.x * rhs.z);
		vec.z = (lhs.x * rhs.y) - (lhs.y * rhs.x);

		return vec;
	}

	float Vector4::dot(const Vector4& lhs, const Vector4& rhs)
	{
		//dot = v1x * v2x + v1y * v2y + etc..
		float dot = lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w;

		return dot;
	}

	const Vector4& Vector4::normalize()
	{
		Vector4 vecNorm = *this / dot(*this, *this);

		x = vecNorm.x;
		y = vecNorm.y;
		z = vecNorm.z;
		w = vecNorm.w;

		return *this;
	}

	Vector4 Vector4::normalized(const Vector4& vec)
	{
		// normV = v / |v|
		// |v| = pyfagorean

		Vector4 v = vec;

		v.x = v.x * v.x;
		v.y = v.y * v.y;
		v.z = v.z * v.z;
		v.w = v.w * v.w;

		float vecMag = v.x + v.y + v.z + v.w;
		vecMag = sqrt(vecMag);
		Vector4 vecNorm = vec / vecMag;

		return vecNorm;
	}

	Vector4 operator+(const Vector4& lhs, const Vector4& rhs)
	{
		Vector4 result;
		result.x = lhs.x + rhs.x;
		result.y = lhs.y + rhs.y;
		result.z = lhs.z + rhs.z;
		result.w = lhs.w + rhs.w;
		return result;
	}

	Vector4 operator-(const Vector4& lhs, const Vector4& rhs)
	{
		Vector4 result;
		result.x = lhs.x - rhs.x;
		result.y = lhs.y - rhs.y;
		result.z = lhs.z - rhs.z;
		result.w = lhs.w - rhs.w;
		return result;
	}

	Vector4 operator*(const float& scalar, const Vector4& rhs)
	{
		Vector4 result;
		result.x = scalar * rhs.x;
		result.y = scalar * rhs.y;
		result.z = scalar * rhs.z;
		result.w = scalar * rhs.w;
		return result;
	}

	Vector4 operator/(const Vector4& lhs, const float& scalar)
	{
		Vector4 result;
		result.x = lhs.x / scalar;
		result.y = lhs.y / scalar;
		result.z = lhs.z / scalar;
		result.w = lhs.w / scalar;
		return result;
	}
}
