using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 

	public bool isLevelStarted = false;
	public int numberOfCompletedTask = 0;
	public bool isCurrentLevelCompleted = false;
	public int numberOfCompletedLevels = 0;

	public LevelManager LM;

	public GameObject testCube;

	// Use this for initialization
	void Start () {
		LM = GameObject.Find("LevelManagerObject").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
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
