using UnityEngine;
using System.Collections;

public class Console : MonoBehaviour {
	//This code creates a console on the screen. 
	//If it is a client build, the clientObject must be dragged into the inspector at "CC" place. You SHOULD NOT drag the ServerObject into the SC place!!!
	//If it is a server build, the serverObject must be dragged into the inspector at "SC" place. You SHOULD NOT drag the ClientObject into the CC place!!!
	//Inorder to write in the console, you must press with the mouse inside the console box in the game.
	//Inorder to execute the command, you must press the mouse outside the console box.



	public Font newFont;     											// Specify a new font for the console onscreen. Can be dragged into the inspector.
	public string userInput; 											// Holds the input from the user
	public string message;   											// Gives feedback to the user depending on what they write.
	public string currentCommand = ""; 									// Writes the userInput on screen.
	private bool isActive = false;     									// Controls the visibility of the console. 
	public ServerCommands SC; 											// Only used by the server build, must be empty for the client build
	public ClientCommands CC; 											// Only used by the client build, must be empty for the server build

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Code which prepares the console.
		if(Input.GetKeyDown(KeyCode.F1)){
			isActive = true;
		}
		//Printing the userinput/creating command and prepares new user command.
		if(isActive == true && Input.GetKeyDown(KeyCode.Return)){
			currentCommand = userInput;
			userInput = "";
		}
		//This is for ServerCommand. This should contain all the server methods (must be writter) and universal methods.
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
		//This is for ClientCommand. This should contain all the client methods and universal methods.
		else if(currentCommand != "" && SC == null){
		switch(currentCommand){
			case "/exit":
				CallExit();
			break;
			case "/reset":
				SC.ResetLevel();
			break;
			case "/getPos":
				CC.GetPosition();
				message = CC.getMessageClient();
			break;
            case "/teleportToCart":
                CC.TeleportToCart();
                message = CC.getMessageClient();
            break;
			default:
				message = "Invalid input!";
			break;
			}
		}	
	}
	void CallExit(){
		//This makes the console not visible
		isActive = false;
		//This resets the text in the console.
		userInput = "";
		currentCommand = "";
	}

	void OnGUI(){
		GUIStyle nStyle = new GUIStyle (); //implement the style.
		nStyle.font = newFont;// font of the text
		nStyle.fontSize = 18; // size of the text
		nStyle.normal.textColor = new Color(250,50,50); //color of text
		if(isActive == true){ //This makes the console visible if the F1 has been pressed.
			userInput = GUI.TextField (new Rect (10, 100, 200, 200), userInput, 40, nStyle);
			GUI.Box(new Rect(0, 0, Screen.width/2, Screen.height/2), message);
		}
	}
}
