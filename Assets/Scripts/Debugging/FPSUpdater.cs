using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSUpdater : MonoBehaviour {
    private Text text;
    
    public bool isActive = false;
    // Use this for initialization

    void Start() {
        text = GetComponent<Text>(); //Get the UI Text Component
        StartCoroutine("UpdateText");
    }

    IEnumerator UpdateText() {
        while (isActive) { 
			text.text = (1 / Time.deltaTime).ToString(); //Set the text of the UIText to the current measured FPS every .1 second
	        yield return new WaitForSeconds(0.1f);
        }
        text.text = "";
    }
    public void Toggle() {
        isActive = !isActive;
        StopAllCoroutines();
        StartCoroutine("UpdateText");
    }
}
