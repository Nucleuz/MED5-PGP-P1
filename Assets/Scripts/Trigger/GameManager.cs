using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 

	protected int numberOfTriggeredEvents = 0; //Is used to determine where we should continue from each time a puzzle sequence is finished. is only updated after each sequence is finished
	protected int currentNumberOfEventsTriggered = 0; //counts up when a single event is triggered, is reset when all events in the sequence is triggered
	protected int index = 0; // used to count which sequence is currently in play
	private bool readyForNextSequence = false;
	private bool isResetting = false;

	public LevelManager LM; //Used in order to access the LevelManagerObject in the scene
	

	// Use this for initialization
	void Start () {
		LM = GameObject.Find("LevelManagerObject").GetComponent<LevelManager>(); //accessing the LevelManager script on the LevelManagerObject
		for(int i = 0; i < LM.eventsInSequence[0]; i++){ //makes the first events in the scene triggerable
			LM.events[i].isReadyToBeTriggered = true;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//Checks if the current object is triggered, and if they are ready to be triggered
		if(LM.eventOrder[index] == 0){ //Checks if there is a desired order in the sequence, runs if there isn't
			for(int i = 0; i < LM.eventsInSequence[index]; i++){ //Goes through the events in the sequence
				//Used in order to not get double values
				if(LM.events[i + numberOfTriggeredEvents].isTriggered == true && LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered == true){ //Checks if they are triggered
					LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered = false; //sets the event untriggerable
					LM.triggeredEvents[i + numberOfTriggeredEvents] = true;
					currentNumberOfEventsTriggered++; //counts up the events in sequence by 1
				}
				//This if statement is used in order to reset interactables if they require it
				if(LM.events[i + numberOfTriggeredEvents].canReset == true && LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered == false){
					StartCoroutine(TimedReset(i));
				}
			}
		}
		else { //Checks if there is a desired order in the sequence. Only checks the objects that should be interacted with in the sequence 
			for(int i = 0; i < LM.eventOrder[index]; i++){  
				if(LM.events[i + numberOfTriggeredEvents + currentNumberOfEventsTriggered].isTriggered == true && LM.events[i + numberOfTriggeredEvents + currentNumberOfEventsTriggered].isReadyToBeTriggered == true){ //Checks if they are triggered 
				LM.events[i + numberOfTriggeredEvents + currentNumberOfEventsTriggered].isReadyToBeTriggered = false; //sets the event untriggerable
				LM.triggeredEvents[i + numberOfTriggeredEvents + currentNumberOfEventsTriggered] = true;
				currentNumberOfEventsTriggered++; //counts up the events in sequence by 1
				}
			}
			//This if statement is used in order to reset interactables if they require it
			for(int i = 0; i < LM.eventsInSequence[index]; i++){
				if(LM.events[i + numberOfTriggeredEvents].canReset == true && LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered == false){
					StartCoroutine(TimedReset(i));
				}
			}
			//This loop goes through the objects, which is not part of the current order, but is still in the sequence
			for(int i = LM.eventOrder[index] + currentNumberOfEventsTriggered; i < LM.eventsInSequence[index]; i++){
				//It then checks if they are triggered
				if(LM.events[i + numberOfTriggeredEvents].isTriggered == true && LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered == true){
					LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered = false;
					Debug.Log("Above current" + currentNumberOfEventsTriggered);
					//If they are triggered which they shouldn't be, then we reset the sequence
					currentNumberOfEventsTriggered = 0;
					for(int j = 0; j < LM.eventsInSequence[index]; j++){ //goes through all the objects in the sequence and untrigger them
						StartCoroutine(FailedReset(j));
						LM.events[j + numberOfTriggeredEvents].isTriggered = false;
						LM.triggeredEvents[j + numberOfTriggeredEvents] = false;
						Debug.Log("YOU FAILED");
					} /*
					for(int u = 0; u < LM.eventOrder[index]; u++){ //goes through all the objects in the sequence and makes them ready to be triggered again
						LM.events[u+ numberOfTriggeredEvents].isReadyToBeTriggered = true;
					} */
				}
			}
			if(currentNumberOfEventsTriggered > 0){ 
				for(int i = 0; i < currentNumberOfEventsTriggered; i++){ //Checking events earlier in the sequence
					if(LM.events[i + numberOfTriggeredEvents].isTriggered == true && LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered == true){
						LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered = false;
						Debug.Log("Below current" + currentNumberOfEventsTriggered);
						currentNumberOfEventsTriggered = 0;
						for(int j = 0; j < LM.eventsInSequence[index]; j++){
							StartCoroutine(FailedReset(j));
							LM.events[j + numberOfTriggeredEvents].isTriggered = false;
							LM.triggeredEvents[j + numberOfTriggeredEvents] = false;
							Debug.Log("YOU FAILED");
						}
					}
				}
			}
		}


		//========== Code segment used for detecting if they are finished with a sequence ======================================================================
		//Checks if all of the events in the current sequence are triggered
		for(int i = 0; i < LM.eventsInSequence[index]; i++){
			if(LM.triggeredEvents[i + numberOfTriggeredEvents] == false){
				readyForNextSequence = false;
				break; //breaks if one of the objects is not triggered
			} else {
				readyForNextSequence = true;
			}
		}
		if(readyForNextSequence == true){
			Debug.Log("Sequence finished" + index);
			if(index < LM.eventsInSequence.Length - 1){ //Checks if it is the last sequence of events - if it is: skip this
				numberOfTriggeredEvents = LM.eventsInSequence[index]; //Increase the total number of events by the amount of events that was in the current sequence
				if(LM.triggerEvents[index] != null){
					LM.triggerEvents[index].isTriggered = true; //Triggers an object with should trigger when a sequence is finished. could for example be a door
				}
				index++;
				for(int i = numberOfTriggeredEvents; i < numberOfTriggeredEvents + LM.eventsInSequence[index]; ++i){ //Goes through the next sequence of events
					LM.events[i].isReadyToBeTriggered = true; //Makes the next sequence ready to be triggered 
				}
			}
			currentNumberOfEventsTriggered = 0; //Resets the amount of objects that was triggered in the current sequence
			readyForNextSequence = false;
		}
		/*
		if(currentNumberOfEventsTriggered == LM.eventsInSequence[index]){ //Checks if the right amount of events are triggered in the current sequence
			if(index < LM.eventsInSequence.Length - 1){ //Checks if it is the last sequence of events - if it is: skip this
				numberOfTriggeredEvents += LM.eventsInSequence[index]; //Increase the total number of events by the amount of events that was in the current sequence
				if(LM.triggerEvents[index] != null){
					LM.triggerEvents[index].isTriggered = true; //Triggers an object with should trigger when a sequence is finished. could for example be a door
				}
				index++; //Goes to the next sequence
				for(int i = numberOfTriggeredEvents; i < numberOfTriggeredEvents + LM.eventsInSequence[index]; ++i){ //Goes through the next sequence of events
					LM.events[i].isReadyToBeTriggered = true; //Makes the next sequence ready to be triggered 
				}
			}
			currentNumberOfEventsTriggered = 0; //Resets the amount of objects that was triggered in the current sequence
		} */	
	}

	IEnumerator TimedReset(int resetIndex){ //Coroutine used for resetting objects which needs to be resetted
		Debug.Log("TimedReset");
		yield return new WaitForSeconds(1.5f); //Resets after x seconds
		LM.events[resetIndex + numberOfTriggeredEvents].isTriggered = false;
		LM.events[resetIndex + numberOfTriggeredEvents].isReadyToBeTriggered = true;
		//isResetting = false;
	}

	IEnumerator FailedReset(int resetIndex){ //MOVE SOME OF THIS STUFF UP! 
		/*LM.events[resetIndex + numberOfTriggeredEvents].isTriggered = false;
		LM.triggeredEvents[resetIndex + numberOfTriggeredEvents] = false; */
		Debug.Log("I resetted the stuff");
		yield return new WaitForSeconds(1.5f); //Resets after x seconds
		LM.events[resetIndex + numberOfTriggeredEvents].isReadyToBeTriggered = true;
		//isResetting = false;
		Debug.Log("Hello");
	}
}
