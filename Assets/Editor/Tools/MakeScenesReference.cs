using System;
using System.IO;
using System.Collections;

using UnityEngine;
using UnityEditor;


public class MakeScenesReference
{
	[MenuItem ("Tools/Make Scenes Reference")]
	public static void Run ()
	{
		Reference_Scenes_Editor.Reload ();
	}
}
