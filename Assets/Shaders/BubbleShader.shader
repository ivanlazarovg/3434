// Made with Amplify Shader Editor v1.9.1.9
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/BubbleShader"
{
	Properties
	{
		_FresnelBias("FresnelBias", Float) = -0.38
		_FresnelPower("FresnelPower", Float) = 1
		_FresnelScale("FresnelScale", Float) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		[HDR]_TextureSample0("Texture Sample 0", CUBE) = "white" {}
		_ReflectionOpacity("Reflection Opacity", Range( 0 , 1)) = 0.11
		_NoiseScale("NoiseScale", Float) = 0.93
		_NoiseIntensity("Noise Intensity", Float) = 1
		_Tesselation("Tesselation", Float) = 0
		_Displacementexponent("Displacement exponent", Float) = 0
		_ColorInfluence("Color Influence", Range( 0 , 1)) = 0
		_RefractionStrength("Refraction Strength", Float) = 0.1
		_PickUpColor("PickUpColor", Color) = (0,0,0,0)
		_MainColorInfluence("MainColorInfluence", Float) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
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
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
			float4 screenPos;
			float3 viewDir;
			float3 worldNormal;
		};

		uniform float _Displacementexponent;
		uniform float _NoiseScale;
		uniform float _NoiseIntensity;
		uniform float4 _PickUpColor;
		uniform float _MainColorInfluence;
		uniform samplerCUBE _TextureSample0;
		uniform float _ColorInfluence;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _RefractionStrength;
		uniform float _ReflectionOpacity;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _Tesselation;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float2 UnityGradientNoiseDir( float2 p )
		{
			p = fmod(p , 289);
			float x = fmod((34 * p.x + 1) * p.x , 289) + p.y;
			x = fmod( (34 * x + 1) * x , 289);
			x = frac( x / 41 ) * 2 - 1;
			return normalize( float2(x - floor(x + 0.5 ), abs( x ) - 0.5 ) );
		}
		
		float UnityGradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 ip = floor( p );
			float2 fp = frac( p );
			float d00 = dot( UnityGradientNoiseDir( ip ), fp );
			float d01 = dot( UnityGradientNoiseDir( ip + float2( 0, 1 ) ), fp - float2( 0, 1 ) );
			float d10 = dot( UnityGradientNoiseDir( ip + float2( 1, 0 ) ), fp - float2( 1, 0 ) );
			float d11 = dot( UnityGradientNoiseDir( ip + float2( 1, 1 ) ), fp - float2( 1, 1 ) );
			fp = fp * fp * fp * ( fp * ( fp * 6 - 15 ) + 10 );
			return lerp( lerp( d00, d01, fp.y ), lerp( d10, d11, fp.y ), fp.x ) + 0.5;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_2 = (_Tesselation).xxxx;
			return temp_cast_2;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float clampResult109 = clamp( pow( ( ( 1.0 - v.texcoord.xy.x ) * v.texcoord.xy.x ) , _Displacementexponent ) , 0.01 , 0.99 );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult72 = (float2(0.0 , _Time.y));
			float2 uv_TexCoord86 = v.texcoord.xy * ase_worldPos.xy + appendResult72;
			float simplePerlin2D85 = snoise( uv_TexCoord86*_NoiseScale );
			simplePerlin2D85 = simplePerlin2D85*0.5 + 0.5;
			float2 appendResult73 = (float2(_Time.y , 0.0));
			float2 uv_TexCoord60 = v.texcoord.xy * ase_worldPos.xy + appendResult73;
			float simplePerlin2D62 = snoise( uv_TexCoord60*_NoiseScale );
			simplePerlin2D62 = simplePerlin2D62*0.5 + 0.5;
			float lerpResult64 = lerp( simplePerlin2D85 , simplePerlin2D62 , 0.5);
			float3 ase_vertex3Pos = v.vertex.xyz;
			v.vertex.xyz += ( ( clampResult109 * ( lerpResult64 * _NoiseIntensity ) ) + ase_vertex3Pos );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			Gradient gradient6 = NewGradient( 0, 8, 2, float4( 1, 0, 0.7640834, 0 ), float4( 0.2352941, 0.3731336, 1, 0.09799344 ), float4( 0.3925856, 1, 0.2117647, 0.1683375 ), float4( 1, 0, 0.009225845, 0.3391928 ), float4( 1, 0, 0.7647059, 0.4974899 ), float4( 0.2352941, 0.372549, 1, 0.6783856 ), float4( 0.3640879, 1, 0.3254717, 0.8492408 ), float4( 0.2117647, 0.772549, 1, 1 ), float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float2 temp_cast_1 = (( _Time.y * 0.2 )).xx;
			float2 uv_TexCoord14 = i.uv_texcoord * ase_worldPos.xy + temp_cast_1;
			float gradientNoise13 = UnityGradientNoise(uv_TexCoord14,2.0);
			gradientNoise13 = gradientNoise13*0.5 + 0.5;
			float4 lerpResult30 = lerp( _PickUpColor , SampleGradient( gradient6, ( abs( ase_worldViewDir ) * gradientNoise13 ).x ) , _MainColorInfluence);
			float4 Colors123 = lerpResult30;
			float3 ase_worldReflection = i.worldRefl;
			float2 appendResult72 = (float2(0.0 , _Time.y));
			float2 uv_TexCoord86 = i.uv_texcoord * ase_worldPos.xy + appendResult72;
			float simplePerlin2D85 = snoise( uv_TexCoord86*_NoiseScale );
			simplePerlin2D85 = simplePerlin2D85*0.5 + 0.5;
			float2 appendResult73 = (float2(_Time.y , 0.0));
			float2 uv_TexCoord60 = i.uv_texcoord * ase_worldPos.xy + appendResult73;
			float simplePerlin2D62 = snoise( uv_TexCoord60*_NoiseScale );
			simplePerlin2D62 = simplePerlin2D62*0.5 + 0.5;
			float lerpResult64 = lerp( simplePerlin2D85 , simplePerlin2D62 , 0.5);
			float DisplacementNoise132 = lerpResult64;
			float4 blendOpSrc135 = Colors123;
			float4 blendOpDest135 = ( round( ( texCUBE( _TextureSample0, ( ase_worldReflection + DisplacementNoise132 ) ) * 4.0 ) ) / 4.0 );
			float4 lerpBlendMode135 = lerp(blendOpDest135, round( 0.5 * ( blendOpSrc135 + blendOpDest135 ) ),_ColorInfluence);
			float4 Reflection149 = ( saturate( lerpBlendMode135 ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor140 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ( DisplacementNoise132 * _RefractionStrength ) + ase_grabScreenPosNorm ).xy);
			float4 Refraction142 = screenColor140;
			float4 lerpResult134 = lerp( Reflection149 , Refraction142 , _ReflectionOpacity);
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, i.viewDir );
			float fresnelNode1 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV1, _FresnelPower ) );
			float Fresnel119 = fresnelNode1;
			float4 lerpResult112 = lerp( lerpResult134 , Colors123 , Fresnel119);
			o.Emission = lerpResult112.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float temp_output_121_0 = Fresnel119;
			o.Alpha = temp_output_121_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
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
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.worldRefl = -worldViewDir;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
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
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19109
Node;AmplifyShaderEditor.SimpleTimeNode;55;-1111.354,324.4215;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;81;-1476.408,298.5283;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;72;-942.1758,149.0946;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;-900.1758,414.0945;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;116;-929.3141,40.99398;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;117;-1203.855,433.7063;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-773.4568,333.9851;Inherit;False;Property;_NoiseScale;NoiseScale;7;0;Create;True;0;0;0;False;0;False;0.93;0.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;122;-2419.982,-2142.271;Inherit;False;2600.335;1041.411;Comment;14;30;15;92;14;21;13;17;8;26;7;6;123;151;152;Colors;1,0,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;86;-812.4126,11.25905;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-764.1758,446.0945;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;15;-2316.259,-1520.69;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;85;-518.179,78.12077;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;62;-496.1758,431.0945;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2065.409,-1391.969;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;64;-206.1759,338.0945;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;92;-2369.982,-1345.242;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1787.226,-1332.077;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;132;-124.4457,192.5282;Inherit;False;DisplacementNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;148;605.6276,-1982.95;Inherit;False;2090.909;520.0103;Comment;11;39;46;45;47;124;136;48;135;131;41;133;Reflection;0,0.8916159,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;8;-1807.465,-1518.178;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;133;649.5515,-1575.147;Inherit;False;132;DisplacementNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldReflectionVector;39;606.8674,-1858.028;Inherit;True;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;26;-1441.433,-1498.987;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;13;-1525.451,-1240.859;Inherit;False;Gradient;True;True;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;942.1231,-1769.645;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1224.844,-1423.905;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GradientSampleNode;7;-689.8306,-1446.489;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;147;-3012.651,-725.0102;Inherit;False;1149.907;457.0372;Comment;7;138;146;137;145;139;140;142;Refraction;0.799102,0.3443396,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;41;1097.982,-1775.838;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;1;[HDR];Create;True;0;0;0;False;0;False;-1;None;ef7513b54a0670140b9b967af7620563;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;1357.317,-1776.013;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;1560.795,-1733.705;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;138;-2962.65,-675.0102;Inherit;False;132;DisplacementNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;30;-306.2565,-1570.332;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;146;-2951.259,-596.079;Inherit;False;Property;_RefractionStrength;Refraction Strength;12;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;101;-1400.033,-409.2745;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RoundOpNode;47;1760.322,-1731.788;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GrabScreenPosition;137;-2930.861,-482.9733;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;-2690.432,-630.8669;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;-11.37244,-1504.323;Inherit;False;Colors;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;136;1977.48,-1552.629;Inherit;False;Property;_ColorInfluence;Color Influence;11;0;Create;True;0;0;0;False;0;False;0;0.563;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;1936.184,-1929.157;Inherit;False;123;Colors;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;48;1929.323,-1776.788;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;105;-1066.447,-445.5343;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;120;702.4406,-1169.114;Inherit;False;1027.208;437.0232;Comment;6;119;1;2;19;18;20;Fresnel;1,0.8389567,0,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-2508.86,-561.9738;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;20;973.007,-880.7295;Inherit;False;Property;_FresnelScale;FresnelScale;2;0;Create;True;0;0;0;False;0;False;1;3.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-764.1354,-126.6906;Inherit;False;Property;_Displacementexponent;Displacement exponent;10;0;Create;True;0;0;0;False;0;False;0;3.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-723.5389,-375.4756;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;2;802.4409,-1069.114;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;19;1036.991,-1035.281;Inherit;False;Property;_FresnelPower;FresnelPower;1;0;Create;True;0;0;0;False;0;False;1;1.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;998.5163,-952.7918;Inherit;False;Property;_FresnelBias;FresnelBias;0;0;Create;True;0;0;0;False;0;False;-0.38;-0.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;135;2292.777,-1849.258;Inherit;True;HardMix;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.29;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;140;-2269.859,-535.9734;Inherit;False;Global;_GrabScreen1;Grab Screen 1;13;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;142;-2086.743,-505.3227;Inherit;False;Refraction;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-245.1287,-31.82757;Inherit;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;0;False;0;False;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;149;2692.824,-1603.007;Inherit;False;Reflection;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;102;-313.9411,-391.5113;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-133.585,73.05835;Inherit;False;Constant;_Float2;Float 2;12;0;Create;True;0;0;0;False;0;False;0.99;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-148.0681,494.4756;Inherit;False;Property;_NoiseIntensity;Noise Intensity;8;0;Create;True;0;0;0;False;0;False;1;7.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;1172.533,-986.0902;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;86.61131,280.6694;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;783.2697,-39.99444;Inherit;False;Property;_ReflectionOpacity;Reflection Opacity;6;0;Create;True;0;0;0;False;0;False;0.11;0.874;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;1505.649,-923.3452;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;109;-108.3232,-132.8811;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;141.7953,74.0374;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;96;289.9706,230.5837;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;121;390.0609,-266.843;Inherit;False;119;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;483.6273,99.04051;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;38;836.0131,131.774;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;0;False;0;False;0;0.194;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;771.5445,270.5147;Inherit;False;Property;_Tesselation;Tesselation;9;0;Create;True;0;0;0;False;0;False;0;1.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;830.0131,57.7741;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;0;False;0;False;0;0.592;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1339.73,-185.6834;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Custom/BubbleShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.LerpOp;112;1047.298,-276.5344;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;144;487.5951,-57.24072;Inherit;False;142;Refraction;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;150;693.2729,-198.6915;Inherit;False;149;Reflection;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;544.1249,-441.9524;Inherit;False;123;Colors;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;134;889.1204,-178.2297;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;6;-965.7988,-1540.756;Inherit;False;0;8;2;1,0,0.7640834,0;0.2352941,0.3731336,1,0.09799344;0.3925856,1,0.2117647,0.1683375;1,0,0.009225845,0.3391928;1,0,0.7647059,0.4974899;0.2352941,0.372549,1,0.6783856;0.3640879,1,0.3254717,0.8492408;0.2117647,0.772549,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.ColorNode;152;-576.47,-1760.595;Inherit;False;Property;_PickUpColor;PickUpColor;13;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.9082565,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;151;-586.87,-1587.696;Inherit;False;Property;_MainColorInfluence;MainColorInfluence;14;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
WireConnection;72;1;55;0
WireConnection;73;0;55;0
WireConnection;116;0;81;0
WireConnection;117;0;81;0
WireConnection;86;0;116;0
WireConnection;86;1;72;0
WireConnection;60;0;117;0
WireConnection;60;1;73;0
WireConnection;85;0;86;0
WireConnection;85;1;74;0
WireConnection;62;0;60;0
WireConnection;62;1;74;0
WireConnection;21;0;15;0
WireConnection;64;0;85;0
WireConnection;64;1;62;0
WireConnection;14;0;92;0
WireConnection;14;1;21;0
WireConnection;132;0;64;0
WireConnection;26;0;8;0
WireConnection;13;0;14;0
WireConnection;131;0;39;0
WireConnection;131;1;133;0
WireConnection;17;0;26;0
WireConnection;17;1;13;0
WireConnection;7;0;6;0
WireConnection;7;1;17;0
WireConnection;41;1;131;0
WireConnection;45;0;41;0
WireConnection;45;1;46;0
WireConnection;30;0;152;0
WireConnection;30;1;7;0
WireConnection;30;2;151;0
WireConnection;47;0;45;0
WireConnection;145;0;138;0
WireConnection;145;1;146;0
WireConnection;123;0;30;0
WireConnection;48;0;47;0
WireConnection;48;1;46;0
WireConnection;105;0;101;1
WireConnection;139;0;145;0
WireConnection;139;1;137;0
WireConnection;107;0;105;0
WireConnection;107;1;101;1
WireConnection;135;0;124;0
WireConnection;135;1;48;0
WireConnection;135;2;136;0
WireConnection;140;0;139;0
WireConnection;142;0;140;0
WireConnection;149;0;135;0
WireConnection;102;0;107;0
WireConnection;102;1;106;0
WireConnection;1;4;2;0
WireConnection;1;1;18;0
WireConnection;1;2;20;0
WireConnection;1;3;19;0
WireConnection;78;0;64;0
WireConnection;78;1;79;0
WireConnection;119;0;1;0
WireConnection;109;0;102;0
WireConnection;109;1;110;0
WireConnection;109;2;111;0
WireConnection;108;0;109;0
WireConnection;108;1;78;0
WireConnection;58;0;108;0
WireConnection;58;1;96;0
WireConnection;0;2;112;0
WireConnection;0;3;38;0
WireConnection;0;4;37;0
WireConnection;0;9;121;0
WireConnection;0;11;58;0
WireConnection;0;14;80;0
WireConnection;112;0;134;0
WireConnection;112;1;125;0
WireConnection;112;2;121;0
WireConnection;134;0;150;0
WireConnection;134;1;144;0
WireConnection;134;2;50;0
ASEEND*/
//CHKSM=679F807ABD8585ED07FFD970120BF7295B49FC20