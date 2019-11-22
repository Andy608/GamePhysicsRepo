#ifndef VECTOR3_H
#define VECTOR3_H

namespace ap
{
	class Vector3
	{
		friend class Quaternion;
	public:
		Vector3(const float& x = 0.0f, const float& y = 0.0f, const float& z = 0.0f);

		float getMagnitude() const;
		float getMagnitudeSquared() const;

		//cross
		static Vector3 cross(const Vector3& lhs, const Vector3& rhs);
		//normalize
		static Vector3 normalized(const Vector3& vec);
		const Vector3& normalize();
		//dot
		static float dot(const Vector3& lhs, const Vector3& rhs);

		//Local operators: negate.
		Vector3 operator-();
		float* toFloatArray() const;
		static Vector3 toVector(float* vec3);


		//overload addition, subtraction, scalar mult,
		friend Vector3 operator+(const Vector3& lhs, const Vector3& rhs);
		friend Vector3 operator-(const Vector3& lhs, const Vector3& rhs);
		friend Vector3 operator*(const float& scalar, const Vector3& rhs);
		friend Vector3 operator*(const Vector3& lhs, const float& scalar);
		friend Vector3 operator/(const Vector3& lhs, const float& scalar);

	private:
		float x = 0, y = 0, z = 0;

	};
}
#endif //!VECTOR4_H