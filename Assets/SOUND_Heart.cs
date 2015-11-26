using UnityEngine;
using System.Collections;

public class SOUND_Heart : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		SoundManager.Instance.PlayEvent("Heartbeat", gameObject);	
		SoundManager.Instance.PlayEvent("HeartMojo", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
