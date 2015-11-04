using UnityEngine;
using System.Collections;

public class ReflectionProbeRefresh : MonoBehaviour {

	ReflectionProbe rProbe;
	public float refreshDelay = 0.01f;

	// Use this for initialization
	void Start () {
		rProbe = GetComponent<ReflectionProbe>();
		StartCoroutine(UpdateProbe());
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	IEnumerator UpdateProbe(){
		while(true){
			rProbe.RenderProbe();
			yield return new WaitForSeconds(refreshDelay);
		}
	}
}
