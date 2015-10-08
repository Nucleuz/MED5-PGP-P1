using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	Trigger trigger;

	[Tooltip("-1 = anyone, 1 = player1, 2 = player2, 4 = player3")]
	public int playerID;
	private int combinedPlayerID;
	private bool[] additionCheck;

	private float delay;
	private float timer;

	// Use this for initialization
	void Start () {
		additionCheck = new bool[4];
		delay = 1.0f;
		trigger = GetComponent<Trigger> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0){
			timer = delay;
			combinedPlayerID = 0;
			for(int i = 0; i < additionCheck.Length; i++)
				additionCheck[i] = false;
		}
		Debug.Log(combinedPlayerID);
	}


	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		if(!additionCheck[playerIndex-1]){
			combinedPlayerID += playerIndex;
			additionCheck[playerIndex-1] = true;
		}
		
		if (trigger.isReadyToBeTriggered && (playerIndex == playerID || playerID == -1 || combinedPlayerID == playerID)) {
			trigger.isTriggered = true;
		}
	}

}
