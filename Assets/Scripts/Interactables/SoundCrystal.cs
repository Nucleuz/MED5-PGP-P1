using UnityEngine;
using System.Collections;

public class SoundCrystal : Interactable
{

	//SoundManager sM;

	// Name of played event (wWise)
	string eventName;

    int currentSequence;

    // Time between sounds that can be changed in the inspector.
    public float timeBetweenSounds;

	bool audioPlaying = false;

    // Use this for initialization
    void Start()
    {
        currentSequence = 0;

    	// Finding soundmanager and setting reference
//    	sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        // Default value for time between the sounds
        timeBetweenSounds = 2.0f;
    }


	public void PlaySounds(int i){
        switch(i){
            case 0:
//		      sM.ToggleSwitch("derp", "herp", gameObject);
//              sM.PlayEvent(eventName, gameObject);
            break;
            
            case 1:
//			sM.ToggleSwitch("derp", "herp", gameObject);
//              sM.PlayEvent(eventName, gameObject);
            break;

            case 2:
//			sM.ToggleSwitch("derp", "herp", gameObject);
//              sM.PlayEvent(eventName, gameObject);
            break;

            default:
                Debug.Log("Sound Crystal gem sequence out of bounds");
            break;
        }
	}

    public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		PlaySounds(0);
	}
}
