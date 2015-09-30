using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DarkRift;

/*
By KasperHdL

Manager for the client. 

When started asks the server for a spawn position and asks all client for their information this client can spawn the other players.
When the server callsback with the server spawn position spawns enables the players and give its position
Then sends a message to every other client so they can spawn this player


*/

public class ClientManager : MonoBehaviour {

	//IP the client will try to connect to 
	//@TODO - client should be able to change the ip when the client starts up, for easier use
	public string IP = "127.0.0.1";

	public ushort networkID;

	//debug text
	//@TODO - create a kind of console instead if this..
	public Text debugText;

	//prefab for the players
	public GameObject prefabPlayer;
	public Transform player;

	void Start(){
		//Connect to the server
		DarkRiftAPI.workInBackground = true;
		DarkRiftAPI.Connect(IP); //halts until connect or timeout
		DarkRiftAPI.onDataDetailed += ReceiveData;

		if(DarkRiftAPI.isConnected){
			//tell everyone else that we have entered so they can tell where they are
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager,Network.Subject.HasJoined,"...");
		}


	}

	void OnApplicationQuit(){
		DarkRiftAPI.Disconnect();
	}


	void ReceiveData(ushort senderID,byte tag, ushort subject, object data){

		//only handle data if it is for the manager
		if(tag == Network.Tag.Manager){
			switch(subject){
				case Network.Subject.ServerSentSpawnPos:{
					//when the server sends the spawn position

					Write("ServerSentSpawnPos");

					//pack the information
					PlayerInfo info = new PlayerInfo();
					info.position = new SVector3(((SVector3) data).get());
					info.rotation = new SQuaternion(Quaternion.identity);

					//spawn the object
					GameObject g = Instantiate(prefabPlayer,info.position.get(),Quaternion.identity) as GameObject;
					player = g.transform;
					//set the network id so it will sync with the player
					NetPlayerSync netPlayer = 	g.GetComponent<NetPlayerSync>();

					netPlayer.networkID = senderID;
					netPlayer.SetAsSender();
					netPlayer.head.rotation = info.rotation.get();


					//send it to everyone else
					DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager,Network.Subject.SpawnPlayer,info);
				}break;
				case Network.Subject.ServerSentNetID:{
					networkID = (ushort)data;
					if(player != null)
						player.GetComponent<NetPlayerSync>().networkID = (ushort)data;
					else
						Write("ServerSentNetID was received before player was instantiated");


				}break;
				case Network.Subject.SpawnPlayer:{
					//spawn other player
					Write("SpawnPlayer sender: " + senderID);
					
					//unpack data
					PlayerInfo info = (PlayerInfo)data;

					//spawn the object
					GameObject g = Instantiate(prefabPlayer,info.position.get(),Quaternion.identity) as GameObject;

					//set the network id so it will sync with the player
					NetPlayerSync netPlayer = 	g.GetComponent<NetPlayerSync>();

					netPlayer.networkID = senderID;
					netPlayer.SetAsReceiver();
					netPlayer.head.rotation = info.rotation.get();

				}break;
				case Network.Subject.HasJoined:{
					//send player info back when someone has joined
					Write("HasJoined sender: " + senderID);

					//pack player data
					PlayerInfo info = new PlayerInfo();
					info.position = new SVector3(player.position);
					info.rotation = new SQuaternion(player.rotation);

					//send player data to the one who asked
					DarkRiftAPI.SendMessageToID(senderID,Network.Tag.Manager,Network.Subject.SpawnPlayer,info);
				}break;
			}
		}
	}

	//for debugging 
	//@TODO - replace with console
	void Write(string mess){
		if(debugText != null)
			debugText.text += mess + "\n";
	}
}
