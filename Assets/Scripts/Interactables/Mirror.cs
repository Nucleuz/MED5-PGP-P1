using UnityEngine;
using System.Collections;
using System;

public class Mirror : Interactable {
	[HideInInspector]
	public GameObject triggeredPlayer;
	public Trigger trigger;
	public float startPoint; 
	public float endPoint = 300; // Set this value to the rotation needed to complete the puzzle.
	public int turnAmount = 50; // How much it is turning.
    public InteractableButton ButtonToTrigger; // The target that the mirror has to hit.

    [Tooltip("Use empty gameobjects as targets that doesn't need to interact and buttons for targets that needs to interact.")]
    public Transform[] targets;

    public int buttonNumber;

    public bool movingForward = true;
    public bool isRotating = false;

	void Start(){
		startPoint = transform.eulerAngles.y; // starPoint is the mirrors rotation at the start.
        buttonNumber = 0;
    }
    
	void Update(){
        if (trigger != null && trigger.isTriggered && !isRotating) {
            rotateMirror();

        }


        /*
		if(trigger != null && trigger.isTriggered) {
			//Debug.Log(transform.eulerAngles.y);
			//Check if the rotation is less then or bigger then the goal rotation
			if(transform.eulerAngles.y < endPoint){
				turnAmount *= -1;
			}
			else if(transform.eulerAngles.y > endPoint){
				turnAmount *= 1;
			}

            //Rotate the object in the y-axis.
            gameObject.transform.Rotate(0, turnAmount * Time.deltaTime, 0, Space.World);
            //Maybe write code locks the mirror positon when the correct postions is reached.

        }
        */
    }

    private void rotateMirror() {

        //Checks if the script is moving up the index or down
        if (movingForward)
            buttonNumber++;
        else
            buttonNumber--;

        if (buttonNumber < 0) {
            buttonNumber = 1;
            movingForward = true;
        } else if (buttonNumber >= targets.Length) {
            buttonNumber = targets.Length - 2;
            movingForward = false;
        }

        //Calculates the angle between the target gameobjects and the mirror
        Vector3 targetDir = targets[buttonNumber].transform.position - transform.position;
        float rotationalAngle = Vector3.Angle(targetDir, transform.forward);

        Quaternion end = Quaternion.LookRotation(targetDir, transform.up);                      //End position for the mirror to rotate to
        StartCoroutine(rotateTowardsTarget(transform.rotation, end, 1f));                       //Starts the coroutine that moves the mirror
        
    }

    public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
        
        //Ray newRay = new Ray (hit.point, Vector3.Reflect (ray.direction, hit.normal));        //Legacy code. Calculates a realistisc reflection off the mirror

        //Shoots a new raycast to the point of the mirror.
        Ray newRay = new Ray(hit.point, ButtonToTrigger.transform.position - transform.position);
        RaycastHit rayhit;

        Vector3 targetDir = ButtonToTrigger.transform.position - transform.position;
        float rotationalAngle = Vector3.Angle(targetDir, transform.forward);

        //Debug.Log(rotationalAngle);

        if (rotationalAngle < 5f) {

            if (Physics.Raycast(newRay, out rayhit)) {
            
		        Debug.DrawRay(hit.point, newRay.direction * 10, Color.cyan, 1f);
                lineRenderer.SetVertexCount(++nextLineVertex);             // resets the number of vertecies of the line renderer to 1
                lineRenderer.SetPosition(nextLineVertex - 1, rayhit.point);  // sets the line origin to the 

                Interactable interactable = rayhit.transform.GetComponent<Interactable>();
                if (interactable != null) {
                    //@Optimize - The mirror is the only one who the ray, hit, lineRenderer, and count
                    interactable.OnRayReceived(playerIndex, newRay, rayhit, ref lineRenderer, nextLineVertex);
                }
            }
        }
	}

    IEnumerator rotateTowardsTarget(Quaternion start, Quaternion end, float length) {
        isRotating = true;
        float startTime = Time.time;
        float endTime = startTime + length;

        while(Time.time < endTime) {
            trigger.isTriggered = false; // @NOTE hack!
            transform.rotation = Quaternion.Slerp(start,end,(Time.time - startTime) / length);
            yield return null;
        }
        isRotating = false;
        trigger.canReset = true;
    }
}