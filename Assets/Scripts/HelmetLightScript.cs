﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Light))]
[RequireComponent (typeof (LineRenderer))]
public class HelmetLightScript : MonoBehaviour {

    public float angleNormal = 45;                  // Angle of the spotlight without focus
    public float angleFocus = 10;                   // Angle of the spotlight during focus
    public float intensityNormal = 1;               // Intensity of the spotlight without focus
    public float intensityFocus = 8;                // Intensity of the spotlight during focus

    private Light helmetLight;                      // Object that refers to the spotlight within the scene
    private bool helmetLightFocused = false;        // Checks if the light is in "focus" mode

    private float startTime;                        // Used for lerping the focus light angle and intensity
    private bool timeSaved = false;                 // Used for lerping the focus light angle and intensity

    private LineRenderer lineRenderer;					// Used for drawing the ray from the helmet
	public int playerIndex;							// index for the player.
	public Transform objectHit;
	public Ray ray;

    void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetWidth(0.1f, 0.1f);

        helmetLight = GetComponent<Light>();        //Calls the light component on the spotlight  
    }
	
	void Update () {

        lineRenderer.enabled = helmetLightFocused;

        //Checks if the focus button is pressed (Default = space)
        if (Input.GetKey("space")) {
            // Sets helmetLightFocused to true - is used later for checking if we are in "focus" mode.
            helmetLightFocused = true;

            // Checks if timeSaved is false.
            if(timeSaved == false) {
                startTime = Time.time;
                timeSaved = true;
            }
            
            // Fadetime for the spotlight angle and intensity
            float fadeTime = 1.0f;

            // Lerps the spotlight angle from normal to focused angle.
            helmetLight.spotAngle = Mathf.Lerp(angleNormal, angleFocus, (Time.time - startTime)/ fadeTime);

            // Lerps the intensity from normal to focused intensity.
            helmetLight.intensity = Mathf.Lerp(intensityNormal, intensityFocus, (Time.time - startTime) / (fadeTime * 5));
        } else {

            // Sets timeSaved to false
            timeSaved = false;

            // Resets start time.
            startTime = 0.0f;

            helmetLightFocused = false;
        }

        // Increase spotlight angle to the normal angle when not in "focus". (Couldnt get the lerp function to work, which is why we did it like this.)
        if(helmetLight.spotAngle <= angleNormal) {
            helmetLight.spotAngle += 1;
        }

        // Decreases spotlight intensity to the normal intensity when not in "focus". (Couldnt get the lerp function to work, which is why we did it like this.)
        if (helmetLight.intensity >= intensityNormal) {
            helmetLight.intensity -= 0.2f;
        }
		
        // Checks if the headlight is being focused
        if (helmetLightFocused) {

            // Makes a ray from slightly above the gameobject going in its forward direction
            ray = new Ray(gameObject.transform.position + gameObject.transform.up/3, gameObject.transform.forward);
			RaycastHit hit;
			int i = 1;
			lineRenderer.SetVertexCount(i);				// resets the number of vertecies of the line renderer to 1
			lineRenderer.SetPosition(i-1, ray.origin);	// sets the line origin to the same as that of the ray (gameobject position)

			if (Physics.Raycast(ray, out hit)) {
				// Declaring objectHit to be the object that the ray hits
				objectHit = hit.transform;
				
				// Updates the line renderer vertecies
				lineRenderer.SetVertexCount(++i);
				lineRenderer.SetPosition(i-1, hit.point);

                Interactable interactable = objectHit.GetComponent<Interactable>();
				if (interactable != null){
				    interactable.OnRayReceived(playerIndex,ray, hit);
				}
			}

            

            
        }
    }
}
