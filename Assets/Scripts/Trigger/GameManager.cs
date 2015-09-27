using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 

	public bool isLevelStarted = false; //IS NOT USED
	public bool isCurrentLevelCompleted = false; //IS NOT USED
	public int numberOfCompletedLevels = 0; //IS NOT USED - These are all made in case of several scenes!
	protected int numberOfTriggeredEvents = 0; //Is used to determine where we should continue from each time a puzzle sequence is finished
	protected int index = 0; // used to count which sequence is currently in play

	public LevelManager LM; //Used in order to access the LevelManagerObject in the scene

	// Use this for initialization
	void Start () {
		LM = GameObject.Find("LevelManagerObject").GetComponent<LevelManager>(); //accessing the LevelManager script on the LevelManagerObject
	}
	
	// Update is called once per frame
	void Update () {

		if(index < LM.eventOrder.Length){ //if the sequence is smaller then the amount of sequences then do the following. THIS MIGHT CAUSE TROUBLE IN THE END! Maybe delete?
			for(int j = numberOfTriggeredEvents; j < LM.eventOrder[index]; j++){ //Runs the objects in the current sequence
				if(LM.events[j].GetComponent<Testingtrigger>().isTriggered == true){ /*Checks if the current object is triggered - "isTriggered" is a boolean we made in order to test.
				"isTriggered" should be replaced by another boolean which is needed on all the interactable objects.
				There should be a if statement for each different interactable object, which fetches the correct script */
				numberOfTriggeredEvents++; //count up if they are triggered
				} 
				if(numberOfTriggeredEvents == LM.eventOrder[index]){ //Checks if the right amount of events are triggered
					index++; //Goes to the next sequence
					Debug.Log("Door is open! :D"); //Should write code here for what is to happen with the next sequence of objects
				}
			}	
		}
	}

	void checkCompletion(GameObject[] events, int numberOfEvents){ //IS NOT USED

		//Receive data from each gameobject in booleans
	}

	/*void checkTaskCompletion(){
			Not used at the moment
	}*/

	void changeLevel(){ //was made in case of several scenes

	}


}
