﻿using UnityEngine;
using System.Collections;

public class LightFade : ReceiveSequenceFail {
	//This script is creating a light pulsating from the sound crystals.
	//When the sound crystal is emitting sound it will start increasing and decreasing the intensity of the light.
	//When the sound crystal is NOT emitting sound= the lights intensity is slowly turned off.
	//The fade in and out part of this code is retrieved from an online source.  

	//Get the SoundCrystal script.

	private SoundCrystal Sc;

    private Light myLight;
    public float maxIntensity = 1f;
    public float minIntensity = 0f;
    public float pulseSpeed = 1f; //here, a value of 0.5f would take 2 seconds and a value of 2f would take half a second
    private float targetIntensity = 1f;
    private float currentIntensity; 

    private bool isFailed = false;
    private bool hasPlayed = false;   
     
     
    void Start(){
        
     	//Get the SoundCrystal script.
        myLight = GetComponent<Light>();
        Sc = transform.parent.GetComponent<SoundCrystal>();
        myLight.color = new Color(0.2F, 0.7F, 0.5F, 1F); //Green/Blue
        
    }    
     void Update(){
        //If the button resets, it checks if the sequence is playing if
        /*
        if(gM.isLightCrystalLightReset == true && Sc.sequenceIsPlaying == false){ //Check if the puzzle resets
            myLight.color = new Color(1, 0.2f, 0.6f, 1F); //red
            myLight.intensity = 1F;
            //myLight.intensity -= 0.1F * (Time.deltaTime*4);
            gM.isLightCrystalLightReset = false;
        } else if(gM.resetSound == false){
        //myLight.color = new Color(0.2F, 0.7F, 0.5F, 1F); //Green/Blue
        }
*/
        if(isFailed && Sc.sequenceIsPlaying == false){
            myLight.color = new Color(1, 0.2f, 0.6f, 1F); //red
            if(!hasPlayed){
                SoundManager.Instance.PlayEvent("SP_Wrong", gameObject);
                hasPlayed = true;
            }
            if(myLight.intensity > 0){
                myLight.intensity -= 0.1F * (Time.deltaTime*4);
            } else if(myLight.intensity <= 0){
                isFailed = false;
                hasPlayed = false;;
            }
        }

        if(Sc.sequenceIsPlaying == true){
            myLight.color = new Color(0.2F, 0.7F, 0.5F, 1F); //Green/Blue
            currentIntensity = Mathf.MoveTowards(myLight.intensity,targetIntensity, Time.deltaTime*pulseSpeed);
            if(currentIntensity >= maxIntensity){
                currentIntensity = maxIntensity;
                targetIntensity = minIntensity;
            }else if(currentIntensity <= minIntensity){
                currentIntensity = minIntensity;
                targetIntensity = maxIntensity;
            }
            myLight.intensity = currentIntensity;
        }
        if(Sc.sequenceIsPlaying == false){
            //slowly turn of the light.
          myLight.intensity -= 0.1F * (Time.deltaTime*4);
        }
    }

    public override void OnReceive(){
        isFailed = true;
        myLight.intensity = 5F;
    }
}
