using UnityEngine;
using System.Collections;

public class ChargeableLightSource : MonoBehaviour {

    public Transform chargedBeamTargetPosition;

    public bool isActivated;

    public ParticleSystem particles;

	void Start () {
        particles = GetComponent<ParticleSystem>();
        particles.Pause();
    }
	
	void Update () {

        if(isActivated) {
            particles.Play();
        }
	    
	}
}
