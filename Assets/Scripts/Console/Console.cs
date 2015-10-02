using UnityEngine;
using System.Collections;

public class Console : MonoBehaviour {
	public Font newFont;
	public string userInput;
	public string message;
	public string currentCommand = "";
	private bool isActive = false;
	public ServerCommands SC; // must be empty for the player.
	public ClientCommands CC; // must be empty for the Tester/Server.

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F1)){
			isActive = true;
		}
		if(isActive == true && Input.GetKeyDown(KeyCode.Return)){
			currentCommand = userInput;
			userInput = "";
		}
		//ServerCommand
		if(currentCommand != "" && CC == null){
			switch(currentCommand){
				case "/exit":
					CallExit();
				break;
				case "/reset":
					SC.ResetLevel();
				break;
				default:
					message = "Invalid input!";
				break;
			}
		}
		//ClientCommand
		else if(currentCommand != "" && SC == null){
		switch(currentCommand){
			case "/exit":
				CallExit();
			break;
			case "/reset":
				SC.ResetLevel();
			break;
			default:
				message = "Invalid input!";
			break;
			}
		}	
	}
	void CallExit(){
		isActive = false;
		userInput = "";
		currentCommand = "";
	}

	void OnGUI(){
		GUIStyle nStyle = new GUIStyle ();
		nStyle.font = newFont;
		nStyle.fontSize = 18;
		nStyle.normal.textColor = new Color(250,50,50);
		if(isActive == true){ //So box does not show
			userInput = GUI.TextField (new Rect (10, 100, 200, 200), userInput, 40, nStyle);
			GUI.Box(new Rect(0, 0, Screen.width/2, Screen.height/2), message);
		}
	}
}
