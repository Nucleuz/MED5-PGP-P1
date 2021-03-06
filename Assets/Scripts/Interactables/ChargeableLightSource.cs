﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ChargeableLightSource : Interactable {

    public Transform chargedBeamTargetPosition;

    public bool isHitByRay; // Bool for checking whether crystal is hit by ray
    private bool readyForCharge; // Bool for checking whether crystal is ready to charge
    private bool readyToShoot; // Bool for checking whether crystal is ready to shoot

    private ParticleSystem particles;

    private float energy;
    public float increaseRate;
    public float decreaseRate;
    public Transform beamPos;
    private bool isCharging;



	float endInteractTime;
	public float minInteractLength;
	int playerIndex;

    Trigger trigger;

	void Start () {

        readyToShoot = false;
        readyForCharge = true;
        energy = 0;

        increaseRate = 1.0f;
        decreaseRate = 2.0f;

        trigger = GetComponent<Trigger>();


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


        // Checks if isHitByRay is true and whether readyForCharge is true
        if (endInteractTime > Time.time && readyForCharge)
        {
            // Checks if we have less than 100 energy.
            if (energy < 100)
            {
                // Increase energy while the Chargeable Button is being triggered.
                energy += increaseRate;

                // Checks if energy is bigger than 100
                if (energy >= 100)
                {
                    // Set isTrigger in Trigger script to true
                    trigger.Activate();

                    // Make sure that we cannot charge it again right away
                    readyForCharge = false;

                    readyToShoot = true;
                }
            }
        }
		else  
		{
			// Checks if energy is higher than 0
			if (energy > 0 && !Input.GetKey(KeyCode.Space)) //TODO: This is dirty, fix me (The key check, we should somehow know this by other scripts)
			{
				// Decrease energy
				energy -= decreaseRate;
			}
		}

        // Checks if energy is less than or equal than 0.
        if(energy <= 0)
        {
            readyToShoot = false;

            // Makes sure that we can charge the button.
            readyForCharge = true;

            // Reset energy in case of energy being lower than 0
            if(energy < 0){
                energy = 0;
            }

            // Set isTrigger in Trigger script to false
            trigger.Deactivate();
            trigger.canReset = true;

        }

    
	    
        // Checks whether crystal is ready to shoot
        if(readyToShoot){
            RaycastHit hit;

            // Sets the to point from the main camera to the mouse position (This needs to be changed later).
            Ray ray = new Ray(transform.position, beamPos.position - transform.position);
            // If we want to use the mouses position as a ray point, then comment out the above line and used this: Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Casts a ray against all colliders
            if (Physics.Raycast(ray, out hit)) {

                // Declaring objectHit to be the object that the ray hits
                Interactable interactable = hit.transform.GetComponent<Interactable>();
				if (interactable != null)
					interactable.OnRayEnter(playerIndex,ray,hit);
				
            }

            // Draws the ray (nice to have as a visual representation)
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
        }

	}
    public override void OnRayEnter(int playerIndex){
        this.playerIndex = playerIndex;
        endInteractTime = Time.time + minInteractLength;

    }
    public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
		OnRayEnter(playerIndex);
	}
    

    public override void OnRayExit(int playerIndex){
        
    }
}
