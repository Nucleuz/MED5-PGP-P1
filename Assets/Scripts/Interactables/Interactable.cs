using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour{
	public abstract void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit);

	public abstract void OnRayExit();
}