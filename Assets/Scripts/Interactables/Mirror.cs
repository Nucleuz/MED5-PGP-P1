using UnityEngine;
using System.Collections;

public class Mirror : Interactable {
	[HideInInspector]
	public GameObject triggeredPlayer;

	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){

		Ray newRay = new Ray (hit.point, Vector3.Reflect (ray.direction, hit.normal));
		RaycastHit rayhit;            
		Debug.DrawRay(hit.point, newRay.direction * 10, Color.cyan, 1f);
		if (Physics.Raycast (newRay, out rayhit)) {

		    lineRenderer.SetVertexCount(++nextLineVertex);             // resets the number of vertecies of the line renderer to 1
		    lineRenderer.SetPosition(nextLineVertex-1, rayhit.point);  // sets the line origin to the 

			Interactable interactable = rayhit.transform.GetComponent<Interactable>();
			if (interactable != null) {
                //@Optimize - The mirror is the only one who the ray, hit, lineRenderer, and count
				interactable.OnRayReceived (playerIndex,newRay, rayhit, ref lineRenderer,nextLineVertex);
			}
		}
	}

}
