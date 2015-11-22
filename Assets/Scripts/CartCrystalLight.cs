using UnityEngine;
using System.Collections;

public class CartCrystalLight : MonoBehaviour {
	private Light cartLight;
	//receive the player ID to match color of player.
	public int playerIndex;                         // index for the player.
	

	// Use this for initialization
	void Start () {
		cartLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		//receive the speed of the cart
		float verticalAxis = Input.GetAxis("Vertical");
		if(verticalAxis<0){ //if negattive value in speed, inverse the value.
			verticalAxis*=-1;
		}

		cartLight.intensity = verticalAxis*3;
		switch (playerIndex){
            case 1:
                cartLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
            break;
            case 2:
                cartLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
            break;
            case 3:
                cartLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
            }   
	}
}
