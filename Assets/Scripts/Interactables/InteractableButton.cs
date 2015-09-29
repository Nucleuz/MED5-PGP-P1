using UnityEngine;
using System.Collections;

public class InteractableButton : MonoBehaviour{
	Trigger trigger;
	// Use this for initialization
	void Start () {
		trigger = GetComponent<Trigger> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RayCastEvent(){
		if (trigger.isReadyToBeTriggered) {
			trigger.isTriggered = true;
		}
	}
}
