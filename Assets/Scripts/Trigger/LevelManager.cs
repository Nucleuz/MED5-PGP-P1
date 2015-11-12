using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	[Tooltip("Interactable gameobjects in the game, should be dragged on in the right order for the eventOrder to work. Because it will be used in eventOrder!!")]
	public Trigger[] events; 				// Events which are dragged into the levelmanager 	- Should be specified in the unity editor!!

	[Tooltip("Bool used to detect if se is finished. Size must be the same size as 'events'")]
	public bool[] triggeredEvents;

	[Tooltip("This contains how many of the gameobjects there is in a specific sequence. You must specifiy the corresponding event size to the number of gameobjects. e.g if you have 6 elements, you must make sure that the total number is also 6 in this array. could be written as the element0 is 2 and element1 is 4. Element0 is the first sequence. Array size is the amount of sequences")]
	public int[] eventsInSequence; 			//amount of events in each sequence 				- Should be specified in the unity editor!!

	[Tooltip("This contains the event order in which you want gameobjects triggered in a sequence. The number specifies the amount of gameobjects that should be triggered, 0 = no order, 1 = first gameobject in the sequence needs to be triggered before the rest, then second, and then third, 2 = first and second can be triggered, then third and fourth. Array size must be the same as amount of sequences")]
	public int[] eventOrder; 				/*The event order in which you want things triggered. if no order leave it at zero!
											should be specified how events should be triggered e.g.
											1 = first event needs to be triggered, then second, then third and so on, which means that only 1 event can be triggered
											2 = first and second can be triggered, then third and fourth
											3 = first, second and third can be triggered, while the rest in the sequence can't until they are finished
											This array was made in order to reset a sequence of events
											these numbers should be specified in the unity editor!! */
	[Tooltip("Array size MUST be the same as amounts of sequences. One object can be triggered after a sequence, drag element into the corresponding array place. e.g if a door wants to be triggered after sequence 3, the door must be dragged onto element2 in the array. Leave other elements in the array empty")]
	public Trigger[] triggerEvents;			//Used to trigger an object when a sequence is finished

	[Tooltip("Needs tooltip")]
	public ReceiveSequenceFail sequenceFail;
	

	[Header("Defining Level start and end")]

	[Tooltip("The 3 rail points where the level starts, Organized Blue, Red, Green")]
	public Rail[] levelStartRail = new Rail[3]; 

	[Tooltip("The 3 rail points where the level end, Organized Blue, Red, Green")]
	public Rail[] levelEndRail   = new Rail[3];

	/*
	[Tooltip("Direction vector pointing to the next level")]
    public Vector3 nextLevelDirection;
	[Tooltip("Direction vector pointing to the previous level")]
    public Vector3 prevLevelDirection;
	*/
}
