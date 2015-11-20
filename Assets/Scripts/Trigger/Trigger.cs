using UnityEngine;
using System.Collections;
using DarkRift;

public class Trigger : MonoBehaviour {

    public float lockStateEnd = 0f;
    public float lockStateLength = 2f;
    public sbyte state = 0;

    //@TODO [HideInInspector] 
    public bool isTriggered = false;
    public bool isReadyToBeTriggered = false;
    public bool canReset = false;
    
    //Networking
    //[HideInInspector] 
    public ushort triggerID;

    //which players are currently interacting
    [HideInInspector]
    public bool[] playersInteracting = new bool[3];

    [HideInInspector]
    public bool playersRequired;

    //players required to trigger the trigger
    public bool bluePlayerRequired;
    public bool redPlayerRequired;
    public bool greenPlayerRequired;

    public void Start(){
        playersRequired = bluePlayerRequired || redPlayerRequired || greenPlayerRequired;
    }
    public void Activate(){
		if (!playersRequired) {
            Debug.Log("debug activated");
			if (isTriggered)
				return;
			isTriggered = true; 
		}

        //Send to Server @TODO to display visuals everyone should get this
        if(!NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger, Network.Subject.TriggerActivate,triggerID); 
        
        Console.Instance.AddMessage("Trigger " + triggerID + " Activated");
    }
   
    public void Deactivate(){
		if (!playersRequired) {

			if (!isTriggered)
				return;
			isTriggered = false;
		}

        if(!NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger,Network.Subject.TriggerDeactivate, triggerID);

        Console.Instance.AddMessage("Trigger " + triggerID + " Deactivated");
    }

    public void SetTriggerState(TriggerState triggerState){
        triggerID               = triggerState.id;
        state                   = triggerState.state;
        isTriggered             = triggerState.isTriggered;
        isReadyToBeTriggered    = triggerState.isReadyToBeTriggered;
        canReset                = triggerState.canReset;
    }

    public void SetTriggerID(ushort id){
        triggerID = id;
    }

    public void SendState(sbyte state){
        this.state = state;
        lockStateEnd = Time.time + lockStateLength;
        DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger,Network.Subject.PlayerSentTriggerState, new TriggerState(this));
    }
}


