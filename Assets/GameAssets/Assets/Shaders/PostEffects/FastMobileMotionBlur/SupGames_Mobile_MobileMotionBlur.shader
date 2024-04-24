Shader "SupGames/Mobile/MobileMotionBlur" {
	Properties {
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}
		[Header(Debug)] [Toggle(DEBUG_VIGNETTE)] _DebugVignette ("Debug Vignette", Float) = 0
		[Toggle(DEBUG_UV)] _DebugUV ("Debug UV", Float) = 0
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