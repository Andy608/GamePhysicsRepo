#ifndef UNITY_PLUGIN_TUTORIAL_H_
#define UNITY_PLUGIN_TUTORIAL_H_

#include "lib.h"

#ifdef __cplusplus	//__cplusplus
extern "C"
{
#else				//!__cplusplus

//C Mode

#endif				//__cplusplus

//C++ Mode - The functions shown in Unity
UNITYPLUGINTUTORIAL_SYMBOL bool InitFoo(int fNew);
UNITYPLUGINTUTORIAL_SYMBOL int DoFoo(int bar);
UNITYPLUGINTUTORIAL_SYMBOL bool TerminateFoo();

#ifdef __cplusplus	//__cplusplus
}
#else				//!__cplusplus

//C Mode

#endif				//__cplusplus

//C++ Mode

#endif				//!UNITY_PLUGIN_TUTORIAL_H_