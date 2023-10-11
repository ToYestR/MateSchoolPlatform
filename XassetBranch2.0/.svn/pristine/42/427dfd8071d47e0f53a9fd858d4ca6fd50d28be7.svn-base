#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
#define FetchTexel UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex,i.uv)
#define FetchTexelAt(uv) FetchTexelAtFrom(_MainTex,uv,_MainTex_ST)
#define FetchTexelAtWithShift(uv,shift) FetchTexelAtFrom(_MainTex,(uv)+(shift),_MainTex_ST)
#define FetchTexelAtFrom(tex,uv,texST) UNITY_SAMPLE_SCREENSPACE_TEXTURE(tex,uv)
#else
#define FetchTexel tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv,_MainTex_ST))
#define FetchTexelAt(uv) FetchTexelAtFrom(_MainTex,uv,_MainTex_ST)
#define FetchTexelAtWithShift(uv,shift) tex2D(_MainTex,UnityStereoScreenSpaceUVAdjust((uv),(_MainTex_ST))+(shift))
#define FetchTexelAtFrom(tex,uv,texST) tex2D(tex,UnityStereoScreenSpaceUVAdjust((uv),(texST)))
#endif