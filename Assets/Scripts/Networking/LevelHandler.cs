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

    //@TODO a better interface for this 
    //@TODO make sure that it is fixed between server and client
	public string[] levelOrder; 
	
    public int levelIndex = 0;
    private float currentRotation;

    void Start(){
        triggerHandler = TriggerHandler._instance;
        if(NetworkManager.isServer)
            loadNextLevel();
    }

    void Update(){
        if(NetworkManager.isServer && Input.GetKeyDown(KeyCode.Space)){
            loadNextLevel(); 
        }

    }
	
	IEnumerator LoadAndHandleLevel(int levelIndex){
        if(levelOrder.Length == 0){
            Debug.LogError("No Level Order -- Can't load level");
            return false;
        }else if(levelIndex < 0 || levelIndex >= levelOrder.Length){
            Debug.LogError("LevelIndex out of bounds");
            return false;
        }
		//@Optimize - can be done async which should be faster
		Application.LoadLevelAdditive(levelOrder[levelIndex]);
		//it must wait 1 frame therefore
		yield return null;

        bool foundLC = false;
        while(!foundLC){
            //waits for one frame 
            yield return null;
            Debug.Log("looking for object tagged levelContainer");
            //runs when loaded
            GameObject[] levelContainers = GameObject.FindGameObjectsWithTag("LevelContainer");

            for(int i = 0;i<levelContainers.Length;i++){
                LevelContainer lc = levelContainers[i].GetComponent<LevelContainer>();
                if(!lc.processed){

                    foundLC = true;
                    
                    lc.processed = true;
                    processLevelContainer(lc);

                    //TriggerHandler - find and store all triggers then sync with server
                    triggerHandler.process(lc);
                    break;
                }

            }
        }

        manager.OnLevelLoaded(levelIndex);
    }

    public void processLevelContainer(LevelContainer levelContainer){
        //check if it should stich them together

        if(currentLevelContainer == null){
            //first run setup
            if(NetworkManager.isServer)
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


        currentLevelContainer = levelContainer;
        //@TODO unloading of scenes
    }

	public void loadNextLevel(){
		StartCoroutine(LoadAndHandleLevel(levelIndex));
        levelIndex++;
	} 

    public void loadLevel(int index){

        StartCoroutine(LoadAndHandleLevel(index));
    }

    public LevelManager getLevelManager(){
        return currentLevelContainer.levelManager;
    }

}
