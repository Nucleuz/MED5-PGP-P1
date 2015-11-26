using UnityEngine;
using System.Collections;

public class RenderSettingsFader : MonoBehaviour {

	public static RenderSettingsFader Instance;

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	public void FogDensityTo(float to, float length){
		StartCoroutine(LerpFogDensity(to, length));
	}
	public void FogColorTo(Color to, float length){
		StartCoroutine(LerpFogColor(to, length));
	}
	public void EquatorColorTo(Color to, float length){
		StartCoroutine(LerpEquatorColor(to, length));
	}
	public void GroundColorTo(Color to, float length){
		StartCoroutine(LerpGroundColor(to, length));
	}
	public void SkyColorTo(Color to, float length){
		StartCoroutine(LerpSkyColor(to, length));
	}
	public void AmbientTo(float to, float length){
		StartCoroutine(LerpAmbientIntensity(to, length));
	}
	
	IEnumerator LerpFogDensity(float to, float length){

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

	IEnumerator LerpFogColor(Color to, float length){

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

	IEnumerator LerpEquatorColor(Color to, float length){

		Color current = RenderSettings.ambientEquatorColor;
		
		float t = 0f;
    	float start = Time.time;

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		RenderSettings.ambientEquatorColor = Color.Lerp(current, to, t);
    		yield return null;
    	}
    	RenderSettings.ambientEquatorColor = to;
	}

	IEnumerator LerpGroundColor(Color to, float length){

		Color current = RenderSettings.ambientGroundColor;
		
		float t = 0f;
    	float start = Time.time;

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		RenderSettings.ambientGroundColor = Color.Lerp(current, to, t);
    		yield return null;
    	}
    	RenderSettings.ambientGroundColor = to;
	}

	IEnumerator LerpSkyColor(Color to, float length){

		Color current = RenderSettings.ambientSkyColor;
		
		float t = 0f;
    	float start = Time.time;

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		RenderSettings.ambientSkyColor = Color.Lerp(current, to, t);
    		yield return null;
    	}
    	RenderSettings.ambientSkyColor = to;
	}

	IEnumerator LerpAmbientIntensity(float to, float length){

		float current = RenderSettings.ambientIntensity;
		
		float t = 0f;
    	float start = Time.time;

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		RenderSettings.ambientIntensity = Mathf.Lerp(current, to, t);
    		yield return null;
    	}
    	RenderSettings.ambientIntensity = to;
	}
}
