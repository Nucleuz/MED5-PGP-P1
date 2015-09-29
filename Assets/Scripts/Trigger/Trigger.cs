using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool isTriggered = false;
	public bool isReadyToBeTriggered = false;
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
		}
		
	}
}
