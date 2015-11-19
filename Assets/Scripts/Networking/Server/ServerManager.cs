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

    //reference to player object so the server has a visual indication of the players position and rotation
	public Transform[] players;
	//id of each sender
	public ushort[] senders;
    public ConnectionService[] connections;

    public GameManager gameManager;
    public LevelHandler levelHandler;
    public TriggerHandler triggerHandler;

    private bool[] clientLoadedLevel = new bool[3];

    private float[] levelTimings;

    private string currentDate;

    public bool updatingStates = true;
    public float stateUpdateInterval;


	void Start () {


      currentDate = System.DateTime.Now.ToString("HHmm");
      gameManager = GetComponent<GameManager>();
      triggerHandler = GetComponent<TriggerHandler>();
      levelHandler = GetComponent<LevelHandler>();
      isServer = true;
	      senders = new ushort[4];
      connections = new ConnectionService[4];
      currentDate = System.DateTime.Now.ToString("HHmm");

  		//Networking - lets the method OnData be called
  		ConnectionService.onData += OnData;

      levelTimings = new float[6];

      StartCoroutine(updateState());

	}

  void Update(){
    if(Input.GetKeyDown(KeyCode.K))
      saveTime();
  }
    //@TODO cleanup OnData -- change ifs to switches and seperate out longer subject switches to seperate functions

	//Called when we receive data
	void OnData(ConnectionService con, ref NetworkMessage data)
	{
		if(data.tag == Network.Tag.Manager){

			if(data.subject == Network.Subject.HasJoined){
				//if a new player has joined

				//save the id of sender
				senders[playerIndex] = con.id;
                connections[playerIndex] = con;
				//set server visuals
				players[playerIndex].gameObject.SetActive(true);

                playerIndex++;

                Debug.Log("Player joined told to load level " + levelHandler.levelManagerIndex);

				//send back the spawnpos to the client
				con.SendReply(
                        Network.Tag.Manager,
                        Network.Subject.ServerSentNetID,
                        con.id);
			}else if(data.subject == Network.Subject.RequestServerLevel){
                Debug.Log("Player requested the servers level index");
                con.SendReply(Network.Tag.Manager,
                        Network.Subject.NewLevelManager,
                        levelHandler.levelManagerIndex);
            }
		}else if(data.tag == Network.Tag.Player){

			if( data.subject == Network.Subject.PlayerPositionUpdate ){

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
                    data.DecodeData();

					players[index].position = Deserializer.Vector3((byte[])data.data);
				}else{
					Debug.LogError("Sender ID not found");
				}
			}else if(data.subject == Network.Subject.PlayerRotationUpdate){
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
                    data.DecodeData();


					players[index].rotation = Deserializer.Quaternion((byte[])data.data);
				}else{
					Debug.LogError("Sender ID not found");
				}
			}
        }else if(data.tag == Network.Tag.Trigger){
            //relay to triggerHandler
            Debug.Log("Trigger Event from " + con.id + " subject" + data.subject );

            if(data.subject == Network.Subject.RequestTriggerIDs){
                TriggerState[] triggerStates = new TriggerState[triggerHandler.triggers.Count];
                //populate

                for(int i = 0;i<triggerHandler.triggers.Count;i++){
                    triggerStates[i] = new TriggerState(triggerHandler.triggers[i]);
                }

                con.SendReply(
                        Network.Tag.Trigger,
                        Network.Subject.ServerSentTriggerIDs,
                        triggerStates);

            }else if(data.subject == Network.Subject.TriggerActivate){
                data.DecodeData();
                Debug.Log("trigger " + (ushort)data.data + " activated");
                triggerHandler.TriggerInteracted((ushort)data.data,con.id,true);
                Debug.Log("state(" + triggerHandler.GetTriggerState((ushort)data.data) + ")");

                //force update GameMnager
                gameManager.DetectTriggerChanges();

                TriggerState state = triggerHandler.GetTriggerState((ushort)data.data);

                Debug.Log("sending: " + state);
                //send to clients but not the sender
                SendToAll(data.tag,Network.Subject.TriggerState,state);
            }else if(data.subject == Network.Subject.TriggerDeactivate){
                data.DecodeData();
                Debug.Log("trigger " + (ushort)data.data + " activated");

                triggerHandler.TriggerInteracted((ushort)data.data,con.id,false);
                Debug.Log("state(" + triggerHandler.GetTriggerState((ushort)data.data) + ")");

                //force update GameMnager
                gameManager.DetectTriggerChanges();

                TriggerState state = triggerHandler.GetTriggerState((ushort)data.data);

                Debug.Log("sending: " + state);

                //send to clients but not the sender
                SendToAll(data.tag,Network.Subject.TriggerState,state);
            }
        }
	}

    public void ResetSequence(){
        //call clients and reset triggers
        SendToAll(Network.Tag.Trigger,Network.Subject.SequenceFailed,0);

    }

    IEnumerator updateState(){
      while(updatingStates){
        TriggerState[] triggerStates = new TriggerState[triggerHandler.triggers.Count];
        //populate

        for(int i = 0;i<triggerHandler.triggers.Count;i++){
            triggerStates[i] = new TriggerState(triggerHandler.triggers[i]);
        }

        SendToAll(
                Network.Tag.Trigger,
                Network.Subject.ServerSentTriggerStates,
                triggerStates);

        yield return new WaitForSeconds(stateUpdateInterval);
      }
    }

    public void TriggerChanged(Trigger trigger){

        TriggerState state = new TriggerState(trigger);

        Debug.Log("sending: " + state);

        //send to clients but not the sender
        SendToAll(Network.Tag.Trigger,Network.Subject.TriggerState,state);
    }

    public override void OnLevelCompleted(){
        //When a Level is Completed get the new Level manager, process triggers and give GM the LM
        Debug.Log("LevelCompleted - levelManagerIndex: " + levelHandler.levelManagerIndex);

        levelTimings[levelHandler.levelManagerIndex - 1] = Time.time - levelTimings[levelHandler.levelManagerIndex - 1];

        gameManager.setNewLevelManager(null);

        if(levelHandler.levelManagerIndex >= levelHandler.levelContainers.Length){
            Debug.Log("Last level completed");
            saveTime();
            return;
        }else if(levelHandler.levelManagerIndex < levelHandler.levelContainers.Length)
          levelTimings[levelHandler.levelManagerIndex] = Time.time;

        LevelContainer lc = levelHandler.levelContainers[levelHandler.levelManagerIndex];
        triggerHandler.process(lc);
        gameManager.setNewLevelManager(lc.levelManager);

        //tell client to set the next level manager
        SendToAll(Network.Tag.Manager,Network.Subject.NewLevelManager,levelHandler.levelManagerIndex);

        Write("Level Completed");

        //write time


    }

    public override void OnLevelLoaded(int levelIndex){
        Debug.Log("Level " + levelIndex + " (name: " + levelHandler.levelOrder[levelIndex] + ") Loaded");

        //set gamemanager if current level
        if(levelHandler.levelManagerIndex == levelIndex && gameManager.LM == null)
            gameManager.setNewLevelManager(levelHandler.levelContainers[levelHandler.levelManagerIndex].levelManager);

        //load next level
        if(levelIndex < levelHandler.levelOrder.Length)
            levelHandler.loadLevel(levelIndex + 1);
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

        //System.Diagnostics.Process.GetCurrentProcess().Kill();
        //System.Environment.Exit(0);

        //Close all connections
    		foreach (var con in connections) {
                if(con != null)
                    con.Close();
    		}

    		// Close server
    		//DarkRiftServer.Close(false);
  	}

    public void saveTime(){
        float overallTime = 0;
        string times = "";
        for(int i = 0;i<levelTimings.Length;i++){
          overallTime += levelTimings[i];
          times += "Timer puzzle#" + i + ": " + levelTimings[i] + "\r\n";
        }
        times += "Timer overall: " + overallTime;
        //Creating a file
        System.IO.StreamWriter file = new System.IO.StreamWriter("Recording/" +currentDate);
        //Write the data(times) in a file.
        file.WriteLine(times);
        file.Close();
    }



}
