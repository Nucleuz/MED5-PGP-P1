using UnityEngine;
using System.Collections;
using DarkRift;
using VoiceChat;

/*
By KasperHdL and Jalict

Syncs, toggles between being send only or receive only

*/
using System.Collections.Generic;


public class NetPlayerSync : MonoBehaviour {

	private bool isSender = false; //if false it is receiver

	public Transform head;

	//Reference to components that needs to be turn on and off when switching from sender to receiver
	public GameObject cam;
	public HeadControl headControl;


	//reference to reduce when it sends data to everyone else
	Quaternion lastRotation;
	Vector3 lastPosition;

	//network id for the object
	public ushort networkID;

	private VoiceChatPlayer voiceChatPlayer;

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
		//has the rotation or position changed since last sent message
		if( head.rotation != lastRotation || transform.position != lastPosition){
			//pack player infomation
			PlayerInfo info = new PlayerInfo();
			info.position = new SVector3(transform.position);
			info.rotation = new SQuaternion(head.rotation);

			//send it to everyone else
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerUpdate, info);


			//save the sent position and rotation
			lastPosition = info.position.get();
			lastRotation = info.rotation.get();
		}
	}

	// Called once there is a new packet/sample ready
	void OnNewSample (VoiceChatPacket packet)
	{
		DarkRiftAPI.SendMessageToOthers (Network.Tag.Player, Network.Subject.VoicePackage, packet);	// Send the packet to all other players
	}

	public void VoiceChatInit ()
	{
		voiceChatPlayer = GameObject.Find("VoiceChat_Player").GetComponent<VoiceChatPlayer>(); 		//TODO: Sorry for I have sinned

		VoiceChatRecorder.Instance.Device = VoiceChatRecorder.Instance.AvailableDevices [0];		// Set Microphone to first avaliable audio device
		VoiceChatRecorder.Instance.NetworkId = networkID;
		VoiceChatRecorder.Instance.StartRecording ();												// Start recording (Not the same as transmitting!)
		VoiceChatRecorder.Instance.NewSample += OnNewSample;										// Subscribe to the NewSample 
	}
	
	void RecieveData(ushort senderID, byte tag, ushort subject, object data){
		//check that it is the right sender
		if(!isSender && senderID == networkID ){
			//check if it wants to update the player
			if( subject == Network.Subject.PlayerUpdate ){

				//unpack the data
				PlayerInfo info = (PlayerInfo)data;
				//apply the data
				transform.position 	= 	info.position.get();
				head.rotation 		= 	info.rotation.get();
				
			}

			// Check if wants to update the voice packet
			if(subject == Network.Subject.VoicePackage) {
				//TODO Check for networkId and send sound to correlating player
				voiceChatPlayer.OnNewSample((VoiceChatPacket) data);	
			}
		}
	}

	//When the player disconnects destroy it
	void PlayerDisconnected(ushort ID){
		Destroy(gameObject);
	}

	//--------------------
	//  Getters / Setters
	public void SetAsSender(){
		isSender = true;
		cam.SetActive(true);
		headControl.enabled = true;
	}

	public void SetAsReceiver(){
		isSender = false;
        cam.SetActive(false);
		headControl.enabled = false;

	}
}
