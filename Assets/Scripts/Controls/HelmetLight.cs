using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelmetLight : MonoBehaviour {
    [HideInInspector] public bool hideBeams;
    [HideInInspector] public float spotlightAnimationLength = .2f;
    [HideInInspector] public float spotlightAnimationTime = 0;
    [HideInInspector] public int playerIndex;                         // index for the player.
    [HideInInspector] public NetPlayerSync netPlayer;

    private float angleNormal = 20;         // Angle of the spotlight without focus
    private float angleFocus = 5;           // Angle of the spotlight during focus
    private float startTime = -1f;          // Used for lerping the focus li
    private float stopTime = -1f;
    private Light mainLight;                // Reference to light
    private MeshRenderer[] beamQuads;           // Refrence to MeshRenderer planes
    private Interactable lastObjectHit;
    private bool soundIsPlaying;
    private Ray ray;
    private Material matForBeam;

    public void SetPlayerIndex (int networkId) {
        playerIndex = networkId;
        mainLight = GetComponent<Light>();
        for(int i = 0; i < transform.childCount - 1; i++) {
            beamQuads[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }
        matForBeam = new Material(beamQuads[0].material);

        //Set the color of the interactable button both background light and particles to the correct user.
        switch (playerIndex){
            case 1: // Blue
            ((Behaviour) transform.FindChild("Halo Blue").GetComponent("Halo")).enabled = true;
            for(int i = 0; i < transform.childCount - 1; i++) {
                beamQuads[i].material.color = new Color(0.2F, 0.2F, 1, 0.1F);
            }
            mainLight.color = new Color(0.2F, 0.2F, 1, 0.1F);
            break;
            case 2: // Red
            ((Behaviour) transform.FindChild("Halo Red").GetComponent("Halo")).enabled = true;
            for(int i = 0; i < transform.childCount - 1; i++) {
                beamQuads[i].material.color = new Color(1, 0.2F, 0.2F, 0.1F);
            }
            mainLight.color = new Color(1, 0.2F, 0.2F, 0.1F);
            break;
            case 3: // Green
            ((Behaviour) transform.FindChild("Halo Green").GetComponent("Halo")).enabled = true;
            for(int i = 0; i < transform.childCount - 1; i++) {
                beamQuads[i].material.color = new Color(0.2F, 1, 0.2F, 0.1F);
            }
            mainLight.color = new Color(0.2F, 1, 0.2F, 0.1F);
            break;
            default:
                Debug.Log("Invalid playerIndex");
            break;
        }
        
        if(hideBeams) {
            ((Behaviour) transform.FindChild("Halo Green").GetComponent("Halo")).enabled = false;
            ((Behaviour) transform.FindChild("Halo Blue").GetComponent("Halo")).enabled = false;
            ((Behaviour) transform.FindChild("Halo Red").GetComponent("Halo")).enabled = false;
            for(int i = 0; i < transform.childCount - 1; i++) {
                beamQuads[i].enabled = false;
            }
        }
    }

    public void LightUpdate(float t){
        // Lerps the spotlight angle from normal to focused angle.
        //helmetLight.spotAngle = Mathf.Lerp(angleNormal, angleFocus,t);
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
}
