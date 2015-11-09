using UnityEngine;
using System.Collections;

public class Waterfall : MonoBehaviour {

	public ParticleSystem pE;

	public GameObject[] planes;
	public float fadeSpeed;
	public float[] scrollSpeed;
	public Color[] colors;

	private ScrollingUVs_Layers[] sUL = new ScrollingUVs_Layers[2];
	private Renderer[] rends = new Renderer[2];
	private bool isPlaying;
	private Color[] originalColor = new Color[2];
	private float originalWidth;
	private float originalEmission;
	private Vector3 pEOriginalScale;

	// Use this for initialization
	void Start () {
		sUL[0] = planes[0].GetComponent<ScrollingUVs_Layers>();
		sUL[1] = planes[1].GetComponent<ScrollingUVs_Layers>();
		
		rends[0] = planes[0].GetComponent<Renderer>();
		rends[1] = planes[1].GetComponent<Renderer>();

		sUL[0].uvAnimationRate.x = scrollSpeed[0];
		sUL[1].uvAnimationRate.y = scrollSpeed[1];

		rends[0].material.color = colors[0];
		rends[1].material.color = colors[1];

		originalColor[0] = colors[0];
		originalColor[1] = colors[1];

		originalWidth = planes[0].transform.localScale.y;

		originalEmission = pE.emissionRate;

		pEOriginalScale = pE.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(pE.isPlaying && !isPlaying){
			SoundManager.Instance.PlayEvent("WaterFall", gameObject);
			isPlaying = true;
		}
		
		if(!pE.isPlaying && isPlaying){
			SoundManager.Instance.StopEvent("WaterFall", gameObject);
		}*/

		if(Input.GetKeyDown(KeyCode.D)){
			Toggle();
		}

	}

	public void Toggle(){
		StartCoroutine("toggleFade");
	}

	IEnumerator toggleFade(){
		Color tempCol0, tempCol1, tempCol0To, tempCol1To;
		Vector3 tempScale, tempScaleTo, pETemp, pETempTo;
		float tempEm;

		if(pE.isPlaying){
			float t = 0;

			tempCol0 = colors[0];
			tempCol0To = tempCol0;
			tempCol0To.a = 0;

			tempCol1 = colors[1];
			tempCol1To = tempCol1;
			tempCol1To.a = 0;

			tempScale = planes[0].transform.localScale;
			tempScaleTo = tempScale;
			tempScaleTo.y = 0;

			pETemp = pE.transform.localScale;
			pETempTo = pETemp;
			pETempTo.x = 0;


			while(pE.isPlaying){
				colors[0] = Color.Lerp(tempCol0, tempCol0To, t);
				colors[1] = Color.Lerp(tempCol1, tempCol1To, t);

				planes[0].transform.localScale = Vector3.Lerp(tempScale, tempScaleTo, t);
				planes[1].transform.localScale = Vector3.Lerp(tempScale, tempScaleTo, t);

				rends[0].material.color = colors[0];
				rends[1].material.color = colors[1];

				pE.emissionRate = Mathf.Lerp(originalEmission, 0, t);
				pE.transform.localScale = Vector3.Lerp(pETemp, pETempTo, t);

				if(t >= 1){
					pE.Stop();
					yield break;
				}

				t += fadeSpeed;
				
				yield return new WaitForSeconds(0.1f);
			}
		}

		if(!pE.isPlaying){
			float t = 0;

			tempCol0 = colors[0];
			tempCol1 = colors[1];

			tempScale = planes[0].transform.localScale;
			tempScaleTo = tempScale;
			tempScaleTo.y = originalWidth;

			pETemp = pE.transform.localScale;
			pETemp.x = 0;

			while(!pE.isPlaying){
				colors[0] = Color.Lerp(tempCol0, originalColor[0], t);
				colors[1] = Color.Lerp(tempCol1, originalColor[1], t);

				planes[0].transform.localScale = Vector3.Lerp(tempScale, tempScaleTo, t);
				planes[1].transform.localScale = Vector3.Lerp(tempScale, tempScaleTo, t);

				rends[0].material.color = colors[0];
				rends[1].material.color = colors[1];

				pE.emissionRate = Mathf.Lerp(0, originalEmission, t);
				pE.transform.localScale = Vector3.Lerp(pETemp, pEOriginalScale, t);

				if(t >= 1){
					pE.Play();
					yield break;
				}
				
				t += fadeSpeed;

				yield return new WaitForSeconds(0.001f);
			}
		}
	}

}
