using UnityEngine;
using System.Collections;

public class LevelContainer : MonoBehaviour {

    [HideInInspector]
	public bool processed = false;
    [HideInInspector]
    public bool triggersProcessed = false;

	public LevelManager levelManager;

    //TODO make an editor script that recursively goes through and stores all found triggers in an array (function is already made in TriggerHandler.. )
}
