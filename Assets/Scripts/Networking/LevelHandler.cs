using UnityEngine;
using System.Collections;

using DarkRift;


/*
By KasperHdL

LevelHandler handles loading of scenes and stitching together loaded scenes
*/


public class LevelHandler : MonoBehaviour {

    public LevelContainer[] levelContainers;
    //for quick access
    private LevelContainer currentLevelContainer;

    private TriggerHandler triggerHandler;

    public NetworkManager manager;

    //@TODO a better interface for this 
    //@TODO make sure that it is fixed between server and client
	public string[] levelOrder; 
	
    //@NOTE levelIndex is first incremented when the level is loaded and processed
    public int levelIndex = -1;
    private float currentRotation;

    void Start(){
        levelContainers = new LevelContainer[levelOrder.Length];
        triggerHandler = TriggerHandler._instance;
        if(NetworkManager.isServer)
            loadNextLevel();
    }

    void Update(){
        if(NetworkManager.isServer && Input.GetKeyDown(KeyCode.Space)){
            loadNextLevel(); 
        }

    }
	
	IEnumerator LoadAndHandleLevel(){
        if(levelOrder.Length == 0){
            Debug.LogError("No Level Order -- Can't load level");
            return false;
        }else if(levelIndex < 0 || levelIndex >= levelOrder.Length){
            Debug.LogError("LevelIndex out of bounds");
            return false;
        }
        
		//@TODO --- Optimize - can be done async which should be faster
		Application.LoadLevelAdditive(levelOrder[levelIndex]);
		//it must wait 1 frame therefore
		yield return null;

        //wait a frame and try to find LevelContainer and repeat until it finds it
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

        if(levelIndex == 0){
            //run first time on the server
            if(NetworkManager.isServer)
                GetComponent<GameManager>().setNewLevelManager(levelContainer.levelManager);
        }else{
            //previous and next LevelManager
            LevelManager pLM = levelContainers[levelIndex-1].levelManager;
            LevelManager nLM = levelContainer.levelManager;

            //Stich togehter with the levelContainers and set the next as current..
            Vector3 pNLD = pLM.nextLevelDirection.normalized;
        
            //rotate next level so that pLM.nextLevelDirection is equal to the inverse nLM.prevLevelDirection
            Debug.Log(pLM.nextLevelDirection + " " + (Mathf.Rad2Deg * levelContainers[levelIndex - 1].transform.rotation.y ));
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
            for(int i = 0;i<3;i++){
                Rail pRail = pLM.levelEndRail[i];
                Rail nRail = nLM.levelStartRail[i];

                //check if it is a Rail Connection
                if(pRail.GetComponent<RailConnection>() != null){
                    RailConnection pRailConnection = pRail.GetComponent<RailConnection>();

                    pRailConnection.nRail = nRail;
                    pRailConnection.connectToNext = true;
                }else
                pRail.NextRail = nRail;

            }
        }

        levelContainers[levelIndex] = levelContainer;

        //@TODO unloading of scenes
    }

	public void loadNextLevel(){
        levelIndex++;
		StartCoroutine(LoadAndHandleLevel());
	} 

    public void loadLevel(int index){
        if(index >= 0 && index < levelContainers.Length){
            levelIndex = index;
            StartCoroutine(LoadAndHandleLevel());
        }else
            Debug.LogError("Trying to Load Level that is out of Index");
    }

    public LevelManager getLevelManager(){
        return levelContainers[levelIndex].levelManager;
    }

    public LevelManager getNextLevelManager(){
        if(levelIndex+1 < levelContainers.Length)
            return levelContainers[levelIndex+1].levelManager;

        Debug.LogError("Trying to get LevelManager for Level that is out of the levelContainers array range");
        return null;

    }
}
