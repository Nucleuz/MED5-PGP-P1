using UnityEngine;
using System.Collections;
using VoiceChat;
using VoiceChat.Networking;
using VoiceChat.Networking.Legacy;

public class MicrophoneSender : MonoBehaviour {
	public VoiceChatPlayer player;
	public VoiceChatRecorder recorder;
	public VoiceChatSettings settings;

	// Use this for initialization
	void Start () {	
		Debug.Log ("Player: " + player.isActiveAndEnabled);
		Debug.Log ("Recorder: " + recorder.isActiveAndEnabled);
		Debug.Log ("Settings: " + settings.isActiveAndEnabled + " " + recorder.HasDefaultDevice + " " + recorder.HasSpecificDevice);

		recorder.NetworkId = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (recorder.PushToTalkKey) && !recorder.IsRecording)
			recorder.StartRecording ();
		if(Input.GetKeyUp(recorder.PushToTalkKey))
			recorder.StopRecording();

		if (recorder.IsRecording)
			Debug.Log ("Recording");
	}

	void PrepareAudio() {
		AudioClip clip = new AudioClip();

		float[] samples = new float[clip.samples * clip.channels];
		clip.GetData (samples, 0);
		
		VoiceChatPacket package = VoiceChatUtils.Compress (samples);
	}
}
