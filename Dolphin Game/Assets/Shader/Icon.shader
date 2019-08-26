Shader "Unlit/Icon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

		_Range1("Range1", Range(0,10)) = 0
		_Range2("Range2", Range(0,10)) = 0

		_RangeX("RangeX", Range(0,10)) = 0
		_RangeY("RangeY", Range(0,10)) = 0

		_GreyScaleRed("Grey Scale Red", Range(0,1)) = 0
		_GreyScaleGreen("Grey Scale Green", Range(0,1)) = 0
		_GreyScaleBlue("Grey Scale Blue", Range(0,1)) = 0

		_GreyScaleMultiplier("Grey Scale Multiplier", Range(0,10)) = 0


					_CooldownDarknes("Cooldown Darknes", Range(0,10)) = 0

			_Filled("Filled", Range(0,1)) = 0

						_LowerEnd("LowerEnd", Range(1.125,2.125)) = 1.125
 
 
			_EndeSharpness("Ende Sharpness" , Range(0,1000)) = 0

						_AbilityUsable("Ability usable" , Range(0,1)) = 0
												_EdgeGlowColor("Edge Glow Color" , Color) = (1,1,1,1)
															_UnusableEdgeGlowColor("Unusable Edge Glow Color" , Color) = (1,0,0,1)

									_EdgeGlow("Edge Glow" , Range(0,1)) = 0
												_EdgeGlowOffset("Edge Glow Offset" , Range(0,0.5)) = 0
															_EdgeGlowMultiplier("Edge Glow Multiplier" , Range(1,10)) = 0
			_StarBrightness("Star Brightness", Range(0,1))=0




			_VX("_VX", Range(0,100)) = 9.
			_VY("_VY", Range(0, 1)) = 0.32
			_DX("_DX", Range(0, 100)) = 13.
			_DY("_DY", Range(0, 1)) = 0.61
			_VHI("_VHI", Range(0, 1)) = 0.25
			_DI("_DI", Range(0, 1)) = 0.19
			_PintPow("_PintPow", Range(0, 10)) = 2.3
			_PintDevider("_PintDevider", Range(0, 100000)) = 80.
			_PintSubstractor("_PintSubstractor", Range(0, 1)) = 0.1
			_DistMultiplier("_DistMultiplier", Range(0, 10)) = 7
			_BranchDistMultiplier("_BranchDistMultiplier", Range(0, 1)) = 1

	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
 
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			 

			float _GreyScaleRed;
			float _GreyScaleGreen;
			float _GreyScaleBlue;

			float _GreyScaleMultiplier;
			float _Filled;

 			float _LowerEnd = 0.125;
 

			float _EndeSharpness;

			float _AbilityUsable;
			float _CooldownDarknes;
			float _EdgeGlow;

			float4 _EdgeGlowColor;
			float4 _UnusableEdgeGlowColor;

			
				float _EdgeGlowOffset;
				float _EdgeGlowMultiplier;
				float _StarBrightness;

				

				float _VX = 9.;
				float _VY = 0.32;

				float _DX = 13.;
				float _DY = 0.61;

				float _VHI = 0.25;
				float _DI = 0.19;

				float _PintPow = 2.3;
				float _PintDevider = 80.;
				float _PintSubstractor = 0.1;

				float _DistMultiplier = 7;
				float _BranchDistMultiplier = 1;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {


				 

                 fixed4 col = tex2D(_MainTex, i.uv);
			float greyscale = _GreyScaleRed * col.r + _GreyScaleGreen * col.g + _GreyScaleBlue * col.b;

			float4 greyscaleColor = float4(greyscale, greyscale, greyscale, 1);
			 
			float renderCooldown = _Filled > 0 && _Filled < 1;
			float dontRenderCooldown = _Filled == 0 || _Filled == 1;


			float lowerEnd = frac(atan2(i.uv.y - 0.5, i.uv.x - 0.5) / 6.2832 + _LowerEnd);

			float upperEnd = frac(atan2(i.uv.y - 0.5, i.uv.x - 0.5) / 6.2832 + _Filled + _LowerEnd);

			float betweenLowerAndUpperEnd = lowerEnd < upperEnd;
			float notBetweenLowerAndUpperEnd = 1 - betweenLowerAndUpperEnd;

			float4 saturatedOrDesaturatedColor = greyscaleColor * (1 - _AbilityUsable) + col * _AbilityUsable;
 
			float4 darkerColor = saturatedOrDesaturatedColor * _CooldownDarknes;
			
 
			float3 fadedOutColor = saturatedOrDesaturatedColor * pow(upperEnd, _EndeSharpness) + darkerColor * (1-pow(lowerEnd, _EndeSharpness));

			float3 fullColor = (fadedOutColor * betweenLowerAndUpperEnd + saturatedOrDesaturatedColor * notBetweenLowerAndUpperEnd) * renderCooldown + saturatedOrDesaturatedColor * dontRenderCooldown;

  
 
 			 
			float maxDistanceToCenter = clamp( max(max(abs(i.uv.y - 0.5), abs(i.uv.y - 0.5)), max(abs(i.uv.x - 0.5), abs(i.uv.x - 0.5))) - _EdgeGlowOffset, 0, 1) *  _EdgeGlowMultiplier;
			float4 edgeGlow = maxDistanceToCenter * _EdgeGlow;
 
			float3 colorAfterEdge = fullColor * (1 - edgeGlow) + (_UnusableEdgeGlowColor * (1-_AbilityUsable)) * edgeGlow +  (_EdgeGlowColor * _AbilityUsable) * edgeGlow;
 

			//col.rgb = fullColor * (1 - edgeGlow) + _EdgeGlowColor * edgeGlow;
			//float4 star = (pow(frac(atan2(i.uv.y - 0.5, i.uv.x - 0.5) / 1.570795 + _Time.y), 3)
			//	+ pow(frac(atan2(i.uv.x - 0.5, i.uv.y - 0.5) / 1.570795 - _Time.y), 3)
			//	) * _StarBrightness;
			//	;
			//col.rgb = fullColor * (1 - star) + star;

			//float pintPow = frac(_Time.w / 6) * 6;
			//float minusPint = pintPow < 3;


			//_PintPow = (frac(_Time.w / 3) * 3) * minusPint + (3 - (frac(_Time.w / 3) * 3)) * (1 - minusPint);
 
			 
			//float distMultiplier = frac(_Time.w / 6) * 6;
			//float minusDistMultiplier = pintPow < 3;
			//_DistMultiplier =  ((frac(_Time.w / 3) * 3) * minusPint + (3 - (frac(_Time.w / 3) * 3)) * (1 - minusPint));

 
				// Main particle
			float2 ppos = float2(0.5, 0.5);
			float dist = distance(i.uv, ppos);

			float dx1 = cos(_Time.y + (3.14159 / 4) * 1);
			float dy1 = sin(_Time.y + (3.14159 / 4) * 1);

			float dx2 = cos(_Time.y + (3.14159 / 4) * 2);
			float dy2 = sin(_Time.y + (3.14159 / 4) * 2);

			float dx3 = cos(_Time.y + (3.14159 / 4) * 3);
			float dy3 = sin(_Time.y + (3.14159 / 4) * 3);

			float dx4 = cos(_Time.y + (3.14159 / 4) * 4);
			float dy4 = sin(_Time.y + (3.14159 / 4) * 4);



 

			// Draws the eight-branched star
			// Horizontal and vertical branches
			float2 uvppos = i.uv - ppos;
			float2 uvpposd2 =  float2(dot(uvppos, normalize(float2(dx3, dy3))), dot(uvppos, normalize(float2(dx4, dy4))));
			float distv = distance(uvpposd2*float2(_VX, _VY).xy + ppos, ppos);
			float disth = distance(uvpposd2*float2(_VX, _VY).yx + ppos, ppos);
			// Diagonal branches
			float2 uvpposd =  float2(dot(uvppos, normalize( float2(dx1, dy1))), dot(uvppos, normalize(float2(dx2, dy2))));
			float distd1 = distance(uvpposd*float2(_DX, _DY).xy + ppos, ppos);
			float distd2 = distance(uvpposd*float2(_DX, _DY).yx + ppos, ppos);
			// Middle point intensity star inensity
			float pint1 = 1. / (dist*_DistMultiplier)
				+ _VHI / (disth*_BranchDistMultiplier)
				+ _VHI / (distv*_BranchDistMultiplier)
				+ _DI / (distd1*_BranchDistMultiplier)
				+ _DI / (distd2*_BranchDistMultiplier);

			float pint = clamp( ( (pow(pint1, _PintPow) / _PintDevider)- _PintSubstractor ),0,1) ;

			col.rgb  = colorAfterEdge   +  float3(pint, pint, pint ) * _StarBrightness;

			//col.rgb = dot( normalize( float3((i.uv.x -0.5) *2  , (i.uv.y - 0.5) * 2, -1  ))
			//	          , normalize(float3( 0,  0, -1)));
 



                return col;
            }
            ENDCG
        }
    }
}
