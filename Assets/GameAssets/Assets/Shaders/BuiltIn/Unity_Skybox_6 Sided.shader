Shader "Unity/Skybox/6 Sided" {
	Properties {
		_Tint ("Tint Color", Vector) = (0.5,0.5,0.5,0.5)
		[Gamma] _Exposure ("Exposure", Range(0, 8)) = 1
		_Rotation ("Rotation", Range(0, 360)) = 0
		[NoScaleOffset] _FrontTex ("Front [+Z]   (HDR)", 2D) = "grey" {}
		[NoScaleOffset] _BackTex ("Back [-Z]   (HDR)", 2D) = "grey" {}
		[NoScaleOffset] _LeftTex ("Left [+X]   (HDR)", 2D) = "grey" {}
		[NoScaleOffset] _RightTex ("Right [-X]   (HDR)", 2D) = "grey" {}
		[NoScaleOffset] _UpTex ("Up [+Y]   (HDR)", 2D) = "grey" {}
		[NoScaleOffset] _DownTex ("Down [-Y]   (HDR)", 2D) = "grey" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}