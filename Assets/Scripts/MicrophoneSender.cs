using UnityEngine;
using System.Collections;
using VoiceChat;
using VoiceChat.Networking;
using VoiceChat.Networking.Legacy;

public class MicrophoneSender : MonoBehaviour {
	private VoiceChatRecorder recorder;

	// Use this for initialization
	void Start () {
		// Get Components for Pointers
		recorder = GetComponent<VoiceChatRecorder> ();

		// Presumes that the first device availiable is the best
		recorder.Device = recorder.AvailableDevices [0];

		//TODO: Get NetworkId from DarkRift before recording 
		recorder.StartRecording();

		//TODO: LIST
		// 1. Set VoiceChatRecorder.Instance.NetworkId to a unique number on each client
		// 2. Subscribe to the VoiceChatRecorder.Instance.NewSample event
		// 3. When the NewSample events get triggered, send the VoiceChatPacket data to all other clients.
		// 4. On the client read the data back, and make sure you have one instance of the Network/Resources/VoiceChat_Player prefab, grab the VoiceChatPlayer script and call OnNewSample(packet) on it.
	}
	
	// Update is called once per frame
	void Update () {

	}
}
