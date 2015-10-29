using UnityEngine;
using System.Collections;

public class SOUND_AnimationKeyComp : MonoBehaviour {

	[Tooltip ("Name of wWise event")]
	public string eventName;

	[Tooltip ("Keypoint positions in seconds")]
	public float[] keyPoints;


	public Animator anim;

	public AnimatorStateInfo currentState;

	private bool isPlaying;

	//The current point in animation
	private float currentPoint;

	//Next keypoint relative to currentPoint
	private float nextKeyPoint;
	private float lastKeyPoint;

	private int testInt;

	// Use this for initialization
	void Start () {
		testInt = 0;
		currentState = anim.GetCurrentAnimatorStateInfo(0);
		currentPoint = currentState.normalizedTime*currentState.length;
		nextKeyPoint = FindNextPoint();
		lastKeyPoint = nextKeyPoint;
	}
	
	// Update is called once per frame
	void Update () {
		currentState = anim.GetCurrentAnimatorStateInfo(0);
		if(GetRealNormalizedTime(currentState.normalizedTime) > 0){
			currentPoint = GetRealNormalizedTime(currentState.normalizedTime)*currentState.speed;
			
			if(currentPoint >= nextKeyPoint && nextKeyPoint != keyPoints[0]){
				if(!isPlaying){
					PlaySound();
					isPlaying = true;
				}
			}
			else if(currentPoint >= nextKeyPoint && currentPoint < keyPoints[keyPoints.Length-1]){
				if(!isPlaying){
					PlaySound();
					isPlaying = true;
				}
			}
			lastKeyPoint = nextKeyPoint;
			nextKeyPoint = FindNextPoint();
		}
	}

	public float GetRealNormalizedTime(float normTime){
		return normTime - Mathf.Floor(normTime);
	}

	public void PlaySound(){
		SoundManager.Instance.PlayEvent(eventName, gameObject);
	}

	private float FindNextPoint(){
		float nearestPos = keyPoints[keyPoints.Length-1];

		for(int i = 0; i < keyPoints.Length; i++){
			if(currentPoint < keyPoints[keyPoints.Length-1]){
				if(keyPoints[i] > currentPoint && keyPoints[i] < nearestPos)
					nearestPos = keyPoints[i];
			} else
				if(keyPoints[i] < nearestPos)
					nearestPos = keyPoints[i];
		}
		isPlaying = false;
		return nearestPos;
	}
}
