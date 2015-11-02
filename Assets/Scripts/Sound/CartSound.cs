using UnityEngine;
using System.Collections;

public class CartSound : MonoBehaviour {

	SoundManager sM;

	// Use this for initialization
	void Start () {
		sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void PlayLegHitSound(){
		sM.PlayEvent("CartLegHit1", gameObject);	
	}

	public void PlayCrystalSound(){

	}
}
