Shader "Custom/RadialBlur_2" {
	Properties {
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}
		_CenterX ("Center X", Float) = 0.5
		_CenterY ("Center Y", Float) = 0.5
		[Space(10)] _VignettePower ("Vignette Power", Range(0, 10)) = 5
		_VignetteXScale ("Vignette X Scale", Range(0.1, 2)) = 1
		_VignetteYScale ("Vignette Y Scale", Range(0.1, 2)) = 1
		_VignetteCenter ("Vignette Center", Vector) = (0,0,0,0)
		[Header(Debug)] [Toggle(DEBUG_VIGNETTE)] _DebugVignette ("Debug Vignette", Float) = 0
		[Toggle(DISABLE_BLUR_MASK)] _DisableBlurMask ("Disable Blur Mask", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}