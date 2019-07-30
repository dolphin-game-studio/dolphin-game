// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// 27.07 11:30 - 15:?? 
// 28.07 1:50 - 3:12


// 30.07.2019 19:00 - 19:12 Bubble doesn't distract shark on collision

// 30.07.2019 19:12 - 20:06 Bug: Orca Ram distracted shark / Orca was able to move when ramming

// 30.07.2019 20:06 - 20:18 Jammer finalisiert

// 30.07.2019 20:18 - 20:27 Patrol bug fixxed


Shader "Hidden/ScannerEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DetailTex("Texture", 2D) = "white" {}
 		_ScanWidth("Scan Width", float) = 10
		_LeadSharp("Leading Edge Sharpness", float) = 10
		_LeadColor("Leading Edge Color", Color) = (1, 1, 1, 0)
		_MidColor("Mid Color", Color) = (1, 1, 1, 0)
		_TrailColor("Trail Color", Color) = (1, 1, 1, 0)
					
		_JammedScanWidth("Jammed Scan Width", float) = 1
		_JammedLeadSharp("Jammed Leading Edge Sharpness", float) = 10
		_JammedLeadColor("Jammed Leading Edge Color", Color) = (1, 0, 0, 0)
		_JammedMidColor("Jammed Mid Color", Color) = (1, 0, 0, 0)
		_JammedTrailColor("Jammed Trail Color", Color) = (1, 0, 0, 0)

		_HackEchoWidth("Hack Echo Width", float) = 1
		_HackLeadSharp("Hack Leading Edge Sharpness", float) = 10
		_HackLeadColor("Hack Leading Edge Color", Color) = (0, 1, 0, 0)
		_HackMidColor("Hack Mid Color", Color) = (0, 1, 0, 0)
		_HackTrailColor("Hack Trail Color", Color) = (0, 1, 0, 0)

		_HBarColor("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
 
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct VertIn
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float4 ray : TEXCOORD1;
				};

				struct VertOut
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uv_depth : TEXCOORD1;
					float4 interpolatedRay : TEXCOORD2;
				};

				float4 _MainTex_TexelSize;
				float4 _CameraWS;

				VertOut vert(VertIn v)
				{
					VertOut o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv.xy;
					o.uv_depth = v.uv.xy;

					#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv.y = 1 - o.uv.y;
					#endif				

					o.interpolatedRay = v.ray;

					return o;
				}

				sampler2D _MainTex;
				sampler2D _DetailTex;
				sampler2D_float _CameraDepthTexture;
				float4 _WorldSpaceScannerPos;
 
				float _EchoDistances[100];
				float _EchoTypes[100];
				float4 _EchoOrigins[100];

				float _ScanWidth;
				float _LeadSharp;
				float4 _LeadColor;
				float4 _MidColor;
				float4 _TrailColor;
				float4 _HBarColor;

				float _JammedScanWidth;
 				float _JammedLeadSharp;
				float4 _JammedLeadColor;
				float4 _JammedMidColor;
				float4 _JammedTrailColor; 

				float _HackEchoWidth;
 				float _HackLeadSharp;
				float4 _HackLeadColor;
				float4 _HackMidColor;
				float4 _HackTrailColor; 

				float4 horizBars(float2 p)
				{
					return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
				}

				float4 horizTex(float2 p)
				{
					return tex2D(_DetailTex, float2(p.x * 30, p.y * 40));
				}

				half4 frag(VertOut i) : SV_Target
				{
					half4 col = tex2D(_MainTex, i.uv);

					float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
					float linearDepth = Linear01Depth(rawDepth);
					float4 wsDir = linearDepth * i.interpolatedRay;
					float3 wsPos = _WorldSpaceCameraPos + wsDir;
					half4 scannerCol = half4(0, 0, 0, 0);

					

					//if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)
					//{
					//	float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
					//	half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));
					//	scannerCol = lerp(_TrailColor, edge, diff) + horizBars(i.uv) * _HBarColor;
					//	scannerCol *= diff;
					//}

					for (int i = 0; i < _EchoDistances.Length; i++) {

						float dist = distance(wsPos, _EchoOrigins[i]);

						if (dist < _EchoDistances[i] && dist > _EchoDistances[i] - _ScanWidth && linearDepth < 1)
						{

							if (_EchoTypes[i] == 0) {
  
								float diff = 1 - (_EchoDistances[i] - dist) / (_ScanWidth);
								half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));
								scannerCol = lerp(_TrailColor, edge, diff) /*+ horizBars(i.uv) */;
								scannerCol *= diff;

								//scannerCol *= (1 - (linearDepth * 5));
								//scannerCol *= (1 - (linearDepth * 8));
								scannerCol /= (dist * 0.04);

							}
							else if(_EchoTypes[i] == 1) {

								float diff = 1 - (_EchoDistances[i] - dist) / (_JammedScanWidth);
								half4 edge = lerp(_JammedMidColor, _JammedLeadColor, pow(diff, _JammedLeadSharp));
								scannerCol = lerp(_JammedTrailColor, edge, diff) /*+ horizBars(i.uv) */;
								scannerCol *= diff;

								//scannerCol *= (1 - (linearDepth * 5));
								//scannerCol *= (1 - (linearDepth * 8));
								scannerCol /= (dist * 0.5);

							} else if (_EchoTypes[i] == 2) { 

								float diff = 1 - (_EchoDistances[i] - dist) / (_HackEchoWidth);
								half4 edge = lerp(_HackMidColor, _HackLeadColor, pow(diff, _HackLeadSharp));
								scannerCol = lerp(_HackTrailColor, edge, diff) /*+ horizBars(i.uv) */;
								scannerCol *= diff;

								//scannerCol *= (1 - (linearDepth * 5));
								//scannerCol *= (1 - (linearDepth * 8));
								scannerCol /= (dist * 0.5);

							}



							
							// scannerCol *= ((_ScanDistances[i]));

							col += scannerCol;
						}
					}


					return col;
				}
				ENDCG
			}
		}
}