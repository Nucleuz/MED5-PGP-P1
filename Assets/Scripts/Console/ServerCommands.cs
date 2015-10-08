using UnityEngine;
using System.Collections;

public class ServerCommands : MonoBehaviour {
    //This should contain all the server methods.
    
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetLevel(){
		Debug.Log("Reset level!");
        Application.LoadLevel(Application.loadedLevel);
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
    

    

}
