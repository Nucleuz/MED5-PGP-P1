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
using VoiceChat;

public class ClientManager : NetworkManager
{

    //IP the client will try to connect to 
    //@TODO - client should be able to change the ip when the client starts up, for easier use
    public string IP = "127.0.0.1";

    public ushort networkID = 0;

    //debug text
    //@TODO - create a kind of console instead if this..
    public Text debug;

    //prefab for the players
    public GameObject prefabPlayer;
    public Transform player;

    private NetPlayerSync[] otherPlayers = new NetPlayerSync[2];

    public LevelHandler levelHandler;
    public TriggerHandler triggerHandler;

    private int serverLevelIndex = -1;

    void Start()
    {
        debugText = debug;
        levelHandler = GetComponent<LevelHandler>();
        triggerHandler = GetComponent<TriggerHandler>();
        //Connect to the server
        DarkRiftAPI.workInBackground = true;
        DarkRiftAPI.Connect(IP); //halts until connect or timeout
        DarkRiftAPI.onDataDetailed += ReceiveData;

        if (DarkRiftAPI.isConnected)
        {
            Console.Instance.AddMessage("Is connected to Server");
            //tell everyone else that we have entered so they can tell where they are
            DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager, Network.Subject.HasJoined,(ushort) 123);
        }
    }

    void OnApplicationQuit()
    {
        DarkRiftAPI.Disconnect();
    }


    void ReceiveData(ushort senderID, byte tag, ushort subject, object data)
    {

        //only handle data if it is for the manager
        if (tag == Network.Tag.Manager)
        {
            switch (subject)
            {
                case Network.Subject.ServerSentNetID:
                    {
                        networkID = (ushort)data;
                        Console.Instance.AddMessage("Got network id " + networkID);
                        DarkRiftAPI.SendMessageToServer(Network.Tag.Manager, Network.Subject.RequestServerLevel,true);

                    }
                    break;

                case Network.Subject.NewLevelManager:
                    {
                        //When the server has loaded a level

                        serverLevelIndex = (int)data;
                        Console.Instance.AddMessage("Server is at level " + serverLevelIndex);

                        //load the previous, current and next level if available(serverLevelIndex >/< x) and not already loaded(levelcontainer == null)
                        //previous level
                        if(serverLevelIndex > 0 && levelHandler.levelContainers[serverLevelIndex - 1] == null)
                            levelHandler.loadLevel(serverLevelIndex - 1);
                        //current level
                        if(levelHandler.levelContainers[serverLevelIndex] == null)
                            levelHandler.loadLevel(serverLevelIndex);
                        //next level
                        if(serverLevelIndex < levelHandler.levelOrder.Length - 1 && levelHandler.levelContainers[serverLevelIndex + 1] == null)
                            levelHandler.loadLevel(serverLevelIndex + 1);


                        //if the level is already loaded process it's triggers
                        if(levelHandler.levelContainers[serverLevelIndex] != null)
                            triggerHandler.process(levelHandler.levelContainers[serverLevelIndex]);

                    }
                    break;
                case Network.Subject.SpawnPlayer: // Spawn OTHER players
                    {
                        //spawn other player
                        Write("SpawnPlayer sender: " + senderID);

                        //unpack data

                        //spawn the object
                        GameObject g = Instantiate(prefabPlayer,Deserializer.Vector3((byte[])data) , Quaternion.identity) as GameObject;

                        //set the network id so it will sync with the player
                        NetPlayerSync netPlayer = g.GetComponent<NetPlayerSync>();

                        // VoiceChat Components
                        g.AddComponent<AudioSource>();
                        netPlayer.SetVoiceChatPlayer(g.AddComponent<VoiceChatPlayer>());

                        netPlayer.networkID = senderID;
                        netPlayer.SetAsReceiver();

                        if(otherPlayers[0] == null)
                            otherPlayers[0] = netPlayer;
                        else
                            otherPlayers[1] = netPlayer;

                        if(player != null)
                            netPlayer.AddCameraToLightShaft(player.GetComponent<NetPlayerSync>().cam);

                    }
                    break;
                case Network.Subject.HasJoined:
                    {
                        //send player info back when someone has joined
                        Write("HasJoined sender: " + senderID);

                        //send player data to the one who asked
                        if(player != null){
                            
                            DarkRiftAPI.SendMessageToID(senderID, Network.Tag.Manager, Network.Subject.SpawnPlayer,player.position.Serialize());
                        }
                    }
                    break;
           }
        }
    }

    public override void OnLevelCompleted(){

    }
    

    public override void OnLevelLoaded(int levelIndex)
    {

        Console.Instance.AddMessage("Level " + levelIndex + " (" + levelHandler.levelOrder[levelIndex] + ") Loaded");

        //try to spawn the player
        SpawnPlayer();

        //if level that is loaded is the level that the server is on currently process it's triggers

        if(serverLevelIndex == levelIndex)
            triggerHandler.process(levelHandler.levelContainers[levelIndex]);

    }

    private void SpawnPlayer(){
        if(player != null || networkID == 0 || serverLevelIndex == -1 || levelHandler.getLevelManager() == null){
            if(player != null)
                Console.Instance.AddMessage("Player is already spawned");
            else
                Console.Instance.AddMessage("failed .. Trying to spawn player: " + player + " netID: " + networkID + " serverLevel: " + serverLevelIndex + " levelHandler: " + levelHandler.getLevelManager());
            return;
        }
        Console.Instance.AddMessage("Spawning Player");
        //spawn the object
        GameObject g = Instantiate(prefabPlayer, Vector3.zero, Quaternion.identity) as GameObject;
        player = g.transform;
        //set the network id so it will sync with the player
        NetPlayerSync netPlayer = g.GetComponent<NetPlayerSync>();

        netPlayer.networkID = networkID;
        netPlayer.SetAsSender();

        for(int i = 0;i<otherPlayers.Length;i++)
            if(otherPlayers[i] != null)
               otherPlayers[i].AddCameraToLightShaft(netPlayer.cam);


/*
        VoiceChatRecorder.Instance.NetworkId = networkID;
        VoiceChatRecorder.Instance.Device = VoiceChatRecorder.Instance.AvailableDevices[0];
        VoiceChatRecorder.Instance.StartRecording();
//         VoiceChatRecorder.Instance.AutoDetectSpeech = true;
        VoiceChatRecorder.Instance.NewSample += netPlayer.OnNewSample;
        */

        //place the player on the correct rail!

        Console.Instance.AddMessage("levelManager: " + levelHandler.getLevelManager());
        Rail startRail = levelHandler.getLevelManager().levelStartRail[networkID - 1];
        Console.Instance.AddMessage("startrail: " + startRail.transform.position);


        player.position = startRail.transform.position;
        player.GetComponent<Cart>().CurrentRail = startRail;

        //send it to everyone else
        DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager, Network.Subject.SpawnPlayer, player.position.Serialize());
    }

}
