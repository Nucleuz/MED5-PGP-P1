// Copyright (c) 2015 Two Big Ears Ltd.
// All Rights Reserved
// TwoBigEars.com

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TBE
{
	namespace Wwise
	{
		[CustomEditor(typeof(TBE_RoomProperties))]
		public class RoomEditor : Editor {
			
			bool showHelpInfo = false;
			
			TBE_RoomProperties RoomProps;
			
			private string[] pivotSelection = {"Corner", "Center"};
			string[] options = new string[]
			{"Custom",
			"Default",
			"Glass",
			"Concrete",
			"Recording Studio",
			"Living Room",
			"Outdoor"};
			
			public override void OnInspectorGUI() {
				
				RoomProps = (TBE_RoomProperties) target;
				
				EditorGUILayout.Space();
				
				RoomProps.hfReflections = EditorGUILayout.Slider("HF Reflections", RoomProps.hfReflections, 0, 1);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amount of high frequencies reflected by the room. A value of 1 results in maximum reflection.", MessageType.None);
				}
				RoomProps.erLevel = EditorGUILayout.Slider("ER Level", RoomProps.erLevel, 0, 2);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amplitude of the reflections in the room.", MessageType.None);
				}
				
				EditorGUILayout.Space();
				
				RoomProps.roomPreset = (TBRoomPresets) EditorGUILayout.Popup("Reflection Presets", (int) RoomProps.roomPreset, options);
				
				EditorGUI.indentLevel = 1;
				
				if (RoomProps.roomPreset != TBRoomPresets.Custom)
				{
					GUI.enabled = false;
				}
				
				RoomProps.reflectionLWall = EditorGUILayout.Slider("Left Wall", RoomProps.reflectionLWall, 0, 1);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amplitude of the reflections off the left wall in the room.", MessageType.None);
				}
				
				RoomProps.reflectionRWall = EditorGUILayout.Slider("Right Wall", RoomProps.reflectionRWall, 0, 1);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amplitude of the reflections off the right wall in the room.", MessageType.None);
				}
				
				RoomProps.reflectionFWall = EditorGUILayout.Slider("Front Wall", RoomProps.reflectionFWall, 0, 1);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amplitude of the reflections off the front wall in the room.", MessageType.None);
				}
				
				RoomProps.reflectionBWall = EditorGUILayout.Slider("Back Wall", RoomProps.reflectionBWall, 0, 1);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amplitude of the reflections off the back wall in the room.", MessageType.None);
				}
				
				RoomProps.reflectionCeiling = EditorGUILayout.Slider("Ceiling", RoomProps.reflectionCeiling, 0, 1);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amplitude of the reflections off the ceiling in the room.", MessageType.None);
				}
				
				RoomProps.reflectionFloor = EditorGUILayout.Slider("Floor", RoomProps.reflectionFloor, 0, 1);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The amplitude of the reflections off the floor in the room.", MessageType.None);
				}
				
				EditorGUI.indentLevel = 0;
				GUI.enabled = true;
				
				EditorGUILayout.Space();
				
				RoomProps.diffuseZone = Mathf.Clamp(RoomProps.diffuseZone, 0f, 50);
				RoomProps.diffuseZone =  EditorGUILayout.FloatField("Diffuse Zone Size", RoomProps.diffuseZone);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("The early reflections gradually fade out in this zone.", MessageType.None);
				}
				
				EditorGUILayout.Space();
				
				RoomProps.showGuides = EditorGUILayout.Toggle("Show Room Guides", RoomProps.showGuides);
				if (showHelpInfo) {
					EditorGUILayout.HelpBox("Show the room boundaries in the editor, even if not selected", MessageType.None);
				}
				
				EditorGUILayout.Space();
				
				EditorGUILayout.LabelField("Room Pivot Point");
				RoomProps.pivotPoint = GUILayout.SelectionGrid(RoomProps.pivotPoint, pivotSelection, 2, GUILayout.Width(170));
				
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
					EditorUtility.SetDirty(RoomProps);
				
			}
		}
	}
}

