#ifndef LIB_H_
#define LIB_H_

#ifdef UNITYPLUGINTUTORIAL_EXPORT
#define UNITYPLUGINTUTORIAL_SYMBOL __declspec(dllexport)
#else		//!UNITYPLUGINTUTORIAL_EXPORT
#ifdef UNITYPLUGINTUTORIAL_IMPORT __declspec(dllimport)
#define UNITYPLUGINTUTORIAL_SYMBOL
#else		//!UNITYPLUGINTUTORIAL_IMPORT
#define UNITYPLUGINTUTORIAL_IMPORT
#endif		//UNITYPLUGINTUTORIAL_IMPORT


#endif		//UNITYPLUGINTUTORIAL_EXPORT

#endif		//!LIB_H_