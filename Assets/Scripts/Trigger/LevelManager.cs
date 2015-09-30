using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public Trigger[] events; // Events which are dragged into the levelmanager - Should be specified in the unity editor!!
	public int[] eventOrder; //The event order in which you want things triggered - Should be specified in the unity editor!!
}
