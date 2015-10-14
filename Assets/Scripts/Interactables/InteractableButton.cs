using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	Trigger trigger;
	Animator buttonAnimator;
	Light buttonLight;
	public AudioClip activatedButtonSound;
	AudioSource audio;
	ParticleSystem par;
	//checks if the sound has been player, inorder to only play the sound only one time.
	private bool isSoundPlayed = false;

	[Tooltip("-1 = anyone, 1 = player1, 2 = player2, 4 = player3")]
	public int playerID;
	private int combinedPlayerID;
	private bool[] additionCheck;

	private float delay;
	private float timer;

	// Use this for initialization
	void Start () {
		buttonAnimator = GetComponent<Animator>();
		additionCheck = new bool[4];
		delay = 1.0f;
		trigger = GetComponent<Trigger>();
		buttonLight = GetComponent<Light>();
		audio = GetComponent<AudioSource>();
		par = GetComponent<ParticleSystem>();

		//Set the color of the interactable button both background light and particles to the correct user.
		switch (playerID){
			case -1:
				buttonLight.color = Color.white;
				par.startColor = Color.white;
			break;
			case 1:
				buttonLight.color = Color.red;
				par.startColor = Color.red;
			break;
			case 2:
				buttonLight.color = Color.blue;
				par.startColor = Color.blue;
			break;
			case 3:
				buttonLight.color = Color.magenta;
				par.startColor = Color.magenta;
			break;
			case 4:
				buttonLight.color = Color.green;
				par.startColor = Color.green;
			break;
			case 5:
				buttonLight.color = Color.yellow;
				par.startColor = Color.yellow;
			break;
			case 6:
				buttonLight.color = Color.cyan;
				par.startColor = Color.cyan;
			break;
			case 7:
				buttonLight.color = Color.white;
				par.startColor = Color.white;
			break;
			default:
                Debug.Log("Invalid playerIndex");
            break;
            }	
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0){
			timer = delay;
			combinedPlayerID = 0;
			for(int i = 0; i < additionCheck.Length; i++)
				additionCheck[i] = false;
		}
		if(trigger.isTriggered){
			buttonAnimator.SetBool("isActivated", true); //starts the animation of the button.
			par.Play(); //starts the particles system.
			playSound(); //starts the method which plays the sound.
		} else {
			 buttonAnimator.SetBool("isActivated", false); //stops the animation of the button.
			 isSoundPlayed = false; //makes sure that the sound only plays once.
			 par.Stop(); //stops the particle system.
		}
	}

	void playSound(){
		if(isSoundPlayed == false){ //if the sound has not been played.
			audio.PlayOneShot(activatedButtonSound, 1F); // play sound with the volume of 1.
			isSoundPlayed = true; // set to true, so the sound wont play twice.
		}
	}

	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		if(!additionCheck[playerIndex-1]){
			combinedPlayerID += playerIndex;
			additionCheck[playerIndex-1] = true;
		}
		
		if (trigger.isReadyToBeTriggered && (playerIndex == playerID || playerID == -1 || combinedPlayerID == playerID)) {
			trigger.isTriggered = true;
		}
	}
}
