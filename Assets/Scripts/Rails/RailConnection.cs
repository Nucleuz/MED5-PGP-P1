using UnityEngine;
using System.Collections;

//Connection point has to be functional as a rail point as well for cart to move on it.
[RequireComponent(typeof(Rail))]
public class RailConnection : MonoBehaviour {

	public Rail nRail; 	//Reference to the connection points next rail point connection
	public Rail pRail; 	//Reference to the connection points previous rail point connection
	[HideInInspector]
	public Rail self; 	//Reference to own rail script (to reduce GetComponent)

	public bool connectToNext = false;
	public bool connectToPrev = false;

	// Use this for initialization
	void Start () {
		self = GetComponent<Rail>();
	}
	
	// Update is called once per frame
	void Update () {
	
		//Checks whether points are already connected
		if(connectToNext && self.next == null){
			self.next = nRail; 		//Connect self to the next rail
			nRail.prev = self; 	//Connect the next rail to self

			//Checks whether points are already disconnected
		} else if(!connectToNext && self.next != null){
			self.next = null;		//Remove connection to next rail
			nRail.prev = null;	//Remove next rails connection to self
		}

		//This bool condition ensures that the script does not try to set a variable that is already set.
		if(connectToPrev && self.prev == null){
			self.prev = pRail; 	//Connect self to the previous rail
			pRail.next = self;		//Connect the previous rail to self

			//Checks whether points are already disconnected
		} else if(!connectToPrev && self.prev != null){
			self.prev = null;	//Remove connection to previous rail
			pRail.next = null;		//Remove previous rails connection to self
		}

	}
}
