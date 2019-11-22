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
ANDRICK_PLUGIN_SYMBOL void CreateDefaultQuaternion(bool identity, float ref[]);
ANDRICK_PLUGIN_SYMBOL void CreateQuaternion(float vec3[], float angle, bool isDegrees, float ref[]);
ANDRICK_PLUGIN_SYMBOL void Normalize(float quaternion[], float ref[]);
ANDRICK_PLUGIN_SYMBOL void Inverted(float quaternion[], float ref[]);
ANDRICK_PLUGIN_SYMBOL void Multiply(float q1[], float q2[], float ref[]);
ANDRICK_PLUGIN_SYMBOL void MultiplyWithVec(float q1[], float vec3[], float ref[]);
ANDRICK_PLUGIN_SYMBOL void Scale(float q1[], float scalar, float ref[]);
ANDRICK_PLUGIN_SYMBOL void Add(float q1[], float q2[], float ref[]);
ANDRICK_PLUGIN_SYMBOL void Rotate(float q1[], float vec3[], float ref[]);
ANDRICK_PLUGIN_SYMBOL float GetMagnitude(float quaternion[]);
ANDRICK_PLUGIN_SYMBOL float GetMagnitudeSquared(float quaternion[]);

#ifdef __cplusplus // Start __cplusplus
}
#else

// C code

#endif // End __cplusplus

// C++ Mode

#endif // End ANDRICKPLUGIN_H_