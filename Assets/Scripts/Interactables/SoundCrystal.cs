using UnityEngine;
using System.Collections;

public class SoundCrystal : Interactable
{
    // Reference to Game Manager, so that SoundCrystal can know when a sequence is done.
    public GameManager gM;
    
    int currentIn;
    public Trigger[] buttons;

    private string[] sequenceSerial = new string[2];
    private byte[] seqLengths;
    
    public bool sequenceIsPlaying = false;

    // Use this for initialization
    void Start()
    {
        seqLengths = new byte[2] {3, 6};

        // Finding gameManager and setting reference
        gM = GameObject.Find("GameManagerObject").GetComponent<GameManager>();

        currentIn = gM.index;
        
        sequenceSerial[0] = createSequenceSerial(seqLengths[0], 3);
        sequenceSerial[1] = createSequenceSerial(seqLengths[1], 3);
        //sequenceSerial[2] = createSequenceSerial(seqLengths[2], 3);

        SetupLevelManager();

        StartCoroutine("playSequence");
    }

    void Update()
    {
        //Plays the sound when the buttons resets.
        if(gM.resetSound){
            //SoundManager.Instance.PlayEvent("Wrong", gameObject);
            gM.resetSound = false;
        }

        if(currentIn != gM.index){
            StartCoroutine("playSequence");
            currentIn = gM.index;
        }
    }
    public string createSequenceSerial(byte seqLength, byte seqVariance){
        // Create number series
        string serial = "";
        int x = 0;
        for(int i = 0; i < seqLength; i++){
            serial = serial + x;
            x++;
            if(x > seqVariance-1)
                x = 0;
        }

        // Randomly scrambles sequence
        string final = "";
        int counter = seqLength;
        for(int i = 0; i < seqLength; i++){
            x = Random.Range(0, counter);
            final = final + serial[x];
            serial = serial.Remove(x, 1);
            counter--;
        }
        Debug.Log(final);
        return final;
    }

    public void SetupLevelManager(){
        for(int i = 0; i < sequenceSerial.Length; i++){
            gM.LM.eventsInSequence[i] = seqLengths[i];
        }

        string fullSeq = sequenceSerial[0] + sequenceSerial[1];

        int x = 0;
        // counts what sequence its at
        for(int i = 0; i < seqLengths.Length; i++){
            // counts through current sequence
            for(int j = 0; j < seqLengths[i]; j++){
                gM.LM.events[x+j] = buttons[i*3+    (int)(char.GetNumericValue(fullSeq[x+j])) ];
            }
        
            x = x + seqLengths[i];
        }
    }

    public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
        if(!sequenceIsPlaying)
		    StartCoroutine("playSequence");
	}

    public override void OnRayExit(){}

    IEnumerator playSequence(){
        sequenceIsPlaying = true;
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < sequenceSerial[gM.index].Length; i++){
            switch( (int)(char.GetNumericValue(sequenceSerial[gM.index][i]))    ){

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
                Debug.Log("Sequence number out of bounds: " + (int)(char.GetNumericValue(sequenceSerial[gM.index][i])));
            break;
            }
            yield return new WaitForSeconds(1f);
        }
        sequenceIsPlaying = false;
    }
}
