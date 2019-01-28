using UnityEngine;
using System.Collections;
using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(House))]
public class HouseEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		House myScript = (House)target;
		if(GUILayout.Button("Next state"))
		{
			myScript.NextState();
		}
		if(GUILayout.Button("Deal 20 damage"))
		{
			myScript.TakeDamage(20f);
		}
	}
    
}
#endif