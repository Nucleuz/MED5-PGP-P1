using UnityEngine;
using System.Collections;

public class HeartEmission : MonoBehaviour {

	Trigger trigger;

	Renderer renderer;
	Material material;
	Material matInstance;

	float lerpColor;
	float t;
	float lerpSpeed;
	
	void Start()
	{
		trigger = GetComponent<Trigger> ();

		renderer = GetComponent<Renderer>();
		material = renderer.sharedMaterial;
		matInstance = Instantiate (material);
		t = 0f;
		lerpSpeed = 0.15f;


	}
	
	void Update()
	{  
		if (trigger.isTriggered) {
			if (t < 1f) {
				Debug.Log(t);
				t += Time.deltaTime * lerpSpeed;
				lerpColor = Mathf.Lerp (0f, 1f, t);
				matInstance.EnableKeyword ("_EMISSION");
				matInstance.SetColor ("_EmissionColor", new Color (lerpColor, lerpColor, lerpColor));
			}

			// Start to fade the screen to white when the heart reaches 0.5 emission
			if(t > 0.5f){
				CanvasFade.Instance.ToWhite (0.5f);
			}
		}
	}
}
