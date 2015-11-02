using UnityEngine;
using System.Collections;

public class SOUND_LegHit : MonoBehaviour {

	public GameObject soundPos;

	// Use this for initialization
	void Awake () {
		PlayLegHitSound();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayLegHitSound(){
		Debug.Log("HEY");
		////SoundManager.Instance.PlayEvent("LegSoundHit1", soundPos);
	}
}
