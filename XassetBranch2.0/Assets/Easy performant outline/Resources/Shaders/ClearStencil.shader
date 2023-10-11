Shader "ClearStencil" 
{    
    SubShader {
        ColorMask 0
        ZWrite Off

        Stencil 
        {
            Ref 0
            Comp Always
            Pass Replace
            ZFail Replace
        }

        CGINCLUDE
            #include "MiskCG.cginc"
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            v2f vert(appdata v) {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target {    
                return half4(0,0,0,1);
            }
        ENDCG

        Pass {
            Cull Back
            ZTest Greater
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            ENDCG
        }
    } 
}