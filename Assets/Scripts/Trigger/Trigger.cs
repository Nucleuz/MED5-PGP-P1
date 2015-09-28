using UnityEngine;
using System.Collections;

public class Testingtrigger : MonoBehaviour {

	public bool isTriggered = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider trigger){
		isTriggered = true;
		Debug.Log("Hello");
	}
}
