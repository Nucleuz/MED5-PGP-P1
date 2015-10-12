using UnityEngine;
using System.Collections;

public class ChangePlayerView : MonoBehaviour {
	//This code can change the view of the each of the players camera. 
	//It starts to toggle between the views when "N" key is pressed. 
	//All the cameras from the each player must be dragged into the inspector.
	//This code is all borrowed from a Unity.com forum!
	
     public Camera[] cameras;
     private int currentCameraIndex;
     
     // Use this for initialization
     void Start () {
         currentCameraIndex = 0;
         
         //Turn all cameras off, except the first default one
         for (int i=1; i<cameras.Length; i++){
             cameras[i].gameObject.SetActive(false);
         }
         
         //If any cameras were added to the controller, enable the first one
         if (cameras.Length>0){
             cameras [0].gameObject.SetActive (true);
         }
     }
     
     // Update is called once per frame
     void Update () {
         //If the c button is pressed, switch to the next camera
         //Set the camera at the current index to inactive, and set the next one in the array to active
         //When we reach the end of the camera array, move back to the beginning or the array.
         if (Input.GetKeyDown(KeyCode.N)){
             currentCameraIndex ++;
             if (currentCameraIndex < cameras.Length){
                 cameras[currentCameraIndex-1].gameObject.SetActive(false);
                 cameras[currentCameraIndex].gameObject.SetActive(true);
             }
             else{
                 cameras[currentCameraIndex-1].gameObject.SetActive(false);
                 currentCameraIndex = 0;
                 cameras[currentCameraIndex].gameObject.SetActive(true);
             }
         }
     }
 }