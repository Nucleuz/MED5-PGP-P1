using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerHelp : Editor {

	SerializedProperty events;
	SerializedProperty triggeredEvents;
	SerializedProperty eventsInSequence;
	SerializedProperty eventOrder;
	SerializedProperty triggerEvents;

	public void OnEnable(){
		events  = serializedObject.FindProperty("events");
		triggeredEvents  = serializedObject.FindProperty("triggeredEvents");
		eventsInSequence  = serializedObject.FindProperty("eventsInSequence");
		eventOrder  = serializedObject.FindProperty("eventOrder");
		triggerEvents  = serializedObject.FindProperty("triggerEvents");
	}

	public void OnInspectorGUI(){
		serializedObject.Update();


    	triggeredEvents.arraySize = events.arraySize;

    	Debug.Log(triggeredEvents.arraySize);


    	/*
    	if(eventOrder.Length != eventsInSequence.Length || eventOrder.Length != triggerEvents.Length || eventsInSequence.Length != triggerEvents.Length)
    		Debug.LogError("LevelManager Error: [Event Order], [Events In Sequence] and [Trigger Events] must be the same length");
		*/

    	serializedObject.ApplyModifiedProperties();
	}
	
}
