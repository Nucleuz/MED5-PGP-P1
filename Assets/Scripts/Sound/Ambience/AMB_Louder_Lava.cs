﻿using UnityEngine;
using System.Collections;

public class AMB_Louder_Lava : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlayEvent("Louder_Lava_Active", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}