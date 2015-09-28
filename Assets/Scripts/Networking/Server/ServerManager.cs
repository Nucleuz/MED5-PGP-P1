using UnityEngine;
using System.Collections;
using DarkRift;

public class ServerManager : MonoBehaviour {

	public ushort nextPos = 0;

	public SVector3[] spawnPos = {
		new SVector3(-1,1,-1),
		new SVector3(-1,1,1),
		new SVector3(1,1,-1),
		new SVector3(1,1,1)
	};

	// Use this for initialization
	void Start () {
	
		ConnectionService.onData += OnData;
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}



	//Called when we receive data
	void OnData(ConnectionService con, ref NetworkMessage data)
	{
		//Decode the data so it is readable
		data.DecodeData ();

		if(data.tag == Network.Tag.Manager){
			if(data.subject == Network.Subject.HasJoined){
				if(nextPos >= spawnPos.Length)nextPos = 0;
				con.SendReply(Network.Tag.Manager,Network.Subject.ServerSentSpawnPos,spawnPos[nextPos++]);
			}
		}

	}
}
