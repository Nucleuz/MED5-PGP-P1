using UnityEngine;
using System.Collections;

public class RenderSettingsTriggerZone : MonoBehaviour {
	public bool fadeDensity;
	[Space(-2)]
	public float fadeDensityTo;
	[Space(10)]
	public bool fadeColor;
	[Space(-2)]
	public Color fadeColorTo;
	[Space(10)]
	public bool fadeEquator;
	[Space(-2)]
	public Color fadeEquatorTo;
	[Space(10)]
	public bool fadeGround;
	[Space(-2)]
	public Color fadeGroundTo;
	[Space(10)]
	public bool fadeSky;
	[Space(-2)]
	public Color fadeSkyTo;
	[Space(10)]
	public bool fadeAmbient;
	[Space(-2)]
	public float fadeAmbientTo;
	[Space(10)]
	public float fadeLength;


	void OnTriggerEnter(Collider col){
		if(fadeDensity)
			RenderSettingsFader.Instance.FogDensityTo(fadeDensityTo, fadeLength);
		if(fadeColor)
			RenderSettingsFader.Instance.FogColorTo(fadeColorTo, fadeLength);
		if(fadeEquator)
			RenderSettingsFader.Instance.EquatorColorTo(fadeEquatorTo, fadeLength);
		if(fadeGround)
			RenderSettingsFader.Instance.GroundColorTo(fadeGroundTo, fadeLength);
		if(fadeSky)
			RenderSettingsFader.Instance.SkyColorTo(fadeSkyTo, fadeLength);
		if(fadeAmbient)
			RenderSettingsFader.Instance.AmbientTo(fadeAmbientTo, fadeLength);

        Destroy(gameObject);
    }
}