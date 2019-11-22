#ifndef QUATERNION_H_
#define QUATERNION_H_

#include "vector3.h"

namespace ap
{
	class Quaternion
	{
	public:
		Quaternion(bool identity = false);
		Quaternion(Vector3& axis, float& angle, bool isDegrees = false);
		Quaternion(const Quaternion& other);
		Quaternion operator=(const Quaternion& other);

		~Quaternion() = default;

		const Quaternion& normalize();

		float getMagnitude() const;
		float getSquaredMagnitude() const;

		Vector3 rotate(const Vector3& vec);

		static Quaternion normalized(const Quaternion& quat);
		static Quaternion inverted(const Quaternion& quat);
		
		Quaternion operator-() const;
		void toFloatArray(float f[]) const;
		static Quaternion toQuaternion(float f[]);

		friend Quaternion operator*(const Quaternion& lhs, const Quaternion& rhs);
		friend Quaternion operator*(const Quaternion& lhs, const Vector3& rhs);
		friend Quaternion operator*(const float& scalar, const Quaternion& rhs);
		friend Quaternion operator*(const Quaternion& lhs, const float& scalar);
		friend Quaternion operator+(const Quaternion& lhs, const Quaternion& rhs);

	//private:
		static Quaternion identity;

		float w;
		Vector3 v;
	};

}

#endif // End QUATERNION_H_