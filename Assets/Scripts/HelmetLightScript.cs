using UnityEngine;
using System.Collections;

public class HelmetLightScript : MonoBehaviour {

    public float angleNormal = 45;                  // Angle of the spotlight without focus
    public float angleFocus = 10;                   // Angle of the spotlight during focus
    public float intensityNormal = 1;               // Intensity of the spotlight without focus
    public float intensityFocus = 8;                // Intensity of the spotlight during focus

    public Light helmetLight;                       // Object that refers to the spotlight within the scene
    private bool helmetLightFocused = false;        // Checks if the light is in "focus" mode

    float startTime;                                // Used for lerping the focus light angle and intensity
    bool timeSaved = false;                         // Used for lerping the focus light angle and intensity

    void Start () {

        helmetLight = GetComponent<Light>();        //Calls the light component on the spotlight
        
    }
	
	void Update () {

        //Checks if the focus button is pressed (Default = space)
        if (Input.GetKey("space"))                   
        {
            // Sets helmetLightFocused to true - is used later for checking if we are in "focus" mode.
            helmetLightFocused = true;

            // Checks if timeSaved is false.
            if(timeSaved == false)
            {
                startTime = Time.time;
                timeSaved = true;
            }
            
            // Fadetime for the spotlight angle and intensity
            float fadeTime = 1.0f;

            // Lerps the spotlight angle from normal to focused angle.
            helmetLight.spotAngle = Mathf.Lerp(angleNormal, angleFocus, (Time.time - startTime)/ fadeTime);

            // Lerps the intensity from normal to focused intensity.
            helmetLight.intensity = Mathf.Lerp(intensityNormal, intensityFocus, (Time.time - startTime) / (fadeTime * 5));
        }
        else
        {
            // Sets timeSaved to false
            timeSaved = false;

            // Resets start time.
            startTime = 0.0f;

            helmetLightFocused = false;
        }

        // Increase spotlight angle to the normal angle when not in "focus". (Couldnt get the lerp function to work, which is why we did it like this.)
        if(helmetLight.spotAngle <= angleNormal)
        {
            helmetLight.spotAngle += 1;
        }

        // Decreases spotlight intensity to the normal intensity when not in "focus". (Couldnt get the lerp function to work, which is why we did it like this.)
        if (helmetLight.intensity >= intensityNormal)
        {
            helmetLight.intensity -= 0.2f;
        }

        // Checks if the headlight is being focused
        if (helmetLightFocused) { 
            RaycastHit hit;

            // Sets the to point from the main camera to the mouse position (This needs to be changed later).
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // If we want to use the mouses position as a ray point, then comment out the above line and used this: Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Casts a ray against all colliders
            if (Physics.Raycast(ray, out hit))
            {
                // Declaring objectHit to be the object that the ray hits
                Transform objectHit = hit.transform;

                // Checks if we hit an object with the "Button" tag.
                if (objectHit.tag == "Button")
                {
                    // Activates the isActivated boolean in the script. (I've just used a test script, so this needs to be corrected when we have the right objects with the right scripts)
                    objectHit.GetComponent<TestButtonScript>().isActivated = true;

                    // Draws the ray (nice to have as a visual representation)
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
                }
            }
        }
    }
}
