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

         DarkRiftAPI.onDataDetailed += ReceiveData;

         
     }


     public void process(LevelContainer levelContainer){
         //get all triggers in the levelcontainer
         triggersReady = false;

         purgeTriggers();
         //@TODO check perf and mem of checkChild method -- should maybe put it in a coroutine
         checkChild(levelContainer.transform);

         //when completed if server call clients and tell level to load
         if(NetworkManager.isServer){
             triggersReady = true;
              
         }else{

            DarkRiftAPI.SendMessageToServer(
                    Network.Tag.Trigger,
                    Network.Subject.RequestTriggerIDs,
                    true
                    );
         }
         //if client when completed call server and ask for list of ids
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
      
	void ReceiveData(ushort senderID, byte tag, ushort subject, object data){

        if(tag == Network.Tag.Trigger){
            if(NetworkManager.isServer){
                if(subject == Network.Subject.RequestTriggerIDs){

                    Debug.Log("request received");
                    ushort[] ids = triggerIDs.ToArray();
                    DarkRiftAPI.SendMessageToID(
                            senderID,
                            Network.Tag.Trigger,
                            Network.Subject.ServerSentTriggerIDs,
                            ids);

                }
            }else{
                if(subject == Network.Subject.ServerSentTriggerIDs){
                    Debug.Log("trigger id's received");
                    
                    ushort[] ids = (ushort[])data;

                    for(int i = 0;i<ids.Length;i++){
                        triggers[i].SetTriggerID(ids[i]);
                    }
                }
            }
        }
    }
}
