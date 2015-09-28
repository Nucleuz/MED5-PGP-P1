using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject[] events; // Events which are dragged into the levelmanager - Should be specified in the unity editor!!
	public int[] eventOrder; //The event order in which you want things triggered - Should be specified in the unity editor!!
	//public int currentLevelNumber; //might not need this if only 1 scene

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
