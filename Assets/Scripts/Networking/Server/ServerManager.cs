using UnityEngine;
using System.Collections;
using DarkRift;

/*
By KasperHdL

Manager for the server. This is a test setup with spawn positions and simple server visuals(cube game objects).


*/


public class ServerManager : MonoBehaviour {

	//index for the next player to join, NOTE: cycles with the spawnPos Length
	public ushort nextPos = 0;

	//test spawn positions
	public SVector3[] spawnPos = {
		new SVector3(-1,1,-1),
		new SVector3(-1,1,1),
		new SVector3(1,1,-1),
		new SVector3(1,1,1)
	};

	//reference to player object so the server has a visual indication of the players position and rotation
	public Transform[] players;
	//id of each sender
	public ushort[] senders;

	void Start () {
		senders = new ushort[4];

		//Networking - lets the method OnData be called
		ConnectionService.onData += OnData;
	}


	//Called when we receive data
	void OnData(ConnectionService con, ref NetworkMessage data)
	{
		//Decode the data so it is readable
		data.DecodeData ();

		if(data.tag == Network.Tag.Manager){

			if(data.subject == Network.Subject.HasJoined){
				//if a new player has joined
				
				if(nextPos >= spawnPos.Length)nextPos = 0;
				
				//save the id of sender
				senders[nextPos] = con.id;
				//set server visuals
				players[nextPos].gameObject.SetActive(true);
				
				//send back the spawnpos to the client
				con.SendReply(Network.Tag.Manager,Network.Subject.ServerSentSpawnPos,spawnPos[nextPos++]);
				con.SendReply(Network.Tag.Manager,Network.Subject.ServerSentNetID,con.id);
			}
		}else if(data.tag == Network.Tag.Player){

			if( data.subject == Network.Subject.PlayerUpdate ){
				//if the message is a player update

				//find the index of the sender
				int index = -1;
				for(int i = 0;i<senders.Length;i++){
					if(con.id == senders[i]){
						index = i;
						break;
					}
                }


				if(index != -1){
					//if the player exist on server update the server object

					PlayerInfo info = (PlayerInfo)data.data;

					players[index].position = info.position.get();
					players[index].rotation = info.rotation.get();
				}else{
					Debug.LogError("Sender ID not found");
				}
			}

		}

	}

    public void loadNextLevel(){
        //load the next level internally (in server) and tell clients to do the same.
        //

    }

}
