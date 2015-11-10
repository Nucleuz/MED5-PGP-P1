Shader "ColoredAlphaTexture"
{
	Properties
	{
		_Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Lighting Off
			SetTexture [_MainTex] {
				constantColor[_Color]

				combine constant * texture
			}

		}
	}
}
