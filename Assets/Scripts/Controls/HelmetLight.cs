using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelmetLight : MonoBehaviour {
    [HideInInspector] public bool hideBeams;
    [HideInInspector] public float spotlightAnimationLength = .2f;
    [HideInInspector] public float spotlightAnimationTime = 0;
    [HideInInspector] public int playerIndex;                         // index for the player.
    [HideInInspector] public NetPlayerSync netPlayer;

    public Light halo;

    private float angleNormal = 12.5f;         // Angle of the spotlight without focus
    private float angleFocus = 5;           // Angle of the spotlight during focus
    private float startTime = -1f;          // Used for lerping the focus li
    private float stopTime = -1f;
    private Light mainLight;                // Reference to light
    private Renderer[] beamQuads;           // Refrence to Renderer planes
    private Interactable lastObjectHit;
    private bool soundIsPlaying;
    private Ray ray;
    private Material matForBeam;

    public void SetPlayerIndex (int networkId) {
        playerIndex = networkId;
        mainLight = GetComponent<Light>();   
    }

    public void LightUpdate(float t){
        //Lerps the spotlight angle from normal to focused angle.
        float angle = Mathf.Lerp(angleNormal, angleFocus,t);
        float haloSize = Mathf.Lerp(6, 2.2f,t);
        mainLight.spotAngle = angle;
        if(beamQuads != null){
            halo.range = haloSize;
            for(int i = 0; i < beamQuads.Length; i++) {
                beamQuads[i].transform.localScale = new Vector3(beamQuads[i].transform.localScale.x, angle, beamQuads[i].transform.localScale.z);
            }
        }
    }

    void Update () {
        //Checks if the focus button is pressed (Default = space)
        if (Input.GetKey("space") || Input.GetAxis("RightTrigger") > 0.1f || Input.GetAxis("LeftTrigger") > 0.1f) {

            // Checks if timeSaved is false.

            if(startTime == -1){
                startTime = Time.time;
                stopTime = -1f;
                if(!soundIsPlaying){
                    //SoundManager.Instance.PlayEvent("Headlamp_Focus_Active", gameObject);
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
                    //SoundManager.Instance.PlayEvent("Headlamp_Focus_Stop", gameObject);
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

    public void SetAsSender(){

        Color color = new Color();

        //Set the color of the interactable button both background light and particles to the correct user.
        switch (playerIndex){
            case 1: // Blue
                color = new Color(0F, 0F, 1, 1F);
            break;
            case 2: // Red
                color = new Color(1, 0F, 0F, 1F);
            break;
            case 3: // Green
                color = new Color(0F, 1, 0F, 1F);
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
        }
        mainLight.color = color;
    }
    public void SetAsReceiver(){
        beamQuads = new Renderer[4];
        for(int i = 0; i < beamQuads.Length; i++) {
            beamQuads[i] = transform.GetChild(i).GetComponent<Renderer>();
            transform.GetChild(i).gameObject.SetActive(true);
        }

        halo.gameObject.SetActive(true);
        Color color = new Color();

        //Set the color of the interactable button both background light and particles to the correct user.
        switch (playerIndex){
            case 1: // Blue
                color = new Color(0.2F, 0.2F, 1, 0.1F);
            break;
            case 2: // Red
                color = new Color(1, 0.2F, 0.2F, 0.1F);
            break;
            case 3: // Green
                color = new Color(0.2F, 1, 0.2F, 0.1F);
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
        }
        for(int i = 0; i < beamQuads.Length; i++){
            beamQuads[i].material.SetColor("_TintColor", color);
        }
        mainLight.color = color;
        halo.color = color;
    }
}
