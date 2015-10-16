﻿using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	Animator buttonAnimator;
	Light buttonLight;
	ParticleSystem par;
	
	public AudioClip activatedButtonSound;
	AudioSource audio;
	//checks if the sound has been player, inorder to only play the sound only one time.

	[Tooltip("NEEDS TO BE THE SIZE 3. Check the players that have to hit the button. 0 = red, 1 = green, 2 = blue")]
	public bool[] playerList = new bool[3];
	
	private bool[] playerCheck = new bool[3];
	
	private bool arraysFit;
	private bool isSoundPlayed = false;

	private float delay;
	private float timer;

	private Trigger trigger;
	
	// Use this for initialization
	void Start () {
		arraysFit 		= false;
		delay 			= 1.0f;
	
		buttonAnimator 	= GetComponent<Animator>();
		buttonLight 	= GetComponent<Light>();
		trigger 		= GetComponent<Trigger>();
		audio 			= GetComponent<AudioSource>();
		par 			= GetComponent<ParticleSystem>();

		//Set the color of the interactable button both background light and particles to the correct user.
		setButtonColor(playerList);
	}
	
	// Update is called once per frame
	void Update () {

		//This essentially updates the playerCheck arrays, so if one player stops hitting the button he will be removed
		timer -= Time.deltaTime;							//CountDown
		if(timer <= 0){										//Check if timer is up
			timer = delay;									//Reset Timer
			for(int i = 0; i < playerCheck.Length; i++){	//Reset playerCheck array
				playerCheck[i] = false;
			}
		}

		if(trigger.isTriggered){
			buttonAnimator.SetBool("isActivated", true); 	//starts the animation of the button.
			par.Play(); 									//starts the particles system.
			playSound(); 									//starts the method which plays the sound.
		} else {
			 buttonAnimator.SetBool("isActivated", false); 	//stops the animation of the button.
			 isSoundPlayed = false; 						//makes sure that the sound only plays once.
			 par.Stop(); 									//stops the particle system.
		}
	}

	void playSound(){
		if(isSoundPlayed == false){ 						//if the sound has not been played.
			audio.PlayOneShot(activatedButtonSound, 1F); 	// play sound with the volume of 1.
			isSoundPlayed = true; 							// set to true, so the sound wont play twice.
		}
	}

	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		if(!playerCheck[playerIndex-1]){
			playerCheck[playerIndex-1] = true;
		}

		arraysFit = checkArrays(playerCheck, playerList);

		if (!trigger.isTriggered && trigger.isReadyToBeTriggered && arraysFit)
		    trigger.Activate();	
		
	}

	public bool checkArrays(bool[] a, bool[] b){
		for(int i = 0; i < a.Length; i++){
			if(a[i] != b[i])
				return false;
		}
		return true;
	}

	public void setButtonColor(bool[] a){
		//a[0] = red, a[1] = green, a[2] = blue
		if(a[0] && !a[1] && !a[2]){							//Only red player
			buttonLight.color = Color.red;
			par.startColor = Color.red;

		} else if(!a[0] && a[1] && !a[2]){					//Only green player
			buttonLight.color = Color.green;
			par.startColor = Color.green;

		} else if(!a[0] && !a[1] && a[2]){					//Only blue player
			buttonLight.color = Color.blue;
			par.startColor = Color.blue;

		} else if(a[0] && !a[1] && a[2]){					//Red and blue player
			buttonLight.color = Color.magenta;
			par.startColor = Color.magenta;

		} else if(a[0] && a[1] && !a[2]){					//Red and green player
			buttonLight.color = Color.yellow;
			par.startColor = Color.yellow;

		} else if(!a[0] && a[1] && a[2]){					//Green and blue
			buttonLight.color = Color.cyan;
			par.startColor = Color.cyan;
		
		} else if(a[0] && a[1] && a[2]){					//All players
			buttonLight.color = Color.white;
			par.startColor = Color.white;        	
		}
	}
}
