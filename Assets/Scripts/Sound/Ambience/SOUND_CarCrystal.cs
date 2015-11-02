using UnityEngine;
using System.Collections;

public class SOUND_CarCrystal : MonoBehaviour {

	public Cart car;
	public bool isPlaying;

	// Use this for initialization
	void Start () {
		isPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(car.isMoving){
			SoundManager.Instance.SetRTPC("CarSpeed", Input.GetAxis("Vertical")*10, gameObject);
			if(!isPlaying){
				SoundManager.Instance.ToggleSwitch("CarOn_Off", "On", gameObject);
				SoundManager.Instance.PlayEvent("CarCrystal", gameObject);
				isPlaying = true;
			}
		}
		if(!car.isMoving && isPlaying){
			SoundManager.Instance.StopEvent("CarCrystal", gameObject);
			SoundManager.Instance.ToggleSwitch("CarOn_Off", "Off", gameObject);
			SoundManager.Instance.PlayEvent("CarCrystal", gameObject);
			isPlaying = false;
		}

	}
}
