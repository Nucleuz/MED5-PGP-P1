using UnityEngine;
using System.Collections;

public class ChargeableLightSource : MonoBehaviour {

    public Transform chargedBeamTargetPosition;

    public bool isActivated;
    public float timeToFullyCharge;

    public ParticleSystem particles;

    public Renderer render;

	void Start () {

        render = GetComponent<Renderer>();

        timeToFullyCharge = 3.0f;

        particles = GetComponent<ParticleSystem>();
        particles.Pause();
    }
	
	void Update () {

        if(isActivated) {
            particles.Play();
            render.material.color = Color.green;
        } else {
            particles.Pause();                                                  //Gotta find a proper way to clear particles. This is not pretty
            render.material.color = Color.grey;
        }
	    
	}
}
