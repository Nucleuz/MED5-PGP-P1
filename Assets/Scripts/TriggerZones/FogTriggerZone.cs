using UnityEngine;
using System.Collections;

public class FogTriggerZone : MonoBehaviour {
	public bool fadeDensity;
	[Space(-2)]
	public float fadeDensityTo;
	[Space(10)]
	public bool fadeColor;
	[Space(-2)]
	public Color fadeColorTo;
	[Space(10)]
	public float fadeLength;


	void OnTriggerEnter(Collider col){
		if(fadeDensity){
			FogControl.Instance.FadeDensityTo(fadeDensityTo, fadeLength);
		}
		if(fadeColor){
			FogControl.Instance.FadeColorTo(fadeColorTo, fadeLength);
		}
        Destroy(gameObject);
    }
}