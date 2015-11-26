using UnityEngine;
using System.Collections;

public class AMB_CrystalSound : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		SoundManager.Instance.PlayEvent("Ambience_Mojo", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
