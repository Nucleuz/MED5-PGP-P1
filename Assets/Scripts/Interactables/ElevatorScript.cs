using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {

    private bool soundIsPlaying;

    public RailConnection rC;                                               //rC = railConnection
    public Rail rP;                                                         //rP = railPoint
    public Trigger trigger;

    public bool isActivated = false;                                        //Check to see if the object has been activated
    public bool goingUp;

    public Transform[] nodes;                                               //Array of transform objects that holds the positions the elevator will visit
    private int activeNode;                                                     //Speed of the elevator

    public float animationLength = 1;
    private float endAnimationTime;                                       //holds the amount of time the lerp has been running
 
	// Use this for initialization
	void Start () {
        soundIsPlaying = false;
        goingUp = false;

        rC = GetComponent<RailConnection>();
        rP = GetComponent<Rail>();
                                                           
        activeNode = 0;                                                     //Sets the visited nodes to zero
	}
	
	// Update is called once per frame
	void Update () {

        // Checks if the mouse button is pressed
        if (trigger.isTriggered && !isActivated)
        {
            isActivated = true;
            endAnimationTime = Time.time + animationLength;
        }

        if (isActivated)                                                    //Activates after a mousepress
        {


            float t = (endAnimationTime - Time.time) / animationLength;                   //Calculates the final speed of the object.
            
            //Moves the elevator from it's current position to the next active node
            transform.position = Vector3.MoveTowards(transform.position, nodes[activeNode].position, t);
            
            
            //Checks if the distance between the elevator and current active node is less than 0.1 and if active node is not larger than array nodes length.
            if (Vector3.Distance(transform.position, nodes[activeNode].transform.position) < 0.1f && activeNode <= nodes.Length - 1) {
                activeNode++;
            }

            // Checks if we've reached the last node. If true, then we reverse the order of the nodes, set the active node to 0, and deactivate the elevator. 
            // It then goes backwards when we activate the elevator again.
            if(Vector3.Distance(transform.position, nodes[nodes.Length-1].transform.position) < 0.1f){
                System.Array.Reverse(nodes);
                activeNode = 0;
                if(!goingUp){
                    goingUp = true;
                } else{
                    goingUp = false;
                }
                isActivated = false;
                trigger.Deactivate();
                trigger.isReadyToBeTriggered = true;
                if(soundIsPlaying){
                    //SoundManager.Instance.PlayEvent("Elevator_Stop", gameObject);
                    soundIsPlaying = false;
                }
            } 

            // Disconnects when the active nodes is not the first or last one
            if(activeNode < nodes.Length-1 && activeNode > 0){
                rC.connectToNext = false;
                rC.connectToPrev = false;
            }

            // Checks whether the elevator has reached the last node in order to connect/disconnect to the rails.
            if(activeNode == nodes.Length-1 && Vector3.Distance(transform.position, nodes[nodes.Length-1].transform.position) < 0.3f){
                    if(goingUp){ //When end points are reached connection and disconnection to next and previous rail points is handled.
                        rC.connectToPrev = false;
                        rC.connectToNext = true;
                    }
                    if(!goingUp){
                        rC.connectToNext = false;
                        rC.connectToPrev = true;
                    }
            }

        }
        
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;                                       //Draws the gizmos magenta
     
        for (int i = 0; i < nodes.Length; i++)
            Gizmos.DrawWireCube(nodes[i].position, new Vector3(0.5f, 0.5f, 0.5f));  //Draws a cube at each node

        for (int i = 1; i < nodes.Length; i++)
            Gizmos.DrawLine(nodes[i - 1].position, nodes[i].position);              //Draws a line between the cubes in the order that they are visited.
        
    }
}
