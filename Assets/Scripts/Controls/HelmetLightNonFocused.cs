using UnityEngine;
using System.Collections;

public class HelmetLightNonFocused : MonoBehaviour {
	public HelmetLightScript hls;
	private Light nonFocusedHelmetLight; 
	//receive the player index from the script.

	// Use this for initialization
	void Start () {
		nonFocusedHelmetLight = GetComponent<Light>();        //Calls the light component on the spotlight  
        //Set the color of the interactable button both background light and particles to the correct user.
        switch (hls.playerIndex){
            case 1:
                nonFocusedHelmetLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
            break;
            case 2:
                nonFocusedHelmetLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
            break;
            case 3:
                nonFocusedHelmetLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
            break;

            default:
                Debug.Log("Invalid playerIndex");
            break;
        }   
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
