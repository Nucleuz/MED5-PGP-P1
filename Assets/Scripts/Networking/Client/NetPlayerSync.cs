using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkRift;

/*
By KasperHdL and Jalict

Syncs, toggles between being send only or receive only

*/

public class NetPlayerSync : MonoBehaviour {
	
	private bool isSender = false; //if false it is receiver
	
	public Transform head;
	
	//Reference to components that needs to be turn on and off when switching from sender to receiver
	public GameObject cam;

	public HeadControl headControl;
	public VrHeadControl vrHeadControl;
	
	public HelmetLightScript helmet;

	public LightShafts nonFocusedLightShaft;
	public LightShafts focusedLightShaft;

	public Cart cart;
	public Renderer[] coloredObjects;
	public Material[] coloredObjectsMaterial;
	
	//reference to reduce when it sends data to everyone else
	private Quaternion lastRotation;
	private Quaternion lastCart;
	private Vector3 lastPosition;

	private float minDistanceMoved = .5f;
	private float minAngleMoved = 2f;

	private float lastPositionTime = -1f;
	
	//network id for the object
    [HideInInspector]
	public ushort networkID;
	
	private IEnumerator positionRoutine;
	private IEnumerator rotationRoutine;
	private IEnumerator cartRoutine;

	// Use this for initialization
	void Start () {
		helmet.netPlayer = this;
		DarkRiftAPI.onPlayerDisconnected += PlayerDisconnected;
		DarkRiftAPI.onDataDetailed += RecieveData;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isSender && DarkRiftAPI.isConnected){
			SendData();
		}
		//add rail rotation compensation for vr
		if(headControl != null)
			headControl.cartOffsetRotY = transform.rotation.eulerAngles.y;
		else if(vrHeadControl != null){
			vrHeadControl.cartOffsetRotY = transform.rotation.eulerAngles.y;
		}
	}
	
	void SendData(){
		//has the rotation or position changed since last sent message
        if((transform.position - lastPosition).magnitude > minDistanceMoved){

            //serialize and send information
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerPositionUpdate, transform.position.Serialize());

            //Save the sent position
            lastPosition = transform.position;
        }
		if(Quaternion.Angle(head.rotation, lastRotation) > minAngleMoved){

            //serialize and send information
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerRotationUpdate, head.rotation.Serialize());
			
			//save the sent position and rotation
			lastRotation = head.rotation;
		}

		if(Quaternion.Angle(transform.rotation, lastCart) > minAngleMoved){

            //serialize and send information
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerCartUpdate, transform.rotation.Serialize());
			
			//save the sent position and rotation
			lastCart = transform.rotation;
		}

	}
	
	void RecieveData(ushort senderID, byte tag, ushort subject, object data){
		
		//check that it is the right sender
		if(!isSender && senderID == networkID ){
			//check if it wants to update the player
			switch(subject) {
                case Network.Subject.PlayerPositionUpdate:
                {
                    Vector3 position = Deserializer.Vector3((byte[])data);

                    if(positionRoutine != null)
                    	StopCoroutine(positionRoutine);
                    
                    if(lastPositionTime == -1f)
                    	lastPositionTime = Time.time;

                    float interpolationLength = .3f;
                    

                   	if(interpolationLength > 0f){
                   		positionRoutine = InterpolatePosition(position,interpolationLength);
                    	StartCoroutine(positionRoutine);
                   	}
                }break;
				case Network.Subject.PlayerRotationUpdate:
				{
                    Quaternion rotation = Deserializer.Quaternion((byte[])data);
                    if(rotationRoutine != null)
                    	StopCoroutine(rotationRoutine);

                    float interpolationLength = .1f;
                    rotationRoutine = InterpolateRotation(rotation,interpolationLength);
                    StartCoroutine(rotationRoutine);
				}
				break;	
				case Network.Subject.PlayerCartUpdate:
				{
                    Quaternion rotation = Deserializer.Quaternion((byte[])data);
                    if(cartRoutine != null)
                    	StopCoroutine(cartRoutine);

                    float interpolationLength = .1f;
                    cartRoutine = InterpolateCart(rotation,interpolationLength);
                    StartCoroutine(cartRoutine);
				}
				break;
				case Network.Subject.PlayerFocus:
				{
					StopAllCoroutines();
					if((bool)data)
						StartCoroutine(StartFocusing());
					else
						StartCoroutine(StopFocusing());

				}break;
			}
		}		
	}
	
	//When the player disconnects destroy it
	void PlayerDisconnected(ushort ID){
        if(!isSender && ID == networkID)
            Destroy(gameObject);
	}
	
	//--------------------
	//  Getters / Setters
	public void SetAsSender(){
		isSender = true;
		cart.enabled = true;
		cam.SetActive(true);
		if(headControl != null)
		headControl.enabled = true;
		helmet.SetPlayerIndex(networkID);
		helmet.enabled = true;
		SetColor();
	}
	
	public void SetAsReceiver(){
		isSender = false;
		cart.enabled = false;
		cam.SetActive(false);
		if(headControl != null)
		headControl.enabled = false;
		helmet.SetPlayerIndex(networkID);
		helmet.enabled = false;
		SetColor();
	}

	public void SetColor(){
		for(int i = 0; i < coloredObjects.Length; i++){
			coloredObjects[i].material = coloredObjectsMaterial[networkID - 1];
		}
	}

    public void AddCameraToLightShaft(GameObject camera){
		nonFocusedLightShaft.m_Cameras[0] = camera.GetComponent<Camera>();
		focusedLightShaft.m_Cameras[0] = camera.GetComponent<Camera>();

		nonFocusedLightShaft.UpdateCameraDepthMode();
		focusedLightShaft.UpdateCameraDepthMode();
    }

    public void UpdateHelmetLight(bool isFocusing){
		DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerFocus,isFocusing);
    }

    IEnumerator StartFocusing(){
    	float t = 0f;
    	float startTime = Time.time;
    	while(t < 1f){
    		t = (Time.time - startTime)/helmet.spotlightAnimationLength;
    		helmet.LightUpdate(t);
    		yield return null;
    	}

    	helmet.LightUpdate(1f);
    }

    IEnumerator StopFocusing(){
    	float t = 1f;
    	float startTime = Time.time;
    	while(t > 0f){
    		t = 1-((Time.time - startTime)/helmet.spotlightAnimationLength);
    		helmet.LightUpdate(t);
    		yield return null;
    	}

    	helmet.LightUpdate(0f);
    }

    IEnumerator InterpolatePosition(Vector3 newPosition, float interpolationLength){
    	float startTime = Time.time;
    	Vector3 startPosition = transform.position;

    	Vector3 lastFrame;
        cart.minecartAnimator.StartPlayback();

    	float t = 0f;
    	while(t < 1f){
    		t = (Time.time - startTime)/interpolationLength;
            lastFrame = transform.position;
    		transform.position = Vector3.Lerp(startPosition,newPosition,t);
// (transform.position - lastFrame).magnitude
            cart.minecartAnimator.speed = 1;

    		yield return null;
    	}
    	lastPositionTime = Time.time;
    	cart.minecartAnimator.speed = 0;
        cart.minecartAnimator.StopPlayback();

    	//transform.position = newPosition;
    }
    IEnumerator InterpolateRotation(Quaternion newRotation, float interpolationLength){
    	float startTime = Time.time;

    	Quaternion startRotation = head.rotation;
    	float t = 0f;
    	while(t < 1f){
    		t = (Time.time - startTime)/interpolationLength;
    		head.rotation = Quaternion.Slerp(startRotation,newRotation,t);
    		yield return null;
    	}
//    	head.rotation = newRotation;
    }
    IEnumerator InterpolateCart(Quaternion newRotation, float interpolationLength){
    	float startTime = Time.time;

    	Quaternion startRotation = transform.rotation;
    	float t = 0f;
    	while(t < 1f){
    		t = (Time.time - startTime)/interpolationLength;
    		transform.rotation = Quaternion.Slerp(startRotation,newRotation,t);
    		yield return null;
    	}
//    	transform.rotation = newRotation;
    }
}
