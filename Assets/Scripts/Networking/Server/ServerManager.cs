using UnityEngine;
using System.Collections;
using DarkRift;

/*
By KasperHdL

Manager for the server. This is a test setup with spawn positions and simple server visuals(cube game objects).


*/


public class ServerManager : NetworkManager {

	//index for the next player to join, NOTE: cycles with the spawnPos Length
	public int playerIndex;

    public int currentLevel;
    
    //reference to player object so the server has a visual indication of the players position and rotation
	public Transform[] players;
	//id of each sender
	public ushort[] senders;
    public ConnectionService[] connections;

    public LevelHandler levelHandler;
	void Start () {

        levelHandler = GetComponent<LevelHandler>();
        isServer = true;
		senders = new ushort[4];
        connections = new ConnectionService[4];

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
				
				//save the id of sender
				senders[playerIndex] = con.id;
                connections[playerIndex] = con;
				//set server visuals
				players[playerIndex].gameObject.SetActive(true);
			
                playerIndex++;

                Debug.Log("Player joined told to load level " + currentLevel);

				//send back the spawnpos to the client
				con.SendReply(
                        Network.Tag.Manager, 
                        Network.Subject.ServerSentNetID, 
                        con.id);
				con.SendReply(Network.Tag.Manager, 
                        Network.Subject.ServerLoadedLevel, 
                        currentLevel);
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
                    //TODO change this update position and roatation individually

					players[index].position = info.position.get();
					players[index].rotation = info.rotation.get();
				}else{
					Debug.LogError("Sender ID not found");
				}
			}
        }

	}


    public override void OnLevelLoaded(int levelIndex){
        currentLevel = levelIndex;

        Debug.Log("Level " + levelIndex + " (" + levelHandler.levelOrder[levelIndex] + ") Loaded");

        // when level is loaded on server tell clients to do the same.
        for(int i = 0;i < connections.Length;i++){
            if(connections[i] != null )
                connections[i].SendReply(Network.Tag.Manager, Network.Subject.ServerLoadedLevel, levelIndex);
        }


    }
}
