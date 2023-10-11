Shader "Unlit/Outline"
{
    Properties
    {
        _Stencil ("Mask stencil", Int) = 6
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Stencil 
        {
            Ref [_Stencil]
            Comp NotEqual
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "MiskCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 depth : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            float4 _Color;
            sampler2D _Depth;

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_DEPTH(o);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return _Color + float4(0.1f, 0.1f, 0.1f, 0.0f);
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            sampler2D _AlphaTexture;
            float _Cutout;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_AlphaTexture, i.uv);
                clip(texColor.a - _Cutout);
                return _Color + float4(0.1f, 0.1f, 0.1f, 0);
            }
            ENDCG
        }
    }
}
