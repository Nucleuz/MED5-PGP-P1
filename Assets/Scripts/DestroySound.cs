using UnityEngine;
using System.Collections;

public class DestroySound : MonoBehaviour {
	SoundManager sM;
	GameObject sMObject;

	// Use this for initialization
	void Start () {
		//sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		//sMObject = GameObject.Find("SoundManager");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.K)){
			Destroy (gameObject);
			//sM.StopAll(sMObject);

		}
	}
}
