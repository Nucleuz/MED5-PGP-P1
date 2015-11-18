using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Rail : MonoBehaviour {

	public string rName = "railPoint";
	public bool spawnRailPoint; 	//bool functions as a button for spawning a new railpoint
	public Rail next; 			    //the next rail in line
	public Rail prev; 		        //the previous rail in line
	public GameObject railPoint; 	//prefab reference
    //private MeshRenderer mesh;

	// Use this for initialization
	void OnEnable () {
		spawnRailPoint = false;
        //mesh = GetComponent<MeshRenderer>();
	}
}
