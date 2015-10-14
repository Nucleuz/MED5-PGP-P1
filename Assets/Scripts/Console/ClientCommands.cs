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

    // Getter to the console script to call for a return message.
    public string getMessageClient() {
        return messageClient;
    }

    // Teleports the player to their cart
    public string TeleportToCart() {
        GameObject.Find("Player").transform.position = GameObject.Find("Cart").transform.position;

        if(GameObject.Find("Player").transform.position == GameObject.Find("Cart").transform.position)
            messageClient = "Teleported player to cart succeeded";
        else
            messageClient = "Teleported player to cart failed!";

        return messageClient;
    }

    public void ToggleFPS() {
        GameObject.Find("FPS Counter").GetComponent<FPSUpdater>().Toggle();
    }

    public void ResetPosition() {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++) {
            player[i].GetComponent<Cart>().ResetPosition();
        }
    }

    // Returns the position of the player
	public string GetPosition(){
		messageClient = "xPos: " + GameObject.Find("Player").transform.position.x + " yPos: " + GameObject.Find("Player").transform.position.y + " zPos: " + GameObject.Find("Player").transform.position.z;
		return messageClient;
	}
	//Receive the method which turns on the no clip function from the no clip camera.
	public void ToggleNoClip(){
    	GameObject.Find("NoClipCam").GetComponent<NoClip>().On();
	}
}
