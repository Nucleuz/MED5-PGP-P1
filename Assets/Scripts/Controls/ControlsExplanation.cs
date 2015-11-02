using UnityEngine;
using System.Collections;

public class ControlsExplanation : MonoBehaviour {

    private bool hasShown = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetAxis("RightTrigger") == 1.0f || Input.GetAxis("LeftTrigger") == 1.0f) {
            gameObject.SetActive(false);
            hasShown = true;
        }
    }
}
