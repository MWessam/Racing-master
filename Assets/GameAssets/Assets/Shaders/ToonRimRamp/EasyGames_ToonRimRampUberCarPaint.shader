Shader "EasyGames/ToonRimRampUberCarPaint" {
	Properties {
		_BaseColor ("Base Color", Vector) = (1,1,1,1)
		_BlurPower ("Blur Color", Float) = 0
		_MainTex ("Texture (RGB) Trans (A)", 2D) = "white" {}
		_TexColor ("Texture Color", Vector) = (1,1,1,1)
		_RampTex ("Ramp", 2D) = "white" {}
		_RampIntensity ("Ramp Intensity", Range(0, 1)) = 1
		_NormTex ("Normal Map", 2D) = "black" {}
		_NormIntensity ("Normal Intensity", Range(-1, 1)) = 1
		_SpecColor ("Spec Color", Vector) = (1,1,1,1)
		_SpecPower ("Spec Power", Float) = 2
		_SpecIntensity ("Spec Intensity", Range(0, 10)) = 1
		_RimPower ("Rim Power", Range(0, 3)) = 1
		_RimColor ("Rim Color", Vector) = (1,1,1,1)
		_Roughness ("Roughness", Range(0, 2)) = 0.2
		_ReflectPower ("Reflect Power", Range(0.1, 2)) = 0.2
		[PerRendererData] _RaceTrackShadowMapU ("_RaceTrackShadowMapU", Float) = 0
		[PerRendererData] _ReflectionRotationMatrixRow1 ("_ReflectionRotationMatrixRow1", Vector) = (0,0,0,0)
		[PerRendererData] _ReflectionRotationMatrixRow2 ("_ReflectionRotationMatrixRow1", Vector) = (0,0,0,0)
		[PerRendererData] _ReflectionRotationMatrixRow3 ("_ReflectionRotationMatrixRow1", Vector) = (0,0,0,0)
		[PerRendererData] _ReflectionRotationMatrixRow4 ("_ReflectionRotationMatrixRow1", Vector) = (0,0,0,0)
		[KeywordEnum(Off, Front, Back)] _Cull ("Cull", Float) = 2
		[HideInInspector] _SrcBlend ("_src", Float) = 1
		[HideInInspector] _DstBlend ("_dst", Float) = 0
		[HideInInspector] _ZWrite ("_zWrite", Float) = 0
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
	Fallback "Unity/Mobile/VertexLit"
	//CustomEditor "UberCarPaintGUI"
}