﻿using UnityEngine;
using System.Collections;

public class AmbientSound : MonoBehaviour {
	SoundManager sM;

	// Use this for initialization
	void Start () {
		sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		sM.PlayEvent("Ambience_Main", gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
