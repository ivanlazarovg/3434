// Made with Amplify Shader Editor v1.9.1.9
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/CumShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        
    }

    SubShader
    {
		LOD 0

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
        	Ref [_Stencil]
        	ReadMask [_StencilReadMask]
        	WriteMask [_StencilWriteMask]
        	Comp [_StencilComp]
        	Pass [_StencilOp]
        }


        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        
        Pass
        {
            Name "Default"
        CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
                float4 ase_texcoord3 : TEXCOORD3;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

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
            
            float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
            float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
            float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
            float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
            float snoise( float3 v )
            {
            	const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
            	float3 i = floor( v + dot( v, C.yyy ) );
            	float3 x0 = v - i + dot( i, C.xxx );
            	float3 g = step( x0.yzx, x0.xyz );
            	float3 l = 1.0 - g;
            	float3 i1 = min( g.xyz, l.zxy );
            	float3 i2 = max( g.xyz, l.zxy );
            	float3 x1 = x0 - i1 + C.xxx;
            	float3 x2 = x0 - i2 + C.yyy;
            	float3 x3 = x0 - 0.5;
            	i = mod3D289( i);
            	float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
            	float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
            	float4 x_ = floor( j / 7.0 );
            	float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
            	float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
            	float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
            	float4 h = 1.0 - abs( x ) - abs( y );
            	float4 b0 = float4( x.xy, y.xy );
            	float4 b1 = float4( x.zw, y.zw );
            	float4 s0 = floor( b0 ) * 2.0 + 1.0;
            	float4 s1 = floor( b1 ) * 2.0 + 1.0;
            	float4 sh = -step( h, 0.0 );
            	float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
            	float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
            	float3 g0 = float3( a0.xy, h.x );
            	float3 g1 = float3( a0.zw, h.y );
            	float3 g2 = float3( a1.xy, h.z );
            	float3 g3 = float3( a1.zw, h.w );
            	float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
            	g0 *= norm.x;
            	g1 *= norm.y;
            	g2 *= norm.z;
            	g3 *= norm.w;
            	float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
            	m = m* m;
            	m = m* m;
            	float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
            	return 42.0 * dot( m, px);
            }
            

            
            v2f vert(appdata_t v )
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
                float4 screenPos = ComputeScreenPos(ase_clipPos);
                OUT.ase_texcoord3 = screenPos;
                

                v.vertex.xyz +=  float3( 0, 0, 0 ) ;

                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = v.texcoord;
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN ) : SV_Target
            {
                //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
                //The incoming alpha could have numerical instability, which makes it very sensible to
                //HDR color transparency blend, when it blends with the world's texture.
                const half alphaPrecision = half(0xff);
                const half invAlphaPrecision = half(1.0/alphaPrecision);
                IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

                float4 color21 = IsGammaSpace() ? float4(0.5660378,0.5501481,0.4031684,0) : float4(0.280335,0.2634281,0.1350997,0);
                float4 color20 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
                float4 screenPos = IN.ase_texcoord3;
                float4 ase_screenPosNorm = screenPos / screenPos.w;
                ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
                float2 temp_output_35_0 = (ase_screenPosNorm).xy;
                float simplePerlin2D1 = snoise( temp_output_35_0*2.93 );
                simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
                float simplePerlin3D4 = snoise( float3( temp_output_35_0 ,  0.0 )*3.4 );
                simplePerlin3D4 = simplePerlin3D4*0.5 + 0.5;
                float blendOpSrc9 = simplePerlin2D1;
                float blendOpDest9 = simplePerlin3D4;
                float clampResult13 = clamp( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc9 - 0.5 ) * ( blendOpDest9 - 0.5 ) ) )) , 0.42 , 1.0 );
                float4 lerpResult19 = lerp( color21 , color20 , clampResult13);
                

                half4 color = lerpResult19;

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
        ENDCG
        }
    }
    CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19109
Node;AmplifyShaderEditor.DynamicAppendNode;14;-1096.558,-191.2363;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-943.5579,-118.2363;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1619.558,-127.2363;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1407.558,-159.2363;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;-1219.558,-130.2363;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-673.263,-454.2028;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2.93;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;4;-678.5579,-147.2363;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;3.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;9;-281.5579,-313.2363;Inherit;True;Exclusion;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;113.0616,-531.876;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;13;109.4421,-199.2363;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.42;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;104.0616,-302.876;Inherit;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;0;False;0;False;0.5660378,0.5501481,0.4031684,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;383.0616,-354.876;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1013.825,-329.4286;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;34;-1279.471,-462.4857;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;35;-981.4709,-427.4857;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;38;634,-362;Float;False;True;-1;2;ASEMaterialInspector;0;3;Custom/CumShader;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;14;0;16;0
WireConnection;7;0;35;0
WireConnection;7;1;16;0
WireConnection;15;0;8;0
WireConnection;16;0;15;0
WireConnection;1;0;35;0
WireConnection;4;0;35;0
WireConnection;9;0;1;0
WireConnection;9;1;4;0
WireConnection;13;0;9;0
WireConnection;19;0;21;0
WireConnection;19;1;20;0
WireConnection;19;2;13;0
WireConnection;2;0;35;0
WireConnection;2;1;14;0
WireConnection;35;0;34;0
WireConnection;38;0;19;0
ASEEND*/
//CHKSM=61A43B7710FE6362C1442DC8E8E5C17D2C8F78D0