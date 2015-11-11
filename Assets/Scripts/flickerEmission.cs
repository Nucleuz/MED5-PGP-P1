using UnityEngine;

public class flickerEmission : MonoBehaviour
{
	public float minIntensity = 0.25f;
	public float maxIntensity = 0.5f;

	private MeshRenderer meshRenderer;

	float random;
	
	void Start()
	{
		random = Random.Range(0.0f, 65535.0f);

		meshRenderer = GetComponent<MeshRenderer> ();
	}
	
	void Update()
	{
		float noise = Mathf.PerlinNoise(random, Time.time);
		float value = Mathf.Lerp (minIntensity, maxIntensity, noise);
		Color C = new Color(value,value,value+0.3f);

		meshRenderer.material.SetColor ("_EmissionColor", C);
	}
}