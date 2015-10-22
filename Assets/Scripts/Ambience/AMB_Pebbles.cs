using UnityEngine;
using System.Collections;

public class AMB_Pebbles : MonoBehaviour {
	SoundManager sM;
	// Use this for initialization
	void Start () {
        sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sM.PlayEvent("Ambience_Pebbles",gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
