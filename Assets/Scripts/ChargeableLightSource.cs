using UnityEngine;
using System.Collections;

public class ChargeableLightSource : MonoBehaviour {

    public Transform chargedBeamTargetPosition;

    public bool isHitByRay;
    private bool readyForCharge;

    public ParticleSystem particles;

    private Renderer render;

    private float energy;
    public float increaseRate;
    public float decreaseRate;

	void Start () {

        readyForCharge = true;
        energy = 0;

        increaseRate = 1.0f;
        decreaseRate = 2.0f;

        render = GetComponent<Renderer>();

        particles = GetComponent<ParticleSystem>();
        particles.enableEmission = false;
    }
	
	void Update () {

        if(energy != 0){
            particles.enableEmission = true;
            particles.emissionRate = energy*10;
            particles.startSpeed = energy/10;
        } else {
            particles.enableEmission = false;
        }

        if(Input.GetMouseButton(0) && readyForCharge){
            isHitByRay = true;
        } else {
            isHitByRay = false;
        }

        // Checks if isHitByRay is true and whether readyForCharge is true
        if (isHitByRay && readyForCharge)
        {
            // Checks if we have less than 100 energy.
            if (energy < 100)
            {
                // Increase energy while the Chargeable Button is being triggered.
                energy += increaseRate;

                // Checks if energy is bigger than 100
                if (energy >= 100)
                {
                    // Set isHitByRay to false if we've reached full energy.
                    isHitByRay = false;

                    // Set isTrigger in Trigger script to true
                    GetComponent<Trigger>().isTriggered = true;

                    // Make sure that we cannot charge it again right away
                    readyForCharge = false;
                }
            }
        }

        // Checks if energy is less than or equal than 0.
        if(energy <= 0)
        {
            // Makes sure that we can charge the button.
            readyForCharge = true;

            // Reset energy in case of energy being lower than 0
            if(energy < 0){
                energy = 0;
            }

            // Set isTrigger in Trigger script to false
            GetComponent<Trigger>().isTriggered = false;
        }

        // Checks if isHitByRay is false
        if (!isHitByRay)
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
