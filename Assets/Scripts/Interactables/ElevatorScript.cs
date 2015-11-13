using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {

    public bool soundIsPlaying;

    [HideInInspector]
    public RailConnection railConnector;  
    [HideInInspector]                                            //railConnector = railConnection
    public Rail railPoint;                                                         //railPoint = railPoint
    public Trigger trigger;

    public bool isActivated = false;                                        //Check to see if the object has been activated
    public bool goingUp;

    public Transform upNode;                                               //Array of transform objects that holds the positions the elevator will visit
    public Transform downNode;                                               //Array of transform objects that holds the positions the elevator will visit

    public float animationLength;                                      //holds the amount of time the lerp has been running
 
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
            StartCoroutine(lerpToPoint());
        }
	}

    IEnumerator lerpToPoint(){
        float startTime = Time.time;
        isActivated = true;
        
        Vector3 startPos = transform.position;
        Vector3 endPos = (goingUp ? upNode.position:downNode.position);

        railConnector.connectToNext = false;
        railConnector.connectToPrev = false;

        if(!soundIsPlaying){
            SoundManager.Instance.PlayEvent("Elevator_Active", gameObject);
            soundIsPlaying = true;
        }
        
        float t = 0;
        while(t<1f){
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

        if(goingUp)
            railConnector.connectToNext = true;
        else
            railConnector.connectToPrev = true;

        goingUp = !goingUp;
        isActivated = false;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;                                       //Draws the gizmos magenta
 
        Gizmos.DrawWireCube(upNode.position, new Vector3(0.5f, 0.5f, 0.5f));  //Draws a cube at each node
        Gizmos.DrawWireCube(downNode.position, new Vector3(0.5f, 0.5f, 0.5f));  //Draws a cube at each node

        Gizmos.DrawLine(upNode.position, downNode.position);              //Draws a line between the cubes in the order that they are visited.
        
    }
}
