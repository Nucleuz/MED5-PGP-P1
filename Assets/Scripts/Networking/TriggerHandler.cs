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

     public bool isServer = false;
     public List<ushort> triggerIds;

     private ushort triggerIdCount = 1;


     void Awake(){
         _instance = this;
     }

     void Start(){
         triggers = new List<Trigger>();
         triggerIds = new List<ushort>();

         DarkRiftAPI.onDataDetailed += ReceiveData;

         
     }


     public void process(LevelContainer levelContainer){

         //get all triggers in the levelcontainer

         //@TODO check perf and mem of checkChild method -- should maybe put it in a coroutine
         checkChild(levelContainer.transform);

         //when completed if server call clients and tell level to load
         if(isServer){

         }else{

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
         if(isServer){
             ushort id = triggerIdCount++;
             triggers.Add(trigger);
             triggerIds.Add(id);
         }else
             triggers.Add(trigger);
     }

      
	void ReceiveData(ushort senderID, byte tag, ushort subject, object data){

    }

 }
