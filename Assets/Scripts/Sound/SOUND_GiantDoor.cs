using UnityEngine;
using System.Collections;

public class SOUND_GiantDoor : MonoBehaviour {
	private Animator anim;

	private bool isStarted;
	private bool hasPlayed;
	private float timeStamp;

	// Use this for initialization
	void Start () {
		hasPlayed = false;
		anim = GetComponent<Animator>();
		isStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Open") && !isStarted && !hasPlayed)
        {
        	hasPlayed = true;
        	isStarted = true;
        	SoundManager.Instance.PlayEvent("DoorSlideStart", gameObject);
        	timeStamp = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
        	Debug.Log("timeStamp: " +timeStamp + " time: " + Time.time);
        }

        if(timeStamp < Time.time && isStarted){
        	SoundManager.Instance.PlayEvent("DoorSlideStop", gameObject);
        	isStarted = false;
        }
	
	}
}
