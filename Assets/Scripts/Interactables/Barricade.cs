using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

    public RailConnection rC;
    public Trigger trigger;

    public Transform upNode;
    public Transform downNode;

    public float animationLength = .5f;
    private bool animationRunning = false;

    private bool isOpened = false;
	public bool isFirstBarricade = false;

    private bool isPlaying;

	public Trigger previousBarricadeHandler;
    public Trigger currentBarricadeHandler;

	// Use this for initialization
	void Start () {
		if(!trigger.isTriggered && !isFirstBarricade){
			transform.position = downNode.transform.position;
		}

        isPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(animationRunning) return;
		if(!isFirstBarricade && !trigger.isTriggered && previousBarricadeHandler.isTriggered && !currentBarricadeHandler.isTriggered){
			StartCoroutine(GoToUpState());
		}
		
		if(animationRunning) return;
		if(currentBarricadeHandler.isTriggered && !isOpened){
            trigger.isTriggered = true;
            StartCoroutine(GoToDownState());
        }
	}

	IEnumerator GoToUpState(){
		animationRunning = true;

		float startTime = Time.time;
    	Vector3 startPosition = transform.position;
    	Vector3 endPosition = upNode.position;
        
        if(!isPlaying){
            SoundManager.Instance.PlayEvent("RailwayBlockerMove", gameObject);
            isPlaying = true;
        }
        
        float t = 0f;
        while(t < 1f){
            t = (Time.time - startTime)/animationLength;
            transform.position = Vector3.Lerp(startPosition,endPosition,t);
            yield return null;
        }
        transform.position = endPosition;
        animationRunning = false;
        isOpened = false;

        if(isPlaying){
            SoundManager.Instance.StopEvent("RailwayBlockerMove", gameObject);
            SoundManager.Instance.PlayEvent("RailwayBlockerStop", gameObject);
            isPlaying = false;
        }

        rC.connectToNext = false;
        rC.connectToPrev = false;
	}


	IEnumerator GoToDownState(){
		animationRunning = true;

		float startTime = Time.time;
    	Vector3 startPosition = transform.position;
    	Vector3 endPosition = downNode.position;
        
        if(!isPlaying){
            SoundManager.Instance.PlayEvent("RailwayBlockerMove", gameObject);
            isPlaying = true;
        }
        
        float t = 0f;
        while(t < 1f){
            t = (Time.time - startTime)/animationLength;
            transform.position = Vector3.Lerp(startPosition,endPosition,t);
            yield return null;
        }
        transform.position = endPosition;
        animationRunning = false;
        isOpened = true;

        if(isPlaying){
            SoundManager.Instance.StopEvent("RailwayBlockerMove", gameObject);
            SoundManager.Instance.PlayEvent("RailwayBlockerStop", gameObject);
            isPlaying = false;
        }

        rC.connectToNext = true;
        rC.connectToPrev = true;
	}



}
