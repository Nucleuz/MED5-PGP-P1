using UnityEngine;
using System.Collections;

public class FPSUpdater : MonoBehaviour {
    public UnityEngine.UI.Text text;
    public string FPS;
    
    public bool isActive = false;
    // Use this for initialization
    void Start() {
        text = GetComponent<UnityEngine.UI.Text>(); //Get the UI Text Component
        StartCoroutine("UpdateText");
    }

    // Update is called once per frame
    void Update() {
   
        FPS = (1 / Time.deltaTime).ToString(); //Update the FPS 
    }
    IEnumerator UpdateText() {
        while (isActive) { 
        text.text = FPS; //Set the text of the UIText to the current measured FPS every .1 second
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
