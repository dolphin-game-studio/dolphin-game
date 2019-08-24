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


					_GreyScaleMultiplier("Grey Scale Multiplier", Range(0,10)) = 0

			_Filled("Filled", Range(0,1)) = 0

						_Joyce("Joyce", Range(0,6.2832)) = 0
			_Will("Will", Range(0,6.2832)) = 0
						_Nancy("Nancy", Range(0,6.2832)) = 0

							_El("El", Range(0,6.2832)) = 0

			_EndeSharpness("Ende Sharpness" , Range(0,1000)) = 0
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

			float _Range1;
			float _Range2;

			float _RangeX;
			float _RangeY;

			float _GreyScaleRed;
			float _GreyScaleGreen;
			float _GreyScaleBlue;

			float _GreyScaleMultiplier;
			float _Filled;

			float _Will;
			float _Joyce;
			float _Nancy;
			float _El;

			float _EndeSharpness;

			
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                 fixed4 col = tex2D(_MainTex, i.uv);
 
			float greyscale = _GreyScaleRed * col.r + _GreyScaleGreen * col.g + _GreyScaleBlue * col.b;
			float4 greyscaleColor = float4(greyscale * _GreyScaleMultiplier, greyscale* _GreyScaleMultiplier, greyscale * _GreyScaleMultiplier, 1);

			//col = float4(0,0,0,1);
			
			float joyce = frac(atan2(i.uv.y - 0.5, i.uv.x - 0.5) / 6.2832 + _Joyce);

			float will = frac(atan2(i.uv.y - 0.5, i.uv.x - 0.5) / 6.2832 + _Will);

			float jonathan = will > joyce;

			float nancy = frac(atan2(i.uv.y - 0.5, i.uv.x - 0.5) / 6.2832 + _Nancy);
 
			float el = frac(atan2(i.uv.x - 0.5, i.uv.y - 0.5) / 6.2832 + _El);

 
			float3 fadedOutColor = col * pow(will, _EndeSharpness) + greyscaleColor * (1-pow(will, _EndeSharpness));

			float3 fullColor = fadedOutColor * jonathan + col * (1 - jonathan);
			col.rgb = fullColor;
			//if (jonathan) {
			//	col.rgb = fullColor + col * pow(nancy,10);
			//}

			

						//col.rgb *= nancy * 2;
						//col.rgb *= el * 2;


			//float2 center = float2(_RangeX, _RangeY);
				//float dist = distance(center, i.uv);
				//col += col * float4(1,0,0,1) * pow(2 - dist, _Range1);

                return col;
            }
            ENDCG
        }
    }
}
