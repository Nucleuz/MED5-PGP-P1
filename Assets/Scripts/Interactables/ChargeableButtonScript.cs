using UnityEngine;
using System.Collections;

public class ChargeableButtonScript : Interactable
{
    bool soundStopped;

    // Float to hold the total charged energy
    public float energy = 0;

    // Float as a decrease rate for the button
    public float decreaseRate = 0.25f;

    // Float as an increase rate for the button
    public float increaseRate = 1.75f;

    // Used to check if we are chargin the button
    public bool isCharging;

    // Used to check if we can charge the button
    public bool readyForCharge;

	public float endInteractTime;
	public float minInteractLength;

    private Trigger trigger;

    ParticleSystem particles;

    void Start(){
        soundStopped = true;
        trigger = GetComponent<Trigger>();

        particles = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update(){

        if (readyForCharge) { 
            // Checks if left mouse button is pressed. (This needs to be removed when we finally use it)
            if (endInteractTime > Time.time){
                // Sets isCharging to true if mouse button is pressed.
                isCharging = true;
            }else{
                // Sets isCharging to false if mouse button is not pressed.
                isCharging = false;
            }
        }
        // Checks if isCharging is true
        if (isCharging && trigger.isReadyToBeTriggered){
            // Checks if we have less than 100 energy.
            if (energy < 100){
                // Increase energy while the Chargeable Button is being triggered.
                energy += increaseRate;

                // Checks if energy is bigger than 100
                if (energy >= 100){

                    // Set isTrigger in Trigger script to true

                    trigger.Activate();
                    SoundManager.Instance.ToggleSwitch("On_Off", "On", gameObject);
                    SoundManager.Instance.PlayEvent("ChannelingButtonOnOff", gameObject);
                    soundStopped = false;

                    //Plays the particles once.
                    particles.Play();

                    // Make sure that we cannot charge it again right away
                    readyForCharge = false;
                    energy = 100;
                    endInteractTime = 0;
                }

                // Makes sure that we have to keep the button triggered in order to charge it.
                isCharging = false;
            }
        }
        if (energy > 0 && !isCharging){
                // Decrease energy
                energy -= decreaseRate;
                Debug.Log(energy);
            
        }

        // Checks if energy is less than or equal than 0.
        if(energy <= 0){
            // Makes sure that we can charge the button.
            readyForCharge = true;
            energy = 0;

            // Set isTrigger in Trigger script to false
            trigger.Deactivate();
            if(!soundStopped){
                SoundManager.Instance.StopEvent("ChannelingButtonOnOff", gameObject);
                SoundManager.Instance.ToggleSwitch("On_Off", "Off", gameObject);
                SoundManager.Instance.PlayEvent("ChannelingButtonOnOff", gameObject);
                soundStopped = true;
            }
            trigger.canReset = true;
        }
    }
    
	public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
        if(trigger.isReadyToBeTriggered)
		  endInteractTime = Time.time + minInteractLength;
	}

    public override void OnRayExit(int playerIndex){
        
    }
}
