using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	[HideInInspector]
	Animator buttonAnimator;
	[HideInInspector]
	Light buttonLight;
	
	[HideInInspector]
	public ParticleSystem particleSystem;
	public ParticleSystem puffSystem;
    
    [HideInInspector]
	public bool playedSound;
	public bool playedPuff = false;

	bool timerRunning = false;
	float lastInteractionTime = 0;
	float activatedLength = 2.0f;
	
	[HideInInspector]
	public Trigger trigger;

	public Renderer[] rend;
	
	// Use this for initialization
	void Start () {
		playedSound 	= false;
	
		buttonAnimator 	= GetComponent<Animator>();
		buttonLight 	= GetComponent<Light>();
		trigger 		= GetComponent<Trigger>();
		particleSystem 			= GetComponent<ParticleSystem>();
		////Was moved here since the sM made it not work!
		//Set the color of the interactable button both background light and particles to the correct user.
		setButtonColor(
			new bool[3]{
				trigger.redPlayerRequired,
				trigger.greenPlayerRequired,
				trigger.bluePlayerRequired
				});
		particleSystem.Play();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(trigger.isTriggered){
			buttonAnimator.SetBool("isActivated", true); 	//starts the animation of the button.
											//starts the particles system.
			PlayPuff();
		} else {
			buttonAnimator.SetBool("isActivated", false); 	//stops the animation of the button.
			if(trigger.isReadyToBeTriggered){
				playedPuff = false;
			}

		}

		if(playedSound && !trigger.isTriggered){
			SoundManager.Instance.ToggleSwitch("On_Off", "Off", gameObject);
			SoundManager.Instance.PlayEvent("ButtonOnOff", gameObject);
			playedSound = false;
		}
	}

    public override void OnRayEnter(int playerIndex){
    	if (trigger.isReadyToBeTriggered){
			if(trigger.playersRequired){
				if(playerIndex == 1 && trigger.bluePlayerRequired ||
                	playerIndex == 2 && trigger.redPlayerRequired ||
                	playerIndex == 3 && trigger.greenPlayerRequired){
					trigger.Activate();
					if(!playedSound){
						SoundManager.Instance.ToggleSwitch("On_Off", "On", gameObject);
						SoundManager.Instance.PlayEvent("ButtonOnOff", gameObject);
						playedSound = true;
					}
				}
			}else{
				trigger.Activate();
				if(!playedSound){
					SoundManager.Instance.ToggleSwitch("On_Off", "On", gameObject);
					SoundManager.Instance.PlayEvent("ButtonOnOff", gameObject);
					playedSound = true;
				}
			}			
		}
    }    

	public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
		OnRayEnter(playerIndex);
	}


    public override void OnRayExit(int playerIndex){
		if (trigger.isReadyToBeTriggered){
			if(trigger.playersRequired){
				if(playerIndex == 1 && trigger.bluePlayerRequired ||
                	playerIndex == 2 && trigger.redPlayerRequired ||
                	playerIndex == 3 && trigger.greenPlayerRequired){
					trigger.Deactivate();
				}
			}else{
				trigger.Deactivate();
			}
		}

    }

	public void setButtonColor(bool[] a){
		//a[0] = red, a[1] = green, a[2] = blue
		if(a[0] && !a[1] && !a[2]){						
		buttonLight.color = Color.red;						
		particleSystem.startColor = Color.red; //Only red player
			puffSystem.startColor = Color.red;
		for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.red;
		}	

		} else if(!a[0] && a[1] && !a[2]){					//Only green player
			buttonLight.color = Color.green;
			particleSystem.startColor = Color.green;
			puffSystem.startColor = Color.green;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.green;

			}

		} else if(!a[0] && !a[1] && a[2]){					//Only blue player
			buttonLight.color = Color.blue;
			particleSystem.startColor = Color.blue;
			puffSystem.startColor = Color.blue;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.blue;
			}

		} else if(a[0] && !a[1] && a[2]){					//Red and blue player
			buttonLight.color = Color.magenta;
			particleSystem.startColor = Color.magenta;
			puffSystem.startColor = Color.magenta;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.magenta;
			}

		} else if(a[0] && a[1] && !a[2]){					//Red and green player
			buttonLight.color = Color.yellow;
			particleSystem.startColor = Color.yellow;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.yellow;
			}

		} else if(!a[0] && a[1] && a[2]){					//Green and blue
			buttonLight.color = Color.cyan;
			particleSystem.startColor = Color.cyan;
			puffSystem.startColor = Color.cyan;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.cyan;
			}
		
		} else if(a[0] && a[1] && a[2]){					//All players
			buttonLight.color = Color.white;
			particleSystem.startColor = Color.white; 
			puffSystem.startColor = Color.white;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.white;
			}       	
		}
	}
	void PlayPuff(){
		if (!playedPuff) {

			particleSystem.Stop();
			puffSystem.loop = false;
			puffSystem.Play();
			playedPuff = true;
		}
	}
}
