Shader "A_Custom_Shader/Goal_Edge_Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
    _Threshold ("Threshold", Range (0, .2)) = .01
    _Color ("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		// No culling or depth
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Tags {"RenderType" = "Transparent" "Queue" = "Transparent" }
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
        if (i.uv.x < _Threshold || i.uv.x > 1 - _Threshold || i.uv.y < _Threshold || i.uv.y > 1 - _Threshold){
          col.a = 1;
          col.rgb = _Color.rgb;
        } else {
          col.a = 0;
        }
				return col;
			}
			ENDCG
		}
	}
}
