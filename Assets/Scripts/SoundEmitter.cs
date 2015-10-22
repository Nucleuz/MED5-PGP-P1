using UnityEngine;
using System.Collections;

public class SoundEmitter : MonoBehaviour {

	private SoundManager sM;
	private InteractableButton iButton;
	private bool hasPlayed;
	
	public int melodyIndex;
	// Use this for initialization
	void Start () {
		iButton = GetComponent<InteractableButton>();
		sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		hasPlayed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(iButton.playedSound && iButton.trigger.isTriggered){
			if(!hasPlayed){
				StartCoroutine("playMelody");
				hasPlayed = true;
			}
		}

		if(!iButton.trigger.isTriggered)
			hasPlayed = false;
	}

	IEnumerator playMelody(){
		yield return new WaitForSeconds(1);

		switch(melodyIndex){
			case 0:
				sM.PlayEvent("SP_PlayerButton1", gameObject);
			break;

			case 1:
				sM.PlayEvent("SP_PlayerButton2", gameObject);
			break;

			case 2:
				sM.PlayEvent("SP_PlayerButton3", gameObject);
			break;

			case 3:
				sM.PlayEvent("SP_PlayerButton4", gameObject);
			break;

			case 4:
				sM.PlayEvent("SP_PlayerButton5", gameObject);
			break;

			case 5:
				sM.PlayEvent("SP_PlayerButton6", gameObject);
			break;

			default:
				Debug.Log("Melody index out of bounds");
			break;
		}
	}
}
