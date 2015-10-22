using UnityEngine;
using System.Collections;

public class AMB_WaterDrops2 : MonoBehaviour {
	SoundManager sM;
	// Use this for initialization
	void Start () {
        sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	    sM.PlayEvent("Ambience_WaterDrops2",gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
