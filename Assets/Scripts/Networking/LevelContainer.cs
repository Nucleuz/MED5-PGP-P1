using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelContainer : MonoBehaviour {

    [HideInInspector]
	public bool processed = false;
	[ContextMenuItem("Populate", "Populate")] //add extra functionality on the right click context menu
    public bool triggersProcessed = false;

    public Trigger[] triggers;
    private List<Trigger> temp;

	public LevelManager levelManager;
	public LevelHandler levelHandler;

    //TODO make an editor script that recursively goes through and stores all found triggers in an array (function is already made in TriggerHandler.. )
    void Populate(){
    	checkChild(transform);
    	triggers = temp.ToArray();
    	temp.Clear();
    	triggersProcessed = true;
    }

	private void checkChild(Transform child){

         if(child.GetComponent<Trigger>() != null)
             Assign(child.GetComponent<Trigger>());
         else if(child.GetComponent<LevelEndedZone>() != null)
             child.GetComponent<LevelEndedZone>().levelHandler = levelHandler;


         foreach(Transform c in child.transform)
             checkChild(c);

     }



     //Assign trigger id, add to dictionary and return the id used.
     public void Assign(Trigger trigger){
        temp.Add(trigger);
     }

}