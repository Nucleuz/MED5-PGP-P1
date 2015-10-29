using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Light))]
[RequireComponent (typeof (LineRenderer))]
public class HelmetLightScript : MonoBehaviour {

    private bool soundIsPlaying;

    public float angleNormal = 45;                  // Angle of the spotlight without focus
    public float angleFocus = 10;                   // Angle of the spotlight during focus
    public float intensityNormal = 1;               // Intensity of the spotlight without focus
    public float intensityFocus = 8;                // Intensity of the spotlight during focus

    private Light helmetLight;                      // Object that refers to the spotlight within the scene
    private bool helmetLightFocused = false;        // Checks if the light is in "focus" mode

    private float startTime;                        // Used for lerping the focus light angle and intensity
    private bool timeSaved = false;                 // Used for lerping the focus light angle and intensity

    private LineRenderer lineRenderer;				// Used for drawing the ray from the helmet

    [Tooltip("1, 2, 3")]
	public int playerIndex;							// index for the player.
	public Transform objectHit;
	public Ray ray;

    void Start () {
        soundIsPlaying = false;
		lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(0.1f, 0.1f);

        helmetLight = GetComponent<Light>();        //Calls the light component on the spotlight  
        //Set the color of the interactable button both background light and particles to the correct user.
        switch (playerIndex){
            case 1:
                helmetLight.color = new Color(1, 0.2F, 0.2F, 1F); //red
            break;
            case 2:
                helmetLight.color = new Color(0.2F, 1, 0.2F, 1F); //green
            break;
            case 3:
                helmetLight.color = new Color(0.2F, 0.2F, 1, 1F); //blue
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
            }   
    }
	
	void Update () {

        if(Input.GetKeyDown("l")){ // || Input.GetKeyDown("NextLevel")){ //It is the "L" key
            Application.LoadLevel(Application.loadedLevel + 1);
        }

        lineRenderer.enabled = helmetLightFocused;

        //Checks if the focus button is pressed (Default = space)
        if (Input.GetKey("space") || Input.GetAxis("RightTrigger") > 0.1f || Input.GetAxis("LeftTrigger") > 0.1f) {

            if(!soundIsPlaying){
                SoundManager.Instance.PlayEvent("Headlamp_Focus_Active", gameObject);
                SoundIsPlaying = true;
            }
            

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

            if(helmetLight.intensity >  intensityFocus * 0.3f){
                // Sets helmetLightFocused to true - is used later for checking if we are in "focus" mode.
                helmetLightFocused = true;
            } else {
                helmetLightFocused = false;
            }
        } else {
            if(SoundIsPlaying){
                SoundManager.Instance.PlayEvent("Headlamp_Focus_Stop", gameObject);
                SoundIsPlaying = false;
            }

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

            // Make a ray from the transforms pos and forward
            ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;

            
			if (Physics.Raycast(ray, out hit)) {
				// Declaring objectHit to be the object that the ray hits
				objectHit = hit.transform;
				
                //setting up the lineRenderer (only if we have actually hit something)
                lineRenderer.SetVertexCount(2);             
                lineRenderer.SetPosition(0, transform.position);  // sets the line origin to the 
				lineRenderer.SetPosition(1, hit.point);

                Interactable interactable = objectHit.GetComponent<Interactable>();
				if (interactable != null){
                    //@Optimize - The mirror is the only one who the ray, hit, lineRenderer, and count
                    interactable.OnRayReceived(playerIndex,ray, hit,ref lineRenderer,2);

				}
			}
        }
    }
}
