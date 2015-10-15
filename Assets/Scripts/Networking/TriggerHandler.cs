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

     public static TriggerHandler _instance;

     public List<Trigger> triggers;

     //Server Specific
     public List<ushort> triggerIDs;
     public bool triggersReady = false;

     private ushort triggerIdCount = 1;


     void Awake(){
         _instance = this;
     }

     void Start(){
         triggers = new List<Trigger>();
         triggerIDs = new List<ushort>();

         if(!NetworkManager.isServer)
             DarkRiftAPI.onDataDetailed += ReceiveData;
         
     }


     public void process(LevelContainer levelContainer){
         //get all triggers in the levelcontainer
         triggersReady = false;

         purgeTriggers();
         //@TODO check perf and mem of checkChild method -- should maybe put it in a coroutine
         checkChild(levelContainer.transform);

         //when completed if server call clients and tell level to load
         //if client when completed call server and ask for list of ids
         if(NetworkManager.isServer){
             triggersReady = true;
              
         }else{

            DarkRiftAPI.SendMessageToServer(
                    Network.Tag.Trigger,
                    Network.Subject.RequestTriggerIDs,
                    true
                    );
         }
     }

     private void checkChild(Transform child){

         if(child.GetComponent<Trigger>() != null)
             Assign(child.GetComponent<Trigger>());

         foreach(Transform c in child.transform)
             checkChild(c);

     }



     //Assign trigger id, add to dictionary and return the id used.
     public void Assign(Trigger trigger){
         if(NetworkManager.isServer){
             ushort id = triggerIdCount++;
             triggers.Add(trigger);
             triggerIDs.Add(id);
         }else
             triggers.Add(trigger);
     }

     public void purgeTriggers(){
         triggers.Clear();
         triggerIDs.Clear();

     }
      
	public void ReceiveData(ushort senderID, byte tag, ushort subject, object data){

        if(tag == Network.Tag.Trigger){
            if(subject == Network.Subject.ServerSentTriggerIDs){
                Debug.Log("trigger id's received");
                
                TriggerState[] triggerStates = (TriggerState[])data;

                for(int i = 0;i<triggerStates.Length;i++){
                    triggers[i].SetState(triggerStates[i]);
                }
            }else if(subject == Network.Subject.TriggerActivate){
                TriggerInteracted((ushort)data,true);
            }else if(subject == Network.Subject.TriggerDeactivate){
                TriggerInteracted((ushort)data,false);
            }
        }
    }

    public void SetTriggerState(TriggerState state){
        int index = FindTriggerIndexFromID(state.id);  
        triggers[index].SetState(state);
    }

    public TriggerState GetTriggerState(ushort triggerID){
        int index = FindTriggerIndexFromID(triggerID);
        return new TriggerState(triggers[index]);

    }

    public void TriggerInteracted(ushort triggerID, bool state){
        int index = FindTriggerIndexFromID(triggerID);  
        triggers[index].isTriggered = state;
    }

    private int FindTriggerIndexFromID(ushort id){
        for(int i = 0;i<triggerIDs.Count;i++){
            if(triggerIDs[i] == id)return i;
        }

        Debug.LogError("Could not find Index from ID " + id);
        return -1;
    }
}
