using UnityEngine;
using System.Collections;

public class SOUND_EpicDoor : MonoBehaviour {

	[Tooltip ("Name of wWise events. Has to match keypoints")]
	public string[] eventNames;

	[Tooltip ("Keypoint positions in seconds")]
	public float[] keyPoints;


	public Animator anim;

	public AnimatorStateInfo currentState;

	private bool isPlaying;

	//The current point in animation
	private float currentPoint;

	//Next keypoint relative to currentPoint
	private int nextKeyPoint;


	// Use this for initialization
	void Start () {
		currentState = anim.GetCurrentAnimatorStateInfo(0);
		currentPoint = currentState.normalizedTime*currentState.length;
		nextKeyPoint = FindNextPoint();
	}
	
	// Update is called once per frame
	void Update () {
		currentState = anim.GetCurrentAnimatorStateInfo(0);
		if(GetRealNormalizedTime(currentState.normalizedTime) > 0){
			currentPoint = GetRealNormalizedTime(currentState.normalizedTime)*currentState.speed;

			if(currentPoint >= keyPoints[nextKeyPoint] && keyPoints[nextKeyPoint] != keyPoints[0]){
				if(!isPlaying){
					PlaySound(nextKeyPoint);
					isPlaying = true;
				}
			}
			else if(currentPoint >= keyPoints[nextKeyPoint] && currentPoint < keyPoints[keyPoints.Length-1]){
				if(!isPlaying){
					PlaySound(nextKeyPoint);
					isPlaying = true;
				}
			}
			nextKeyPoint = FindNextPoint();
		}
	}

	public float GetRealNormalizedTime(float normTime){
		return normTime - Mathf.Floor(normTime);
	}

	public void PlaySound(int i){
		SoundManager.Instance.PlayEvent(eventNames[i], gameObject);
	}

	private int FindNextPoint(){
		float nearestPos = keyPoints[keyPoints.Length-1];
		int x = keyPoints.Length-1;

		for(int i = 0; i < keyPoints.Length; i++){
			if(currentPoint < keyPoints[keyPoints.Length-1]){
				if(keyPoints[i] > currentPoint && keyPoints[i] < nearestPos)
					nearestPos = keyPoints[i];
					x = i;
			} else
				if(keyPoints[i] < nearestPos){
					nearestPos = keyPoints[i];
					x = i;
				}
		}
		isPlaying = false;
		return x;
	}
}
