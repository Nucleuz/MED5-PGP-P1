using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Rail : MonoBehaviour {

	public bool spawnRailPoint; 	//bool functions as a button for spawning a new railpoint
	public Rail NextRail; 			//the next rail in line
	public Rail PreviousRail; 		//the previous rail in line
	public GameObject railPoint; 	//prefab reference

	// Use this for initialization
	void OnEnable () {
		spawnRailPoint = false;
	}

	void Update(){
		
			//Makes sure that application is not running.
			//This part is for level editing.
		if(!Application.isPlaying){
			//Checks for corner rails (if a previous or next rail is null
			//it must be an end railpoint) and sets rotation according to neighbouring railpoints
			if(NextRail != null && PreviousRail != null) {
				transform.rotation = Quaternion.LookRotation(NextRail.transform.position - PreviousRail.transform.position);

			//sets rotation for first end point
			} else if(PreviousRail == null && NextRail != null) {
				transform.LookAt(NextRail.transform);

			//sets rotation for second end point
			} else if(NextRail == null && PreviousRail != null) {
				transform.rotation = Quaternion.LookRotation(transform.position - PreviousRail.transform.position);
			}

			//Check if bool is true, when runs createNewPoint function
			if(spawnRailPoint){
				createNewPoint();
				spawnRailPoint = false;
			}

			//Visual debugging (shows direction vector)
			if(NextRail != null){
				Debug.DrawLine(transform.position, NextRail.transform.position, Color.red);
			}
		}
	}
	
	//Find the end of the rail segment
	//(the one railpoint that has an empty NextRail variable)
	//Creates new railpoint and set its PreviousRail, and itself as the previous rails NextRail
	protected void createNewPoint(){
		if(NextRail == null){
			GameObject g = Instantiate(railPoint, new Vector3(transform.position.x+1, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			g.transform.parent = this.transform.parent;
			g.GetComponent<Rail>().PreviousRail = this;
			NextRail = g.GetComponent<Rail>();
		} else {
			NextRail.createNewPoint();
		}
	}
}
