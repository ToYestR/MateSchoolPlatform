// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/AddTextureShader"
{
    Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_MainColor("Main Color",COLOR)=(1,1,1,1)
		
		_SecondTex("Texture", 2D) = "white" {}
		_Alpha("Alpha", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100

		pass
		{
			SetTexture[_MainTex]
			{
				 constantColor [_MainColor]
				 combine constant  * texture
			}
		}
		Pass
		{
			Blend 	   SrcAlpha  one
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _SecondTex;
			float4 _SecondTex_ST;
			float _Alpha;

			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _SecondTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_SecondTex, i.uv);
				col.a*=_Alpha;
				return col;
			}
			ENDCG
		}
	}
}
