#ifndef VECTOR3_H
#define VECTOR3_H

namespace ap
{
	class Vector3
	{
	private:
		float x = 0, y = 0, z = 0;




	public:
		Vector3() { this->x = 0; this->y = 0; this->z = 0; };
		Vector3(int x, int y, int z) { this->x = x; this->y = y; this->z = z; };

		//cross
		Vector3 cross(Vector3 lhs, Vector3 rhs);
		//normalize
		Vector3 normalize();
		//dot
		Vector3 dot(Vector3 lhs, Vector3 rhs);

		//overload addition, subtraction, scalar mult, 
		static Vector3 operator+ (const Vector3& lhs, const Vector3& rhs)
		{
			Vector3 vec;
			vec.x = lhs.x + rhs.x;
			vec.y = lhs.y + rhs.y;
			vec.z = lhs.z + rhs.z;

			return vec;
		}

		static Vector3 operator- (const Vector3& lhs, const Vector3& rhs)
		{
			Vector3 vec;
			vec.x = lhs.x - rhs.x;
			vec.y = lhs.y - rhs.y;
			vec.z = lhs.z - rhs.z;

			return vec;
		}

	};
}
#endif //!VECTOR4_H