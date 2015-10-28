using UnityEngine;
using System.Collections;

public class Cart : MonoBehaviour {

	public Rail currentRail;            // The railpoint that the cart is currently on
    public float movementSpeed = 1;        // Speed of animation (At full speed);

    private Vector3 startingPosition;   // Starting Position (In case of reset)
    private Rail startingRail;          // Starting Rail (In case of reset)
    private Animator minecartAnimator;  // Animator pointer for Minecart
	private float currentStep;          // How long the player is between two points

	void Start() {


        startingRail = currentRail;
        startingPosition = transform.position;
		minecartAnimator = GetComponent<Animator>();

        currentStep = 0;
    }
	
	void Update(){
        //This is specifically for elevator connection points where the cart has to move with the elevator
        if (currentRail.GetComponent<RailConnection>() == true){	
			transform.position = new Vector3(currentRail.transform.position.x, currentRail.transform.position.y+1,currentRail.transform.position.z);
		}

        float verticalAxis = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalAxis) > 0.01f) { 
            minecartAnimator.StartPlayback();
            Move(verticalAxis);
        } else
        {
            minecartAnimator.speed = 0;
            minecartAnimator.StopPlayback();
        }
    }


    void Move(float verticalAxis) {
        // Decide which way rail we are moving towards (Towards Next or Previous rail)
        Rail railMoveTowards = null;    // Hopefully not null afterwards
        if (currentStep > 0)
            railMoveTowards = currentRail.next;
        else if (currentStep < 0)
            railMoveTowards = currentRail.prev;
        else if (currentStep == 0 && verticalAxis > 0)
            railMoveTowards = currentRail.next;
        else if (currentStep == 0 && verticalAxis < 0)
            railMoveTowards = currentRail.prev;
        else
            Debug.LogError("Movement went terrible wrong, this should not be possible!");

        if(railMoveTowards != null) { 
            // Find distance between Current Rail and the Next Rail for Normalize amount of movement
            float length = Vector3.Distance(currentRail.transform.position, railMoveTowards.transform.position);

            // Set the currentStep (t) to move towards verticalAxis with normalize distance of the two rails, times acceleration and animationSpeed.
            currentStep += (1 / length) * verticalAxis * movementSpeed * 0.0075f;

            // Set new position using the currentStep and move that position just a tad up 
            transform.position = Vector3.Lerp(currentRail.transform.position, railMoveTowards.transform.position, Mathf.Abs(currentStep)) + (currentRail.transform.up / 4)*2;

            // Check if the cart have reached a rail and set that rail to the current rail.
            if (currentStep >= 1) 
            {
                currentRail = currentRail.next;
                currentStep = 0;

            }
            else if(currentStep <= -1)
            {
                currentRail = currentRail.prev;
                currentStep = 0;
            }
            // Check if the cart have move beyond the starting point (In opposite direction) and save it
            else if(currentStep <= 0 && currentRail.Equals(startingRail))
            {
                currentStep = 0;
            }

            // Set minecart animation speed
            minecartAnimator.speed = (movementSpeed) * verticalAxis;
        }
    }

    public void ResetPosition() {
        transform.position = startingPosition;
        currentRail = startingRail;
    }
}
