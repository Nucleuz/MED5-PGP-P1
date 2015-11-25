using UnityEngine;
using System.Collections;

public class SoundEmitter : MonoBehaviour {	
	public int melodyIndex;

	public void Play(){
		Debug.Log("playing sound " + melodyIndex);
		
		switch(melodyIndex){
			case 0:
				SoundManager.Instance.PlayEvent("SP_PlayerButton1", gameObject);
			break;

			case 1:
				SoundManager.Instance.PlayEvent("SP_PlayerButton2", gameObject);
			break;

			case 2:
				SoundManager.Instance.PlayEvent("SP_PlayerButton3", gameObject);
			break;

			case 3:
				SoundManager.Instance.PlayEvent("SP_PlayerButton4", gameObject);
			break;

			case 4:
				SoundManager.Instance.PlayEvent("SP_PlayerButton5", gameObject);
			break;

			case 5:
				SoundManager.Instance.PlayEvent("SP_PlayerButton6", gameObject);
			break;

			default:
				Debug.Log("Melody index out of bounds");
			break;
		}
		
	}
}
