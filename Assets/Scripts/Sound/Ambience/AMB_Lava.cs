using UnityEngine;
using System.Collections;

public class AMB_Lava : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		SoundManager.Instance.PlayEvent("Lava_Active", gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
