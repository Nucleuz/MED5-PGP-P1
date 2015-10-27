using UnityEngine;
using System.Collections;

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

	public void PlayEvent(string eventName, GameObject g){
		AkSoundEngine.PostEvent(eventName, g);
	}

	public void StopEvent(string eventName, GameObject g){
		AkSoundEngine.ExecuteActionOnEvent(eventName, AkActionOnEventType.AkActionOnEventType_Stop, g, 0, 0);
	}

	public void StopAll(GameObject g){
		  AkSoundEngine.StopAll(g);
	}

	public void ToggleSwitch(string switchName, string switchMode, GameObject g){
		AkSoundEngine.SetSwitch(switchName, switchMode, g);
	}

	public void ToggleState(string stateName, string stateMode, GameObject g){
		AkSoundEngine.SetSwitch(stateName, stateMode, g);
	}


}
