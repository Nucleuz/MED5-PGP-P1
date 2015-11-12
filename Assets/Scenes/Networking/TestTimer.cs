using UnityEngine;
using System.Collections;
using System.IO;

public class TestTimer : MonoBehaviour {

	//public string savePath;
	private string fileName;
	private int fileNameNumber;

	// Use this for initialization
	void Start () {
		fileNameNumber = 0;
		fileName = "Test" + fileNameNumber.ToString();
		if(File.Exists(fileName)){
			Debug.Log("The filename " + fileName + " already exists. Creating a new file with 1 added to its number");
			fileNameNumber++;
			fileName = "Test" + fileNameNumber.ToString();	
		}

		// Compose a string that consists of three lines.
		string lines = "First line.\r\nSecond line.\r\nThird line.";
		
		// Write the string to a file.
		System.IO.StreamWriter file = new System.IO.StreamWriter();
		file.WriteLine(lines);
		
		file.Close();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
