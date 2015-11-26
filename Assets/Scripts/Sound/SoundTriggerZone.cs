using UnityEngine;
using System.Collections;

public class SoundTriggerZone : MonoBehaviour {
	public GameObject soundObject_Deactivate;
	public GameObject soundObjects_Activate;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(soundObject_Deactivate != null){
			soundObject_Deactivate.SetActive(false);
			for(int i = 0; i < soundObject_Deactivate.transform.childCount; i++){
				SoundManager.Instance.StopAllEventsOnObject(soundObject_Deactivate.transform.GetChild(i).gameObject);
			}
		}

		if(soundObjects_Activate != null){
			soundObjects_Activate.SetActive(true);
		}
	}
}
