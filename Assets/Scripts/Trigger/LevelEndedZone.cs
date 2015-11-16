using UnityEngine;
using System.Collections;

public class LevelEndedZone : MonoBehaviour {

    public LevelHandler levelHandler;

    void OnTriggerEnter(Collider col){
        if(NetworkManager.isServer && col.gameObject.tag == "Player"){
            levelHandler.OnLevelCompleted();
            Destroy(gameObject);
        }

    }

}
