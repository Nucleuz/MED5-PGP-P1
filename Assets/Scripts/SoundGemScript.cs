using UnityEngine;
using System.Collections;

public class SoundGemScript : MonoBehaviour
{
    // Initialize an audio object
    AudioSource audio;

    // Use this for initialization
    void Start()
    {
        // Set audio to the AudioSource component
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // This just detects the left mouse click atm, but we will eventually need something else to activate it.
        if (Input.GetMouseButtonDown(0))
        {
            // Sets the isTrigger variable in the trigger script to true.
            GetComponent<Trigger>().isTriggered = true;
        }

        // Check if SoundGem is triggered
        if (GetComponent<Trigger>().isTriggered)
        {
            // Play audio if SoundGem is triggered
            audio.Play();
            
            // Set isTriggered to false so that we can keep activating it again.
            GetComponent<Trigger>().isTriggered = false;
        }
    }
}
