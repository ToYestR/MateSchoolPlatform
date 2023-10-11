Shader "Hidden/Noise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "..\MiskCG.cginc"

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
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

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
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            float4 _MainTex_ST;
            float _Amount;

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                fixed4 col = FetchTexel;
                col.rgb /= max(col.a, 0.01f);

                col.a = lerp(col.a, abs(rand(i.uv.xy * _Time.z)), _Amount) * col.a;
                col.rgb *= col.a;

                return col;
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
            #include "..\MiskCG.cginc"

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
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

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
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            float4 _MainTex_ST;
            float _Amount;
            float _RedFactor;
            float _GreenFactor;
            float _BlueFactor;
            float _AlphaFactor;

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                fixed4 col = FetchTexel;
                col.rgb /= max(col.a, 0.01f);

                float4 newColor;
                newColor.r = lerp(col.r, abs(rand(i.uv.xy * _Time.w)), _RedFactor);
                newColor.g = lerp(col.g, abs(rand(i.uv.xy * (_Time.w + 1))), _GreenFactor);
                newColor.b = lerp(col.b, abs(rand(i.uv.xy * (_Time.w + 2))), _BlueFactor);
                newColor.a = lerp(col.a, abs(rand(i.uv.xy * (_Time.w + 3)) * col.a), _AlphaFactor);
                col = lerp(col, newColor, _Amount * (col.a > 0));

                col.rgb *= col.a;

                return col;
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
            #include "..\MiskCG.cginc"
            #define PI 3.14159265358979323846
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
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            float4 _MainTex_ST;
            float _Amount;
            float4 _Resolution;
            float _Size;
            
            float2 hash( float2 x )  // replace this by something better
            {
                float2 k = float2( 0.3183099, 0.3678794 );
                x = x*k + k.yx;
                return -1.0 + 2.0*frac( 16.0 * k*frac( x.x*x.y*(x.x+x.y)) );
            }

            float noise( float2 p )
            {
                float2 i = floor( p );
                float2 f = frac( p );
	
	            float2 u = f*f*(3.0-2.0*f);

                return lerp( lerp( dot( hash( i + float2(0.0,0.0) ), f - float2(0.0,0.0) ), 
                                 dot( hash( i + float2(1.0,0.0) ), f - float2(1.0,0.0) ), u.x),
                            lerp( dot( hash( i + float2(0.0,1.0) ), f - float2(0.0,1.0) ), 
                                 dot( hash( i + float2(1.0,1.0) ), f - float2(1.0,1.0) ), u.x), u.y);
            }

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

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                fixed4 col = FetchTexel;
                col.rgb /= max(col.a, 0.01f);

                col.a = lerp(col.a, abs(noise(i.uv * _Resolution * 0.1f * _Size)) * col.a, _Amount);

                col.rgb *= col.a;

                return col;
            }
            ENDCG
        }
    }
}
