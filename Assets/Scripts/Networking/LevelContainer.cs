using UnityEngine;
using System.Collections;

public class LevelContainer : MonoBehaviour {

	public bool processed = false;

	public LevelManager levelManager;

	public void process(LevelHandler levelHandler){
		if(levelManager == null)
			Debug.LogError("The Level should contain a levelManager");

        levelHandler.setNextLevelManager(levelManager);
            

		processed = true;
	}


	public bool HasBeenProcessed(){
		return processed;
	}

}
