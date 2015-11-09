using UnityEngine;
using System.Collections;
using DarkRift;


/*
By KasperHdL

LevelHandler handles loading of scenes and stitching together loaded scenes
*/


public class LevelHandler : MonoBehaviour {
    [HideInInspector]
    public LevelContainer[] levelContainers;
    //for quick access
    private LevelContainer currentLevelContainer;

    private TriggerHandler triggerHandler;

    public NetworkManager manager;

    //@TODO a better interface for this
    //@TODO make sure that it is fixed between server and client
	public string[] levelOrder;

    [HideInInspector]
    public int levelManagerIndex = 0;
    [HideInInspector]
    public int loadedLevelIndex = -1;

    private float currentRotation;

    void Start(){
        levelContainers = new LevelContainer[levelOrder.Length];
        triggerHandler = TriggerHandler.Instance;
        if(NetworkManager.isServer)
            loadNextLevel();
    }

    void Update(){
        if(NetworkManager.isServer && Input.GetKeyDown(KeyCode.Space)){
            loadNextLevel();
        }

    }

	IEnumerator LoadAndHandleLevel(){
        int loadingIndex = loadedLevelIndex;
        if(levelOrder.Length == 0){
            Debug.LogError("No Level Order -- Can't load level");
            return false;
        }else if(loadingIndex < 0 || loadingIndex >= levelOrder.Length){
            Debug.LogError("loadedLevelIndex out of bounds");
            return false;
        }

		//@TODO --- Optimize - can be done async which should be faster
		Application.LoadLevelAdditive(levelOrder[loadingIndex]);

        //wait a frame and try to find LevelContainer and repeat until it finds it
        bool foundLC = false;
        while(!foundLC){
            //waits for one frame
            yield return null;
            Console.Instance.AddMessage("looking for object tagged levelContainer");
            //runs when loaded
            GameObject[] levelContainers = GameObject.FindGameObjectsWithTag("LevelContainer");

            for(int i = 0;i<levelContainers.Length;i++){
                LevelContainer lc = levelContainers[i].GetComponent<LevelContainer>();
                if(!lc.processed){

                    foundLC = true;

                    processLevelContainer(lc,loadingIndex);
                    lc.processed = true;
                    break;
                }
            }
        }

        manager.OnLevelLoaded(loadingIndex);
    }

    public void processLevelContainer(LevelContainer levelContainer,int loadingIndex){
        //check if it should stich them together

        if(loadingIndex == 0){
            //run first time on the server

            triggerHandler.process(levelContainer);
        }else{
            //previous and next LevelManager
            LevelManager pLM = levelContainers[loadingIndex - 1].levelManager;
            LevelManager nLM = levelContainer.levelManager;

            //Stich togehter with the levelContainers and set the next as current..
            Vector3 pLD = pLM.levelEndRail[0].transform.position - pLM.levelEndRail[0].prev.transform.position;
            Vector3 nLD = nLM.levelStartRail[0].transform.position - nLM.levelStartRail[0].next.transform.position;


            nLD.y = 0;
            nLD.Normalize();


            pLD.y = 0;
            pLD.Normalize();

            //rotate next level so that pLD is equal to the inverse nLD
            Debug.Log(pLD + " - " + nLD + " " + (Mathf.Rad2Deg * levelContainers[loadingIndex - 1].transform.rotation.y ) + " magnitudes: " + pLD.magnitude + ";" + nLD.magnitude);

            float a = Vector3.Angle(pLD,nLD) + (Mathf.Rad2Deg * levelContainers[loadingIndex - 1].transform.rotation.y);

            Vector3 cross = Vector3.Cross(pLD,nLD);

            if(cross.y > 0)
            currentRotation = a;
            else if(cross.y == 0)
            currentRotation = a-180;
            else if(cross.y < 0)
            currentRotation = a-180;

            Debug.Log("a: " + a + " next rot : " + currentRotation + " c: " + cross);
            //rotate new level
            levelContainer.transform.RotateAround(pLM.levelEndRail[0].transform.position,Vector3.up,currentRotation);

            Debug.Log("lc rot: " + (Mathf.Rad2Deg * levelContainer.transform.rotation.y));

            //levelOffset is the amount of space between levels @TODO should be something meaningful
            float levelOffset = 0.1f;
            Vector3 nLevelRailPos = pLM.levelEndRail[0].transform.position + pLD.normalized * levelOffset;
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
                pRail.next = nRail;

            }
        }

        levelContainers[loadingIndex] = levelContainer;


        //@TODO unloading of scenes
    }

	public void loadNextLevel(){
        if(loadedLevelIndex < levelContainers.Length - 1){
            loadedLevelIndex++;
            StartCoroutine(LoadAndHandleLevel());
        }
	}

    public void loadLevel(int index){
        if(index >= 0 && index < levelContainers.Length){
            loadedLevelIndex = index;
            StartCoroutine(LoadAndHandleLevel());
        }else
            Console.Instance.AddMessage("Trying to Load Level that is out of Index");
    }

    public LevelManager getLevelManager(){
        return levelContainers[levelManagerIndex].levelManager;
    }

    //current level completed
    public void OnLevelCompleted(){
        if(levelManagerIndex+1 <= levelContainers.Length){
            levelManagerIndex++;
            Debug.Log("lmindex is now " + levelManagerIndex);

            manager.OnLevelCompleted();

        }else{
            Console.Instance.AddMessage("Last Level Completed");
        }

    }
}
