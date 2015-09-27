using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 

	public bool isLevelStarted = false;
	public int numberOfCompletedTask = 0;
	public bool isCurrentLevelCompleted = false;
	public int numberOfCompletedLevels = 0;
	protected int numberOfTriggeredEvents = 0;
	protected int index = 0;

	public LevelManager LM;

	public GameObject testCube;

	// Use this for initialization
	void Start () {
		LM = GameObject.Find("LevelManagerObject").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {

		if(index < LM.eventOrder.Length){
			for(int j = numberOfTriggeredEvents; j < LM.eventOrder[index]; j++){
				if(LM.events[j].GetComponent<Testingtrigger>().isTriggered == true){
				numberOfTriggeredEvents++;
				}
				if(numberOfTriggeredEvents == LM.eventOrder[index]){
					index++;
					Debug.Log("Door is open! :D");
				}
			}	
		}
	}

	void checkCompletion(GameObject[] events, int numberOfEvents){

		//Receive data from each gameobject in booleans
	}

	/*void checkTaskCompletion(){
			Not used at the moment
	}*/

	void changeLevel(){

	}


}
