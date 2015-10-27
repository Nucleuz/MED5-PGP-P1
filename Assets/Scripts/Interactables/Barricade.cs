using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

    public RailConnection rC;
    public Trigger trigger;
    public float moveSpeed = 100.0f;
    private float t;

    private Vector3 startPos;
    public GameObject endNode;
    private Vector3 endPos;
    private bool isOpened = false;

	// Update is called once per frame
	void Update () {

		if(trigger.isTriggered && !isOpened){
            isOpened = true;
            startPos = transform.position;
            
		}

        if(isOpened && t < 1){
            t += Time.time / moveSpeed;
            transform.position = Vector3.Lerp(startPos, endNode.transform.position, t);
        }

        if(isOpened && t >= 1)
        {
            rC.connectToNext = true;
            rC.connectToPrev = true;
        }
	}



}
