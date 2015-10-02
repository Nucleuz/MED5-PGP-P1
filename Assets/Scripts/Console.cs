using UnityEngine;
using System.Collections;

public class Console : MonoBehaviour {
	public Font newFont;
	private string message;
	private string message2;
	private bool takeUserInput;
	public string userInput;
	public string currentCommand = "";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F1)){
			currentCommand = userInput;
			userInput = "";
		}
		/*
		if(Input.GetKeyDown(KeyCode.F1)){
			message = ("Hej");
			Debug.Log("Open Console");
		}
		*/
	}

	void OnGUI(){
		GUIStyle nStyle = new GUIStyle ();
		nStyle.font = newFont;
		nStyle.fontSize = 18;
		//GUI.TextField (new Rect (10, 100, 200, 200), "message" + message, 40, nStyle);
		userInput = GUI.TextField (new Rect (10, 100, 200, 200), userInput, 40, nStyle);
		GUI.Box(new Rect(0, 0, Screen.width/2, Screen.height/2), userInput);
	}

}
