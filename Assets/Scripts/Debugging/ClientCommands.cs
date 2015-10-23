using UnityEngine;
using System.Collections;

public static class ClientCommands {

	//This should contain all the client methods.
	public static string messageClient;

    // Getter to the console script to call for a return message.
	public static string getMessageClient() {
        return messageClient;
    }

    // Teleports the player to their cart
	public static string TeleportToCart() {
        GameObject.Find("Player").transform.position = GameObject.Find("Cart").transform.position;

        if(GameObject.Find("Player").transform.position == GameObject.Find("Cart").transform.position)
            messageClient = "Teleported player to cart succeeded";
        else
            messageClient = "Teleported player to cart failed!";

        return messageClient;
    }

	public static void ToggleFPS(FPSUpdater fps) {
		fps.GetComponent<FPSUpdater>().Toggle();
    }

	public static void ResetPosition() {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++) {
            player[i].GetComponent<Cart>().ResetPosition();
        }
    }

    // Returns the position of the player
	public static string GetPosition(){
		messageClient = "[x: " + GameObject.Find("Player").transform.position.x + " y: " + GameObject.Find("Player").transform.position.y + " z: " + GameObject.Find("Player").transform.position.z + "]";
		return messageClient;
	}
	//Receive the method which turns on the no clip function from the no clip camera.
	public static void ToggleNoClip(){
    	NoClip.Instance.GetComponent<NoClip>().On();
	}

    public void ResetLevel(){
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ResetGame(){
        Application.LoadLevel(0);
    }

    public void SkipLevel(){
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void JumpToLevel(int levelNumber){
        Application.LoadLevel(levelNumber);
    }
}
