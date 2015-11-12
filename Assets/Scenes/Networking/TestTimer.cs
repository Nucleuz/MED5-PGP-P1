using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class TestTimer : MonoBehaviour {
	private string fileName;
	public String currentDate;
	int levelIndex;
	float lvl1, lvl2, lvl3, lvl4, lvl5, lvl6;


	// Use this for initialization
	void Start () {
		//Setting the name for the file to the current time(hour and minute)
		currentDate = System.DateTime.Now.ToString("HHmm");
	}
	
	// Update is called once per frame
	void Update () {
		saveTime(); //Only call then the game ends.
		checkLevel();
		if(Input.GetKeyDown(KeyCode.F)){ //This should be handled by gameManager.
			levelIndex+=1;
		}
	}

	public void checkLevel(){
		switch (levelIndex){
            case 1:
            	lvl1+=Time.deltaTime;
            break;
            case 2:
            	lvl2+=Time.deltaTime; 	
            break;
            case 3:
            	lvl3+=Time.deltaTime;
            break;

			case 4:
            	lvl4+=Time.deltaTime;
            break;
            case 5:
            	lvl5+=Time.deltaTime;
            break;
            case 6:
            	lvl6+=Time.deltaTime;           	
            break;

            default:
                Debug.Log("Invalid levelIndex");
            break;
        }  
	}

	public void saveTime(){
		float overallTime = lvl1 + lvl2 +lvl3 +lvl4 +lvl5 +lvl6;
		//Saving the times for all the puzzle
		string times = 
		     "Timer puzzle#1: " + lvl1 + 
		".\r\nTimer puzzle#2: " + lvl2 +
		".\r\nTimer puzzle#3: " + lvl3 +
		".\r\nTimer puzzle#4: " + lvl4 +
		".\r\nTimer puzzle#5: " + lvl5 +
		".\r\nTimer puzzle#6: " + lvl6 +
		".\r\nTimer overall: " + overallTime;
		//Creating a file
		System.IO.StreamWriter file = new System.IO.StreamWriter(currentDate);
		//Write the data(times) in a file.
		file.WriteLine(times);
		file.Close();
	}	
}
