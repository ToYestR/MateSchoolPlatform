Shader "Blank" {    
    Properties
    {
        _Stencil ("Stencil", Int) = 6
    }
    SubShader {
        ZWrite Off

        Stencil 
        {
            Ref [_Stencil]
            Comp Always
            Pass Replace
            ZFail Keep
        }

        Pass 
        {
            Cull Off
            ZTest Always
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata 
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            v2f vert(appdata v) 
            {
                v2f o;    
                
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
    
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target 
            {
                return float4(0,0,0,0.0);
            }

            ENDCG
        }

        Pass
        {
            Cull Off
            ZTest Always

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
                return float4(0,0,0,0.0);
            }
            ENDCG
        }
    } 
}