using UnityEngine;
using System.Collections;
using DarkRift;


/*
By KasperHdL

LevelHandler handles loading of scenes and stitching together loaded scenes
*/


public class LevelHandler : MonoBehaviour {

	public GameManager gameManager;

	public LevelManager currentLevelManager;
	public LevelManager nextLevelManager;


	public int[] levelOrder = {1,2}; 
	
    [HideInInspector]
	public int levelIndex = 0;

	// Use this for initialization
	void Start () {

		loadNextLevel();

	}
	

	IEnumerator LoadAndHandleLevel(int levelIndex){
		//@Optimize - can be done async which should be faster
		Application.LoadLevelAdditive(levelOrder[levelIndex]);
		//it must wait 1 frame therefore
		yield return null;

		//runs when loaded
		GameObject[] levelContainers = GameObject.FindGameObjectsWithTag("LevelContainer");

		for(int i = 0;i<levelContainers.Length;i++){
			LevelContainer lc = levelContainers[i].GetComponent<LevelContainer>();
			if(!lc.HasBeenProcessed()){

				lc.process(this);
			}
		}
        this.levelIndex++; 
    }

    public void setNextLevelManager(LevelManager levelManager){
        //check if it should stich them together

        if(currentLevelManager == null){

            currentLevelManager = levelManager;

        }else{
            //Stich togehter with the currentLevelManager and set the next as current..
            //

        }

        //@TODO unloading of scenes

    }

	public void loadNextLevel(){
		StartCoroutine(LoadAndHandleLevel(levelIndex));

	}


}
