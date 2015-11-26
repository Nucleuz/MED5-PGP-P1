using UnityEngine;
using System.Collections;

public class AMB_WaterDrops1 : MonoBehaviour {
	// Use this for initialization
	void OnEnable () {
	    SoundManager.Instance.PlayEvent("Ambience_WaterDrops1",gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
