#include "pch.h"
#include "foo.h"

Foo::Foo(int newF) : f(newF)
{

}

int Foo::doFoo(int bar)
{
	return (bar + 1);
}