using UnityEngine;
using System.Collections;

public static class ServerCommands {
    //This should contain all the server methods.
	
	public static void ResetLevel(){
		Debug.Log("Reset level!");
        Application.LoadLevel(Application.loadedLevel);
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

	public static void ToggleNoClip(){
    	GameObject.Find("NoClipCam").GetComponent<NoClip>().On();
	}
}
