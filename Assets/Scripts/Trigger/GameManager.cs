﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 

	protected int numberOfTriggeredEvents = 0; //Is used to determine where we should continue from each time a puzzle sequence is finished. is only updated after each sequence is finished
	protected int currentNumberOfEventsTriggered = 0; //counts up when a single event is triggered, is reset when all events in the sequence is triggered
	protected int index = 0; // used to count which sequence is currently in play

	public LevelManager LM; //Used in order to access the LevelManagerObject in the scene
	

	// Use this for initialization
	void Start () {
		LM = GameObject.Find("LevelManagerObject").GetComponent<LevelManager>(); //accessing the LevelManager script on the LevelManagerObject
		for(int k = 0; k < LM.eventOrder[0]; k++){ //makes the first events in the scene triggerable
			LM.events[k].isReadyToBeTriggered = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		for(int j = 0; j < LM.eventOrder[index]; j++){ //Runs the objects in the current sequence
			//Checks if the current object is triggered, and if they are ready to be triggered
			//Used in order to not get double values
			if(LM.events[j + numberOfTriggeredEvents].isTriggered == true && LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered == true){ 
				LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered = false; //sets the event untriggerable
				currentNumberOfEventsTriggered++; //counts up the events in sequence by 1
			} 
		}

		if(currentNumberOfEventsTriggered == LM.eventOrder[index]){ //Checks if the right amount of events are triggered in the current sequence
			if(index < LM.eventOrder.Length - 1){ //Checks if it is the last sequence of events - if it is: skip this
				numberOfTriggeredEvents += LM.eventOrder[index]; //Increase the total number of events by the amount of events that was in the current sequence
				index++; //Goes to the next sequence
				for(int i = numberOfTriggeredEvents; i < numberOfTriggeredEvents + LM.eventOrder[index]; ++i){ //Goes through the next sequence of events
					LM.events[i].isReadyToBeTriggered = true; //Makes the next sequence ready to be triggered 
				}
			}
			Debug.Log("Door is open! :D"); //Should write code here for what is to happen with the next sequence of objects
			currentNumberOfEventsTriggered = 0; //Resets the amount of objects that was triggered in the current sequence
		}	
	}
}
