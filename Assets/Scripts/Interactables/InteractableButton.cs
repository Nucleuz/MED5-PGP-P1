using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	Trigger trigger;

	[Tooltip("-1 = anyone, 1 = player1, 2 = player2, 4 = player3")]
	public int playerID;
	private int receivedPlayerID;

	private float timeDelay;
	private float timer;

	// Use this for initialization
	void Start () {
		timeDelay = 1.0f;
		trigger = GetComponent<Trigger> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0){
			timer = timeDelay;
			receivedPlayerID = 0;
		}

		Debug.Log(receivedPlayerID);
	}


	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		
		receivedPlayerID = playerIndex;
		if (trigger.isReadyToBeTriggered && (playerIndex == playerID || playerID == -1)) {
			trigger.isTriggered = true;
		}
	}

}
