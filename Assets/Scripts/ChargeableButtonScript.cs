using UnityEngine;
using System.Collections;

public class ChargeableButtonScript : MonoBehaviour {

    // Float to hold the total charged energy
    public float energy;

    public float decreaseRate;
    public float increaseRate;

    private bool isCharging;

	// Use this for initialization
	void Start () {
        // Start with 0 energy
        energy = 0;
        increaseRate = 1.75f;
        decreaseRate = 0.25f;
    }
	
	// Update is called once per frame
	void Update () {

        // Checks if left mouse button is pressed. (This needs to be removed when we finally use it)
        if (Input.GetMouseButton(0))
        {
            // Sets isCharging to true if mouse button is pressed.
            isCharging = true;
        }
        else
        {
            // Sets isCharging to false if mouse button is not pressed.
            isCharging = false;
        }

        // Checks if isCharging is true
        if (isCharging)
        {
            // Checks if we have less than 100 energy.
            if (energy < 100)
            {
                // Increase energy while the Chargeable Button is being triggered.
                energy += increaseRate;

                // Checks if energy is bigger than 100
                if(energy > 100)
                {
                    // Set isCharging to false if we've reached full energy.
                    isCharging = false;
                }

                // Makes sure that we have to keep the button triggered in order to charge it.
                isCharging = false;
            }
        }

        // Checks if isCharging is false
        if (!isCharging)
        {
            // Checks if energy is higher than 0
            if(energy > 0)
            {   
                // Decrease energy
                energy -= decreaseRate;
            }
        }

        // Checks of enery is equal to or higher than 100
        if(energy >= 100)
        {
            // Set isTrigger in Trigger script to true
            GetComponent<Trigger>().isTriggered = true;
        }
        else
        {
            // Set isTrigger in Trigger script to false
            GetComponent<Trigger>().isTriggered = false;
        }
        
	}
}
