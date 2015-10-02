using UnityEngine;
using System.Collections;

//Make sure there is a renderer on object
[RequireComponent (typeof(Renderer))]
public class TextureScroll : MonoBehaviour {

	private Renderer rend;					//Reference for renderer on object
	public float scrollSpeed = 0.5f; 		//Speed scrolling scalar

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		//Set offset of texture to be a product of time and scrollSpeed scalar.
		float offset = scrollSpeed*Time.time;

		//Offset texture using offset variable
		rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset)); 		
	}
}
