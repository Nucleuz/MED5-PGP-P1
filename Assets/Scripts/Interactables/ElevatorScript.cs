using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {

    public bool soundIsPlaying;

    [HideInInspector]
    public RailConnection railConnector;     //railConnector = railConnection
    [HideInInspector]                                            
    public Rail railPoint;                   //railPoint = railPoint
    public Trigger trigger;

    public bool isActivated = false;         //Check to see if the object has been activated
    public int currentState = 1;

    public Transform upNode;                 //Array of transform objects that holds the positions the elevator will visit
    public Transform downNode;               //Array of transform objects that holds the positions the elevator will visit

    public float animationLength;            //holds the amount of time the lerp has been running
 
	// Use this for initialization
	void Start () {
        soundIsPlaying = false;

        railConnector = GetComponent<RailConnection>();
        railPoint = GetComponent<Rail>();
                                            //Sets the visited nodes to zero
	}
	
	// Update is called once per frame
	void Update () {
        if(trigger.isTriggered && !isActivated){
            currentState = ++currentState % 2;
        
            trigger.SendState((sbyte)currentState);
            StartCoroutine(lerpToPoint());
        }

        if(trigger.lockStateEnd < Time.time && currentState != trigger.state){
            currentState = trigger.state;
            StartCoroutine(lerpToPoint());

        }
	}

    IEnumerator lerpToPoint(){
        float startTime = Time.time;
        isActivated = true;
        
        Vector3 startPos = transform.position;
        Vector3 endPos = (currentState == 0 ? upNode.position:downNode.position);

        railConnector.DisconnectNext();
        railConnector.DisconnectPrev();

        if(!soundIsPlaying){
            SoundManager.Instance.PlayEvent("Elevator_Active", gameObject);
            soundIsPlaying = true;
        }
        
        float t = 0;
        while(t < 1f){
            t = (Time.time - startTime) / animationLength;                   //Calculates the final speed of the object.
            float smoothstepFactor = t * t * (3 - 2 * t);
            //Moves the elevator from it's current position to the next active node
            transform.position = Vector3.Lerp(startPos, endPos, smoothstepFactor);
            yield return null;
        }      

        if(soundIsPlaying){
            SoundManager.Instance.PlayEvent("Elevator_Stop", gameObject);
            soundIsPlaying = false;
        }

        if(currentState == 0)
            railConnector.ConnectToNext();
        else
            railConnector.ConnectToPrev();

        isActivated = false;

    }
}
