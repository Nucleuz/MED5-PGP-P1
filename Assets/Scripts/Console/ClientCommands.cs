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

    public string getMessageClient() {
        return messageClient;
    }

    public string TeleportToCart() {
        GameObject.Find("Player").transform.position = GameObject.Find("Cart").transform.position;

        if(GameObject.Find("Player").transform.position != GameObject.Find("Cart").transform.position)
            messageClient = "Teleport " + GameObject.Find("Player") + " to " + GameObject.Find("Cart") + " failed!";
        else
            messageClient = "Teleport " + GameObject.Find("Player") + " to " + GameObject.Find("Cart") + " succeeded!";

        return messageClient;
    }

	public string GetPosition(){
		messageClient = "xPos: " + GameObject.Find("Player").transform.position.x + " yPos: " + GameObject.Find("Player").transform.position.y + " zPos: " + GameObject.Find("Player").transform.position.z;
		return messageClient;
	}
}
