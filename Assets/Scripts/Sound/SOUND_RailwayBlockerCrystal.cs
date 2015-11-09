using UnityEngine;
using System.Collections;

public class SOUND_RailwayBlockerCrystal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlayEvent("RailwayBlockerCrystal", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
