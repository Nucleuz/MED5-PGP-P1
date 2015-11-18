using UnityEngine;
using System.Collections;

public class AnimationTrigger : MonoBehaviour {
    Animator anim;
    Trigger trigger;

    public OcclusionPortal portal;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        trigger = GetComponent<Trigger>();
	}
	
	// Update is called once per frame
	void Update () {
        if (trigger.isTriggered) {
            TriggerAnimation();
            portal.open = true;     //FIXME Opens portal to Legion
        }
	}

    public void TriggerAnimation() {
        anim.SetBool("isTriggered", true);
    }
}
