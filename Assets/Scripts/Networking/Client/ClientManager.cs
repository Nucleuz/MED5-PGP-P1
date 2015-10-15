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

public class ClientManager : NetworkManager
{

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

    public LevelHandler levelHandler;


    void Start()
    {
        levelHandler = GetComponent<LevelHandler>();
        //Connect to the server
        DarkRiftAPI.workInBackground = true;
        DarkRiftAPI.Connect(IP); //halts until connect or timeout
        DarkRiftAPI.onDataDetailed += ReceiveData;

        if (DarkRiftAPI.isConnected)
        {
            //tell everyone else that we have entered so they can tell where they are
            DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager, Network.Subject.HasJoined, "");
        }


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !DarkRiftAPI.isConnected)
        {

            DarkRiftAPI.Connect(IP); //halts until connect or timeout
            DarkRiftAPI.onDataDetailed += ReceiveData;

            if (DarkRiftAPI.isConnected)
            {
                //tell everyone else that we have entered so they can tell where they are
                DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager, Network.Subject.HasJoined, "");
            }

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

                case Network.Subject.ServerLoadedLevel:
                    {
                        //When the server has loaded a level
                        Write("Server Load Level " + (int)data);
                        levelHandler.loadLevel((int)data);
                    }
                    break;
                case Network.Subject.SpawnPlayer:
                    {
                        //spawn other player
                        Write("SpawnPlayer sender: " + senderID);

                        //unpack data

                        //spawn the object
                        GameObject g = Instantiate(prefabPlayer, ((SVector3)data).get(), Quaternion.identity) as GameObject;

                        //set the network id so it will sync with the player
                        NetPlayerSync netPlayer = g.GetComponent<NetPlayerSync>();

                        netPlayer.networkID = senderID;
                        netPlayer.SetAsReceiver();

                    }
                    break;
                case Network.Subject.HasJoined:
                    {
                        //send player info back when someone has joined
                        Write("HasJoined sender: " + senderID);

                        //send player data to the one who asked
                        DarkRiftAPI.SendMessageToID(senderID, Network.Tag.Manager, Network.Subject.SpawnPlayer, new SVector3(player.position));
                    }
                    break;
            }
        }
    }

    //for debugging 
    //@TODO - replace with console
    void Write(string mess)
    {
        if (debugText != null)
            debugText.text += mess + "\n";
    }




    public override void OnLevelLoaded(int levelIndex)
    {

        Debug.Log("Level " + levelIndex + " (" + levelHandler.levelOrder[levelIndex] + ") Loaded");

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

            //place the player on the correct rail!

            Rail startRail = levelHandler.getLevelManager().levelStartRail[networkID - 1];

            player.position = startRail.transform.position;
            player.GetComponent<Cart>().CurrentRail = startRail;


            //send it to everyone else
            DarkRiftAPI.SendMessageToOthers(Network.Tag.Manager, Network.Subject.SpawnPlayer, new SVector3(player.position));


        }



    }
}
