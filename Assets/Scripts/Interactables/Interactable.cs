using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour{
	public abstract void OnRayEnter(int playerIndex, Ray ray, RaycastHit hit, ref LineRenderer lineRenderer,int nextLineVertex);

	public abstract void OnRayExit();
}