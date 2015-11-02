using UnityEngine;
using System.Collections;

public class AmbientSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlayEvent("Ambience_Main", gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
