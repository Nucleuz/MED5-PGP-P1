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
	public float rotateSpeed = 0.5f;
	public int turnAmount = 50; // How much it is turning.
    public Interactable objectToTrigger; // The target that the mirror has to hit.

    //public Rail railPoint;
    private LightShafts LS;

    [Tooltip("Use empty gameobjects as targets that doesn't need to interact and buttons for targets that needs to interact.")]
    public Transform[] targets;

    public int currentInteractable;
    public int correctInteractable;

    public bool movingForward = true;
    public bool isRotating = false;
    public bool isBeingLitOn;

    //light to reflect 
    private Light reflectedLight;   

	void Start(){
        soundIsPlaying = false;
		startPoint = transform.eulerAngles.y; // starPoint is the mirrors rotation at the start.
        currentInteractable = 0;
        reflectedLight = GetComponent<Light>();        //Calls the light component on the mirror.
        LS = GetComponent<LightShafts>();
    }
    
	void Update(){
		//The mirror will reflect only when the player is lighting on the mirror.
		
        if (trigger != null && trigger.isTriggered && !isRotating) {
            rotateMirror();
            if(!soundIsPlaying){
                SoundManager.Instance.PlayEvent("Mirror_Turning_Active", gameObject);
                soundIsPlaying = true;
            }
        }

    }

    private void rotateMirror() {
        //Checks if the script is moving up the index or down
        if (movingForward)
            currentInteractable++;
        else
            currentInteractable--;

        if (currentInteractable < 0) {
            currentInteractable = 1;
            movingForward = true;
        } else if (currentInteractable >= targets.Length) {
            currentInteractable = targets.Length - 2;
            movingForward = false;
        }

        //Calculates the angle between the target gameobjects and the mirror
        Vector3 targetDir = targets[currentInteractable].transform.position - transform.position;
        float rotationalAngle = Vector3.Angle(targetDir, transform.forward);

        Quaternion end = Quaternion.LookRotation(targetDir, transform.up);                      //End position for the mirror to rotate to
        StartCoroutine(rotateTowardsTarget(transform.rotation, end, rotateSpeed));                       //Starts the coroutine that moves the mirror
    }

    public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
        //player hitting


    	//Used for turning on the relfectance of the mirror.
        isBeingLitOn = true;

    	//Set the color of the reflected light to the correct user.
        switch (playerIndex){
            case 1:
                reflectedLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
            break;
            case 2:
                reflectedLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
            break;
            case 3:
                reflectedLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
        }   

    
        Debug.Log("Ray received");
        if(currentInteractable == correctInteractable){
                reflectedLight.enabled = true;
                LS.enabled = true;
                objectToTrigger.OnRayEnter(playerIndex);
        } else {
            reflectedLight.enabled = true;
            LS.enabled = true;
        }

    }

    public override void OnRayEnter(int playerIndex){
        //reflecting from mirror

        //Used for turning on the relfectance of the mirror.
        isBeingLitOn = true;

        //Set the color of the reflected light to the correct user.
        switch (playerIndex){
            case 1:
                reflectedLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
            break;
            case 2:
                reflectedLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
            break;
            case 3:
                reflectedLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
        }   

    
        Debug.Log("Ray received");
        if(currentInteractable == correctInteractable){
                reflectedLight.enabled = true;
                LS.enabled = true;
                objectToTrigger.OnRayEnter(playerIndex);
        } else {
            reflectedLight.enabled = true;
            LS.enabled = true;
        }
    }

    public override void OnRayExit(int playerIndex){
            reflectedLight.enabled = false;
            LS.enabled = false;
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
        if(soundIsPlaying){
            SoundManager.Instance.PlayEvent("Mirror_Turning_Stop", gameObject);
            soundIsPlaying = false;
        }
        //trigger.canReset = true;
        trigger.isReadyToBeTriggered = true;
    }
}