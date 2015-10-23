using UnityEngine;
using System.Collections;

public class AMB_WaterDrops1 : MonoBehaviour {
	SoundManager sM;
	// Use this for initialization
	void Start () {
        sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	    sM.PlayEvent("Ambience_WaterDrops1",gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
