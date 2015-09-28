using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class RailHandler : MonoBehaviour {
	//BATTLE PLAN!:

	//Needs to spawn to edge cubes
	//Edge cubes needs to be set as connector (so that other rail segments can be connected)
	//Upon button press spawn new cube
	//Needs to automatically set new cubes variables
	public bool spawnRailPoint = false;
	protected GameObject railPoint;
	public static List<GameObject> railSegment = new List<GameObject>();

	// Use this for initialization
	void Start () {
		if(railSegment.Count == 0 && !Application.isPlaying){
			Debug.Log(railSegment.Count);
			railPoint = new GameObject("railPoint");
			railPoint.AddComponent<Rail>();
			railPoint.transform.parent = this.transform;
			
			railSegment.Add(railPoint);
			createRailPoint();
			railSegment[1].GetComponent<Rail>().PreviousRail = railSegment[0].GetComponent<Rail>();
			railSegment[0].GetComponent<Rail>().NextRail = railSegment[1].GetComponent<Rail>();
		}
		Debug.Log(railSegment.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
		//Checks whether application is running.
		if(!Application.isPlaying){
			if(spawnRailPoint){
				createRailPoint();
				spawnRailPoint = false;
			}
		}
	
	}

	protected void createRailPoint(){
		GameObject g = Object.Instantiate(railPoint, railPoint.transform.position, Quaternion.identity) as GameObject;
		railSegment.Add(g);
		g.transform.parent = this.transform;
		railSegment[railSegment.Count-1].transform.position = new Vector3(	railSegment[railSegment.Count-2].transform.position.x+1, 
																			railSegment[railSegment.Count-2].transform.position.y, 
																			railSegment[railSegment.Count-2].transform.position.z);

		railSegment[railSegment.Count-2].GetComponent<Rail>().NextRail = railSegment[railSegment.Count-1].GetComponent<Rail>();
		railSegment[railSegment.Count-1].GetComponent<Rail>().PreviousRail = railSegment[railSegment.Count-2].GetComponent<Rail>();
		railSegment[railSegment.Count-1].GetComponent<Rail>().NextRail = null;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1,1,1,1);
		for(int i = 0; i < railSegment.Count; i++){
			Gizmos.DrawCube(railSegment[i].transform.position, new Vector3(1,1,1));
		}
	}
}
