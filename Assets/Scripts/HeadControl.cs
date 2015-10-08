﻿using UnityEngine;
using System.Collections;

/*
By KasperHdL

simple head control with mouse... for testing

rotates head one full rot around y, half around x and is centered

*/

public class HeadControl : MonoBehaviour {

	float horizontalRotationAmount = 360;
	float verticalRotationAmount = 180;
	
	float horizontalRotationOffset = -180;
	float verticalRotationOffset = -90;



	// Update is called once per frame
	void Update () {
		Vector2 mouse = Input.mousePosition;
		float h = mouse.x / Camera.main.pixelWidth;
		float v = (Camera.main.pixelHeight - mouse.y)/Camera.main.pixelHeight;
	
		transform.rotation = Quaternion.Euler(v * verticalRotationAmount + verticalRotationOffset,h * horizontalRotationAmount + horizontalRotationOffset,0);


	}
}
