using UnityEngine;
using System.Collections;

public class AnimationTrigger : MonoBehaviour {
    Animator anim;
    Trigger trigger;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        trigger = GetComponent<Trigger>();
	}
	
	// Update is called once per frame
	void Update () {
        if (trigger.isTriggered) {
            TriggerAnimation();
        }
	}

    public void TriggerAnimation() {
        anim.SetBool("isTriggered", true);
    }
}
