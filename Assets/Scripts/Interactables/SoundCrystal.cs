using UnityEngine;
using System.Collections;

public class SoundCrystal : Interactable
{

	SoundManager sM;

	// Name of played event (wWise)
	string eventName;

    // Time between sounds that can be changed in the inspector.
    public float timeBetweenSounds;

	bool audioPlaying = false;

    // Use this for initialization
    void Start()
    {
    	// Finding soundmanager and setting reference
    	sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        // Default value for time between the sounds
        timeBetweenSounds = 2.0f;
    }


	public void PlaySounds(){
		sM.playEvent(eventName, gameObject);
	}

    public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		PlaySounds();
	}
}
