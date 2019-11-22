#ifndef ANDRICK_PLUGIN_H_
#define ANDRICK_PLUGIN_H_

#include "lib.h"

// If C++ is being used in the DLL
#ifdef __cplusplus
extern "C"
{
#else

// C code

#endif // End __cplusplus

// C++ code exposed to user program.
ANDRICK_PLUGIN_SYMBOL float* CreateQuaternion(bool identity);
ANDRICK_PLUGIN_SYMBOL float* CreateQuaternion(float* vec3, float angle, bool isDegrees);
ANDRICK_PLUGIN_SYMBOL float* Normalize(float* quaternion);
ANDRICK_PLUGIN_SYMBOL float* Inverted(float* quaternion);
ANDRICK_PLUGIN_SYMBOL float* Multiply(float* q1, float* q2);
ANDRICK_PLUGIN_SYMBOL float* MultiplyWithVec(float* q1, float* vec3);
ANDRICK_PLUGIN_SYMBOL float* Scale(float* q1, float scalar);
ANDRICK_PLUGIN_SYMBOL float* Add(float* q1, float* q2);
ANDRICK_PLUGIN_SYMBOL float* Rotate(float* q1, float* vec3);
ANDRICK_PLUGIN_SYMBOL float GetMagnitude(float* quaternion);
ANDRICK_PLUGIN_SYMBOL float GetMagnitudeSquared(float* quaternion);
ANDRICK_PLUGIN_SYMBOL char* ToString(float* quaternion);

#ifdef __cplusplus // Start __cplusplus
}
#else

// C code

#endif // End __cplusplus

// C++ Mode

#endif // End ANDRICKPLUGIN_H_