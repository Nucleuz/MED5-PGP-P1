using UnityEngine;
using System.Collections;

/*

simple head control with mouse... for testing

rotates head one full rot around y, half around x and is centered

*/

public class HeadControl : MonoBehaviour {

	float horizontalRotationAmount = 360;
	float verticalRotationAmount = 180;
	
	float horizontalRotationOffset = -180;
	float verticalRotationOffset = -90;

	private float turnSpeed = 80.0f;
	public bool controllerConnected = false;

	float controllerRotX = 0;
	float controllerRotY = 0;

	public float cartOffsetRotX;
	public float cartOffsetRotY;

	public Camera cam;

	// Update is called once per frame
	void Update () {

		Vector2 mouse = Input.mousePosition;
		float h = mouse.x / cam.pixelWidth;
		float v = (cam.pixelHeight - mouse.y)/cam.pixelHeight ;

		//Script used for detecting if controller should be used
		if(controllerConnected == false) {
			transform.rotation = Quaternion.Euler(v * verticalRotationAmount + verticalRotationOffset,h * horizontalRotationAmount + horizontalRotationOffset,0);
		}

		//Controller code. Gets its input via the Unity input manager
		else if(controllerConnected) {
			controllerRotX += (Input.GetAxis("ViewY") * turnSpeed * Time.deltaTime);
			controllerRotY += (Input.GetAxis("ViewX") * turnSpeed * Time.deltaTime);

			controllerRotX = Mathf.Clamp(controllerRotX,-90,90);

			transform.rotation = Quaternion.Euler(controllerRotX + cartOffsetRotX,controllerRotY + cartOffsetRotY,0);
		} 

		if(Input.GetKeyDown("c")){
			controllerConnected = !controllerConnected;
		}


	}
}
