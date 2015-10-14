using UnityEngine;
using System.Collections;
using DarkRift;


/*
By KasperHdL

LevelHandler handles loading of scenes and stitching together loaded scenes
*/


public class LevelHandler : MonoBehaviour {

    private LevelContainer currentLevelContainer;

    private TriggerHandler triggerHandler;

    public NetworkManager manager;

	public int[] levelOrder = {1,1,1,2,2,2,2,2,2,2}; 
	
	private int levelIndex = 0;
    private float currentRotation;

	// Use this for initialization
	void Awake () {
    }

    void Start(){
        triggerHandler = TriggerHandler._instance;
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

                //TriggerHandler - find and store all triggers then sync with server
                triggerHandler.process(lc);
			}

		}

        manager.OnLevelLoaded(levelIndex);

        this.levelIndex++; 
    }

    public void processLevelContainer(LevelContainer levelContainer){
        //check if it should stich them together

        if(currentLevelContainer == null){

            //first run setup

            currentLevelContainer = levelContainer;

            GetComponent<GameManager>().setNewLevelManager(levelContainer.levelManager);

        }else{
            //previous and next LevelManager
            LevelManager pLM = currentLevelContainer.levelManager;
            LevelManager nLM = levelContainer.levelManager;

            //Stich togehter with the currentLevelContainer and set the next as current..
            Vector3 pNLD = pLM.nextLevelDirection.normalized;
        
            //rotate next level so that pLM.nextLevelDirection is equal to the inverse nLM.prevLevelDirection
            Debug.Log(pLM.nextLevelDirection + " " + (Mathf.Rad2Deg * currentLevelContainer.transform.rotation.y ));
            float a = Vector3.Angle(pLM.nextLevelDirection,nLM.prevLevelDirection) - currentRotation;
            currentRotation = 180-a;

            //rotate new level
            levelContainer.transform.RotateAround(levelContainer.transform.position,Vector3.up,currentRotation);

            //levelOffset is the amount of space between levels @TODO should be something meaningful
            float levelOffset = 2;
            Vector3 nLevelRailPos = pLM.levelEndRail[0].transform.position + pNLD * levelOffset;
            Vector3 delta = nLevelRailPos - nLM.levelStartRail[0].transform.position;
            
            //offset the next level so that it is positioned correctly 
            levelContainer.transform.position += delta;
            
            //set rails
            for(int i = 0;i<4;i++)
                pLM.levelEndRail[i].NextRail = nLM.levelStartRail[i];
        }


        //@TODO unloading of scenes
    }

	public void loadNextLevel(){
		StartCoroutine(LoadAndHandleLevel(levelIndex));

	} 


    public LevelManager getLevelManager(){
        return currentLevelContainer.levelManager;
    }

}
