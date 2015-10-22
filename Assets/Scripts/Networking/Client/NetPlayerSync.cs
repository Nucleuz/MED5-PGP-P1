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
		//has the rotation or position changed since last sent message
        if(transform.position != lastPosition){

            //serialize and send information
            SVector3 pos = new SVector3(transform.position);
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerPositionUpdate, pos);

            //Save the sent position
            lastPosition = pos.get();
        }
		if( head.rotation != lastRotation ){

            //serialize and send information
			SQuaternion rot = new SQuaternion(head.rotation);
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Player, Network.Subject.PlayerRotationUpdate, rot);
			
			//save the sent position and rotation
			lastRotation = rot.get();
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
                    transform.position = ((SVector3)data).get();
                }break;
				case Network.Subject.PlayerRotationUpdate:
				{
					head.rotation = ((SQuaternion)data).get();	
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
	}
	
	public void SetAsReceiver(){
		isSender = false;
		cam.SetActive(false);
		headControl.enabled = false;
		helmet.enabled = false;
		
	}

    public void SetVoiceChatPlayer(VoiceChatPlayer player)
    {
        this.player = player;
    }
}
