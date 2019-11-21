#ifndef LIB_H_
#define LIB_H_

// Export DLL and define Symbol as export
#ifdef ANDRICKPLUGIN_EXPORT
#define ANDRICK_PLUGIN_SYMBOL __declspec(dllexport)
#else
#ifdef ANDRICKPLUGIN_IMPORT __declspec(dllimport)

// Define Symbol as import (in a user program)
#define ANDRICKPLUGIN_SYMBOL
#else

// Define Import if not already defined
#define ANDRICKPLUGIN_IMPORT
#endif // End ANDRICKPLUGIN_IMPORT

#endif // End ANDRICKPLUGIN_EXPORT

#endif // End LIB_H_