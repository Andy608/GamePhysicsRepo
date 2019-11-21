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

		float getMagnitude();
		float getSquaredMagnitude() const;
		float dot() const;

		static Quaternion normalized(const Quaternion& quat);
		static Quaternion inverted(const Quaternion& quat);

		friend Quaternion operator*(const Quaternion& lhs, const Quaternion& rhs);

	private:
		static Quaternion identity;

		float w;
		Vector3 v;
	};

}

#endif // End QUATERNION_H_