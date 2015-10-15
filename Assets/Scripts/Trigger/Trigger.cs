using UnityEngine;
using System.Collections;
using DarkRift;

public class Trigger : MonoBehaviour {
	public bool isTriggered = false;
	public bool isReadyToBeTriggered = false;
	public bool canReset = false;
    
    //Networking
    public ushort triggerID;

    //@TODO(kasper) sync state with server 
    public void Activate(){
        isTriggered = true; 

        //Send to Server @TODO to display visuals everyone should get this
        if(!NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger, Network.Subject.TriggerActivate,triggerID); 
        Debug.Log("Trigger " + triggerID + " Activated");

    }
   
    public void Deactivate(){
        isTriggered = false;

        if(!NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger,Network.Subject.TriggerDeactivate, triggerID);
        Debug.Log("Trigger " + triggerID + " Deactivated");
    }

    public void SetState(TriggerState state){
        isTriggered = state.isTriggered;
        isReadyToBeTriggered = state.isReadyToBeTriggered;
        canReset = state.canReset;
    }

    public void SetTriggerID(ushort id){
        triggerID = id;
    }
}



