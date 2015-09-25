using UnityEngine;
using System.Collections;

public class Cart : MonoBehaviour {

	public Rail CurrentRail;
	bool isMoving = false;
	float speed = 5.0f;

	void Start() {
		StartCoroutine (UpdatePosition());
	}
	
	void Update(){

		/*Checks for key presses. If up/down key is pressed
		 then run move function. This can only happen if cart is not
		 already moving.*/
		if(Input.GetKey(KeyCode.UpArrow) && !isMoving)
			MoveForward();

		if(Input.GetKey(KeyCode.DownArrow) && !isMoving)
			MoveBackward();
	}

	/*Sets current railpoint to next railpoint
	 and runs movement coroutine*/
	void MoveForward(){

		if(CurrentRail.NextRail != null)
			CurrentRail = CurrentRail.NextRail;
		StartCoroutine(UpdatePosition());
	}

	/*Sets current railpoint to previous railpoint
	 and runs movement coroutine*/
	void MoveBackward(){
		if(CurrentRail.PreviousRail != null)
			CurrentRail = CurrentRail.PreviousRail;
		StartCoroutine(UpdatePosition());
	}

	/*Utilizes lerps for movement and rotation adjustment between position and
	 current railpoint*/
	IEnumerator UpdatePosition(){
		if(CurrentRail != null) {
			Vector3 currentPos = transform.position;
			Vector3 targetPos = CurrentRail.transform.position + CurrentRail.transform.up;
			float dist = Vector3.Distance(currentPos, targetPos);
			isMoving = true;
			float t = 0;
			while(t <= 1) {
				transform.position = Vector3.Lerp(currentPos, targetPos, t);
				transform.rotation = Quaternion.Lerp(transform.rotation, CurrentRail.transform.rotation, t);
				t += (Time.deltaTime/dist)*speed;
				yield return null;
			}
			isMoving = false;
		}
	}
}
