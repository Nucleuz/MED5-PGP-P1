using UnityEngine;
using System.Collections;

public class DeactivateButton : MonoBehaviour {

	SoundCrystal sc;
	SoundEmitter soundEmitter;
	Light buttonLight;
	ParticleSystem par;
	// Use this for initialization
	void Start () {
		sc 				= GameObject.Find("SoundCrystal").GetComponent<SoundCrystal>();
		soundEmitter    = GetComponent<SoundEmitter>();
		buttonLight 	= GetComponent<Light>();
		par 			= GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		// Needs to be reviewed after Usability Test.
				if(sc.sequenceIsPlaying){
					soundEmitter.enabled = false;
					buttonLight.intensity = 0;
					par.enableEmission = false;
				} else{
					soundEmitter.enabled = true;
					buttonLight.intensity = 1.92f;
					par.enableEmission = true;
				}
	}
}
