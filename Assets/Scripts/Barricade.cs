using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

    public RailConnection rC;
    public Trigger trigger;
    public float moveSpeed = 100.0f;
    private float t = -1;

    private Vector3 startPos;
    public Transform endNode;
    private Vector3 endPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if(trigger.isTriggered && t == -1){
            startPos = transform.position;
            endPos = endNode.position;
            t = 0;
        }

		if(trigger.isTriggered && t < 1){
            t += Time.time / moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);

		}else if(trigger.isTriggered && t >= 1)
        {
            rC.connectToNext = true;
            rC.connectToPrev = true;
        }
	}



}
