using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	[HideInInspector]
	Animator buttonAnimator;
	[HideInInspector]
	Light buttonLight;
	
	[HideInInspector]
	public ParticleSystem par;
	public ParticleSystem placeHolder; //needs better name!? anyone!
    
    [HideInInspector]
	public bool playedSound;
	public bool particlesReplaced = false;
	
	[HideInInspector]
	public Trigger trigger;

	public Renderer[] rend;
	
	// Use this for initialization
	void Start () {
		playedSound 	= false;
	
		buttonAnimator 	= GetComponent<Animator>();
		buttonLight 	= GetComponent<Light>();
		trigger 		= GetComponent<Trigger>();
		par 			= GetComponent<ParticleSystem>();
		////Was moved here since the sM made it not work!
		//Set the color of the interactable button both background light and particles to the correct user.
		setButtonColor(
			new bool[3]{
				trigger.redPlayerRequired,
				trigger.greenPlayerRequired,
				trigger.bluePlayerRequired
				});
		par.Play();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(trigger.isTriggered){
			buttonAnimator.SetBool("isActivated", true); 	//starts the animation of the button.
											//starts the particles system.
			ReplaceParticles();
		} else {
			buttonAnimator.SetBool("isActivated", false); 	//stops the animation of the button.

		}

		if(playedSound && !trigger.isTriggered){
			SoundManager.Instance.ToggleSwitch("On_Off", "Off", gameObject);
			SoundManager.Instance.PlayEvent("ButtonOnOff", gameObject);
			playedSound = false;
		}
	}

	public override void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit){
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
		par.startColor = Color.red; //Only red player
			placeHolder.startColor = Color.red;
		for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.red;
		}	

		} else if(!a[0] && a[1] && !a[2]){					//Only green player
			buttonLight.color = Color.green;
			par.startColor = Color.green;
			placeHolder.startColor = Color.green;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.green;

			}

		} else if(!a[0] && !a[1] && a[2]){					//Only blue player
			buttonLight.color = Color.blue;
			par.startColor = Color.blue;
			placeHolder.startColor = Color.blue;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.blue;
			}

		} else if(a[0] && !a[1] && a[2]){					//Red and blue player
			buttonLight.color = Color.magenta;
			par.startColor = Color.magenta;
			placeHolder.startColor = Color.magenta;
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
			placeHolder.startColor = Color.cyan;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.cyan;
			}
		
		} else if(a[0] && a[1] && a[2]){					//All players
			buttonLight.color = Color.white;
			par.startColor = Color.white; 
			placeHolder.startColor = Color.white;
			for(int i = 0; i < rend.Length; i++){
				rend[i].material.color = Color.white;
			}       	
		}
	}
	void ReplaceParticles(){
		if (!particlesReplaced) {

			par.Stop();
			placeHolder.loop = false;
			placeHolder.Play();
			particlesReplaced = true;
		}
	}
}
