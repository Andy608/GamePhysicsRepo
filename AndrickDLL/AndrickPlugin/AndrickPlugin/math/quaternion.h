#ifndef QUATERNION_H_
#define QUATERNION_H_

namespace ap
{
	class Vector3;

	class Quaternion
	{
	public:
		Quaternion(bool identity = false);
		//Quaternion(Vector3 axis, float angle, float isDegrees = false);

		~Quaternion() = default;



	private:
		static Quaternion identity;

		float w;
		//Vector3* v;
	};

}

#endif // End QUATERNION_H_