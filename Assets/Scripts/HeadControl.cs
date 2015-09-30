using UnityEngine;
using System.Collections;

/*
By KasperHdL

simple head control with mouse... for testing

*/

public class HeadControl : MonoBehaviour {

	public float rotationSpeed = 1;

	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Mouse X");
		float v = Input.GetAxis("Mouse Y");
		Debug.Log("h: " + h + ", v: " + v);
	
		transform.RotateAround(Vector3.zero,transform.right,v * rotationSpeed * Time.deltaTime);
		transform.RotateAround(Vector3.zero,transform.up,h * rotationSpeed * Time.deltaTime);


	}
}
