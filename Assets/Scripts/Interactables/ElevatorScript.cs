using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {

    private bool soundIsPlaying;

    public Trigger trigger;

    public bool isActivated = false;                                        //Check to see if the object has been activated
    public bool goingUp;

    public Transform upNode;
    public Transform downNode;

    public float animationLength = 1;

	// Use this for initialization
	void Start () {
        soundIsPlaying = false;
                                                    //Sets the visited nodes to zero
	}

	// Update is called once per frame
	void Update () {
    if (trigger.isTriggered && !isActivated)
      StartCoroutine(lerpToPoint());

      if((goingUp && trigger.state == 0) || (!goingUp && trigger.state == 1)){
        lerpToPoint();
      }
	}

  IEnumerator lerpToPoint(){
    float startTime = Time.time;
    isActivated = true;
    Vector3 startPos = transform.position;
    Vector3 endPos = (goingUp ? upNode.position : downNode.position);

    float t = 0f;
    while(t < 1f){
      t = (Time.time - startTime) / animationLength;
      float smoothStepFactor = t * t * (3 - 2 * t);
      transform.position = Vector3.Lerp(startPos,endPos,smoothStepFactor);
      yield return null;
    }

    goingUp = !goingUp;
    isActivated = false;
  }
}
