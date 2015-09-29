using UnityEngine;
using System.Collections;

public class ChargeableLightSource : MonoBehaviour {

    public Transform chargedBeamTargetPosition;

    public bool isHitByRay; // Bool for checking whether crystal is hit by ray
    private bool readyForCharge; // Bool for checking whether crystal is ready to charge
    private bool readyToShoot; // Bool for checking whether crystal is ready to shoot

    public ParticleSystem particles;

    private Renderer render;

    private float energy;
    public float increaseRate;
    public float decreaseRate;
    public Transform beamPos;
	float lastInteracted;
	public float maxInterval;
	int playerIndex;


	void Start () {

        readyToShoot = false;
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

	

        // Checks if isHitByRay is true and whether readyForCharge is true
		if (lastInteracted > Time.time && readyForCharge)
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
                    GetComponent<Trigger>().isTriggered = true;

                    // Make sure that we cannot charge it again right away
                    readyForCharge = false;

                    readyToShoot = true;
                }
            }
        }
		else  
		{
			// Checks if energy is higher than 0
			if (energy > 0)
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
            GetComponent<Trigger>().isTriggered = false;
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
                Transform objectHit = hit.transform;
				if (objectHit.tag == "Interactable"){
					objectHit.GetComponent<RaycastReceiver>().OnRayReceived(playerIndex);
				}
				else if (objectHit.tag == "Mirror"){
					objectHit.GetComponent<Mirror>().Reflect(ray, hit, playerIndex);
				}
            }

            // Draws the ray (nice to have as a visual representation)
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
        }

	}
	public void RayCastEvent(int playerIndex){
		playerIndex = playerIndex;
		lastInteracted = Time.time + maxInterval;

	}
}
