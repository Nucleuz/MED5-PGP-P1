﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasFade : MonoBehaviour {

	public Image startWith;

	[Space(5)]

	public Image black;
	public Image white;
	public Image blackTitle;
	public Image whiteTitle;

	public Image lastImage;
	public bool fadedIn = false;

	public static CanvasFade Instance;

	public void Start(){
		Instance = this;
		if(startWith != null){
			lastImage = startWith;
    		startWith.color = new Color(startWith.color.r, startWith.color.g, startWith.color.b, 1);
    		fadedIn = true;
		}
	}

	public void ToBlack(float length){
		StartCoroutine(fadeIn(black,length));
	}
	public void ToWhite(float length){
		StartCoroutine(fadeIn(white,length));
	}
	public void ToBlackTitle(float length){
		StartCoroutine(fadeIn(blackTitle,length));
	}
	public void ToWhiteTitle(float length){
		StartCoroutine(fadeIn(whiteTitle,length));
	}

	public void ToGame(float length){
		if(fadedIn){
			StartCoroutine(fadeOut(length));
			fadedIn = false;
    		lastImage = null;
		}
	}

	IEnumerator fadeIn(Image image, float length){
		if(fadedIn){
			StartCoroutine(fadeOut(length));
		}
		
		lastImage = image;

		float t = 0f;
    	float start = Time.time;

    	Color fromColor = new Color(image.color.r, image.color.g, image.color.b, 0);
    	Color toColor = new Color(image.color.r, image.color.g, image.color.b, 1);

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		
    		image.color = Color.Lerp(fromColor, toColor, t);

    		yield return null;
    	}

    	image.color = toColor;

    	fadedIn = true;
	}
	IEnumerator fadeOut(float length){
		
		Image image = lastImage;

		float t = 0f;
    	float start = Time.time;

    	Color fromColor = new Color(image.color.r, image.color.g, image.color.b, 1);
    	Color toColor = new Color(image.color.r, image.color.g, image.color.b, 0);

    	while(t < 1f){
    		t = (Time.time - start) / length;
    		
    		image.color = Color.Lerp(fromColor, toColor, t);

    		yield return null;
    	}

    	image.color = toColor;
	}
}