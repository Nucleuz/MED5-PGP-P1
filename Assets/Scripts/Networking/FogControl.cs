using UnityEngine;
using System.Collections;

public class FogControl : MonoBehaviour {

	public static FogControl Instance;

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	public void FadeDensityTo(float to, float length){
		StartCoroutine(FadeDensity(to, length));
	}

	public void FadeColorTo(Color to, float length){
		StartCoroutine(FadeColor(to, length));
	}

	IEnumerator FadeDensity(float to, float length){

		float current = RenderSettings.fogDensity;
		
		float t = 0f;
    	float start = Time.time;

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		RenderSettings.fogDensity = Mathf.Lerp(current, to, t);
    		yield return null;
    	}
    	RenderSettings.fogDensity = to;
	}

	IEnumerator FadeColor(Color to, float length){

		Color current = RenderSettings.fogColor;
		
		float t = 0f;
    	float start = Time.time;

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		RenderSettings.fogColor = Color.Lerp(current, to, t);
    		yield return null;
    	}
    	RenderSettings.fogColor = to;
	}
}
