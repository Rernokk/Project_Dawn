Shader "A_Custom_Shader/AuraBurnShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
    _BurnTex ("Burn Texture", 2D) = "white" {}
    _Value ("% Complete", Range(-1,1)) = 1.0
	}
	SubShader
	{
    Tags {"RenderType" = "Transparent" "Queue" = "Transparent" }
		Cull Off ZWrite Off ZTest Always
    Blend SrcAlpha OneMinusSrcAlpha

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
      sampler2D _BurnTex;
      fixed _Value;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
        fixed val = clamp(tex2D(_BurnTex, i.uv).r + _Value, 0, 1);
        col.a *= val;
        if (val < 1 && val > .95) {
          col *= 3;
          col.a = 1;
        }
        else if (val < .9) {
          col.a = 0;
        }

        if (tex2D(_MainTex, i.uv).a == 0) {
          col.a = 0;
        }
				return col;
			}
			ENDCG
		}
	}
}
