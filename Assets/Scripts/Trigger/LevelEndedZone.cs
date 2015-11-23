using UnityEngine;
using System.Collections;

public class LevelEndedZone : MonoBehaviour {

  [ContextMenuItem("Complete Level", "Complete")] //add extra functionality on the right click context menu
    public LevelHandler levelHandler;

    void OnTriggerEnter(Collider col){
        if(NetworkManager.isServer && col.gameObject.tag == "Player"){
            levelHandler.OnLevelCompleted();
            Destroy(gameObject);
        }

    }

    void Complete(){
      levelHandler.OnLevelCompleted();
      Destroy(gameObject);
    }

}
