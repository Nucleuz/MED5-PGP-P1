using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {
 

	protected int numberOfTriggeredEvents = 0; //Is used to determine where we should continue from each time a puzzle sequence is finished. is only updated after each sequence is finished
	protected int currentNumberOfEventsTriggered = 0; //counts up when a single event is triggered, is reset when all events in the sequence is triggered
	protected int index = 0; // used to count which sequence is currently in play

	public LevelManager LM; //Used in order to access the LevelManagerObject in the scene
    [HideInInspector]
    public LevelHandler levelHandler;

    private bool readyForNextSequence = false;

    public ServerManager server;	

	// Use this for initialization
	void Start () {
        server = GetComponent<ServerManager>();
		GameObject g = GameObject.Find("LevelManagerObject"); //accessing the LevelManager script on the LevelManagerObject
        levelHandler = GetComponent<LevelHandler>();

            
		if(g != null)
			LM = g.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(LM == null)return;
        try{
            if(LM.eventOrder[index] == 0){ //Checks if there is a desired order in the sequence, runs if there isn't
                for(int j = 0; j < LM.eventsInSequence[index]; j++)
                    StartCoroutine(TimedReset(j));
            }else{
                for(int h = 0; h < LM.eventOrder[index]; h++)
                    StartCoroutine(TimedReset(h));
            }
        }catch(System.IndexOutOfRangeException e){
            Debug.Log("GM ERROR with index: " + index);
        }
    }

    public void DetectTriggerChanges(){
        try{
            //Checks if the current object is triggered, and if they are ready to be triggered
            if(LM.eventOrder[index] == 0){ //Checks if there is a desired order in the sequence, runs if there isn't
                for(int j = 0; j < LM.eventsInSequence[index]; j++){ //Goes through the events in the sequence
                    //Used in order to not get double values
                    if(LM.events[j + numberOfTriggeredEvents].isTriggered == true && LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered == true){ //Checks if they are triggered
                        LM.events[j + numberOfTriggeredEvents].isReadyToBeTriggered = false; //sets the event untriggerable
                        LM.triggeredEvents[j + numberOfTriggeredEvents + currentNumberOfEventsTriggered] = true;

                        currentNumberOfEventsTriggered++; //counts up the events in sequence by 1
                    }
                    resetTriggers(j);
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
                        //If they are triggered which they shouldn't be, then we reset the sequence
                        currentNumberOfEventsTriggered = 0;
                        for(int j = 0; j < LM.eventsInSequence[index]; j++){ //goes through all the objects in the sequence and untrigger them
                            StartCoroutine(FailedReset(j));
                            LM.events[j + numberOfTriggeredEvents].isTriggered = false;
                            LM.triggeredEvents[j + numberOfTriggeredEvents] = false;
                        }
                    }
                }
                //This if statement goes through all objects earlier then the current in the event order and detects if they trigger them, which they shouldn't
                if(currentNumberOfEventsTriggered > 0){ 
                    for(int i = 0; i < currentNumberOfEventsTriggered; i++){ //Checking events earlier in the sequence
                        if(LM.events[i + numberOfTriggeredEvents].isTriggered == true && LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered == true){
                            LM.events[i + numberOfTriggeredEvents].isReadyToBeTriggered = false;
                            currentNumberOfEventsTriggered = 0;
                            for(int j = 0; j < LM.eventsInSequence[index]; j++){ //goes through all the objects in the sequence and untrigger them
                                StartCoroutine(FailedReset(j));
                                LM.events[j + numberOfTriggeredEvents].isTriggered = false;
                                LM.triggeredEvents[j + numberOfTriggeredEvents] = false;
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
                if(LM.triggerEvents[index] != null){
                    LM.triggerEvents[index].isTriggered = true; //Triggers an object with should trigger when a sequence is finished. could for example be a door
                    server.TriggerChanged(LM.triggerEvents[index]);
                }
                if(index < LM.eventsInSequence.Length - 1){ //Checks if it is the last sequence of events - if it is: skip this
                    numberOfTriggeredEvents = LM.eventsInSequence[index]; //Increase the total number of events by the amount of events that was in the current sequence
                    index++;
                    for(int i = numberOfTriggeredEvents; i < numberOfTriggeredEvents + LM.eventsInSequence[index]; ++i){ //Goes through the next sequence of events
                        LM.events[i].isReadyToBeTriggered = true; //Makes the next sequence ready to be triggered 
                    }
                }
                currentNumberOfEventsTriggered = 0; //Resets the amount of objects that was triggered in the current sequence
                readyForNextSequence = false;
            }   
        } 
        catch(System.IndexOutOfRangeException e){
            Debug.LogWarning("This was an error -- needs to implement Andreas GameManager Fix");
			Debug.LogWarning(e);
        }

   }

	public void setNewLevelManager(LevelManager levelManager){
		LM = levelManager;

        if(LM == null)return;
        
        index = 0;
        numberOfTriggeredEvents = 0;
        currentNumberOfEventsTriggered = 0;

		for(int k = 0; k < LM.eventsInSequence[0]; k++){ //makes the first events in the scene triggerable
			LM.events[k].isReadyToBeTriggered = true;
		}
	}

    private void resetTriggers(int triggerIndex){
        //This if statement is used in order to reset interactables if they require it
        try{
            if(LM.events[triggerIndex + numberOfTriggeredEvents].canReset == true && LM.events[triggerIndex + numberOfTriggeredEvents].isReadyToBeTriggered == false){
                LM.events[triggerIndex + numberOfTriggeredEvents].isReadyToBeTriggered = true;
                LM.events[triggerIndex  + numberOfTriggeredEvents].canReset = false;
                server.resetTriggers();
            }
        }catch(System.IndexOutOfRangeException e){
            Debug.LogWarning("This was an error -- needs to implement Andreas GameManager Fix");
			Debug.LogWarning(e);
        }
   }


    //===== Code used for resetting objects based on if they needs to be resetted or if they fail =================================================================
    IEnumerator TimedReset(int resetIndex){ //Coroutine used for resetting objects which needs to be resetted
        yield return new WaitForSeconds(1.5f); //Resets after x seconds
        LM.events[resetIndex + numberOfTriggeredEvents].isTriggered = false;
        LM.events[resetIndex + numberOfTriggeredEvents].isReadyToBeTriggered = true;
    }

    IEnumerator FailedReset(int resetIndex){ //MOVE SOME OF THIS STUFF UP! 
        yield return new WaitForSeconds(1.5f); //Resets after x seconds
        LM.events[resetIndex + numberOfTriggeredEvents].isReadyToBeTriggered = true;

    }
	
}
