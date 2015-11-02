using UnityEngine;
using System.Collections;

public class SoundCrystal : Interactable
{
    // Reference to Game Manager, so that SoundCrystal can know when a sequence is done.
    
    public Trigger[] buttons;

    public LevelManager LM;

    private byte[][] sequences = new byte[2][];
    public  short seqIndex = -1;
    
    public bool sequenceIsPlaying = false;

    // Use this for initialization
    void Start()
    {
        sequences[0] = new byte[3] {0,2,1};
        sequences[1] = new byte[6] {1,0,1,2,1,2};
    }

    void Update()
    {
    }

    public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
        if(!sequenceIsPlaying)
		    StartCoroutine(StartPlayingSequence());
	}

    public override void OnRayExit(){}

    public void reset(){
        if(!sequenceIsPlaying)
        StartCoroutine(StartPlayingSequence());
    }

    public void PlayerHitTriggerZone(){
        seqIndex++;
        if(!sequenceIsPlaying)
        StartCoroutine(StartPlayingSequence()); 
    }
    
    IEnumerator StartPlayingSequence(){
        sequenceIsPlaying = true;
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < sequences[seqIndex].Length; i++){
            switch(sequences[seqIndex][i]){
                case 0:
                    SoundManager.Instance.PlayEvent("SP_PlayerButton1", gameObject);
                break;

                case 1:
                    SoundManager.Instance.PlayEvent("SP_PlayerButton2", gameObject);
                break;

                case 2:
                    SoundManager.Instance.PlayEvent("SP_PlayerButton3", gameObject);
                break;

                case 3:
                    SoundManager.Instance.PlayEvent("SP_PlayerButton4", gameObject);
                break;

                case 4:
                    SoundManager.Instance.PlayEvent("SP_PlayerButton5", gameObject);
                break;

                case 5:
                    SoundManager.Instance.PlayEvent("SP_PlayerButton6", gameObject);
                break;

                default:
                    Debug.Log("Sequence number out of bounds: ");
                break;
            }
            yield return new WaitForSeconds(1f);
        }
        sequenceIsPlaying = false;
    }
}
