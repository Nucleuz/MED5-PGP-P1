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
	private SoundEmitter soundEmitter;
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

		soundEmitter = GetComponent<SoundEmitter>();
	
		buttonAnimator 	= GetComponent<Animator>();
		buttonLight 	= GetComponent<Light>();
		trigger 		= GetComponent<Trigger>();
		particleSystem 			= GetComponent<ParticleSystem>();
		////Was moved here since the sM made it not work!
		//Set the color of the interactable button both background light and particles to the correct user.
		setButtonColor(
			new bool[3]{
				trigger.bluePlayerRequired,
				trigger.redPlayerRequired,
				trigger.greenPlayerRequired
				});
		//particleSystem.Play();
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
				particleSystem.Stop();
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
						if(soundEmitter != null)
							soundEmitter.Play();

						playedSound = true;
					}
				}
			}else{
				trigger.Activate();
				if(!playedSound){
					SoundManager.Instance.ToggleSwitch("On_Off", "On", gameObject);
					SoundManager.Instance.PlayEvent("ButtonOnOff", gameObject);
					if(soundEmitter != null)
						soundEmitter.Play();

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

	void PlayPuff(){
		if (!playedPuff) {
			particleSystem.Play();
			puffSystem.loop = false;
			puffSystem.Play();
			playedPuff = true;
		}
	}
	public void setButtonColor(bool[] a){

        bool    r = a[1],
                g = a[2],
                b = a[0];


		if(r && !g && !b){						
		buttonLight.color = Color.red;						
		particleSystem.startColor = Color.red; //Only red player
			puffSystem.startColor = Color.red;
		for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.red;
		}	

		} else if(!r && g && !b){					//Only green player
			buttonLight.color = Color.green;
			particleSystem.startColor = Color.green;
			puffSystem.startColor = Color.green;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.green;

			}

		} else if(!r && !g && b){					//Only blue player
			buttonLight.color = Color.blue;
			particleSystem.startColor = Color.blue;
			puffSystem.startColor = Color.blue;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.blue;
			}

		} else if(r && !g && b){					//Red and blue player
			buttonLight.color = Color.magenta;
			particleSystem.startColor = Color.magenta;
			puffSystem.startColor = Color.magenta;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.magenta;
			}

		} else if(r && g && !b){					//Red and green player
			buttonLight.color = Color.yellow;
			particleSystem.startColor = Color.yellow;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.yellow;
			}

		} else if(!r && g && b){					//Green and blue
			buttonLight.color = Color.cyan;
			particleSystem.startColor = Color.cyan;
			puffSystem.startColor = Color.cyan;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.cyan;
			}
		
		} else if(r && g && b){					//All players
			buttonLight.color = Color.white;
			particleSystem.startColor = Color.white; 
			puffSystem.startColor = Color.white;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.white;
			}       	
		}
	}
}
