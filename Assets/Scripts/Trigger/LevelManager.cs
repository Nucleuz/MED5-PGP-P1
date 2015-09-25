using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public int numberOfEvents;
	public GameObject[] orderOfEvents;
	public int[] eventOrder; 
	//public int currentLevelNumber; //might not need this if only 1 scene

	// Use this for initialization
	void Start () {
		countEventsInScene();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void countEventsInScene() { //Was made for recounting if there is several scenes
		//Should count the number of events that is going to happen in each level
		numberOfEvents = GameObject.FindGameObjectsWithTag("Test Tag").Length; //counts how many events should be triggered
	}

	void newLevel() {
		numberOfEvents = 0;
		//Should reset the values for how many events etc. there is in the level
		//should call the method "countEventsInScene"
	}
}
