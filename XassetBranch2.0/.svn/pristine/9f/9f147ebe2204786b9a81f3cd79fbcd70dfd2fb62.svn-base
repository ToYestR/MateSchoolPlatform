Shader "OutlinePostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass // Blur
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile BOX_BLUR GAUSSIAN5X5 GAUSSIAN9X9 GAUSSIAN13X13

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
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                float4x4 mvp = UNITY_MATRIX_MVP;
                o.vertex = mul(mvp, v.vertex);
				
                o.uv = v.uv;

                return o;
            }
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            float4 _MainTex_ST;

            float4 _OutlinePostProcessDirection;

            float4 frag (v2f i) : SV_Target // Blur
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

#if defined (BOX_BLUR)
                float4 first = FetchTexelAtWithShift(i.uv, _OutlinePostProcessDirection.xy);
                float4 second = FetchTexelAtWithShift(i.uv, -_OutlinePostProcessDirection.xy);
                float4 center = FetchTexelAt(i.uv);    
                float4 result = (first + second + center) / 3.0f;

                return result;
#endif

#if defined (GAUSSIAN5X5)
                
                float4 result = float4(0, 0, 0, 0);
                float2 off = _OutlinePostProcessDirection.xy *  1.3333333333333333;
                result += FetchTexel * 0.29411764705882354;
                result += FetchTexelAtWithShift(i.uv,  off) * 0.35294117647058826;
                result += FetchTexelAtWithShift(i.uv, -off) * 0.35294117647058826;

                return result;
#endif

#if defined (GAUSSIAN9X9)
                
                float4 result = float4(0, 0, 0, 0);
                float2 off1 = 1.3846153846 * _OutlinePostProcessDirection.xy;
                float2 off2 = 3.2307692308 * _OutlinePostProcessDirection.xy;
                result += FetchTexel * 0.2270270270;
                result += FetchTexelAtWithShift(i.uv,  off1) * 0.3162162162;
                result += FetchTexelAtWithShift(i.uv, -off1) * 0.3162162162;
                result += FetchTexelAtWithShift(i.uv,  off2) * 0.0702702703;
                result += FetchTexelAtWithShift(i.uv, -off2) * 0.0702702703;

                return result;
#endif

#if defined (GAUSSIAN13X13)

                float4 result = float4(0, 0, 0, 0);
                float2 off1 = 1.411764705882353 * _OutlinePostProcessDirection.xy;
                float2 off2 = 3.2941176470588234 * _OutlinePostProcessDirection.xy;
                float2 off3 = 5.176470588235294 * _OutlinePostProcessDirection.xy;

                result += FetchTexel * 0.1964825501511404;
                result += FetchTexelAtWithShift(i.uv,  off1) * 0.2969069646728344;
                result += FetchTexelAtWithShift(i.uv, -off1) * 0.2969069646728344;
                result += FetchTexelAtWithShift(i.uv,  off2) * 0.09447039785044732;
                result += FetchTexelAtWithShift(i.uv, -off2) * 0.09447039785044732;
                result += FetchTexelAtWithShift(i.uv,  off3) * 0.010381362401148057;
                result += FetchTexelAtWithShift(i.uv, -off3) * 0.010381362401148057;

                return result;
