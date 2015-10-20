using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DarkRift;

/*
By KasperHdL & Jalict

Manager for the server. This is a test setup with spawn positions and simple server visuals(cube game objects).


*/


public class ServerManager : NetworkManager {

    //for debugging
    public Text debug;

	//index for the next player to join, NOTE: cycles with the spawnPos Length
	public int playerIndex;

    public int currentLevel;
    
    //reference to player object so the server has a visual indication of the players position and rotation
	public Transform[] players;
	//id of each sender
	public ushort[] senders;
    public ConnectionService[] connections;

    public GameManager gameManager;
    public LevelHandler levelHandler;
    public TriggerHandler triggerHandler;

	void Start () {

        gameManager = GetComponent<GameManager>();
        triggerHandler = GetComponent<TriggerHandler>();
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
        }else if(data.tag == Network.Tag.Trigger){
            //relay to triggerHandler
            if(data.subject == Network.Subject.RequestTriggerIDs){
                TriggerState[] triggerStates = new TriggerState[triggerHandler.triggers.Count];
                //populate
                
                for(int i = 0;i<triggerHandler.triggers.Count;i++){
                    triggerStates[i] = new TriggerState(triggerHandler.triggers[i]);
                    Debug.Log(triggerStates[i].id);
                }

                con.SendReply(
                        Network.Tag.Trigger,
                        Network.Subject.ServerSentTriggerIDs,
                        triggerStates);
            }else if(data.subject == Network.Subject.TriggerActivate){
                triggerHandler.TriggerInteracted((ushort)data.data,true);

                //force update GameMnager
                gameManager.DetectTriggerChanges();

                TriggerState state = triggerHandler.GetTriggerState((ushort)data.data);
                
                //send to clients but not the sender
                SendToAll(data.tag,Network.Subject.TriggerState,state);
            }else if(data.subject == Network.Subject.TriggerDeactivate){
                triggerHandler.TriggerInteracted((ushort)data.data,false);

                //force update GameMnager
                gameManager.DetectTriggerChanges();

                TriggerState state = triggerHandler.GetTriggerState((ushort)data.data);

                //send to clients but not the sender
                SendToAllBut(con, data.tag,Network.Subject.TriggerState,data.data);
            }
        }
	}

    public void TriggerChanged(Trigger trigger){

        TriggerState state = new TriggerState(trigger);
        
        //send to clients but not the sender
        SendToAll(Network.Tag.Trigger,Network.Subject.TriggerState,state);
    }

    public override void OnLevelLoaded(int levelIndex){
        currentLevel = levelIndex;

        Debug.Log("Level " + levelIndex + " (" + levelHandler.levelOrder[levelIndex] + ") Loaded");
        // when level is loaded on server tell clients to do the same.
        SendToAll(Network.Tag.Manager, Network.Subject.ServerLoadedLevel, levelIndex);
    }

    private void SendToAll(byte tag, ushort subject, object data){
        for(int i = 0;i < connections.Length;i++){
            if(connections[i] != null)
                connections[i].SendReply(tag,subject,data);
        }
    }

    private void SendToAllBut(ConnectionService con, byte tag, ushort subject, object data){
        for(int i = 0;i < connections.Length;i++)
            if(connections[i] != null || connections[i] == con)
                connections[i].SendReply(tag, subject, data);
    }

	private void OnApplicationQuit() {
		// Close all connections
		foreach (var con in connections) {
			con.Close();
		}

		// Close server
		DarkRiftServer.Close(false);
	}
}
