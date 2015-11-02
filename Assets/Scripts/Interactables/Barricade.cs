using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

    public RailConnection rC;
    public Trigger trigger;
    public float moveSpeed = 500.0f;
    private float t;

    public Vector3 startPos;
    public GameObject endNode;
    public Vector3 endPos;
    private bool isOpened = false;
	public bool isFirstBarricade = false;

	public Trigger previousBarricadeHandler;

	// Use this for initialization
	void Start () {
        t = 0;
        startPos = transform.position;
        endPos = endNode.transform.position;

		if(!trigger.isTriggered && !isFirstBarricade){
			transform.position = endPos;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!isFirstBarricade){
			if(!trigger.isTriggered && previousBarricadeHandler.isTriggered){
				t += Time.time / moveSpeed;
				transform.position = Vector3.Lerp (endPos, startPos, t);
			}
		}

		if(trigger.isTriggered && !isOpened){
			t = 0;
            isOpened = true;
            
		}

        if(isOpened && t < 1){
            t += Time.time / moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
        }

        if(isOpened && t >= 1)
        {
            rC.connectToNext = true;
            rC.connectToPrev = true;
        }
	}



}
