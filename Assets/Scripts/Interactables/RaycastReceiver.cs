using UnityEngine;
using System.Collections;

public class RaycastReceiver : MonoBehaviour {
	public IRaycastEvents rayEvent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnRayReceived(int playerIndex){
		rayEvent.RayCastEvent (playerIndex);

	}
}
