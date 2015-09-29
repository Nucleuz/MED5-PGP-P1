using UnityEngine;
using System.Collections;

public class Mirror : Interactable {
	[HideInInspector]
	public GameObject triggeredPlayer;

	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit){

		Ray newRay = new Ray (hit.point, Vector3.Reflect (ray.direction, hit.normal));
		RaycastHit rayhit;            
		Debug.DrawRay(hit.point, newRay.direction * 10, Color.cyan);
		if (Physics.Raycast (newRay, out rayhit)) {
			Interactable interactable = rayhit.transform.GetComponent<Interactable>();
			if (interactable != null) {
				interactable.OnRayReceived (playerIndex,newRay, rayhit);
			}
		}
	}

}
