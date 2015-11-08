using UnityEngine;
using System.Collections;

public class ScrollingUVs_Layers : MonoBehaviour 
{
	//public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );
	public Vector2 uvDistortAnimationRate = new Vector2( 1.0f, 0.0f );
	public string textureName = "_MainTex";
	public string textureName1 = "_Distort";
	
	Vector2 uvOffset = Vector2.zero;
	Vector2 uvOffset1 = Vector2.zero;

	
	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		uvOffset1 += ( uvDistortAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset( textureName, uvOffset );
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset( textureName1, uvOffset1 );
		}
	}
}