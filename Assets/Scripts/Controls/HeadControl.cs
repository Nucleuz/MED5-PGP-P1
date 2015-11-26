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

	public enum ControlState{
		DEBUG,
		NVR,
		VR
	}

	public ControlState controlState = ControlState.NVR;

	float controllerRotX = 0;
	float controllerRotY = 180;

	public float cartOffsetRotX;
	public float cartOffsetRotY;

	public Camera cam;

	void Start(){
		Console.Instance.AddMessage("ControlState: " + controlState);
	}

	// Update is called once per frame
	void Update () {		
		switch(controlState){
			case ControlState.DEBUG:
				Vector2 mouse = Input.mousePosition;
				float h = mouse.x / cam.pixelWidth;
				float v = (cam.pixelHeight - mouse.y)/cam.pixelHeight ;

				//Script used for detecting if controller should be used
				transform.rotation = Quaternion.Euler(v * verticalRotationAmount + verticalRotationOffset,h * horizontalRotationAmount + horizontalRotationOffset,0);

			break;
			case ControlState.NVR:
				controllerRotX += (Input.GetAxis("ViewY") * turnSpeed * Time.deltaTime);
				controllerRotY += (Input.GetAxis("ViewX") * turnSpeed * Time.deltaTime);

				controllerRotX = Mathf.Clamp(controllerRotX,-90,90);

				transform.rotation = Quaternion.Euler(controllerRotX,controllerRotY + cartOffsetRotY,0);
			break;
			case ControlState.VR:
				transform.rotation = Quaternion.Euler(0,cartOffsetRotY - 180,0);
			break;
		}

		if(Input.GetKeyDown("c")){
			if((int)++controlState % 3 == 0)
				controlState = ControlState.DEBUG;
				Console.Instance.AddMessage("ControlState: " + controlState);
				Debug.Log("ControlState: " + controlState);
		}


	}
}
