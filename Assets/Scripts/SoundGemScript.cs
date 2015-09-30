using UnityEngine;
using System.Collections;

public class SoundGemScript : MonoBehaviour
{
    // Initialize an Audio Source in order to  play sounds
    AudioSource audio;

    // Array for the audio clips we choose to use.
    public AudioClip[] audioClips;

    // Timer for counting
    float timer;

    // Time between sounds that can be changed in the inspector.
    public float timeBetweenSounds;

    // Used to check if the timer is triggered.
    bool timerIsTriggered;

    // Used to loop through the sounds in the array
    int currentSound;

    // Use this for initialization
    void Start()
    {
        // Set audio to the AudioSource component
        audio = GetComponent<AudioSource>();

        // Make sure the timer is a 0 when we start.
        timer = 0f;

        // Default value for time between the sounds
        timeBetweenSounds = 2.0f;

        // Make sure we dont trigger the timer until we activate the sounds
        timerIsTriggered = false;

        // Start with 0, since that is the first sound in the array
        currentSound = 0;
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
            // Trigger the timer
            timerIsTriggered = true;

            // Reset sound to 0
            currentSound = 0;

            // Set isTriggered to false so that we can keep activating it again.
            GetComponent<Trigger>().isTriggered = false;
        }

        // Checks if the timer is triggered
        if (timerIsTriggered)
        {
            // Adds Time.deltatime to timer in order to create counting timer.
            timer += Time.deltaTime;
            
            // Checks if the timer has reached its maximum (which is the actual time between each sound)
            if(timer >= timeBetweenSounds)
            {
                // Check if current sound is in the array index
                if(currentSound <= audioClips.Length-1)
                {
                    // Set the audio clip to current sound
                    audio.clip = audioClips[currentSound];

                    // Play the current audio clip
                    audio.Play();

                    // Increment currentSound to get to the next sound in the array.
                    currentSound++;
                }
                else
                {
                    // Deactivate the timer if we've looped through the whole array.
                    timerIsTriggered = false;
                }
                
                // Reset timer
                timer = 0;
            }
        }
    }
}
