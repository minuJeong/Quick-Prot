using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;


public enum SCENEKEY
{
	Intro = 0,
	Home,
	Game
}


[Serializable]
public class Reference_Scenes : ScriptableObject
{
	private static Reference_Scenes _Instance;

	public static Reference_Scenes Instance
	{
		get
		{
			return _Instance;
		}
	}

	public Dictionary<SCENEKEY, string> SceneMap = new Dictionary<SCENEKEY, string> ();

	public Reference_Scenes ()
	{
		_Instance = this;
	}
}

#if UNITY_EDITOR
[CustomEditor (typeof(Reference_Scenes))]
public class Reference_Scenes_Editor : Editor
{
	const string SCENES_REFERENCE_PATH_DIR = "Assets/Resources/Reference/Scene";
	const string SCENES_REFERENCE_PATH_FILE = "SceneReference.asset";
	const string SCENES_PATH = "Assets/Scenes/";

	// reload scenes list and save as asset
	public static void Reload ()
	{
		if (!Directory.Exists (SCENES_REFERENCE_PATH_DIR))
		{
			Debug.Log ("Created a new directory");
			Directory.CreateDirectory (SCENES_REFERENCE_PATH_DIR);
		}

		string path = string.Format (
			              "{0}/{1}",
			              SCENES_REFERENCE_PATH_DIR,
			              SCENES_REFERENCE_PATH_FILE
		              );

		Reference_Scenes newSceneReference = ScriptableObject.CreateInstance<Reference_Scenes> ();
		AssetDatabase.CreateAsset (newSceneReference, path);
		Array.ForEach (Directory.GetFiles (SCENES_PATH), (filename) =>
		{
			if (filename.EndsWith (".meta"))
			{
				return;
			}

			string sceneKeyName = new FileInfo (filename)
				.Name
				.Replace (".unity", "")
				.Split ('_') [1];

			if (!Enum.IsDefined (typeof(SCENEKEY), sceneKeyName))
			{
				Debug.Log (string.Format ("scene {0} is not defined.", filename));
				return;
			}
			SCENEKEY sceneKey = (SCENEKEY)Enum.Parse (typeof(SCENEKEY), sceneKeyName);
			newSceneReference.SceneMap.Add (
				sceneKey, filename
			);
		});

		// Select created asset
		Selection.activeObject = newSceneReference;
	}

	// custom inspector
	public override void OnInspectorGUI ()
	{
		Reference_Scenes typedTarget = (Reference_Scenes)target;
		Debug.Assert (typedTarget != null);
		if (typedTarget.SceneMap.Keys.Count == 0)
		{
			Reload ();
			return;
		}

		GUIStyle header1 = new GUIStyle ();
		header1.fontSize = 16;

		GUIStyle header2 = new GUIStyle ();
		header2.fontSize = 14;

		GUIStyle body = new GUIStyle ();
		body.fontSize = 12;

		GUILayout.Label ("Scene Reference", header1);

		GUILayout.Label ("Tools");
		if (GUILayout.Button ("Reload",
			    GUILayout.Width (50),
			    GUILayout.Height (50)))
		{
			Reload ();
			return;
		}

		GUILayout.Label ("Data", header2);

		var e = typedTarget.SceneMap.GetEnumerator ();
		while (e.MoveNext ())
		{
			if (GUILayout.Button (e.Current.Value))
			{
				EditorSceneManager.OpenScene (e.Current.Value);
			}
		}
	}
}
#endif