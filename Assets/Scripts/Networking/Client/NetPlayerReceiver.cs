using UnityEngine;
using System.Collections;


using DarkRift;
public class NetPlayerReceiver : MonoBehaviour {

	public ushort networkID;

	// Use this for initialization
	void Start () {
	
		DarkRiftAPI.onDataDetailed += RecieveData;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void RecieveData(ushort senderID, byte tag, ushort subject, object data){
		//Right then. When data is recieved it will be passed here, 
		//we then need to process it if it's got a tag of 1 or 2 
		//(the tags for position and rotation), check it's for us 
		//and update ourself.

		//The catch is we need to do this quite quickly because data
		//is going to be comming in thick and fast and it'll create 
		//lag if we spend time here.

		//If the data is about us, process it.
		if( senderID == networkID ){




			//If it has a PlayerUpdate tag then...
			if( subject == Network.Subject.PlayerUpdate ){

				PlayerInfo info = (PlayerInfo)data;

				transform.position = info.position.get();
				transform.rotation = info.rotation.get();
				
			}
		}
	}
}
