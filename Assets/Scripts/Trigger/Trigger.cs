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
        if(NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger, Network.Subject.TriggerActivate,triggerID); 

    }
   
    public void Deactivate(){
        isTriggered = false;

        if(NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger,Network.Subject.TriggerDeactivate, triggerID);

    }

    public void SetState(bool state){
        isTriggered = state;
    }

    public void SetTriggerID(ushort id){
        triggerID = id;
    }
}



