using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 

	protected int numberOfTriggeredEvents = 0; //Is used to determine where we should continue from each time a puzzle sequence is finished
	protected int currentNumberOfEventsTriggered = 0;
	protected int index = 0; // used to count which sequence is currently in play

	public LevelManager LM; //Used in order to access the LevelManagerObject in the scene
	

	// Use this for initialization
	void Start () {
		LM = GameObject.Find("LevelManagerObject").GetComponent<LevelManager>(); //accessing the LevelManager script on the LevelManagerObject
		for(int k = 0; k < LM.eventOrder[0]; k++){
			LM.events[k].GetComponent<Trigger>().isReadyToBeTriggered = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		for(int j = 0; j < LM.eventOrder[index]; j++){ //Runs the objects in the current sequence
			//Debug.Log(j);
			if(LM.events[j + numberOfTriggeredEvents].GetComponent<Trigger>().isTriggered == true && LM.events[j + numberOfTriggeredEvents].GetComponent<Trigger>().isReadyToBeTriggered == true){ //Checks if the current object is triggered
				//numberOfTriggeredEvents++; //count up if they are triggered
				//Debug.Log("Triggered events " + numberOfTriggeredEvents);
				LM.events[j + numberOfTriggeredEvents].GetComponent<Trigger>().isReadyToBeTriggered = false;
				currentNumberOfEventsTriggered++;
				Debug.Log("CNOET " + currentNumberOfEventsTriggered);
			} 
		}
		if(currentNumberOfEventsTriggered == LM.eventOrder[index]){ //Checks if the right amount of events are triggered
			if(index < LM.eventOrder.Length - 1){
				Debug.Log("CNOET2" + currentNumberOfEventsTriggered);
				Debug.Log("event order" + " " + index);
				numberOfTriggeredEvents += LM.eventOrder[index];
				index++; //Goes to the next sequence
				for(int i = numberOfTriggeredEvents; i < numberOfTriggeredEvents + LM.eventOrder[index]; ++i){
					//Debug.Log("i " + i);
					LM.events[i].GetComponent<Trigger>().isReadyToBeTriggered = true;
					//Debug.Log("event order" + " " + index);
					Debug.Log("This is now ready");
				}
			}
			Debug.Log("Door is open! :D"); //Should write code here for what is to happen with the next sequence of objects
			currentNumberOfEventsTriggered = 0;
		}	
	}
}
