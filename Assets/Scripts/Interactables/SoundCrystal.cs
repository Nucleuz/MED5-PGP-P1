using UnityEngine;
using System.Collections;

public class SoundCrystal : Interactable
{
	SoundManager sM;
	public ParticleSystem pS;

    // Reference to Game Manager, so that SoundCrystal can know when a sequence is done.
    public GameManager gM;
    
    int currentIn;
    public Trigger[] buttons;

    private string[] sequenceSerial = new string[3];
    private byte[] seqLengths;
    
    private bool sequenceIsPlaying = false;

    // Use this for initialization
    void Start()
    {
        seqLengths = new byte[3] {3, 6, 6};

        // Finding soundmanager and setting reference
        sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        // Finding gameManager and setting reference
      //  gM = GameObject.Find("GameManagerObject").GetComponent<GameManager>();

        currentIn = gM.index;
        
        sequenceSerial[0] = createSequenceSerial(seqLengths[0], 3);
        sequenceSerial[1] = createSequenceSerial(seqLengths[1], 3);
        sequenceSerial[2] = createSequenceSerial(seqLengths[2], 3);

        SetupLevelManager();

        StartCoroutine("playSequence");
    }

    void Update()
    {
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

        string fullSeq = sequenceSerial[0] + sequenceSerial[1] + sequenceSerial[2];

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

    public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
        if(!sequenceIsPlaying)
		    StartCoroutine("playSequence");
	}

    IEnumerator playSequence(){
        sequenceIsPlaying = true;
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < sequenceSerial[gM.index].Length; i++){
            switch( (int)(char.GetNumericValue(sequenceSerial[gM.index][i]))    ){

            case 0:
                sM.PlayEvent("SP_PlayerButton1", gameObject);
        		pS.Play();
            break;

            case 1:
                sM.PlayEvent("SP_PlayerButton2", gameObject);
        		pS.Play();
            break;

            case 2:
                sM.PlayEvent("SP_PlayerButton3", gameObject);
        		pS.Play();
            break;

            case 3:
                sM.PlayEvent("SP_PlayerButton4", gameObject);
        		pS.Play();
            break;

            case 4:
                sM.PlayEvent("SP_PlayerButton5", gameObject);
        		pS.Play();
            break;

            case 5:
                sM.PlayEvent("SP_PlayerButton6", gameObject);
        		pS.Play();
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
