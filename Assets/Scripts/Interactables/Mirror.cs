using UnityEngine;
using System.Collections;
using System;

public class Mirror : Interactable {
	[HideInInspector]
    private bool soundIsPlaying;

	public GameObject triggeredPlayer;
	public Trigger trigger;
	public float startPoint; 
	public float endPoint = 300; // Set this value to the rotation needed to complete the puzzle.
	public int turnAmount = 50; // How much it is turning.
    public InteractableButton ButtonToTrigger; // The target that the mirror has to hit.

    public Cart player;
    //public Rail railPoint;
    private LightShafts LS;

    [Tooltip("Use empty gameobjects as targets that doesn't need to interact and buttons for targets that needs to interact.")]
    public Transform[] targets;

    public int buttonNumber;

    public bool movingForward = true;
    public bool isRotating = false;
    public bool isBeingLitOn;

    //light to reflect 
    private Light reflectedLight;   

	void Start(){
        soundIsPlaying = false;
		startPoint = transform.eulerAngles.y; // starPoint is the mirrors rotation at the start.
        buttonNumber = 0;
        reflectedLight = GetComponent<Light>();        //Calls the light component on the mirror.
        LS = GetComponent<LightShafts>();
    }
    
	void Update(){
		//The mirror will reflect only when the player is lighting on the mirror.
		if(isBeingLitOn == true){
			reflectedLight.enabled = true;
			LS.enabled = true;
		} else if(isBeingLitOn == false){
			reflectedLight.enabled = false;
			LS.enabled = false;
		} 
		if(isBeingLitOn == true){
			isBeingLitOn = false;
		}
		
		
        if (trigger != null && trigger.isTriggered && !isRotating) {
            rotateMirror();
            if(!soundIsPlaying)
                SoundManager.Instance.PlayEvent("Mirror_Turning_Active", gameObject);
                soundIsPlaying = true;
            }
        }

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
    	//Used for turning on the relfectance of the mirror.
        isBeingLitOn = true;

    	//Set the color of the reflected light to the correct user.
        switch (playerIndex){
            case 1:
                reflectedLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
            break;
            case 2:
                reflectedLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
            break;
            case 3:
                reflectedLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
            }   
        
        //Ray newRay = new Ray (hit.point, Vector3.Reflect (ray.direction, hit.normal));        //Legacy code. Calculates a realistisc reflection off the mirror

        //Shoots a new raycast to the point of the mirror if they are at the correct rail.
        //if(player.CurrentRail == railPoint){
            Ray newRay = new Ray(hit.point, ButtonToTrigger.transform.position - transform.position);
            RaycastHit rayhit;

            Vector3 targetDir = ButtonToTrigger.transform.position - transform.position;
            float rotationalAngle = Vector3.Angle(targetDir, transform.forward);

            if (rotationalAngle < 5f) {
                if (Physics.Raycast(newRay, out rayhit)) {
                	//turn on the light off the mirror.
                	//reflectedLight.enabled = true;
                    //Debug.DrawRay(hit.point, newRay.direction * 10, Color.cyan, 1f);
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
	//}

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
        if(soundIsPlaying){
            SoundManager.Instance.PlayEvent(Mirror_Turning_Stop);
            soundIsPlaying = false;
        }
        //trigger.canReset = true;
        trigger.isReadyToBeTriggered = true;
    }
}