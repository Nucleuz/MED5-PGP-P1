using UnityEngine;
using System.Collections;
using System;
using DarkRift;


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

    private ushort networkID;

    [Tooltip("Use empty gameobjects as targets that doesn't need to interact and buttons for targets that needs to interact.")]
    public Transform[] targets;

    public int currentInteractable;
    public int correctInteractable;

    public bool movingForward = true;
    public bool isRotating = false;
    private Trigger reflectingTrigger;

    private bool playerSet = false; 

    //light to reflect 
    private bool[] playersReflecting = new bool[3];
    private Light reflectedLight;   
    private LightShafts LS;

	void Start(){
        soundIsPlaying = false;
		startPoint = transform.eulerAngles.y; // starPoint is the mirrors rotation at the start.
        currentInteractable = 0;
        reflectedLight = GetComponent<Light>();        //Calls the light component on the mirror.
        LS = GetComponent<LightShafts>();
        reflectingTrigger = GetComponent<Trigger>();

        DarkRiftAPI.onDataDetailed += RecieveData;
    }
    
	void Update(){
		//The mirror will reflect only when the player is lighting on the mirror.
		
        if (trigger != null && trigger.isTriggered && !isRotating) {
            RotateToNext();
            if(!soundIsPlaying){
                SoundManager.Instance.PlayEvent("Mirror_Turning_Active", gameObject);
                soundIsPlaying = true;
            }
        }

        if(!trigger.isTriggered && currentInteractable != trigger.state){
            currentInteractable = trigger.state;
            RotateToCurrent();
        }
    }

    private void RotateToNext(){
        currentInteractable = (currentInteractable +1) % targets.Length;
        RotateToCurrent();
    }

    private void RotateToCurrent() {
        //Calculates the angle between the target gameobjects and the mirror
        Vector3 targetDir = targets[currentInteractable].transform.position - transform.position;
        float rotationalAngle = Vector3.Angle(targetDir, transform.forward);

        Quaternion end = Quaternion.LookRotation(targetDir, transform.up);                      //End position for the mirror to rotate to
        StartCoroutine(rotateTowardsTarget(transform.rotation, end, rotateSpeed));                       //Starts the coroutine that moves the mirror
    }


    public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
        //player hitting
        DarkRiftAPI.SendMessageToOthers(Network.Tag.Mirror, Network.Subject.MirrorStarted, new ushort[2] {(ushort)playerIndex,reflectingTrigger.triggerID});
        OnRayEnter(playerIndex);
    }

    public override void OnRayEnter(int playerIndex){
        Debug.Log("Player received from " + playerIndex);

        //reflecting from mirror
        if(!playerSet && ClientManager.player != null){
            LS.m_Cameras[0] = ClientManager.player.GetComponent<NetPlayerSync>().cam.GetComponent<Camera>();
            LS.UpdateCameraDepthMode();
            playerSet = true;
        }

        playersReflecting[playerIndex - 1] = true;
        setReflectedColor(playersReflecting);
    
        reflectedLight.enabled = true;
        LS.enabled = true;

        if(currentInteractable == correctInteractable){
            objectToTrigger.OnRayEnter(playerIndex);
        }
    }

    public override void OnRayExit(int playerIndex){

        DarkRiftAPI.SendMessageToOthers(Network.Tag.Mirror, Network.Subject.MirrorEnded, new ushort[2] {(ushort)playerIndex,reflectingTrigger.triggerID});
        StopReflecting(playerIndex);
    }

    private void StopReflecting(int playerIndex){
        playersReflecting[playerIndex - 1] = false;

        if(playersReflecting[0] || playersReflecting[1] || playersReflecting[2]){
            setReflectedColor(new bool[3] {playersReflecting[0],playersReflecting[1],playersReflecting[2] });
        }else{
            reflectedLight.enabled = false;
            LS.enabled = false;
        }

        if(currentInteractable == correctInteractable)
            objectToTrigger.OnRayExit(playerIndex);
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

    public void RecieveData(ushort senderID, byte tag, ushort subject, object data){
        //check that it is the right sender
        if(tag == Network.Tag.Mirror){
            //The first in this array is the playerIndex, the second is triggerID of the mirrors trigger
            ushort[] indices = (ushort[]) data;


            if(indices[1] == reflectingTrigger.triggerID){
                //check if it wants to update the player
                if(subject == Network.Subject.MirrorStarted){
                    Debug.Log("mirrorstart rec> " + indices[0] + " tid> " + indices[1]);
                   OnRayEnter(indices[0]);
                }if(subject == Network.Subject.MirrorEnded){
                    StopReflecting(indices[0]);
                    Debug.Log("mirrorEnded rec> " + indices[0] + " tid> " + indices[1]);
                }     
            }
        }     
    }

    public void setReflectedColor(bool[] a){
        //a[0] = red, a[1] = green, a[2] = blue
        bool    r = a[1],
                g = a[2],
                b = a[0];

        if(r && !g && !b)                    
            reflectedLight.color = Color.red;                         
        else if(!r && g && !b)
            reflectedLight.color = Color.green;
        else if(!r && !g && b)
            reflectedLight.color = Color.blue;
        else if(r && !g && b)
            reflectedLight.color = Color.magenta;
        else if(r && g && !b)
            reflectedLight.color = Color.yellow;
        else if(!r && g && b)
            reflectedLight.color = Color.cyan;
        else if(r && g && b)
            reflectedLight.color = Color.white;                
    }
}