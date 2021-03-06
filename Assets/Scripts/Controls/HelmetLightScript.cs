﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Light))]
public class HelmetLightScript : MonoBehaviour {

    [HideInInspector]
    public float spotlightAnimationLength = .2f;
    [HideInInspector] 
    public float spotlightAnimationTime = 0;
    
    private float angleNormal = 20;                  // Angle of the spotlight without focus
    private float angleFocus = 5;                   // Angle of the spotlight during focus

    private Light helmetLight;                      // Object that refers to the spotlight within the scene
    public Light nonFocusedHelmetLight; 

    private bool soundIsPlaying;


    private float startTime = -1f;                  // Used for lerping the focus light angle and intensity
    private float stopTime = -1f;

    [HideInInspector] 
    public int playerIndex;                         // index for the player.
    
    private Interactable lastObjectHit;
    private Ray ray;

    [HideInInspector]
    public NetPlayerSync netPlayer;

    public void SetPlayerIndex (int networkId) {
        /*
        switch(networkId){
            case 1: playerIndex = 3;break;
            case 2: playerIndex = 1;break;
            case 3: playerIndex = 2;break;
        }
        */

        playerIndex = networkId;

        helmetLight = GetComponent<Light>();        //Calls the light component on the spotlight  
        //Set the color of the interactable button both background light and particles to the correct user.
        switch (playerIndex){
            case 1:
                helmetLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
                nonFocusedHelmetLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
            break;
            case 2:
                helmetLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
                nonFocusedHelmetLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
            break;
            case 3:
                helmetLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
                nonFocusedHelmetLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
            }   
    }

    public void LightUpdate(float t){
        // Lerps the spotlight angle from normal to focused angle.
        helmetLight.spotAngle = Mathf.Lerp(angleNormal, angleFocus,t);
    }
    
    void Update () {
        //Checks if the focus button is pressed (Default = space)
        if (Input.GetKey("space") || Input.GetAxis("RightTrigger") > 0.1f || Input.GetAxis("LeftTrigger") > 0.1f) {

            // Checks if timeSaved is false.

            if(startTime == -1){
                startTime = Time.time;
                stopTime = -1f;
                if(!soundIsPlaying){
                    SoundManager.Instance.PlayEvent("Headlamp_Focus_Active", gameObject);
                    soundIsPlaying = true;
                }

                netPlayer.UpdateHelmetLight(true);
            }else{
                spotlightAnimationTime = (Time.time - startTime)/spotlightAnimationLength;
                if(spotlightAnimationTime < 1f)
                    LightUpdate(spotlightAnimationTime);
                else
                    initRay();
            }
           
        } else {

            // Resets start time.
            if(stopTime == -1f){
                stopTime = Time.time;
                startTime = -1f;

                if(soundIsPlaying){
                    SoundManager.Instance.PlayEvent("Headlamp_Focus_Stop", gameObject);
                    soundIsPlaying = false;
                }


                netPlayer.UpdateHelmetLight(false);

                if(lastObjectHit != null){
                    lastObjectHit.OnRayExit(playerIndex);
                    lastObjectHit = null;
                }
            }else{
                spotlightAnimationTime = 1-((Time.time - stopTime)/ spotlightAnimationLength);
                if(spotlightAnimationTime > 0f)
                    LightUpdate(spotlightAnimationTime);
            }
        }
    }

    public void initRay(){
      // Make a ray from the transforms pos and forward
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

            Interactable interactable = hit.transform.GetComponent<Interactable>();            

            if(interactable != null){
                if(lastObjectHit != null && interactable != lastObjectHit){
                    lastObjectHit.OnRayExit(playerIndex);
                    lastObjectHit = null;
                }
                if (interactable != lastObjectHit){
                    interactable.OnRayEnter(playerIndex,ray, hit);
                    lastObjectHit = interactable;
                }
            }
        }else if(lastObjectHit != null){
            lastObjectHit.OnRayExit(playerIndex);
            lastObjectHit = null;
        }
    }
}
