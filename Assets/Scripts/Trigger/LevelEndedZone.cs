using UnityEngine;
using System.Collections;

public class LevelEndedZone : MonoBehaviour {

    [HideInInspector]
    public TriggerHandler triggerHandler;

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            triggerHandler.OnLevelCompleted();
            Destroy(gameObject);
        }

    }

}
