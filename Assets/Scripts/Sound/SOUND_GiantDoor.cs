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
		if (!isStarted && !hasPlayed && anim.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
        	hasPlayed = true;
        	isStarted = true;
        	SoundManager.Instance.PlayEvent("DoorSlideStart", gameObject);
        	timeStamp = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
        }

        if(isStarted && timeStamp < Time.time){
        	SoundManager.Instance.StopEvent("DoorSlideStart", gameObject);
        	SoundManager.Instance.PlayEvent("DoorSlideStop", gameObject);
        	isStarted = false;
        }
	
	}
}
