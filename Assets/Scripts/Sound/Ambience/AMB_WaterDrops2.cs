using UnityEngine;
using System.Collections;

public class AMB_WaterDrops2 : MonoBehaviour {
	// Use this for initialization
	void OnEnable () {
	    SoundManager.Instance.PlayEvent("Ambience_WaterDrops2",gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
