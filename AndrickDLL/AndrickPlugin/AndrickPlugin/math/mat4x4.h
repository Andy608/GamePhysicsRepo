#ifndef MAT4X4_H
#define MAT4X4_H
#include "vector4.h"

namespace ap
{
	class Mat4x4
	{
	public:
		float m00;
		float m33;
		float m23;
		float m13;
		float m03;
		float m32;
		float m22;
		float m02;
		float m12;
		float m21;
		float m11;
		float m01;
		float m30;
		float m20;
		float m10;
		float m31;

		Mat4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3);

		//public float determinant{ get; }
		//public Matrix4x4 inverse{ get; }
		//public Matrix4x4 transpose{ get; }
		//
		//public static float Determinant(Matrix4x4 m);
		//public static Matrix4x4 Inverse(Matrix4x4 m);
		//public static Matrix4x4 Translate(Vector3 vector);
		//public static Matrix4x4 Transpose(Matrix4x4 m);
		//public bool Equals(Matrix4x4 other);
		//public override bool Equals(object other);
		//public Vector3 MultiplyVector(Vector3 vector);
	private:

	};
}
#endif