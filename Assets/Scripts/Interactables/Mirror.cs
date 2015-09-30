using UnityEngine;
using System.Collections;

public class Mirror : Interactable {
	[HideInInspector]
	public GameObject triggeredPlayer;
	public Trigger tr;
	public float startPoint;
	public float endPoint;


	void Start(){
		//transform.rotation.y = startPoint;
	}
	void Update(){
		if(tr.isTriggered==true){
			//Check if the rotation is less then or bigger then the goal rotation
			//Rotate forth
			if(transform.rotation.y < endPoint){
				gameObject.transform.Rotate(0* Time.deltaTime,50* Time.deltaTime,0 * Time.deltaTime, Space.World);
				//transform.rotation = Quaternion.Euler(startPoint, endPoint, 5*Time.deltaTime);
			}
			//Rotate back
			else if(transform.rotation.y == endPoint){
				transform.rotation = Quaternion.Euler(endPoint, startPoint, 5*Time.deltaTime);
			}
		}
	}
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
