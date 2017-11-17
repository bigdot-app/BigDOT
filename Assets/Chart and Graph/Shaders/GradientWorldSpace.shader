// this shader created a basic gradient between an initial color to an
Shader "Chart/WorldSpace/Gradient"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Angle("Angle", Float) = 0
		_Combine("Combine", Color) = (1,1,1,0)
		_ColorFrom("From", Color) = (1,1,1,1)
		_ColorTo("To", Color) = (1,1,1,1)
		_Ramp("Illumination Ramp (RGB)", 2D) = "gray" {}
		_Outline("Outline",Float) = 4
		_ChartTiling("Tiling",Float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Cull Off
		LOD 100
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma surface surf ToonRamp vertex:lerpVertex

			fixed _Angle;
			fixed _ChartTiling;
			fixed4 _Combine;
			fixed4 _ColorFrom;
			fixed4 _ColorTo;
			fixed4 _OutlineColor;
			fixed _Outline;

			sampler2D _MainTex;
			sampler2D _Ramp;

			#pragma lighting ToonRamp exclude_path:prepass
			inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
			{
				#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
				#endif

				half d = dot(s.Normal, lightDir)*0.5 + 0.5;
				half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * ramp;// *(atten * 2);
				c.a = 0;
				return c;
			}

			struct Input
			{
				float2 uv_MainTex : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 worldpos : TEXCOORD1;
				fixed4 color : COLOR;
			};

			void lerpVertex(inout appdata_full v)
			{
				fixed angle = _Angle * 3.14159 * 2 / 360.0;
				fixed lerpValue = (v.texcoord.x / _ChartTiling - 0.5) * sin(angle) + (v.texcoord.y - 0.5) * cos(angle);
				lerpValue = lerpValue + 0.5;
				v.color = lerp(_ColorFrom,_ColorTo, lerpValue);
				lerpValue = _Combine.a;
				fixed alpha = v.color.a;
				v.color = lerp(v.color, _Combine, lerpValue);
				v.color.a = alpha;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 texData = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				o.Albedo = texData.rgb;
				o.Alpha = texData.a;
			}
		ENDCG
	}
}