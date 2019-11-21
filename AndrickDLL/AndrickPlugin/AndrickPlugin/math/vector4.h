#ifndef VECTOR4_H
#define VECTOR4_H

namespace ap
{
	class Vector4
	{

	public:
		Vector4(const float& x = 0.0f, const float& y = 0.0f, const float& z = 0.0f, const float& w = 0.0f);

		float getMagnitude() const { sqrt(getMagnitudeSquared()); };
		float getMagnitudeSquared() const { return dot(*this, *this); };

		//cross
		static Vector4 cross(const Vector4& lhs, const Vector4& rhs);
		//normalize
		static Vector4 normalized(const Vector4& vec);
		const Vector4& normalize();
		//dot
		static float dot(const Vector4& lhs, const Vector4& rhs);

		//overload addition, subtraction, scalar mult,
		friend Vector4 operator+(const Vector4& lhs, const Vector4& rhs);
		friend Vector4 operator-(const Vector4& lhs, const Vector4& rhs);
		friend Vector4 operator*(const float& scalar, const Vector4& rhs);
		friend Vector4 operator/(const Vector4& lhs, const float& scalar);

	private:
		float x = 0, y = 0, z = 0, w = 0;
	};
}
#endif //!VECTOR4_H