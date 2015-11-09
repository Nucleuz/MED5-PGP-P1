using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DarkRift;

public class ServerInfo : MonoBehaviour {
	private Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}

	void OnGUI() {
		string output = "";
		output += "Clients: " + DarkRiftServer.GetNumberOfConnections();
		output += "\ntotalExecutionCounts: " + PerformanceMonitor.totalExecutionCounts;
		output += "\ntotalExecutionTime: " + PerformanceMonitor.totalExecutionTime;
		output += "\naverageExecutionTime: " + PerformanceMonitor.averageExecutionTime;
		text.text = output;
	}
}
