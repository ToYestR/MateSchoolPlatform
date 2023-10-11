Shader "Kirk/Water"
{
	Properties
	{
		_ColorShallow("Color (Shallow)", Color) = (0,0.2941177,0.2078431,0)
		_ColorDeep("Color (Deep)", Color) = (0,0.1803922,0.1254902,0)
		_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalBlendStrength("Normal Blend Strength", Range( 0 , 1)) = 0.5
		_NormalMap1Strength("Normal Map 1 Strength", Range( 0 , 1)) = 1
		_NormalMap2Strength("Normal Map 2 Strength", Range( 0 , 1)) = 1
		_UVScale("UV Scale", Float) = 10
		_UV1Tiling("UV 1 Tiling", Float) = 0.5
		_UV2Tiling("UV 2 Tiling", Float) = 0.5
		_UV1Animator("UV 1 Animator", Vector) = (0.5,1,0,0)
		_UV2Animator("UV 2 Animator", Vector) = (1,0.5,0,0)
		_RefTex("_RefTex", 2D) = "black" {}
		_ReflectStrength("ReflectStrength", Range( 0 , 1)) = 0.5
		_OpacityScale("OpacityScale", Range( 0.3 , 1)) = 0
		_smooth("smooth", Range( 0 , 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalMap;
		uniform float _NormalMap1Strength;
		uniform float2 _UV1Animator;
		uniform float _UVScale;
		uniform float _UV1Tiling;
		uniform float _NormalMap2Strength;
		uniform float2 _UV2Animator;
		uniform float _UV2Tiling;
		uniform float _NormalBlendStrength;
		uniform float4 _ColorDeep;
		uniform float4 _ColorShallow;
		uniform sampler2D _RefTex;
		uniform float4 _RefTex_ST;
		uniform float _ReflectStrength;
		uniform float _smooth;
		uniform float _OpacityScale;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 worldPos = i.worldPos;
			float2 worldPosXZ = (float2(worldPos.x , worldPos.z));
			float2 scale = ( worldPosXZ / _UVScale );
			float2 wave1 = ( _Time.x * _UV1Animator + ( scale * _UV1Tiling ));
			float2 wave2 = ( _Time.x * _UV2Animator + ( scale * _UV2Tiling ));
			float3 _normal = lerp( UnpackScaleNormal( tex2D( _NormalMap, wave1 ), _NormalMap1Strength ) , UnpackScaleNormal( tex2D( _NormalMap, wave2 ), _NormalMap2Strength ) , _NormalBlendStrength);
			float fresnel = pow( 1.0 - dot( _normal, normalize( UnityWorldSpaceViewDir( worldPos ) ) ), 1.336 );
			float4 mainColor = lerp( _ColorDeep , _ColorShallow , fresnel);
			float2 uv_RefTex = i.uv_texcoord * _RefTex_ST.xy + _RefTex_ST.zw;
			float4 finalRGB = ( mainColor + ( tex2D( _RefTex, uv_RefTex ) * _ReflectStrength ) );
			o.Normal = _normal;
			o.Emission = ( finalRGB * finalRGB ).rgb;
			o.Smoothness = _smooth;
			o.Alpha = _OpacityScale;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
