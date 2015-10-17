/**
 * Blank editor/GUI for TBE_Environment 
 */ 

using UnityEngine;
// Copyright (c) 2015 Two Big Ears Ltd.
// All Rights Reserved
// TwoBigEars.com

using UnityEditor;
using System.Collections;

namespace TBE
{
	namespace Wwise
	{
		[CustomEditor(typeof(TBE_Environment))]
		public class EnvironmentEditor : Editor
		{
			TBE_Environment Environment;
			GUIStyle boxStyle;
			bool showHelpInfo = false;
			
			public override void OnInspectorGUI() {
				
				Environment = (TBE_Environment) target;
				
				EditorGUILayout.Space();
				
				boxStyle = new GUIStyle(GUI.skin.label);
				boxStyle.wordWrap = true;
				EditorGUILayout.LabelField("\nThis sets the world and environment properties. Make sure there is only one instance of this component in the scene.", boxStyle);
				EditorGUILayout.Space();
				
				Environment.worldScale = Mathf.Clamp(Environment.worldScale, 0.0001f, 10000);
				Environment.worldScale =  EditorGUILayout.FloatField("World Scale", Environment.worldScale);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("1 unit in the game world translated to metres.", MessageType.None);
				}
				
				Environment.speedOfSound = Mathf.Clamp(Environment.speedOfSound, 10, 6000);
				Environment.speedOfSound =  EditorGUILayout.FloatField("Speed Of Sound", Environment.speedOfSound);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The speed of sound in m/s.", MessageType.None);
				}
				
				EditorGUILayout.Space();
				
				showHelpInfo = EditorGUILayout.Toggle("Show Help", showHelpInfo);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("Isn't contextual help awesome?!", MessageType.None);
				}
				
				EditorGUILayout.Space();
				
				EditorGUILayout.LabelField("3Dception Wwise: v" + Constants.getVersion() + " — TwoBigEars.com");
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("Say hello on Twitter or Facebook, or shoot us an email!", MessageType.None);
				}
				
				if (GUI.changed)
					EditorUtility.SetDirty(Environment);
			}
		}
	}
}

