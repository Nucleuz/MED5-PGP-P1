using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Rail : MonoBehaviour {

	public string rName = "railPoint";
	public bool spawnRailPoint; 	//bool functions as a button for spawning a new railpoint
	public Rail next; 			    //the next rail in line
	public Rail prev; 		        //the previous rail in line
	public GameObject railPoint; 	//prefab reference
    private MeshRenderer mesh;

	// Use this for initialization
	void OnEnable () {
		spawnRailPoint = false;
        mesh = GetComponent<MeshRenderer>();
	}

	void Update(){
        if (Application.isPlaying && mesh.enabled)
            mesh.enabled = false;
			//Makes sure that application is not running.
			//This part is for level editing.
		if(!Application.isPlaying){
            if (!mesh.enabled)
                mesh.enabled = true;
			//Checks for corner rails (if a previous or next rail is null
			//it must be an end railpoint) and sets rotation according to neighbouring railpoints
			if(next != null && prev != null) {
				transform.rotation = Quaternion.LookRotation(next.transform.position - prev.transform.position);

			//sets rotation for first end point
			} else if(prev == null && next != null) {
				transform.LookAt(next.transform);

			//sets rotation for second end point
			} else if(next == null && prev != null) {
				transform.rotation = Quaternion.LookRotation(transform.position - prev.transform.position);
			}

			//Check if bool is true, when runs createNewPoint function
			if(spawnRailPoint){
				createNewPoint();
				spawnRailPoint = false;
			}

			//Visual debugging (shows direction vector)
			if(next != null){
				Debug.DrawLine(transform.position, next.transform.position, Color.red);
			}
		}
	}
	
	//Find the end of the rail segment
	//(the one railpoint that has an empty NextRail variable)
	//Creates new railpoint and set its PreviousRail, and itself as the previous rails NextRail
	protected void createNewPoint(){
		if(next == null){
			GameObject g = Instantiate(railPoint, new Vector3(transform.position.x+1, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			g.transform.parent = this.transform.parent;
			g.GetComponent<Rail>().prev = this;
			g.name = rName + g.transform.parent.childCount.ToString();
			next = g.GetComponent<Rail>();
		} else {
			next.createNewPoint();
		}
	}
}
