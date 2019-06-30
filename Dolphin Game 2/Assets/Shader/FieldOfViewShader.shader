Shader "Custom/FieldOfViewShader"
{
   Properties
    {
		[PerRendererData]_Color("Color", Color) = (1,1,1,1)
        _Transparency("Transparency", Range(0.0,0.5)) = 0.25
    }

    SubShader 
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }

        LOD 100
		Cull Off

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

            CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag

 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Transparency;

            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                 fixed4 col = _Color;
                 col.a = _Transparency;
                 return col;
            }
            ENDCG
        }
    }
}
