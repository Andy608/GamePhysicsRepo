#ifndef VECTOR4_H
#define VECTOR4_H

namespace ap
{
	class Vector4
	{
	private:
		float x = 0, y = 0, z = 0, w = 0;


	public:
		Vector4(float x, float y, float z, float w) { this->x = x; this->y = y; this->z = z; this->w = w; };

	};
}
#endif //!VECTOR4_H