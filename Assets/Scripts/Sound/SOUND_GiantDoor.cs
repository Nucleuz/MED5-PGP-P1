using UnityEngine;
using System.Collections;

public class SOUND_GiantDoor : MonoBehaviour {
	private Animator anim;

	private bool isStarted;
	private float timeStamp;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		isStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Open") && !isStarted)
        {
        	isStarted = true;
        	SoundManager.Instance.PlayEvent("DoorSlide1", gameObject);
        	timeStamp = Time.time + anim.GetCurrentAnimatorStateInfo(0).length;
        }

        if(timeStamp < Time.time && !isStarted)
        	isStarted = false;
	
	}
}
