using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	Animator buttonAnimator;
	Light buttonLight;
	ParticleSystem par;
	
	SoundManager sM;
	public bool playedSound;

	[Tooltip("NEEDS TO BE THE SIZE 3. Check the players that have to hit the button. 0 = red, 1 = green, 2 = blue")]
	public bool[] playerList;
	
	private bool[] playerCheck = new bool[3];
	
	private bool arraysFit;

	private float delay;
	private float timer;

	public Trigger trigger;

	public Renderer[] rend;
	
	// Use this for initialization
	void Start () {
		playedSound 	= false;
		arraysFit 		= false;
		delay 			= 1.0f;
	
		buttonAnimator 	= GetComponent<Animator>();
		buttonLight 	= GetComponent<Light>();
		trigger 		= GetComponent<Trigger>();
		par 			= GetComponent<ParticleSystem>();
		sM 				= GameObject.Find("SoundManager").GetComponent<SoundManager>();

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
		} else {
			buttonAnimator.SetBool("isActivated", false); 	//stops the animation of the button.
			par.Stop(); 									//stops the particle system.
		}

		if(playedSound && !trigger.isTriggered){
			sM.ToggleSwitch("On_Off", "Off", gameObject);
			sM.PlayEvent("ButtonOnOff", gameObject);
			playedSound = false;
		}
	}

	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		if(!playerCheck[playerIndex-1]){
			playerCheck[playerIndex-1] = true;
		}

		arraysFit = checkArrays(playerCheck, playerList);

		if (trigger.isReadyToBeTriggered && arraysFit){
			trigger.isTriggered = true;
			if(!playedSound){
				sM.ToggleSwitch("On_Off", "On", gameObject);
				sM.PlayEvent("ButtonOnOff", gameObject);
				playedSound = true;
			}
		}
		
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
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.red;
			}

		} else if(!a[0] && a[1] && !a[2]){					//Only green player
			buttonLight.color = Color.green;
			par.startColor = Color.green;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.green;
			}

		} else if(!a[0] && !a[1] && a[2]){					//Only blue player
			buttonLight.color = Color.blue;
			par.startColor = Color.blue;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.blue;
			}

		} else if(a[0] && !a[1] && a[2]){					//Red and blue player
			buttonLight.color = Color.magenta;
			par.startColor = Color.magenta;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.magenta;
			}

		} else if(a[0] && a[1] && !a[2]){					//Red and green player
			buttonLight.color = Color.yellow;
			par.startColor = Color.yellow;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.yellow;
			}

		} else if(!a[0] && a[1] && a[2]){					//Green and blue
			buttonLight.color = Color.cyan;
			par.startColor = Color.cyan;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.cyan;
			}
		
		} else if(a[0] && a[1] && a[2]){					//All players
			buttonLight.color = Color.white;
			par.startColor = Color.white; 
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.white;
			}       	
		}
	}
}
