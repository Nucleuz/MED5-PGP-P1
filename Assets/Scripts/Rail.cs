using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Rail : MonoBehaviour {

	public Rail NextRail;
	public Rail PreviousRail;

	void Start(){

	}

	void Update(){
		/*
			Makes sure that application is not running.
			This part is for level editing.
		 */
		if(!Application.isPlaying){
			/*Checks for corner rails (if a previous or next rail is null
			 it must be an end railpoint) and sets rotation according to neighbouring railpoints*/
			if(NextRail != null && PreviousRail != null) {
				transform.rotation = Quaternion.LookRotation(NextRail.transform.position - PreviousRail.transform.position);

				/*sets rotation for first end point*/
			} else if(PreviousRail == null) {
				transform.LookAt(NextRail.transform);
				/*sets rotation for second end point*/
			} else if(NextRail == null) {
				transform.rotation = Quaternion.LookRotation(transform.position - PreviousRail.transform.position);
			}

			/*Visual debugging (shows direction vector)*/
			if(NextRail != null){
				Debug.DrawLine(transform.position, NextRail.transform.position, Color.red);
			}
		}
	}
}