using UnityEngine;
using System.Collections;

public class SoundGemScript : Interactable
{
    // Initialize an Audio Source in order to  play sounds
    AudioSource audioSource;

    // Array for the audio clips we choose to use.
    public AudioClip[] audioClips;

    // Time between sounds that can be changed in the inspector.
    public float timeBetweenSounds;

	bool audioPlaying = false;

    private Trigger trigger;

    // Use this for initialization
    void Start()
    {
        // Set audio to the AudioSource component
        audioSource = GetComponent<AudioSource>();
        trigger = GetComponent<Trigger>();

        // Default value for time between the sounds
        timeBetweenSounds = 2.0f;
    }


	IEnumerator PlaySounds(){
		audioPlaying = true;
        trigger.isTriggered = true;

		for (int i = 0; i < audioClips.Length; i++) {
			audioSource.clip = audioClips[i];
			audioSource.Play ();
			yield return new WaitForSeconds(audioClips[i].length + timeBetweenSounds);
		}
        trigger.isTriggered = false;
        trigger.canReset = true;
		audioPlaying = false;
	}

    public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		if (!audioPlaying && trigger.isReadyToBeTriggered){
			StartCoroutine(PlaySounds());
		}
	}
}
