shader "Minimalist/BRP/Standard LocalSpace" {
	Properties{
		//Texture Module
        _ShowTexture ("Show Texture settngs", Float) = 0
        _MainTexture ("Main Texture", 2D) = "white" { }
        _MainTexturePower ("Main Texture Power", Range(-1, 1)) = 0
        //Custom Shading
        _ShowCustomShading ("Show Custom Shading settngs", Float) = 0
        
        _ShowFront ("Front", Float) = 0
        _Shading_F ("Shading mode", Float) = 0
        _GradSettings_F ("Shading mode", Float) = 0
        _GizmoPosition_F ("front gizmo", Vector) = (0, 0, 10, 10)
        
        _Color1_F ("Forward Color 1", Color) = (1, 1, 1, 1)
        _Color2_F ("Forward Color 2", Color) = (1, 1, 1, 1)
        _GradientYStartPos_F ("Gradient start Y", Vector) = (0, 0, 0, 0)
        _GradientHeight_F ("Gradient Height", Float) = 1
        _Rotation_F ("Rotation", Range(0, 360)) = 0
        
        _ShowBack ("Front", Float) = 0
        _Shading_B ("Shading mode", Float) = 0
        _GradSettings_B ("Shading mode", Float) = 0
        _GizmoPosition_B ("back gizmo", Vector) = (0, 0, 10, 10)
        
        _Color1_B ("Backward Color 1", Color) = (1, 1, 1, 1)
        _Color2_B ("Backward Color 2", Color) = (1, 1, 1, 1)
        _GradientYStartPos_B ("Gradient start Y", Vector) = (0, 0, 0, 0)
        _GradientHeight_B ("Gradient Height", Float) = 1
        _Rotation_B ("Rotation", Range(0, 360)) = 0
        
        _ShowLeft ("Front", Float) = 0
        _Shading_L ("Shading mode", Float) = 0
        _GradSettings_L ("Shading mode", Float) = 0
        _GizmoPosition_L ("Left gizmo", Vector) = (0, 0, 10, 10)
        
        _Color1_L ("Left Color 1", Color) = (1, 1, 1, 1)
        _Color2_L ("Left Color 2", Color) = (1, 1, 1, 1)
        _GradientYStartPos_L ("Gradient start Y", Vector) = (0, 0, 0, 0)
        _GradientHeight_L ("Gradient Height", Float) = 1
        _Rotation_L ("Rotation", Range(0, 360)) = 0
        
        _ShowRight ("Front", Float) = 0
        _Shading_R ("Shading mode", Float) = 0
        _GradSettings_R ("Shading mode", Float) = 0
        _GizmoPosition_R ("Right gizmo", Vector) = (0, 0, 10, 10)
        
        _Color1_R ("Right Color 1", Color) = (1, 1, 1, 1)
        _Color2_R ("Right Color 2", Color) = (1, 1, 1, 1)
        _GradientYStartPos_R ("Gradient start Y", Vector) = (0, 0, 0, 0)
        _GradientHeight_R ("Gradient Height", Float) = 1
        _Rotation_R ("Rotation", Range(0, 360)) = 0
        
        _ShowTop ("Top", Float) = 0
        _Shading_T ("Shading mode", Float) = 0
        _GradSettings_T ("Gradient mode", Float) = 0
        _GizmoPosition_T ("Top gizmo", Vector) = (0, 0, 10, 10)
        
        _Color1_T ("Top Color 1", Color) = (1, 1, 1, 1)
        _Color2_T ("Top Color 2", Color) = (1, 1, 1, 1)
        _GradientXStartPos_T ("Gradient start X", Vector) = (0, 0, 0, 0)
        _GradientHeight_T ("Gradient Height", Float) = 1
        _Rotation_T ("Rotation", Range(0, 360)) = 0
        
        _ShowBottom ("Botttom", Float) = 0
        _Shading_D ("Shading mode", Float) = 0
        _GradSettings_D ("Gradient mode", Float) = 0
        _GizmoPosition_D ("Down gizmo", Vector) = (0, 0, 10, 10)
        
        _Color1_D ("Bottom Color 1", Color) = (1, 1, 1, 1)
        _Color2_D ("Bottom Color 2", Color) = (1, 1, 1, 1)
        _GradientXStartPos_D ("Gradient start X", Vector) = (0, 0, 0, 0)
        _GradientHeight_D ("Gradient Height", Float) = 1
        _Rotation_D ("Rotation", Range(0, 360)) = 0
        //Ambient Occlution
        _ShowAO ("AO", Float) = 0
        _AOEnable ("Enable", Float) = 0
        _AOTexture ("AO Texture", 2D) = "white" { }
        _AOColor ("AO Color", Color) = (1, 1, 1, 1)
        _AOPower ("AO Texture Power", Range(0, 3)) = 0
        _AOuv ("UV", Float) = 0
        //Lightmap
        _ShowLMap ("Lightmap", Float) = 0
        _LmapEnable ("Enable", Float) = 0
        _LmapBlendingMode ("Blend Mode", Float) = 0
        _LMColor ("LightMap Color", Color) = (1, 1, 1, 1)
        _LMPower ("LightMap Power", Range(0, 5.0)) = 0
        //Fog
        _ShowFog ("ShowFog", Float) = 0
        _UnityFogEnable ("Fog", Float) = 0
        _HFogEnable ("Fog", Float) = 0
        _Color_Fog ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _FogYStartPos ("Gradient start Y", Float) = 0
        _FogHeight ("Gradient Height", Float) = 10
        //Color Correction
        _ShowColorCorrection ("Color Correction", Float) = 0
        _ColorCorrectionEnable ("Enable", Float) = 0
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
        _Saturation ("Saturation", Range(0, 1)) = 1
        _Brightness ("Brightness", Range(-1, 1)) = 0
        //OtherSettings
        _OtherSettings ("OtherSettings", Float) = 0
        
        _ShowGlobalGradientSettings ("Show Global Gradient Settings", Float) = 0
        _GradientYStartPos_G ("Gradient start Y", Vector) = (0, 0, 0, 0)
        _GradientHeight_G ("Gradient Height", Float) = 1
        _Rotation_G ("Rotation", Float) = 0
        
        _ShowAmbientSettings ("Show Ambient Settings", Float) = 0
        _AmbientColor ("Ambient Color", Color) = (0, 0, 0, 0)
        _AmbientPower ("Ambient Power", Range(0, 2.0)) = 0
        
        _RimEnable ("Use Rim", Float) = 0
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Power", Range(0, 4)) = 1
        
        _RealtimeShadow ("RealTime Shadow", Float) = 0
        _ShadowColor ("ShadowColor", Color) = (0.1, 0.1, 0.1, 1)
        _ShadowInfluence ("Influence", Range(0.001, 2)) = 0.5
        _ShadowBlend ("Strength", Range(0, 1)) = 0.5
        
        _GradientSpace ("Gradient Space", Float) = 0
        _DontMix ("Don't Mix Color", Float) = 0
        _Fade ("Fade", Range(0, 1)) = 1
        
        _Mode ("Mode", Float) = 0
        _SrcBlend ("_src", Float) = 1.0
        _DstBlend ("_dst", Float) = 0.0
        _ZWrite ("_zWrite", Float) = 0.0
        _ZWriteOverride ("_zWrite", Float) = 0.0
        _Cull ("Cull", Float) = 2
	}

	SubShader{
		Tags{"DisableBatching" = "True"}
		
		Pass{
			Name "StandardPass"
			Tags {"RenderType"="Opaque" "Queue"="Geometry" "LightMode"="ForwardBase"}
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]
			ZTest LEqual
			cull [_Cull]
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase_fullshadows
			#pragma multi_compile_fog
			#pragma fragmentoption ARB_precision_hint_fastest
			//shader_features
			#pragma shader_feature_local _ TEXTUREMODULE_ON
            #pragma shader_feature_local _ FRONTSOLID FRONTGRADIENT
            #pragma shader_feature_local _ BACKSOLID BACKGRADIENT
            #pragma shader_feature_local _ LEFTSOLID LEFTGRADIENT
            #pragma shader_feature_local _ RIGHTSOLID RIGHTGRADIENT
            #pragma shader_feature_local _ TOPSOLID TOPGRADIENT
            #pragma shader_feature_local _ BOTTOMSOLID BOTTOMGRADIENT
            #pragma shader_feature_local _ AO_ON_UV0 AO_ON_UV1
            #pragma shader_feature_local _ LIGHTMAP_ADD LIGHTMAP_MULTIPLY LIGHTMAP_AO
            #pragma shader_feature_local _ HEIGHT_FOG UNITY_FOG
            #pragma shader_feature_local _ SHADOW_ON
            #pragma shader_feature_local _ COLORCORRECTION_ON
            #pragma shader_feature_local _ USERIM
            #pragma shader_feature_local _ DONTMIX
			
			#pragma multi_compile_instancing

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			//Uniforms
			#if TEXTUREMODULE_ON
				uniform sampler2D _MainTexture; uniform half4 _MainTexture_ST;
				uniform half _MainTexturePower;
			#endif
			
			#if FRONTSOLID || FRONTGRADIENT
				uniform half3 _Color1_F;
				#if FRONTGRADIENT
					uniform half3 _Color2_F;
					uniform half2 _GradientYStartPos_F;
					uniform half _GradientHeight_F;
					uniform half _Rotation_F;
				#endif
			#endif
			
			#if BACKSOLID || BACKGRADIENT
				uniform half3 _Color1_B;
				#if BACKGRADIENT
					uniform half3 _Color2_B;
					uniform half2 _GradientYStartPos_B;
					uniform half _GradientHeight_B;
					uniform half _Rotation_B;
				#endif
			#endif
			
			#if LEFTSOLID || LEFTGRADIENT
				uniform half3 _Color1_L;
				#if LEFTGRADIENT
					uniform half3 _Color2_L;
					uniform half2 _GradientYStartPos_L;
					uniform half _GradientHeight_L;
					uniform half _Rotation_L;
				#endif
			#endif
			
			#if RIGHTSOLID || RIGHTGRADIENT
				uniform half3 _Color1_R;
				#if RIGHTGRADIENT
					uniform half3 _Color2_R;
					uniform half2 _GradientYStartPos_R;
					uniform half _GradientHeight_R;
					uniform half _Rotation_R;
				#endif
			#endif
			
			#if TOPSOLID || TOPGRADIENT
				uniform half3 _Color1_T;
				#if TOPGRADIENT
					uniform half3 _Color2_T;
					uniform half2 _GradientXStartPos_T;
					uniform half _GradientHeight_T;
					uniform half _Rotation_T;
				#endif
			#endif
			
			#if BOTTOMSOLID || BOTTOMGRADIENT
				uniform half3 _Color1_D;
				#if BOTTOMGRADIENT
					uniform half3 _Color2_D;
					uniform half2 _GradientXStartPos_D;
					uniform half _GradientHeight_D;
					uniform half _Rotation_D;
				#endif
			#endif
			
			#if AO_ON_UV0 || AO_ON_UV1
				uniform sampler2D _AOTexture; uniform half4 _AOTexture_ST;
				uniform half3 _AOColor;
				uniform half _AOPower;
			#endif
			
			#if LIGHTMAP_AO || LIGHTMAP_ADD || LIGHTMAP_MULTIPLY
			    #if LIGHTMAP_AO
				    uniform half3 _LMColor;
				#endif
				uniform half _LMPower;
			#endif
			
			#if HEIGHT_FOG
				uniform half3 _Color_Fog;
				uniform half _FogYStartPos;
				uniform half _FogHeight;
			#endif
			
			#if USERIM
				uniform half3 _RimColor;
				uniform half _RimPower;
			#endif
			
			#if COLORCORRECTION_ON
				uniform half3 _TintColor;
				uniform half _Saturation;
				uniform half _Brightness;
			#endif
			
			uniform half3 _AmbientColor;
			uniform half _AmbientPower;
			#if SHADOW_ON
				uniform half3 _ShadowColor;
				uniform half _ShadowInfluence;
				uniform half _ShadowBlend;
			#endif
			uniform half _GradientSpace;
			uniform half _Fade;
			
			//Direction vector constants
			static const half3 FrontDir = half3(0, 0, 1);
			static const half3 BackDir = half3(0, 0, -1);
			static const half3 LeftDir = half3(1, 0, 0);
			static const half3 RightDir = half3(-1, 0, 0);
			static const half3 TopDir = half3(0, 1, 0);
			static const half3 BottomDir = half3(0, -1, 0);
			static const half3 whiteColor = half3(1, 1, 1);
			
			struct vertexInput{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				half3 vColor : COLOR;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct vertexOutput{
				float4 pos : POSITION;
				float4 worldPos : TEXCOORD0;
				#if TEXTUREMODULE_ON
					float2 uv : TEXCOORD1;
				#endif
				
				#if LIGHTMAP_ADD || LIGHTMAP_MULTIPLY || LIGHTMAP_AO
					float2 lightmapUV : TEXCOORD2;
				#endif
				float3 customLighting : COLOR0;
				
				#if AO_ON_UV0 || AO_ON_UV1  
					float2 aouv : TEXCOORD4;
				#endif
				#if UNITY_FOG
					UNITY_FOG_COORDS(5)
				#endif
				#if SHADOW_ON
					LIGHTING_COORDS(6, 7)
					float3 normalDir : TEXCOORD8;
				#endif
				#if USERIM
					float rimCol : COLOR1;
				#endif
			};
			
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v)
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				half3 normal = normalize(v.normal).xyz;
				
				//Maping Texture
				#if TEXTUREMODULE_ON
					o.uv = TRANSFORM_TEX(v.uv0, _MainTexture);
				#endif
				
				//Maping AO maps
				#if AO_ON_UV0
					o.aouv = TRANSFORM_TEX(v.uv0, _AOTexture);
				#endif
				#if AO_ON_UV1
					o.aouv = TRANSFORM_TEX(v.uv1, _AOTexture);
				#endif
				
				
				//Calculating custom shadings
				half3 colorFront, colorBack, colorLeft, colorRight, colorTop, colorDown;
				half dirFront  = max(dot(normal, FrontDir),  0.0);
				half dirBack   = max(dot(normal, BackDir),   0.0);
				half dirLeft   = max(dot(normal, LeftDir),   0.0);
				half dirRight  = max(dot(normal, RightDir),  0.0);
				half dirTop    = max(dot(normal, TopDir),    0.0);
				half dirBottom = max(dot(normal, BottomDir), 0.0);
				
				colorFront = colorBack = colorLeft = colorRight = colorTop = colorDown = v.vColor;
				
				#if FRONTSOLID 
					colorFront = _Color1_F; 
				#endif
				#if BACKSOLID
					colorBack = _Color1_B;
				#endif
				#if LEFTSOLID
					colorLeft = _Color1_L;
				#endif
				#if RIGHTSOLID
					colorRight = _Color1_R;
				#endif
				#if TOPSOLID
					colorTop = _Color1_T;
				#endif
				#if BOTTOMSOLID
					colorDown = _Color1_D;
					
				#endif
				
				float4 GradPos = v.vertex;
				
				#if FRONTGRADIENT
					half RotatedGrad_F = (GradPos.x - _GradientYStartPos_F.x) * sin(_Rotation_F/57.32) + (GradPos.y - _GradientYStartPos_F.y) * cos(_Rotation_F/57.32);
					half GradientFactor_F = saturate(RotatedGrad_F / -_GradientHeight_F);
					colorFront = lerp(_Color1_F, _Color2_F, GradientFactor_F);
				#endif
				#if BACKGRADIENT
					half RotatedGrad_B = (GradPos.x - _GradientYStartPos_B.x) * sin(_Rotation_B/57.32) + (GradPos.y - _GradientYStartPos_B.y) * cos(_Rotation_B/57.32);
					half GradientFactor_B = saturate(RotatedGrad_B / -_GradientHeight_B);
					colorBack  = lerp(_Color1_B, _Color2_B, GradientFactor_B);
				#endif
				#if LEFTGRADIENT
					half RotatedGrad_L = (GradPos.z - _GradientYStartPos_L.x) * sin(_Rotation_L/57.32) + (GradPos.y - _GradientYStartPos_L.y) * cos(_Rotation_L/57.32);
					half GradientFactor_L = saturate(RotatedGrad_L / -_GradientHeight_L);
					colorLeft  = lerp(_Color1_L, _Color2_L, GradientFactor_L);
				#endif
				#if RIGHTGRADIENT
					half RotatedGrad_R = (GradPos.z - _GradientYStartPos_R.x) * sin(_Rotation_R/57.32) + (GradPos.y - _GradientYStartPos_R.y) * cos(_Rotation_R/57.32);
					half GradientFactor_R = saturate(RotatedGrad_R / -_GradientHeight_R);
					colorRight = lerp(_Color1_R, _Color2_R, GradientFactor_R);
				#endif
				#if TOPGRADIENT
					half RotatedGrad_T = (GradPos.z - _GradientXStartPos_T.x) * cos(_Rotation_T/57.32) + (GradPos.x - _GradientXStartPos_T.y) * sin(_Rotation_T/57.32);
					half GradientFactor_T = saturate(RotatedGrad_T / -_GradientHeight_T);
					colorTop   = lerp(_Color1_T, _Color2_T, GradientFactor_T);
				#endif
				#if BOTTOMGRADIENT
					half RotatedGrad_D = (GradPos.z - _GradientXStartPos_D.x) * cos(_Rotation_D/57.32) + (GradPos.x - _GradientXStartPos_D.y) * sin(_Rotation_D/57.32);
					half GradientFactor_D = saturate(RotatedGrad_D / -_GradientHeight_D);
					colorDown  = lerp(_Color1_D, _Color2_D, GradientFactor_D);
				#endif
				
				
				half3 Maincolor;
				#if DONTMIX
					Maincolor = colorFront * dirFront + colorBack * dirBack + colorLeft * dirLeft + colorRight * dirRight + colorTop * dirTop + colorDown * dirBottom;
				#else
					Maincolor = lerp(colorFront, whiteColor, 1-dirFront) * lerp(colorBack, whiteColor, 1-dirBack) * lerp(colorLeft, whiteColor, 1-dirLeft) * lerp(colorRight, whiteColor, 1-dirRight) * lerp(colorTop, whiteColor, 1-dirTop) * lerp(colorDown, whiteColor, 1-dirBottom);
				#endif
				o.customLighting = Maincolor + (_AmbientColor * _AmbientPower);
				
				//Lightmap
				#if	LIGHTMAP_ADD || LIGHTMAP_MULTIPLY || LIGHTMAP_AO
					o.lightmapUV = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				// Transfer realtime shadows
				#if SHADOW_ON
					TRANSFER_SHADOW(o);
					o.normalDir = UnityObjectToWorldNormal(v.normal);
				#endif
				//Apply Unity fog
				#if UNITY_FOG
					UNITY_TRANSFER_FOG(o, o.pos);
				#endif
				
				#if USERIM
					float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
					o.rimCol = pow(1 - dot(o.normalDir, viewDir), _RimPower);
				#endif
				
				return o;
			}
			
			half4 frag(vertexOutput i) : COLOR
			{
				half4 Result = half4(whiteColor,1);
				//applying main texture
				#if TEXTUREMODULE_ON
					half4 _MainTexture_var = tex2D(_MainTexture, i.uv) + half4(_MainTexturePower, _MainTexturePower, _MainTexturePower, 0);
					_MainTexture_var = clamp(_MainTexture_var, 0.0, 1.0);
					Result *= _MainTexture_var;
				#endif
				//Applying AO
				#if AO_ON_UV0
					half4 AOTexVar = lerp(half4(_AOColor, 1), half4(whiteColor, 1), lerp(half4(1,1,1,1), tex2D(_AOTexture, i.aouv), _AOPower));
					Result *= AOTexVar;
				#endif
				#if AO_ON_UV1
					half4 AOTexVar = lerp(half4(_AOColor, 1), half4(whiteColor, 1), lerp(half4(1,1,1,1), tex2D(_AOTexture, i.aouv), _AOPower));
					Result *= AOTexVar;
				#endif
				//applying custom Lighting Data
				Result *= half4(i.customLighting, 1);
				
				//Calculating Lightmap
				#if LIGHTMAP_AO
					half4 lmColor = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV);
					half4 lmPower = lerp(half4(1,1,1,1), half4(DecodeLightmap(lmColor), 0), _LMPower);
					Result *= lerp(half4(_LMColor, 0), half4(1,1,1,1) , lmPower);
				#endif
				#if LIGHTMAP_ADD
					Result += (half4(DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV)), 0) * _LMPower);
					Result = clamp(Result, 0.0, 1.0);
				#endif
				#if LIGHTMAP_MULTIPLY
					Result *= half4(DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV)), 0) * _LMPower;
					Result = clamp(Result, 0.0, 1.0);
				#endif
				//Calculating Shadows with shadow color
				#if SHADOW_ON
					i.normalDir = normalize(i.normalDir);
				    float3 normalDirection = i.normalDir;
                    float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.worldPos.xyz,_WorldSpaceLightPos0.w));
                    half attenuation = LIGHT_ATTENUATION(i);
                    float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                    float shadow = pow(max( 0.0, NdotL) * attenuation, _ShadowInfluence);
					half4 shadowColor = lerp(half4(_ShadowColor, 1), Result, shadow);
					Result = lerp(Result, shadowColor, _ShadowBlend);
				#endif
				 
				#if USERIM
					Result = half4(lerp(Result.rgb, _RimColor, i.rimCol), 1);
				#endif
				
				#if COLORCORRECTION_ON
					Result *= half4(_TintColor, 1);
					Result = clamp(Result + Result * _Brightness, 0.0, 1.0);
					half maxVal = max(max(Result.r, Result.g), Result.b);
					Result = half4(lerp(Result.r, maxVal, 1-_Saturation), lerp(Result.g, maxVal, 1-_Saturation), lerp(Result.b, maxVal, 1-_Saturation), Result.a);
				#endif
				
				//HeightFog
				#if HEIGHT_FOG
					half3 fogGradient = lerp(_Color_Fog, Result.rgb,  smoothstep( _FogYStartPos, _FogHeight, i.worldPos.y )).rgb;
					Result = half4(fogGradient, 1);
				#endif
				//Unity Fog
				#if UNITY_FOG
					UNITY_APPLY_FOG(i.fogCoord, Result);
				#endif
				Result *= half4(1, 1, 1, _Fade);
				return  Result;
			}
			ENDCG
		}
	}
	FallBack "Minimalist/Standard"
	CustomEditor "Minimalist.MinimalistStandardEditor"
}