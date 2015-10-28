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

			if(currentPoint >= nextKeyPoint){
				if(!isPlaying){
					PlaySound();
					isPlaying = true;
				}
				nextKeyPoint = FindNextPoint();
			}
		}
	}

	public float GetRealNormalizedTime(float normTime){
		return normTime - Mathf.Floor(normTime);
	}

	public void PlaySound(){
		SoundManager.Instance.PlayEvent(eventName, gameObject);
	}

	private float FindNextPoint(){
		float currentDif = keyPoints[0]-currentPoint;
		float smallestNeg = currentDif;
		float smallestPos = currentDif;

		for(int i = 0; i < keyPoints.Length; i++)
			currentDif = keyPoints[i]-currentPoint;
			if(currentDif < 0)
				if(currentDif < smallestNeg)
					smallestNeg = currentDif;
			else
				if(currentDif < smallestPos)
					smallestPos = currentDif;

		isPlaying = false;
		if(smallestPos != null)
			return smallestPos;
		else
			return smallestNeg;
	}
}
