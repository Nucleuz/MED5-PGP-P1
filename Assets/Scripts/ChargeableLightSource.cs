using UnityEngine;
using System.Collections;

public class ChargeableLightSource : MonoBehaviour {

    public Transform chargedBeamTargetPosition;

    public bool isActivated;
    public float timeToFullyCharge;

    public ParticleSystem particles;

    private Renderer render;

	void Start () {

        render = GetComponent<Renderer>();

        timeToFullyCharge = 3.0f;

        particles = GetComponent<ParticleSystem>();
        particles.enableEmission = false;
    }
	
	void Update () {

        if(isActivated) {
            particles.enableEmission = true;
            render.material.color = Color.green;
        } else {
            particles.enableEmission = false;
            render.material.color = Color.grey;
        }
	    
	}
}
