using UnityEngine;
using System.Collections;

public class LightFade : MonoBehaviour {
	//This script is creating a light pulsating from the sound crystals.
	//When the sound crystal is emitting sound it will start increasing and decreasing the intensity of the light.
	//When the sound crystal is NOT emitting sound the lights intensity is slowly turned off.
	//The fade in and out part of this code is retrieved from an online source.  

	//Get the SoundCrystal script.
	public SoundCrystal Sc;

    private Light myLight;
    public float maxIntensity = 1f;
    public float minIntensity = 0f;
    public float pulseSpeed = 1f; //here, a value of 0.5f would take 2 seconds and a value of 2f would take half a second
    private float targetIntensity = 1f;
    private float currentIntensity;    
     
     
    void Start(){
     	//Get the SoundCrystal script.
     	Sc = GameObject.Find("SoundCrystal").GetComponent<SoundCrystal>();

        myLight = GetComponent<Light>();
    }    
     void Update(){
     	if(Sc.sequenceIsPlaying == true){
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
}
