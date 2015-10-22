﻿using UnityEngine;
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

    public ushort networkID;

    //debug text
    //@TODO - create a kind of console instead if this..
    public Text debug;

    //prefab for the players
    public GameObject prefabPlayer;
    public Transform player;

    public LevelHandler levelHandler;
    public TriggerHandler triggerHandler;

    private int serverLevelIndex;

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
                    }
                    break;

                case Network.Subject.NewLevelManager:
                    {
                        //When the server has loaded a level

                        serverLevelIndex = (int)data;
                        Write("Server is at level " + serverLevelIndex);

                        //load the previous, current and next level if available(serverLevelIndex >/< x) and not already loaded(levelcontainer == null)

                        if(serverLevelIndex > 0 && levelHandler.levelContainers[serverLevelIndex - 1] == null)
                            levelHandler.loadLevel(serverLevelIndex - 1);
                        
                        if(levelHandler.levelContainers[serverLevelIndex] == null)
                            levelHandler.loadLevel(serverLevelIndex);

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

        Debug.Log("Level " + levelIndex + " (" + levelHandler.levelOrder[levelIndex] + ") Loaded");

        //check if player is not spawned, then spawn
        if (player == null)
        {
            //spawn the object
            GameObject g = Instantiate(prefabPlayer, Vector3.zero, Quaternion.identity) as GameObject;
            player = g.transform;
            //set the network id so it will sync with the player
            NetPlayerSync netPlayer = g.GetComponent<NetPlayerSync>();

            netPlayer.networkID = networkID;
            netPlayer.helmet.playerIndex = networkID;
            Debug.Log(netPlayer.helmet.playerIndex);
            Write("Player Index: " + netPlayer.helmet.playerIndex);
            netPlayer.SetAsSender();

			VoiceChatRecorder.Instance.NetworkId = networkID;
			VoiceChatRecorder.Instance.Device = VoiceChatRecorder.Instance.AvailableDevices[0];
			VoiceChatRecorder.Instance.StartRecording();
   //         VoiceChatRecorder.Instance.AutoDetectSpeech = true;
			VoiceChatRecorder.Instance.NewSample += netPlayer.OnNewSample;

            //place the player on the correct rail!

            Rail startRail = levelHandler.getLevelManager().levelStartRail[networkID - 1];

            player.position = startRail.transform.position;
            player.GetComponent<Cart>().CurrentRail = startRail;


             DarkRiftWriter writer = new DarkRiftWriter();
            

            //send it to everyone else
            DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager, Network.Subject.SpawnPlayer, player.position.Serialize());
            writer.Close();
        }


        //if level that is loaded is the level that the server is on currently process it's triggers

        if(serverLevelIndex == levelIndex)
            triggerHandler.process(levelHandler.levelContainers[levelIndex]);

    }
}
