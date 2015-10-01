using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public Trigger[] events; 				// Events which are dragged into the levelmanager 	- Should be specified in the unity editor!!
	public int[] eventsInSequence; 			//amount of events in each sequence 				- Should be specified in the unity editor!!
	public int[] eventOrder; 				/*The event order in which you want things triggered. if no order leave it at zero!
											should be specified how events should be triggered e.g.
											1 = first event needs to be triggered, then second, then third and so on, which means that only 1 event can be triggered
											2 = first and second can be triggered, then third and fourth
											3 = first, second and third can be triggered, while the rest in the sequence can't until they are finished
											This array was made in order to reset a sequence of events
											these numbers should be specified in the unity editor!! */

}
