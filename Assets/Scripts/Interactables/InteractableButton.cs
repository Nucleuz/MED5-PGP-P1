using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	Trigger trigger;

	// Use this for initialization
	void Start () {
		trigger = GetComponent<Trigger> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		if (trigger.isReadyToBeTriggered) {
			trigger.Activate();
		}
	}

}
