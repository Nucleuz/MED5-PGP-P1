using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerDebugger : MonoBehaviour {

	public bool activated = false;

	public TriggerHandler triggerHandler;
	

	public GameObject triggerPrefab;
	public GameObject triggerContainer;

	public KeyCode keyToOpen;					// Which key to press to open Debugger

	private List<TriggerDebugItem> triggers = new List<TriggerDebugItem>(0);
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKeyDown(keyToOpen)){
			activated = !activated;


			if(triggerHandler.triggers.Length > 0 && activated){
				for(int i = 0; i<triggerHandler.triggers.Length;i++){
					Vector3 pos = new Vector3(i * 25,64,0);
					GameObject g = Instantiate(triggerPrefab,pos,Quaternion.identity) as GameObject;
					g.transform.SetParent(triggerContainer.transform);

					TriggerDebugItem script = g.GetComponent<TriggerDebugItem>();
					
					Trigger trigger = triggerHandler.triggers[i];
					script.id.text = trigger.triggerID.ToString();
					script.triggered.color = (trigger.isTriggered ? Color.green : Color.red);
					script.ready.color = (trigger.isReadyToBeTriggered ? Color.green : Color.red);
					script.reset.color = (trigger.canReset ? Color.green : Color.red);
					triggers.Add(script);
				}


			}else{
				for(int i = 0; i<triggers.Count;i++){
					Destroy(triggers[i].gameObject);
				}

				triggers.Clear();

			}

			triggerContainer.SetActive(activated);
		}

		if(activated){
			for(int i = 0; i<triggerHandler.triggers.Length;i++){
				Trigger trigger = triggerHandler.triggers[i];
				triggers[i].id.text = trigger.triggerID.ToString();
				triggers[i].triggered.color = (trigger.isTriggered ? Color.green : Color.red);
				triggers[i].ready.color = (trigger.isReadyToBeTriggered ? Color.green : Color.red);
				triggers[i].reset.color = (trigger.canReset ? Color.green : Color.red);
			}
		}
	}
}
