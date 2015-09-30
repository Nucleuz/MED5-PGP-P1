using UnityEngine;
using System.Collections;
using DarkRift;

/*
By KasperHdL

Receiver for another player

*/

public class NetPlayerReceiver : MonoBehaviour {

	//network id for the object
	[HideInInspector]
	public ushort networkID;

	void Start () {	
		DarkRiftAPI.onDataDetailed += RecieveData;
	}

	void RecieveData(ushort senderID, byte tag, ushort subject, object data){
		//check that it is the right sender
		if( senderID == networkID ){
			//check if it wants to update the player
			if( subject == Network.Subject.PlayerUpdate ){

				//unpack the data
				PlayerInfo info = (PlayerInfo)data;
				//apply the data
				transform.position = info.position.get();
				transform.rotation = info.rotation.get();
				
			}
		}
	}
}
