using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager: MonoBehaviour {
/*
		Game Plan!
- Sound Manager class needs to handle all sound playing
- Sound Manager will hold all needed functions for: 
	- Events
		- Play / stop event (PostEvent will play sound)
		- Handle switch modes
		- Handle state modes
		- Handle triggers
*/
	private List<GameObject> objectList = new List<GameObject>();

	public void StopAllEvents(){
		for(int i = 0; i < objectList.Count; i++){
			StopAllEventsOnObject(objectList[i]);
		}
	}
	
	public void StopAllEventsOnObject(GameObject g){
		AkSoundEngine.StopAll(g);
	}

	public void PlayEvent(string eventName, GameObject g){
		AkSoundEngine.PostEvent(eventName, g);
		
		for(int i = 0; i < objectList.Count; i++){
			if(objectList[i] == g)
				return;
		}
		AddToList(g);
	}

	public void StopEvent(string eventName, GameObject g){
		AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, g, 0, 0);
		RemoveFromList(g);
	}

	public void ToggleSwitch(string switchName, string switchMode, GameObject g){
		AkSoundEngine.SetSwitch(switchName, switchMode, g);
	}

	public void ToggleState(string stateName, string stateMode, GameObject g){
		AkSoundEngine.SetSwitch(stateName, stateMode, g);
	}

	public void AddToList(GameObject g){
		objectList.Add(g);
	}

	public void RemoveFromList(GameObject g){
		objectList.Remove(g);
	}

	public void ClearList(){
		objectList.Clear();
	}

	public void PrintList(){
		Debug.Log("OBJECT LIST FROM SOUND MANAGER: ");
		for(int i = 0; i < objectList.Count; i++){
			Debug.Log("Object " + i + ": " + objectList[i].name);
		}
		Debug.Log("END OF LIST");
	}

}
