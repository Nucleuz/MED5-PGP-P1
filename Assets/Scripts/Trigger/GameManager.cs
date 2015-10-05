using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 

	protected int numberOfTriggeredEvents = 0; //Is used to determine where we should continue from each time a puzzle sequence is finished. is only updated after each sequence is finished
	protected int currentNumberOfEventsTriggered = 0; //counts up when a single event is triggered, is reset when all events in the sequence is triggered
	protected int index = 0; // used to count which sequence is currently in play

	public LevelManager LM; //Used in order to access the LevelManagerObject in the scene
	

	// Use this for initialization
	void Start () {
		GameObject g = GameObject.Find("LevelManagerObject"); //accessing the LevelManager script on the LevelManagerObject
		if(g != null)
			LM = g.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(LM == null)return;
		//Checks if the current object is triggered, and if they are ready to be triggered
		if(LM.eventOrder[index] == 0){ //Checks if there is a desired order in the sequence, runs if there isn't
			for(int j = 0; j < LM.eventsInSequence[index]; j++){ //Goes through the events in the sequence
				//Used in order to not get double values
				if(LM.events[j + numberOfTriggeredEvents].isTriggered == true && LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered == true){ //Checks if they are triggered
					LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered = false; //sets the event untriggerable
					currentNumberOfEventsTriggered++; //counts up the events in sequence by 1
				}
				//This if statement is used in order to reset interactables if they require it
				if(LM.events[j + numberOfTriggeredEvents].canReset == true && LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered == false){
					LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered = true;
					LM.events[j + numberOfTriggeredEvents].canReset = false;
				}
			} 
		}
		else { //Checks if there is a desired order in the sequence. Only checks the objects that should be interacted with in the sequence 
			for(int h = 0; h < LM.eventOrder[index]; h++){  
				if(LM.events[h + numberOfTriggeredEvents].isTriggered == true && LM.events[h + numberOfTriggeredEvents].isReadyToBeTriggered == true){ //Checks if they are triggered 
				LM.events[h + numberOfTriggeredEvents].isReadyToBeTriggered = false; //sets the event untriggerable
				currentNumberOfEventsTriggered++; //counts up the events in sequence by 1
				}
				//This if statement is used in order to reset interactables if they require it
				if(LM.events[h + numberOfTriggeredEvents].canReset == true && LM.events[h + numberOfTriggeredEvents].isReadyToBeTriggered == false){
					LM.events[h + numberOfTriggeredEvents].isReadyToBeTriggered = true;
					LM.events[h + numberOfTriggeredEvents].canReset = false;
				}
			}
			//This loop goes through the objects, which is not part of the current order, but is still in the sequence
			for(int g = LM.eventOrder[index] + currentNumberOfEventsTriggered; g < LM.eventsInSequence[index]; g++){
				//It then checks if they are triggered
				if(LM.events[g + numberOfTriggeredEvents].isTriggered == true && LM.events[g + numberOfTriggeredEvents].isReadyToBeTriggered == true){
					//If they are triggered which they shouldn't be, then we reset the sequence
					currentNumberOfEventsTriggered = 0;
					for(int f = 0; f < LM.eventsInSequence[index]; f++){ //goes through all the objects in the sequence and untrigger them
						LM.events[f + numberOfTriggeredEvents].isTriggered = false;
					}
					for(int u = 0; u < LM.eventOrder[index]; u++){ //goes through all the objects in the sequence and makes the ready to be triggered again
						LM.events[u+ numberOfTriggeredEvents].isReadyToBeTriggered = true;
					}
				}
			}
		}

		if(currentNumberOfEventsTriggered == LM.eventsInSequence[index]){ //Checks if the right amount of events are triggered in the current sequence
			if(index < LM.eventsInSequence.Length - 1){ //Checks if it is the last sequence of events - if it is: skip this
				numberOfTriggeredEvents += LM.eventsInSequence[index]; //Increase the total number of events by the amount of events that was in the current sequence
				index++; //Goes to the next sequence
				for(int i = numberOfTriggeredEvents; i < numberOfTriggeredEvents + LM.eventsInSequence[index]; ++i){ //Goes through the next sequence of events
					LM.events[i].isReadyToBeTriggered = true; //Makes the next sequence ready to be triggered 
				}
			}
			currentNumberOfEventsTriggered = 0; //Resets the amount of objects that was triggered in the current sequence
		}	
	}


	public void setNewLevelManager(LevelManager levelManager){
		LM = levelManager;

		for(int k = 0; k < LM.eventsInSequence[0]; k++){ //makes the first events in the scene triggerable
			LM.events[k].isReadyToBeTriggered = true;
		}
	}

}
