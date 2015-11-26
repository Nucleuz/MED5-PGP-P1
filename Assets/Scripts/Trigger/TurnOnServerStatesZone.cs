using UnityEngine;
using System.Collections;

public class TurnOnServerStatesZone : MonoBehaviour {
	  public ServerManager serverManager;

	  void OnTriggerEnter(Collider col){
	      if(NetworkManager.isServer && col.gameObject.tag == "Player"){
						serverManager.startPushingStates();
	          Destroy(gameObject);
	      }

	  }

}
