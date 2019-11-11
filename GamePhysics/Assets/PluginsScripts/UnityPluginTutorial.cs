using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UnityPluginTutorial
{
    //Make sure the functions match the functions specified in your DLL.
    //[DllImport("The dll file name")]
     
    [DllImport("UnityPluginTutorial")]
    public static extern bool InitFoo(int fNew = 0);

    [DllImport("UnityPluginTutorial")]
    public static extern int DoFoo(int bar = 0);

    [DllImport("UnityPluginTutorial")]
    public static extern bool TerminateFoo();
}
