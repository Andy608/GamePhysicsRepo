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
ANDRICK_PLUGIN_SYMBOL bool helloWorld();

#ifdef __cplusplus // Start __cplusplus
}
#else

// C code

#endif // End __cplusplus

// C++ Mode

#endif // End ANDRICKPLUGIN_H_