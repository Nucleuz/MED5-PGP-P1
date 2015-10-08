using UnityEngine;
using System.Collections;
using VoiceChat;
using VoiceChat.Networking;
using VoiceChat.Networking.Legacy;
using DarkRift;
using System;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneSender : MonoBehaviour {
	public VoiceChatRecorder recorder;

	private NetPlayerSync client;

	float lastTime = 0;
	double played = 0;
	double received = 0;
	int index = 0;
	float[] data;
	float playDelay = 0;
	bool shouldPlay = false;
	float lastRecvTime = 0;
	
	[SerializeField]
	int playbackDelay = 2;
	
	public float LastRecvTime
	{
		get { return lastRecvTime; }
	}

	// Use this for initialization
	void Start () {
		client = GetComponent<NetPlayerSync> ();

		// Presumes that the first device availiable is the best
		recorder.Device = recorder.AvailableDevices [0];

		recorder.NetworkId = (int) client.networkID;

		recorder.StartRecording ();

		VoiceChatRecorder.Instance.NewSample += OnNewSample;
	}

	// Called by recorder whenever we get a new sample packet.
	//TODO: Play specific audio on specific player for spatialised audio
	//HINT: Use packet.NetworkId and AudioSource on each player
	void OnNewSample (VoiceChatPacket packet)
	{
		// Set last time we got something
		lastRecvTime = Time.time;
		
		// Decompress
		float[] sample = null;
		int length = VoiceChatUtils.Decompress(packet, out sample);
		
		// Add more time to received
		received += VoiceChatSettings.Instance.SampleTime;
		
		// Push data to buffer
		Array.Copy(sample, 0, data, index, length);
		
		// Increase index
		index += length;
		
		// Handle wrap-around
		if (index >= GetComponent<AudioSource>().clip.samples)
		{
			index = 0;
		}
		
		// Set data
		GetComponent<AudioSource>().clip.SetData(data, 0);
		
		// If we're not playing
		if (!GetComponent<AudioSource>().isPlaying)
		{
			// Set that we should be playing
			shouldPlay = true;
			
			// And if we have no delay set, set it.
			if (playDelay <= 0)
			{
				playDelay = (float)VoiceChatSettings.Instance.SampleTime * playbackDelay;
			}
		}
		
		VoiceChatFloatPool.Instance.Return(sample);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
