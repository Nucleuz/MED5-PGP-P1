﻿using UnityEngine;
using System.Collections;

public class Cart : MonoBehaviour {

	public Rail CurrentRail; 	//The railpoint that the cart is currently on
	bool isMoving = false;		//Bool for checking whether cart is currently moving
	float speed = 5.0f;			//Cart move speed
    Vector3 startingPosition;
    Rail startingRail;
    Animator minecartAnimator;

    private float movingSpeed;

	void Start() {
        startingRail = CurrentRail;
        startingPosition = transform.position;
		StartCoroutine (UpdatePosition());
		minecartAnimator = GetComponent<Animator>();
	}
	
	void Update(){

		if(CurrentRail.GetComponent<RailConnection>() == true){	//This is specifically for elevator connection points where the cart has to move with the elevator
			transform.position = new Vector3(CurrentRail.transform.position.x, CurrentRail.transform.position.y+1,CurrentRail.transform.position.z);
		}

		//Checks for key presses. If up/down key is pressed
		//then run move function. This can only happen if cart is not
		//already moving.
		if(Input.GetAxis("Vertical")>0 && !isMoving){
			MoveForward();
			minecartAnimator.speed = movingSpeed; //Change the speed of the animation accordingly to the speed of the cart
		} else if(Input.GetAxis("Vertical")<0 && !isMoving){
			MoveBackward();
			minecartAnimator.speed = movingSpeed; //Change the speed of the animation accordingly to the speed of the cart
		}
	}


	//Sets current railpoint to next railpoint
	//and runs movement coroutine
	void MoveForward(){
		if(CurrentRail.NextRail != null){
			CurrentRail = CurrentRail.NextRail;
		}
		StartCoroutine(UpdatePosition());
	}

	//Sets current railpoint to previous railpoint
	//and runs movement coroutine
	void MoveBackward(){
		if(CurrentRail.PreviousRail != null){
			CurrentRail = CurrentRail.PreviousRail;
		}
		StartCoroutine(UpdatePosition());
	}

	//Utilizes lerps for movement and rotation adjustment between position and
	//current railpoint
	IEnumerator UpdatePosition(){
		if(CurrentRail != null) {
			Vector3 currentPos = transform.position;
			Quaternion currentRotation = transform.rotation;
			Vector3 targetPos = CurrentRail.transform.position + CurrentRail.transform.up;
			float dist = Vector3.Distance(currentPos, targetPos);
			isMoving = true;
			float t = 0;
			while(t <= 1) {
				transform.position = Vector3.Lerp(currentPos, targetPos, t);
				transform.rotation = Quaternion.Slerp(currentRotation, CurrentRail.transform.rotation, t);
				t += (Time.deltaTime/dist)*speed;
				yield return null;
				movingSpeed = t * 3.2f; //Change the speed of the animation accordingly to the speed of the cart
			}
			isMoving = false;
			minecartAnimator.speed = 0; //Change the speed of the animation accordingly to the speed of the cart
		}
	}

    public void ResetPosition() {
        transform.position = startingPosition;
        CurrentRail = startingRail;
    }
}
