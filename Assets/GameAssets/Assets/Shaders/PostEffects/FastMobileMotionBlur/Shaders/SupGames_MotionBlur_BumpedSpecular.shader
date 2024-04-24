Shader "SupGames/MotionBlur/BumpedSpecular" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_BumpTex ("Normal Map", 2D) = "bump" {}
		_SpecColor ("Specular Color", Vector) = (1,1,1,1)
		_Glossiness ("Glossiness", Range(0.01, 100)) = 0.03
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}