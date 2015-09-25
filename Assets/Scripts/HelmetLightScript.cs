using UnityEngine;
using System.Collections;

public class HelmetLightScript : MonoBehaviour {

    public float angleNormal = 45;                                                  //Angle of the spotlight without focus
    public float angleFocus = 10;                                                   //Angle of the spotlight during focus
    public float intensityNormal = 1;                                               //Intensity of the spotlight without focus
    public float intensityFocus = 8;                                                //Intensity of the spotlight during focus

    public Light helmetLight;                                                       //Object that refers to the spotlight within the scene

    float startTime;
    bool timeSaved = false;

    void Start () {

        helmetLight = GetComponent<Light>();                                        //Calls the light component on the spotlight

        Debug.Log("AngleNorm: " + angleNormal);
        Debug.Log("AngleFocus: " + angleFocus);

    }
	
	void Update () {

        if(Input.GetKey("space"))                                                   //Checks if the focus button is pressed (Default = space)
        {

            if(timeSaved == false)
            {
                startTime = Time.time;
                timeSaved = true;
            }
            
            float fadeTime = 1.0f;

            Debug.Log("startTime: " + (Time.time - startTime) / fadeTime);

            helmetLight.spotAngle = Mathf.Lerp(angleNormal, angleFocus, (Time.time - startTime)/ fadeTime);
            helmetLight.intensity = Mathf.Lerp(intensityNormal, intensityFocus, (Time.time - startTime) / (fadeTime * 5));

        } else
        {
            helmetLight.spotAngle = angleNormal;
            helmetLight.intensity = intensityNormal;

            timeSaved = false;
        }
	
	}
}
