using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {
	public GameObject triggeredPlayer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reflect(Ray ray, RaycastHit hit, int playerIndex){

		Ray newRay = new Ray (hit.point, Vector3.Reflect (ray.direction, hit.normal));
		RaycastHit rayhit;
		if (Physics.Raycast (newRay, out rayhit)) {
			Transform newHit = rayhit.transform;
			if (newHit.tag == "Interactable") {
				newHit.GetComponent<RaycastReceiver> ().OnRayReceived (playerIndex);
			} else if (newHit.tag == "Mirror") {
				newHit.GetComponent<Mirror> ().Reflect (newRay, rayhit, playerIndex);
			}
		}
	}

}
