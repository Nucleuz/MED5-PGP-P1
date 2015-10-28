using UnityEngine;
using System.Collections;
using DarkRift;

public class Trigger : MonoBehaviour {
    public bool isTriggered = false;
    public bool isReadyToBeTriggered = false;
    public bool canReset = false;
    
    //Networking
    public ushort triggerID;

    //which players are currently interacting
    public bool[] playersInteracting = new bool[3];

    [HideInInspector]
    public bool playersRequired;

    //players required to trigger the trigger
    public bool redPlayerRequired;
    public bool greenPlayerRequired;
    public bool bluePlayerRequired;

    public void Start(){
        playersRequired = bluePlayerRequired || redPlayerRequired || greenPlayerRequired;
    }
    public void Activate(){
        if(isTriggered) return;
        if(!playersRequired)
            isTriggered = true; 

        //Send to Server @TODO to display visuals everyone should get this
        if(!NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger, Network.Subject.TriggerActivate,triggerID); 
        
        Console.Instance.AddMessage("Trigger " + triggerID + " Activated");
    }
   
    public void Deactivate(){
        if(!isTriggered) return;
        if(!playersRequired)
            isTriggered = false;

        if(!NetworkManager.isServer)
            DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger,Network.Subject.TriggerDeactivate, triggerID);

        Console.Instance.AddMessage("Trigger " + triggerID + " Deactivated");
    }

    public void SetState(TriggerState state){
        triggerID = state.id;
        isTriggered = state.isTriggered;
        isReadyToBeTriggered = state.isReadyToBeTriggered;
        canReset = state.canReset;
    }

    public void SetTriggerID(ushort id){
        triggerID = id;
    }
}


