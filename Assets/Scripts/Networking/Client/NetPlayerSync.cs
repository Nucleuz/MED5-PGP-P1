using UnityEngine;
using System.Collections;
using DarkRift;
using VoiceChat;

/*
By KasperHdL and Jalict

Syncs, toggles between being send only or receive only

*/
using System.Collections.Generic;
using VoiceChat.Networking;

public class NetPlayerSync : MonoBehaviour {
	
	private bool isSender = false; //if false it is receiver
	
	public Transform head;
	
	//Reference to components that needs to be turn on and off when switching from sender to receiver
	public GameObject cam;
	public HeadControl headControl;
	private VoiceChatPlayer player;
	
	public HelmetLightScript helmet;

	public LightShafts nonFocusedLightShaft;
	public LightShafts focusedLightShaft;
	
	//reference to reduce when it sends data to everyone else
	Quaternion lastRotation;
	Vector3 lastPosition;
	
	//network id for the object
	public ushort networkID;
	
	// Use this for initialization
	void Start () {
		DarkRiftAPI.onPlayerDisconnected += PlayerDisconnected;
		DarkRiftAPI.onDataDetailed += RecieveData;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isSender && DarkRiftAPI.isConnected){
			SendData();
		}
	}
	
	void SendData(){
        //@TODO -- currently dobble dipping

		//has the rotation or position changed since last sent message
        if(transform.position != lastPosition){

            //serialize and send information
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerPositionUpdate, transform.position.Serialize());

            //Save the sent position
            lastPosition = transform.position;
        }
		if( head.rotation != lastRotation ){

            //serialize and send information
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerRotationUpdate, head.rotation.Serialize());
			
			//save the sent position and rotation
			lastRotation = head.rotation;
		}
	}
	
	// Called once there is a new packet/sample ready
	public void OnNewSample (VoiceChatPacket packet)
	{
		DarkRiftAPI.SendMessageToOthers (Network.Tag.Player, Network.Subject.VoiceChat, VoiceChatUtils.Serialise(packet));	// Send the packet to all other players
	}
	
	void RecieveData(ushort senderID, byte tag, ushort subject, object data){
		
		//check that it is the right sender
		if(!isSender && senderID == networkID ){
			//check if it wants to update the player
			switch(subject) {
                case Network.Subject.PlayerPositionUpdate:
                {
                    Vector3 position = Deserializer.Vector3((byte[])data);
                    transform.position = position;
                }break;
				case Network.Subject.PlayerRotationUpdate:
				{
                    Quaternion rotation = Deserializer.Quaternion((byte[])data);
					head.rotation = rotation;	
				}
				break;
				case Network.Subject.VoiceChat:
				{
					VoiceChatPacket packet = VoiceChatUtils.Deserialise((byte[])data);
					player.OnNewSample(packet); // Queue package to the VoiceChatPlayer
				}
				break;
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
		cam.SetActive(true);
		headControl.enabled = true;
		helmet.enabled = true;
		helmet.SetPlayerIndex(networkID);
	}
	
	public void SetAsReceiver(){
		isSender = false;
		cam.SetActive(false);
		headControl.enabled = false;
		helmet.enabled = false;
		helmet.SetPlayerIndex(networkID);
	}

    public void SetVoiceChatPlayer(VoiceChatPlayer player)
    {
        this.player = player;
    }

    public void AddCameraToLightShaft(GameObject camera){
    	if(nonFocusedLightShaft.m_Cameras[0] == null){
    		nonFocusedLightShaft.m_Cameras[0] = camera.GetComponent<Camera>();
    		focusedLightShaft.m_Cameras[0] = camera.GetComponent<Camera>();
    	}else{
    		nonFocusedLightShaft.m_Cameras[1] = camera.GetComponent<Camera>();
    		focusedLightShaft.m_Cameras[1] = camera.GetComponent<Camera>();
    	}
    }
}
