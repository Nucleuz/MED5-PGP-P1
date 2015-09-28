using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool isTriggered = false;
	public bool isReadyToBeTriggered = false;
	//color
	Color ourColor = new Color(0,255,0,1);
	Color triggeredColor = new Color(255,0,0,1);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isReadyToBeTriggered == true){

		}
	}

	void OnTriggerEnter(Collider trigger){
		if(isReadyToBeTriggered == true){
			isTriggered = true;
			Debug.Log("Cube is touched");

		}
		
	}
}
