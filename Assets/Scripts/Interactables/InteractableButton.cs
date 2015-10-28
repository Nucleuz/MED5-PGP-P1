using UnityEngine;
using System.Collections;

public class InteractableButton : Interactable{
	Animator buttonAnimator;
	Light buttonLight;
	ParticleSystem par;
	
	//SoundManager sM;
	bool playedSound;
	
	private Trigger trigger;

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
				trigger.bluePlayerRequired,
				trigger.redPlayerRequired,
				trigger.greenPlayerRequired
				});
//		sM 				= GameObject.Find("SoundManager").GetComponent<SoundManager>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(trigger.isTriggered){
			buttonAnimator.SetBool("isActivated", true); 	//starts the animation of the button.
			if(!playedSound){
//				sM.ToggleSwitch("On_Off", "On", gameObject);
//				sM.PlayEvent("ButtonOnOff", gameObject);
				playedSound = true;
			}

			if(!par.isPlaying)
				par.Play(); 								//starts the particles system.
		} else {
			buttonAnimator.SetBool("isActivated", false); 	//stops the animation of the button.
			if(par.isPlaying)
				par.Stop(); 								//stops the particle system.
		}
	}

	public override void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex){
		if (trigger.isReadyToBeTriggered){
			trigger.Activate();
		}
	}

	public void setButtonColor(bool[] a){
		//a[0] = red, a[1] = green, a[2] = blue
		if(a[0] && !a[1] && !a[2]){						
		buttonLight.color = Color.red;						
		par.startColor = Color.red;                         //Only red player
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
