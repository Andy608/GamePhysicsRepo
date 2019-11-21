#include "quaternion.h"
#include "vector3.h"
#include "math_util.h"

namespace ap
{
	Quaternion::Quaternion(bool identity)
	{
		w = identity ? 1.0f : 0.0f;
		//v = Vector3.zero;
	}

	//Quaternion::Quaternion(Vector3 axis, float angle, bool isDegrees)
	//{
	//	if (isDegrees)
	//	{
	//		angle = (angle / 360.0f) * PI * 2.0f;
	//	}
	//
	//	w = cos(0.5f * angle);
	//	//v = axis.normalize() * sin(0.5f * angle);
	//}
}