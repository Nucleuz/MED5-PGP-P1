using UnityEngine;
using System.Collections;

public class ClientCommands : MonoBehaviour {
	//This should contain all the client methods.
	public string messageClient;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public string GetPosition(){
		messageClient = "xPos: " + GameObject.Find("Player").transform.position.x + " yPos: " + GameObject.Find("Player").transform.position.y + " zPos: " + GameObject.Find("Player").transform.position.z;
		return messageClient;
	}
}