#endif

                return float4(0, 0, 0, 0);
            }
            ENDCG
        }

        Pass // Dilate
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
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                float4x4 mvp = UNITY_MATRIX_MVP;
                o.vertex = mul(mvp, v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            float4 _MainTex_ST;

            float4 _OutlinePostProcessDirection;

            float4 frag (v2f i) : SV_Target // Dilate
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                float4 first = FetchTexelAtWithShift(i.uv, _OutlinePostProcessDirection.xy);
                float4 second = FetchTexelAtWithShift(i.uv, -_OutlinePostProcessDirection.xy);
                float4 center = FetchTexelAt(i.uv);

                return max(max(first, second), center);
            }
            ENDCG
        }
        
        Pass // Blur
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile BOX_BLUR GAUSSIAN5X5 GAUSSIAN9X9 GAUSSIAN13X13

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
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                float4x4 mvp = UNITY_MATRIX_MVP;
                o.vertex = mul(mvp, v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            float4 _MainTex_ST;

            UNITY_DECLARE_SCREENSPACE_TEXTURE(_InfoBuffer);
            float4 _InfoBuffer_ST;

            float4 _OutlinePostProcessDirection;

            float4 frag (v2f i) : SV_Target // Blur
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                float4 info = FetchTexelAtFrom(_InfoBuffer, i.uv, _MainTex_ST);

#if defined (BOX_BLUR)

                float4 first = FetchTexelAtWithShift(i.uv, _OutlinePostProcessDirection.xy * info.r);
                float4 second = FetchTexelAtWithShift(i.uv, -_OutlinePostProcessDirection.xy * info.r);
                float4 center = FetchTexelAt(i.uv);    
                float4 result = (first + second + center) / 3.0f;

                return result;
#endif

#if defined (GAUSSIAN5X5)
                
                float4 result = float4(0, 0, 0, 0);
                float2 off = _OutlinePostProcessDirection.xy * info.r * 1.3333333333333333;
                result += FetchTexel * 0.29411764705882354;
                result += FetchTexelAtWithShift(i.uv,  off) * 0.35294117647058826;
                result += FetchTexelAtWithShift(i.uv, -off) * 0.35294117647058826;

                return result;
#endif

#if defined (GAUSSIAN9X9)
                
                float4 result = float4(0, 0, 0, 0);
                float2 off1 = 1.3846153846 * info.r * _OutlinePostProcessDirection.xy;
                float2 off2 = 3.2307692308 * info.r * _OutlinePostProcessDirection.xy;
                result += FetchTexel * 0.2270270270;
                result += FetchTexelAtWithShift(i.uv,  off1) * 0.3162162162;
                result += FetchTexelAtWithShift(i.uv, -off1) * 0.3162162162;
                result += FetchTexelAtWithShift(i.uv,  off2) * 0.0702702703;
                result += FetchTexelAtWithShift(i.uv, -off2) * 0.0702702703;

                return result;
#endif

#if defined (GAUSSIAN13X13)

                float4 result = float4(0, 0, 0, 0);
                float2 off1 = 1.411764705882353 * info.r * _OutlinePostProcessDirection.xy;
                float2 off2 = 3.2941176470588234 * info.r * _OutlinePostProcessDirection.xy;
                float2 off3 = 5.176470588235294 * info.r * _OutlinePostProcessDirection.xy;

                result += FetchTexel * 0.1964825501511404;
                result += FetchTexelAtWithShift(i.uv,  off1) * 0.2969069646728344;
                result += FetchTexelAtWithShift(i.uv, -off1) * 0.2969069646728344;
                result += FetchTexelAtWithShift(i.uv,  off2) * 0.09447039785044732;
                result += FetchTexelAtWithShift(i.uv, -off2) * 0.09447039785044732;
                result += FetchTexelAtWithShift(i.uv,  off3) * 0.010381362401148057;
                result += FetchTexelAtWithShift(i.uv, -off3) * 0.010381362401148057;

                return result;
#endif

                return float4(0, 0, 0, 0);
            }
            ENDCG
        }

        Pass // Dilate
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
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                float4x4 mvp = UNITY_MATRIX_MVP;
                o.vertex = mul(mvp, v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            float4 _MainTex_ST;
            
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_InfoBuffer);
            float4 _InfoBuffer_ST;

            float4 _OutlinePostProcessDirection;

            float4 frag (v2f i) : SV_Target // Dilate
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                float4 info = FetchTexelAtFrom(_InfoBuffer, i.uv, _MainTex_ST);
                float4 first = FetchTexelAtWithShift(i.uv, _OutlinePostProcessDirection.xy * info.g);
                float4 second = FetchTexelAtWithShift(i.uv, -_OutlinePostProcessDirection.xy * info.g);
                float4 center = FetchTexelAt(i.uv);

                return max(max(first, second), center);
            }
            ENDCG
        }
    }
}