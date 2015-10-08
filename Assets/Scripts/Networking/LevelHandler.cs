using UnityEngine;
using System.Collections;
using DarkRift;


/*
By KasperHdL

LevelHandler handles loading of scenes and stitching together loaded scenes
*/


public class LevelHandler : MonoBehaviour {

	public GameManager gameManager;

	public LevelContainer currentLevelContainer;

	public int[] levelOrder = {1,1,1,2,2,2,2,2,2,2}; 
	
	public int levelIndex = 0;
    private float currentRotation;

	// Use this for initialization
	void Start () {

        loadNextLevel();

	}

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
                loadNextLevel();

        }

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
			if(!lc.processed){
                
                lc.processed = true;
                processLevelContainer(lc);
			}
		}
        this.levelIndex++; 
    }

    public void processLevelContainer(LevelContainer levelContainer){
        //check if it should stich them together

        if(currentLevelContainer == null){

            currentLevelContainer = levelContainer;

        }else{
            LevelManager pLM = currentLevelContainer.levelManager;
            LevelManager nLM = levelContainer.levelManager;

            //Stich togehter with the currentLevelContainer and set the next as current..
            Vector3 pNLD = pLM.nextLevelDirection.normalized;
        
            //rotate next level so that pLM.nextLevelDirection is equal to the inverse nLM.prevLevelDirection
            Debug.Log(pLM.nextLevelDirection + " " + (Mathf.Rad2Deg * currentLevelContainer.transform.rotation.y ));
            float a = Vector3.Angle(pLM.nextLevelDirection,nLM.prevLevelDirection) - currentRotation;
            currentRotation = 180-a;

            levelContainer.transform.RotateAround(levelContainer.transform.position,Vector3.up,currentRotation);

            Vector3 nLevelRailPos = pLM.levelEndRail[0].transform.position + pNLD * 2;
            Vector3 delta = nLevelRailPos - nLM.levelStartRail[0].transform.position;
            
            levelContainer.transform.position += delta;

            
            for(int i = 0;i<4;i++)
                pLM.levelEndRail[i].NextRail = nLM.levelStartRail[i];

            currentLevelContainer = levelContainer;
        }


        //@TODO unloading of scenes

    }

	public void loadNextLevel(){
		StartCoroutine(LoadAndHandleLevel(levelIndex));

	} 




}
