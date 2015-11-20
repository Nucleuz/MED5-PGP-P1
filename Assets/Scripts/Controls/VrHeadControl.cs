using UnityEngine;
using System.Collections;

public class VrHeadControl : MonoBehaviour {

	public float cartOffsetRotX = 0;
	public float cartOffsetRotY = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(cartOffsetRotX,cartOffsetRotY,0);
	}
}
