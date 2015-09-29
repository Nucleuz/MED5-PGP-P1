using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour{
	public abstract void OnRayReceived(int playerIndex, Ray ray, RaycastHit hit);
}