using UnityEngine;
using System.Collections;
using DarkRift;

public class Trigger : MonoBehaviour {
	public bool isTriggered = false;
	public bool isReadyToBeTriggered = false;
	public bool canReset = false;
    
    //Networking
    public ushort triggerID;

    //server should set this to true
    [HideInInspector]
    public static bool isServer = false;


    //@TODO(kasper) sync state with server 
    public void Activate(){
        isTriggered = true; 

        //Send to Server @TODO to display visuals everyone should get this
        DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger, Network.Subject.TriggerActivate,triggerID); 

    }
   
    public void Deactivate(){
        isTriggered = false;

        DarkRiftAPI.SendMessageToServer(Network.Tag.Trigger,Network.Subject.TriggerDeactivate, triggerID);

    }

	void RecieveData(ushort senderID, byte tag, ushort subject, object data){
		//check that it is the right sender
		if(isServer ){

			//check if it wants to update the player
			if(tag == Network.Tag.Trigger && (ushort)data == triggerID){
                

                if(subject == Network.Subject.TriggerActivate ){
                    isTriggered = true;

                }else if(subject == Network.Subject.TriggerDeactivate){
                    isTriggered = false;

                }
			}
		}

        //client should also handle some eevents

	}

    public void setTriggerID(ushort id){

        triggerID = id;
    }
}



