Shader "A_Custom_Shader/Health_Bar_Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
    _Tex ("TextureMap", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Value ("Health Value", Range(0,1)) = .5
		_Direction ("Direction", Int) = 0
	}
	SubShader
	{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always
    ZWrite Off
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
      
			sampler2D _MainTex;
      sampler2D _Tex;
      fixed4 _Tex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Tex);
				return o;
			}
			
			fixed4 _Color;
			fixed _Value;
			fixed _Direction;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
        if (col.a == 0){
          return col;
        }
				if (_Direction == 0){
					if (i.uv.x > _Value){
						col.a = .2f;
					}
				} else {
					if (i.uv.y > _Value){
						col.a = .2f;
            col.rgb *= .1;
					}
				}
        col *= tex2D(_Tex, i.uv + _Time.x);
				return col;
			}
			ENDCG
		}
	}
}
