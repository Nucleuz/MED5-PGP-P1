﻿using UnityEngine;
using System.Collections;

public class AMB_RunningWater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlayEvent("RiverRunning", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
