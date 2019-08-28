// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Operation Whalekyrie/Echo"
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

			_NormalDotCameraRayPower("Normal Dot Camera Ray Power", Range(0,10)) = 0.7


		_HBarColor("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)
		_MaxEchoDistance("Max Echo Distance", Range(0, 1000)) = 100
			_EchoFadePower("Echo Fade Power", Range(0,10)) = 1



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
					o.uv = v.uv;
					o.uv_depth = v.uv;

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

				//the depth normals texture
				sampler2D _CameraDepthNormalsTexture;

				float4 _JammerPositions[100];
				float _JammerJammingDistances[100];
				float _JammerActive[100];

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

				float _NormalDotCameraRayPower;

				float _MaxEchoDistance;
				float _EchoFadePower;





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
					float linearDepth;
					float3 normal;
					DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv_depth), linearDepth, normal);
					float fragmentNormalDotCameraRay = 1 - dot(normal, normalize(i.interpolatedRay));


					//float linearDepth = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.uv_depth).zw);
					//float linearDepth = Linear01Depth(rawDepth);
					float4 wsDir = linearDepth * i.interpolatedRay;
					float3 fragmentWorldPosition = _WorldSpaceCameraPos + wsDir;
					half4 scannerCol = half4(0, 0, 0, 0);



					//if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)
					//{
					//	float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
					//	half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));
					//	scannerCol = lerp(_TrailColor, edge, diff) + horizBars(i.uv) * _HBarColor;
					//	scannerCol *= diff;
					//}

					for (int i = 0; i < _EchoDistances.Length; i++) {

						float echoDistance = _EchoDistances[i];

						if (echoDistance == 0) {
							continue;
						}

						if (linearDepth == 1) {
							continue;
						}

						float fragmentDistanceToOrigin = distance(fragmentWorldPosition, _EchoOrigins[i]);

						if (fragmentDistanceToOrigin > _MaxEchoDistance) {
							continue;
						}


						bool echoStartHasReachedBeyondFragment = echoDistance > fragmentDistanceToOrigin;
						bool echoEndHasNotReachedBeyondFragment = fragmentDistanceToOrigin > echoDistance - _ScanWidth;


						if (echoStartHasReachedBeyondFragment && echoEndHasNotReachedBeyondFragment)
						{

							if (_EchoTypes[i] == 0) {
								
								float fragmentDistanceToEcho = 1 - (echoDistance - fragmentDistanceToOrigin) / (_ScanWidth);
								half4 edge = lerp(_MidColor, _LeadColor, pow(fragmentDistanceToEcho, _LeadSharp));
								scannerCol = lerp(_TrailColor, edge, fragmentDistanceToEcho) * pow(fragmentNormalDotCameraRay, _NormalDotCameraRayPower) /* */   /*+ horizBars(i.uv) */;

								float scannerDistance01 = ((echoDistance - fragmentDistanceToOrigin) / _ScanWidth);
								float scannerDistance10 = 1 - scannerDistance01;
								float scannerDistance10Powered = pow(scannerDistance10, _EchoFadePower);
								float scannerDistance10Clamped = clamp(scannerDistance10Powered, 0, 1);

								scannerCol *= scannerDistance10Clamped;// pow(clamp(scannerDistance10, 0, 1), _EchoFadePower);


								




								//scannerCol *= diff;
								//scannerCol /= pow(dist, 0.2) ;

								//scannerCol *= (1 - (linearDepth * 5));
								//scannerCol *= (1 - (linearDepth * 8));
								//scannerCol /= ((1 - ndotd) * dist); //* 0.04); 

							}
							
 

							for (int j = 0; j < _JammerPositions.Length; j++) {
								float4 jammerPosition = _JammerPositions[j];
								float jammerActive = _JammerActive[j];

								if (jammerActive == 1) {

									float jammerJammingDistance = _JammerJammingDistances[j];

									float fragmentDistanceToJammer = distance(float2(fragmentWorldPosition.x, fragmentWorldPosition.y), float2(jammerPosition.x, jammerPosition.y));
									
									
									if (fragmentDistanceToJammer < jammerJammingDistance) {
										float3 fromJammerToFragmentVector = fragmentWorldPosition - jammerPosition;
										float fromJammerToFragmentAngle = (atan2(fromJammerToFragmentVector.x, fromJammerToFragmentVector.y) + 3.14159);
										float fromJammerToFragmentAngleAbsedFraced = frac(fromJammerToFragmentAngle / (3.14159 / 10) + frac(fragmentDistanceToJammer / 3 + _Time.z));
										float fromJammerToFragmentAngleWave = abs((fromJammerToFragmentAngleAbsedFraced - 0.5));

										//float fromJammerToFragmentAngle0TwoPi = fromJammerToFragmentAngle + 3.14159;
										//float fromJammerToFragmentAngleFraced = frac(fromJammerToFragmentAngle0TwoPi / (3.14159/10) + _Time.z);
										//float fromJammerToFragmentAngleFracedByTwoPi =   fromJammerToFragmentAngle ;
										//fragmentDistanceToJammer -= fromJammerToFragmentAngleFracedByTwoPi;

										if ((fragmentDistanceToJammer + pow(fromJammerToFragmentAngleWave * 4, 4)) < jammerJammingDistance) {
											float fragmentDistanceToJammer01 = fragmentDistanceToJammer / jammerJammingDistance;
											float fragmentDistanceToJammer01Powered = pow(fragmentDistanceToJammer01, fromJammerToFragmentAngleWave * 2 + 25);
											scannerCol = float4(fragmentDistanceToJammer01Powered, 0, 0, 1);
										}
									}
								}
							}

							if (_EchoTypes[i] == 2) {

								if (fragmentDistanceToOrigin > 1) {
									//continue;
								}

								float diff = 1 - (echoDistance - fragmentDistanceToOrigin) / (_HackEchoWidth);
								half4 edge = lerp(_HackMidColor, _HackLeadColor, pow(diff, _HackLeadSharp));
								scannerCol = lerp(_HackTrailColor, edge, diff) /*+ horizBars(i.uv) */;
								scannerCol *= diff;

								//scannerCol *= (1 - (linearDepth * 5));
								//scannerCol *= (1 - (linearDepth * 8));
								scannerCol /= (fragmentDistanceToOrigin * 0.5);

							}

							  if (_EchoTypes[i] == 1) {

								float diff = 1 - (echoDistance - fragmentDistanceToOrigin) / (_JammedScanWidth);
								half4 edge = lerp(_JammedMidColor, _JammedLeadColor, pow(diff, _JammedLeadSharp));
								scannerCol = lerp(_JammedTrailColor, edge, diff) /*+ horizBars(i.uv) */;
								scannerCol *= diff;

								//scannerCol *= (1 - (linearDepth * 5));
								//scannerCol *= (1 - (linearDepth * 8));
								scannerCol /= (fragmentDistanceToOrigin * 0.5);

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