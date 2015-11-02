using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

	private bool soundPlaying;

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
		soundPlaying = false;
        t = 0;
        startPos = transform.position;
        endPos = endNode.transform.position;

		if(!trigger.isTriggered && !isFirstBarricade){
			transform.position = endPos;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// ONLY RUNS TO POSITION NEXT SET OF BARRICADES AFTER OPENING FIRST
		if(!isFirstBarricade){
			if(!trigger.isTriggered && previousBarricadeHandler.isTriggered){
				t += Time.time / moveSpeed;
				transform.position = Vector3.Lerp (endPos, startPos, t);
				if(!soundPlaying && !isOpened){
					SoundManager.Instance.PlayEvent("RailwayBlockerMove", gameObject);
					soundPlaying = true;
				}
			}
		}

		if(trigger.isTriggered && !isOpened){
			t = 0;
            isOpened = true;
		}

        if(isOpened && t < 1){
            t += Time.time / moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            if(soundPlaying){
				SoundManager.Instance.PlayEvent("RailwayBlockerMove", gameObject);
				soundPlaying = false;
			}
        }

        if(isOpened && t >= 1)
        {
            rC.connectToNext = true;
            rC.connectToPrev = true;
        }
	}



}
