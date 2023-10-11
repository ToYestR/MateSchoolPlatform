Shader "Unlit/Info"
{   
    Properties
    {
        _Stencil ("Mask stencil", Int) = 6
    }
    SubShader
    {
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always

            Stencil 
            {
                Ref [_Stencil]
                Comp NotEqual
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "MiskCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float depth : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = COMPUTE_DEPTH_01;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 result = _Color;
                result.b = _Color.a;

                return result;
            }
            ENDCG
        }
    }
}
