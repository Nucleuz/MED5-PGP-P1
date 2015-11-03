using UnityEngine;
using System.Collections;

public class SoundCrystal : Interactable
{
    private byte[][] sequences = new byte[2][];
    public  short seqIndex = -1;
    
    public bool sequenceIsPlaying = false;

    private IEnumerator sequenceRoutine;

    private Trigger trigger;

    [Tooltip("First trigger will start the first sound sequence")]
    public Trigger[] sequenceTriggers;

    // Use this for initialization
    void Start()
    {
        sequences[0] = new byte[3] {0,2,1};
        sequences[1] = new byte[6] {1,0,1,2,1,2};
        trigger = GetComponent<Trigger>();
    }

    void Update()
    {
        if(seqIndex >= 0 && seqIndex < sequenceTriggers.Length && !sequenceIsPlaying && trigger.isTriggered){
            sequenceRoutine = StartPlayingSequence();
            StartCoroutine(sequenceRoutine);
        }

        if(seqIndex >= - 1 && seqIndex < sequences.Length - 1 && sequenceTriggers[seqIndex + 1].isTriggered){
            seqIndex++;
            Debug.Log("seqIndex: "+ seqIndex);
            if(sequenceRoutine != null){
                sequenceIsPlaying = false;
                StopCoroutine(sequenceRoutine);
            }
            if(!sequenceIsPlaying){
                sequenceRoutine = StartPlayingSequence();
                StartCoroutine(sequenceRoutine); 
            }
        }
    }

    public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
        if(!sequenceIsPlaying){
            trigger.Activate();
        }
	}

    public override void OnRayExit(){
    }
    
    IEnumerator StartPlayingSequence(){
        sequenceIsPlaying = true;
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < sequences[seqIndex].Length; i++){
            Debug.Log("Sound crystal:" + i);
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
        trigger.isTriggered = false;
        trigger.isReadyToBeTriggered = true;
        sequenceIsPlaying = false;
    }
}
