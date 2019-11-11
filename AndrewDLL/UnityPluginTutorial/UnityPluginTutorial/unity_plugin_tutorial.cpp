#include "pch.h"
#include "unity_plugin_tutorial.h"
#include "foo.h"

Foo *instance = nullptr;

bool InitFoo(int fNew)
{
	if (!instance)
	{
		instance = new Foo(fNew);
		return true;
	}

	return false;
}

int DoFoo(int bar)
{
	if (instance)
	{
		return instance->doFoo(bar);
	}

	return 0;
}

bool TerminateFoo()
{
	if (instance)
	{
		delete instance;
		instance = nullptr;
		return true;
	}

	return false;
}