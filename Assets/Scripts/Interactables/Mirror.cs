using UnityEngine;
using System.Collections;

public class Mirror : Interactable {
	[HideInInspector]
	public GameObject triggeredPlayer;
	public Trigger tr;
	public float startPoint; 
	public float endPoint = 300; // Set this value to the rotation needed to complete the puzzle.
	private int turnAmount = 50; // How much it is turning.


	void Start(){
		startPoint = transform.eulerAngles.y; // starPoint is the mirrors rotation at the start.
	}
	void Update(){
		if(tr.isTriggered==true){
			//Debug.Log(transform.eulerAngles.y);
			//Check if the rotation is less then or bigger then the goal rotation
			if(transform.eulerAngles.y < endPoint){
				turnAmount *= -1;
			}
			else if(transform.eulerAngles.y > endPoint){
				turnAmount *= 1;
			}
			//Rotate the object in the y-axis.
			gameObject.transform.Rotate(0,turnAmount* Time.deltaTime,0, Space.World);
			//Maybe write code locks the mirror positon when the correct postions is reached.
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
