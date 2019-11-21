#ifndef VECTOR3_H
#define VECTOR3_H

namespace ap
{
	class Vector3
	{
	private:
		float x = 0, y = 0, z = 0;

	public:
		Vector3(const float& x = 0.0f, const float& y = 0.0f, const float& z = 0.0f);

		//cross
		Vector3 cross(Vector3 lhs, Vector3 rhs);
		//normalize
		Vector3 normalize();
		//dot
		Vector3 dot(Vector3 lhs, Vector3 rhs);

		//overload addition, subtraction, scalar mult,
		friend Vector3 operator+(const Vector3& lhs, const Vector3& rhs);
		friend Vector3 operator-(const Vector3& lhs, const Vector3& rhs);
		friend Vector3 operator*(const float& scalar, const Vector3& rhs);
		friend Vector3 operator/(const Vector3& lhs, const float& scalar);
	};
}
#endif //!VECTOR4_H