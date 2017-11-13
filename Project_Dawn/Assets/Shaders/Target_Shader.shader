﻿Shader "A_Custom_Shader/Target_Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1,1)
    _Threshold ("Distance", Range(0,1)) = 0.0
	}
	SubShader
	{
    Cull Off
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
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
      fixed4 _Color;
      fixed _Threshold;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
        col = 1 - col;
        if (i.uv.x < _Threshold || i.uv.x > 1 - _Threshold || i.uv.y < _Threshold || i.uv.y > 1 - _Threshold){
            col.rgb *= _Color;
        }
				return col;
			}
			ENDCG
		}
	}
}
