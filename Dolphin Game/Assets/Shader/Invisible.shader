
Shader "MyShaders/Invisible" {
	Properties{
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(0,5)) = 0.5
				_RimMultiplier("Color Multiplier", Range(0,5)) = 0.5
						_AlphaClamp("Alpha Clamp", Range(0,10)) = 0.5

	}
		SubShader{
			Tags{
				"Queue" = "Transparent"
			}

			Pass{
				ZWrite on
								ColorMask 0

			}

			CGPROGRAM
			#pragma surface surf Lambert alpha:fade

			struct Input {
				float3 viewDir;
			};

			half _RimPower;
			half _AlphaClamp;

			half _RimMultiplier;
			fixed4 _RimColor;

			void surf(Input IN, inout SurfaceOutput o) {

				float rim = 1 - dot(o.Normal, IN.viewDir);
				o.Albedo = _RimColor.rgb * _RimMultiplier;// *pow(rim, _RimPower);

				o.Alpha = clamp( pow(rim, _RimPower), 0, _AlphaClamp);
			}
			ENDCG
	}
		FallBack "Diffuse"
}
