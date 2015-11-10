using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Console : MonoBehaviour {
	//This code creates a console on the screen.
	//Inorder to write in the console, you must press with the mouse inside the console box in the game.
	//Inorder to execute the command, you must press the mouse outside the console box.


	static Console instance;

	public static Console Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType(typeof(Console)) as Console;
			}

			return instance;
		}
	}

	public Text consoleCanvasText;				// Pointer to Canvas Text
	public GameObject consoleContainer;				// Pointer to whole Canvas (For Enable/Disable)
	public KeyCode keyToOpen;					// Which key to press to open Console
	private string output;
	private bool activated = false;     			// Controls the visibility of the console.
	public bool isServer;						// Active server commands or client commands (True = Server Commands, False = Client Commands)
	public ClientManager clientManagerPtr;
	public InputField inputField;

	public FPSUpdater fps;
	private bool isWaitingForInput;

	// Use this for initialization
	void Start () {
		isServer = GameObject.Find("Manager Handler") != null;	// This is really weird way to do it.
		isWaitingForInput = false;
	}

	// Update is called once per frame
	void Update () {
		//Code which prepares the console.
		if(Input.GetKeyDown(keyToOpen)){
			activated = !activated;
			consoleContainer.SetActive(activated);
		}

        if(consoleCanvasText.text != output)
			consoleCanvasText.text = output;
	}

	public void AddMessage(string message) {
		output += "[" + DateTime.Now.ToLongTimeString() + "] " + (message + "\n");
	}

    public void ExecuteCommand(string currentCommand) {
        if (currentCommand != "")
        {
			inputField.text = "";

			if(isWaitingForInput) {
				clientManagerPtr.ConnectToServer(currentCommand);
				return;
			}
			// First check commands which are the same on both Client and Server
			switch(currentCommand)
			{
				case "exit":
					//This makes the console not visible
					activated = false;
				break;
				case "clear":
					output = "";
				break;
				case "connect" :
					isWaitingForInput = true;
					AddMessage("Please insert IP to connect to.");
				break;
			}

			// Then checks commands specific for Server or Client
			if(isServer)
	            switch (currentCommand)
	            {
	                case "reset":
	                    ServerCommands.ResetLevel();
                    break;
	                case "fps":
						ServerCommands.ToggleFPS(fps);
                    break;
	                case "resetPosition":
						ServerCommands.ResetPosition();
                    break;
	                case "noclip":
						ServerCommands.ToggleNoClip();
                    break;
	            }
			else
				switch (currentCommand)
				{
					case "getPos":
						ClientCommands.GetPosition();
						AddMessage(ClientCommands.getMessageClient());
					break;
					case "teleportToCart":
						ClientCommands.TeleportToCart();
						AddMessage(ClientCommands.getMessageClient());
					break;
						case "fps":
						ClientCommands.ToggleFPS(fps);
					break;
						case "resetPosition":
						ClientCommands.ResetPosition();
					break;
						case "noclip":
						ClientCommands.ToggleNoClip();
					break;
				}
        }
    }
}
