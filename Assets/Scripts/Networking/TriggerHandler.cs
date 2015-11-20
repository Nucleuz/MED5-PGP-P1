using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkRift;

/*
 * By KasperHdL
 *
 * Handles triggers over the net, contains a dictionary of all triggers map to id's 
 *
 * sends trigger info to Other Clients 
 *
     * (Singleton)
     */

 
 public class TriggerHandler : MonoBehaviour{

     private static TriggerHandler instance;

    //[HideInInspector]
     public Trigger[] triggers;

     //Server Specific
    //[HideInInspector]
     public ushort[] triggerIDs;
    
    [HideInInspector]
     public bool triggersReady = false;

     private ushort triggerIdCount = 1;

     //ref

     private LevelHandler levelHandler;

    public static TriggerHandler Instance{
        get{
            if (instance == null)
                instance = FindObjectOfType(typeof(TriggerHandler)) as TriggerHandler;
            return instance;
        }
    }

     void Start(){
         levelHandler = GetComponent<LevelHandler>();
         if(!NetworkManager.isServer)
             DarkRiftAPI.onDataDetailed += ReceiveData;
         
     }
      
    public void ReceiveData(ushort senderID, byte tag, ushort subject, object data){

        if(tag == Network.Tag.Trigger){
            switch(subject){
                case Network.Subject.ServerSentTriggerIDs:
                {

                    Debug.Log("trigger IDs received");
                    triggers = levelHandler.levelContainers[levelHandler.levelManagerIndex].triggers;

                    TriggerState[] triggerStates = (TriggerState[])data;
                    triggerIDs = new ushort[triggers.Length];

                    for(int i = 0;i<triggerStates.Length;i++){
                        triggers[i].SetTriggerState(triggerStates[i]);
                        triggerIDs[i] = triggers[i].triggerID;
                    }
                }
                break;
                case Network.Subject.ServerSentTriggerStates:
                {

                    // Debug.Log("trigger states received");
                    triggers = levelHandler.levelContainers[levelHandler.levelManagerIndex].triggers;

                    TriggerState[] triggerStates = (TriggerState[])data;

                    for(int i = 0;i<triggerStates.Length;i++){
                        triggers[i].SetTriggerState(triggerStates[i]);
                    }
                }
                break;
                case Network.Subject.TriggerState:
                {
                    Debug.Log("Received> " + (TriggerState)data);
                    SetTriggerState((TriggerState)data);
                }
                break;
                case Network.Subject.SequenceFailed:
                {
                    ReceiveSequenceFail obj = levelHandler.getLevelManager().sequenceFail;
                    if(obj != null){
                        obj.OnReceive();
                    }
                }
                break;
            }
        }
    }

    public void SetTriggerState(TriggerState state){
        int index = FindTriggerIndexFromID(state.id);  
        triggers[index].SetTriggerState(state);
    }

    public TriggerState GetTriggerState(ushort triggerID){
        int index = FindTriggerIndexFromID(triggerID);
        return new TriggerState(triggers[index]);

    }

    public void TriggerInteracted(ushort triggerID,ushort playerID, bool state){
        int index = FindTriggerIndexFromID(triggerID);  
        
        Trigger trigger = triggers[index];

        if(trigger.playersRequired){
            trigger.playersInteracting[playerID-1] = state;

            //is players interacting the correct ones
            if(trigger.playersInteracting[0] == trigger.bluePlayerRequired &&
                trigger.playersInteracting[1] == trigger.redPlayerRequired &&
                trigger.playersInteracting[2] == trigger.greenPlayerRequired){
                
                trigger.isTriggered = true;
            }else
                trigger.isTriggered = false;


        }else
            trigger.isTriggered = state;


    }

    private int FindTriggerIndexFromID(ushort id){
        for(int i = 0;i<triggerIDs.Length;i++){
            if(triggerIDs[i] == id)return i;
        }

        Debug.LogError("Could not find Index from ID " + id);
        return -1;
    }
}