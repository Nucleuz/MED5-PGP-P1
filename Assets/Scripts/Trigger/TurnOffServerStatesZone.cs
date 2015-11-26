using UnityEngine;
using System.Collections;

public class TurnOffServerStatesZone : MonoBehaviour {
    public ServerManager serverManager;

    void OnTriggerEnter(Collider col){
        if(NetworkManager.isServer && col.gameObject.tag == "Player"){
						serverManager.stopPushingStates();
            Destroy(gameObject);
        }

    }
}
