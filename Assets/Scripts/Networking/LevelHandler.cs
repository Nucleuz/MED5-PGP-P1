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

    private NetworkManager manager;

    [HideInInspector]
    public int levelManagerIndex = 0;
    [HideInInspector]
    public int loadedLevelIndex = -1;

    void Start(){

        triggerHandler = TriggerHandler.Instance;
        if(NetworkManager.isServer){
            manager = GetComponent<ServerManager>();
        }
    }

    public LevelManager getLevelManager(){
        return levelContainers[levelManagerIndex].levelManager;
    }

    //current level completed
    public void OnLevelCompleted(){
        if(levelManagerIndex+1 <= levelContainers.Length){
            levelManagerIndex++;

            manager.OnLevelCompleted();

        }else{
            Console.Instance.AddMessage("Last Level Completed");
        }

    }
}
