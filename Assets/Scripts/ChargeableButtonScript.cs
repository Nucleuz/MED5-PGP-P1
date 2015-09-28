using UnityEngine;
using System.Collections;

public class ChargeableButtonScript : MonoBehaviour {

    // Float to hold the total charged energy
    public float energy;

    public float decreaseRate;
    public float increaseRate;

    // A more simple boolean instead writing GetComponent<Trigger>().... each time.
    private bool isTriggered;

	// Use this for initialization
	void Start () {
        // Start with 0 energy
        energy = 0;
        increaseRate = 1.75f;
        decreaseRate = 0.25f;

        // Set isTriggered to the same as the isTrigger bool in the Trigger script.
        isTriggered = GetComponent<Trigger>().isTriggered;

    }
	
	// Update is called once per frame
	void Update () {

        // Checks if left mouse button is pressed. (This needs to be removed when we finally use it)
        if (Input.GetMouseButton(0))
        {
            // Sets isTriggered to true if mouse button is pressed.
            isTriggered = true;
        }
        else
        {
            // Sets isTriggered to false if mouse button is not pressed.
            isTriggered = false;
        }

        // Checks if isTrigger is true
        if (isTriggered)
        {
            // Checks if we have less than 100 energy.
            if (energy < 100)
            {
                // Increase energy while the Chargeable Button is being triggered.
                energy += increaseRate;

                // Checks if energy is bigger than 100
                if(energy > 100)
                {
                    // Set isTriggered to false if we've reached full energy.
                    isTriggered = false;
                }

                // Makes sure that we have to keep the button triggered in order to charge it.
                isTriggered = false;
            }
        }

        // Checks if isTriggered is false
        if (!isTriggered)
        {
            // Checks if energy is higher than 0
            if(energy > 0)
            {   
                // Decrease energy
                energy -= decreaseRate;
            }
        }
	}
}
