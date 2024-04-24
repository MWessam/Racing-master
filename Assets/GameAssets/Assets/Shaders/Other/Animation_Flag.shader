Shader "Animation/Flag" {
	Properties {
		_JumpSpeed ("Jump Speed", Float) = 10
		_JumpAmplitude ("Jump Amplitude", Float) = 0.18
		_JumpFrequency ("Jump Frequency", Float) = 2
		_JumpVerticalOffset ("Jump Vertical Offset", Float) = 0.33
		_TailExtraSwing ("Tail Extra Swing", Float) = 0.15
		_LegsAmplitude ("Legs Amplitude", Float) = 0.1
		_LegsFrequency ("Legs Frequency", Float) = 10
		_LegsPhaseOffset ("Legs Phase Offset", Float) = -1
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
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