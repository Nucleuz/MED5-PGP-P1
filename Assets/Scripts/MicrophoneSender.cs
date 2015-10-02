using UnityEngine;
using System.Collections;
using VoiceChat;
using VoiceChat.Networking;
using VoiceChat.Networking.Legacy;
using DarkRift;
using System;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneSender : MonoBehaviour {
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
		VoiceChatRecorder.Instance.Device = VoiceChatRecorder.Instance.AvailableDevices [0];

		VoiceChatRecorder.Instance.NetworkId = (int) client.networkID;

		VoiceChatRecorder.Instance.StartRecording ();

		VoiceChatRecorder.Instance.NewSample += OnNewSample;
	}
	
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

	void Update() {
		if (GetComponent<AudioSource>().isPlaying)
		{
			// Wrapped around
			if (lastTime > GetComponent<AudioSource>().time)
			{
				played += GetComponent<AudioSource>().clip.length;
			}
			
			lastTime = GetComponent<AudioSource>().time;
			
			// Check if we've played to far
			if (played + GetComponent<AudioSource>().time >= received)
			{
				Stop();
				shouldPlay = false;
			}
		}
		else
		{
			if (shouldPlay)
			{
				playDelay -= Time.deltaTime;
				
				if (playDelay <= 0)
				{
					GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	void Stop()
	{
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().time = 0;
		index = 0;
		played = 0;
		received = 0;
		lastTime = 0;
	}
}
