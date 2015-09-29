using UnityEngine;
using System.Collections;

public class ChargeableButtonScript : MonoBehaviour
{

    // Float to hold the total charged energy
    public float energy;

    // Float as a decrease rate for the button
    public float decreaseRate;

    // Float as an increase rate for the button
    public float increaseRate;

    // Used to check if we are chargin the button
    private bool isCharging;

    // Used to check if we can charge the button
    public bool readyForCharge;

    // Use this for initialization
    void Start()
    {
        // Start with 0 energy
        energy = 0;

        // Standard increase rate
        increaseRate = 1.75f;

        // Standard decrease rate
        decreaseRate = 0.25f;

        // Make sure that the button is ready for charge when we start
        readyForCharge = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyToBeTriggered) { 
            if (readyForCharge) { 
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
                    if (energy >= 100)
                    {
                        // Set isCharging to false if we've reached full energy.
                        isCharging = false;

                        // Set isTrigger in Trigger script to true
                        GetComponent<Trigger>().isTriggered = true;

                        // Make sure that we cannot charge it again right away
                        readyForCharge = false;
                    }

                    // Makes sure that we have to keep the button triggered in order to charge it.
                    isCharging = false;
                }
            }

            // Checks if energy is less than or equal than 0.
            if(energy <= 0)
            {
                // Makes sure that we can charge the button.
                readyForCharge = true;

                // Set isTrigger in Trigger script to false
                GetComponent<Trigger>().isTriggered = false;
            }

            // Checks if isCharging is false
            if (!isCharging)
            {
                // Checks if energy is higher than 0
                if (energy > 0)
                {
                    // Decrease energy
                    energy -= decreaseRate;
                }
            }
        }
    }
}
