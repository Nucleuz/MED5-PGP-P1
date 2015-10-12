using UnityEngine;
using System.Collections;

public class NoClip : MonoBehaviour {

	float horizontalRotationAmount = 360;
	float verticalRotationAmount = 180;
	
	float horizontalRotationOffset = -180;
	float verticalRotationOffset = -90;

	private float turnSpeed = 80.0f;
	public bool controllerConnected = false;

	public bool isActive = false;

	private Camera cam;

	void Start(){
		cam = gameObject.GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {
		if(isActive == true){ //If the ClipCamera has been activated in the console.
			transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * 15);
			transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 15);

			Vector2 mouse = Input.mousePosition;
			float h = mouse.x / cam.pixelWidth;
			float v = (cam.pixelHeight - mouse.y)/cam.pixelHeight ;
			//Debug.Log("h: " + h + ", v: " + v);

			//Script used for detecting if controller should be used
			if(controllerConnected == false) {
				transform.rotation = Quaternion.Euler(v * verticalRotationAmount + verticalRotationOffset,h * horizontalRotationAmount + horizontalRotationOffset,0);
			}

			//Controller code. Gets its input via the Unity input manager
			else if(controllerConnected) {
				if(transform.eulerAngles.x <= 50.01f || transform.eulerAngles.x > 279.99f) {
					transform.Rotate(Vector3.up, Input.GetAxis("ViewX") * turnSpeed * Time.deltaTime, Space.World);
					transform.Rotate(Vector3.right, Input.GetAxis("ViewY") * turnSpeed * Time.deltaTime, Space.Self);
				} else if(transform.eulerAngles.x > 50 && transform.eulerAngles.x < 100) {
					transform.rotation = Quaternion.Euler(50, transform.eulerAngles.y, transform.eulerAngles.z);
				} else if(transform.eulerAngles.x < 280 && transform.eulerAngles.x > 200){
					transform.rotation = Quaternion.Euler(280, transform.eulerAngles.y, transform.eulerAngles.z);
				}
			} 
				if(Input.GetKeyDown("c")){
					controllerConnected = !controllerConnected;
				}
			}
		}
		
	public void On(){
		isActive = !isActive;
	}
}
