using UnityEngine;
using System.Collections;

public class Scene_Intro : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		var e = Reference_Scenes.Instance.SceneMap.GetEnumerator ();
		while (e.MoveNext ())
		{
			Debug.Log (e.Current.Key);
			Debug.Log (e.Current.Value);
		}
		Debug.Log ("VDV");
	}

	void GoLevel ()
	{
		
	}
}
