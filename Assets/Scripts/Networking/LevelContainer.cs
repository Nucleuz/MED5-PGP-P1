using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelContainer : MonoBehaviour {

    //TODO [HideInInspector]
	public bool processed = false;
    //TODO [HideInInspector]
    public bool triggersProcessed = false;

	public LevelManager levelManager;
/*
    //TODO make an editor script that recursively goes through and stores all found triggers in an array (function is already made in TriggerHandler.. )
	[ContextMenuItem("Find all Triggers", "FindTriggers")] //add extra functionality on the right click context menu
	public List<Trigger> triggers;

	public void FindTriggers(){
		checkChild(transform);
	}

	private void checkChild(Transform child){

		if(child.GetComponent<Trigger>() != null)
			Assign(child.GetComponent<Trigger>());

		foreach(Transform c in child.transform)
			checkChild(c);

	}*/


}
