using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DarkRift;

public class ClientManager : MonoBehaviour {

	public string IP = "127.0.0.1";

	public Text debugText;

	public GameObject prefabOtherPlayer;

	//reference to send info when new player joins..
	public Transform player; 

	void Start()
	{
		DarkRiftAPI.workInBackground = true;
		DarkRiftAPI.Connect(IP);
		DarkRiftAPI.onDataDetailed += ReceiveData;

		if(DarkRiftAPI.isConnected){
			//tell everyone else that we have entered so they can tell where they are
			DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager,Network.Subject.HasJoined,"...");
			//DarkRiftAPI.SendMessageToServer(Network.Tag.Manager,Network.Subject.HasJoined,"...");
		}


	}

	void OnApplicationQuit()
	{
		DarkRiftAPI.Disconnect();
	}


	void ReceiveData(ushort senderID,byte tag, ushort subject, object data)
	{


		if(tag == Network.Tag.Manager){
			switch(subject){
				case Network.Subject.ServerSentSpawnPos:{
					Write("ServerSentSpawnPos");
					player.position = ((SVector3) data).get();

					PlayerInfo info = new PlayerInfo();
					info.position = new SVector3(player.position);
					info.rotation = new SQuaternion(player.rotation);

					DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager,Network.Subject.SpawnPlayer,info);
				}break;
				case Network.Subject.SpawnPlayer:{
					Write("SpawnPlayer sender: " + senderID);
					//spawn other player
					Debug.Log(data);
					PlayerInfo info = (PlayerInfo)data;
					GameObject other = Instantiate(prefabOtherPlayer,info.position.get(),info.rotation.get()) as GameObject;

					other.GetComponent<NetPlayerReceiver>().networkID = senderID;

				}break;
				case Network.Subject.HasJoined:{
					Write("HasJoined sender: " + senderID);
					//send player info back

					PlayerInfo info = new PlayerInfo();
					info.position = new SVector3(player.position);
					info.rotation = new SQuaternion(player.rotation);
					DarkRiftAPI.SendMessageToID(senderID,Network.Tag.Manager,Network.Subject.SpawnPlayer,info);
				}break;
			}
		}
	}


	void Write(string mess){
		debugText.text += mess;
	}
}
