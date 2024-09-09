Shader "HDRP/6.0.17/OceanExpanse"
{
    Properties
    {
        [HideInInspector]_EmissionColor("Color", Color) = (1, 1, 1, 1)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="HDRenderPipeline"
            "RenderType"="HDLitShader"
            "Queue"="Transparent+0"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="WaterSubTarget"
        }
        Pass
        {
            Name "WaterGBuffer"
            Tags
            {
                "LightMode" = "WaterGBuffer"
            }

            // Render State
            Cull [_CullWaterMask]
        ZTest LEqual
        ZWrite On
        Stencil
        {
        ReadMask [_StencilWaterReadMaskGBuffer]
        WriteMask [_StencilWaterWriteMaskGBuffer]
        Ref [_StencilWaterRefGBuffer]
        CompFront Equal
        FailFront Keep
        PassFront Replace
        CompBack Equal
        FailBack Keep
        PassBack Replace
        }

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 5.0
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma instancing_options procedural:SetupInstanceID

            // Keywords
            #pragma multi_compile WATER_ONE_BAND WATER_TWO_BANDS WATER_THREE_BANDS
        #pragma multi_compile _ WATER_LOCAL_CURRENT
        #pragma multi_compile WATER_DECAL_PARTIAL WATER_DECAL_COMPLETE
        #pragma multi_compile _ PROCEDURAL_INSTANCING_ON
        #pragma multi_compile _ STEREO_INSTANCING_ON
        #pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
            // GraphKeywords: <None>

            // Defines
            #define SHADERPASS SHADERPASS_GBUFFER
        #define SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        #define HAS_REFRACTION 1
        #define WATER_SURFACE_GBUFFER 1
        #define DECAL_SURFACE_GRADIENT 1
        #define USE_CLUSTERED_LIGHTLIST 1
        #define PUNCTUAL_SHADOW_LOW
        #define DIRECTIONAL_SHADOW_LOW
        #define AREA_SHADOW_MEDIUM
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        #define WATER_DISPLACEMENT 1
        #define SUPPORT_GLOBAL_MIP_BIAS 1
        #define REQUIRE_DEPTH_TEXTURE

            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT



            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl" // Required before including properties as it defines UNITY_TEXTURE_STREAMING_DEBUG_VARS
            // Always include Shader Graph version
            // Always include last to avoid double macros
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl" // Need to be here for Gradient struct definition

            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD1
        
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
            // Define when IsFontFaceNode is included in ShaderGraph
            #define VARYINGS_NEED_CULLFACE
        
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _EmissionColor;
        CBUFFER_END
        
        
        // Object and Global properties
        float _StencilWaterRefGBuffer;
        float _StencilWaterWriteMaskGBuffer;
        float _StencilWaterReadMaskGBuffer;
        float _CullWaterMask;
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Water/Water.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
            // GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 WorldSpaceViewDirection;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 ScreenPosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float4 uv1;
             float FaceSign;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        StructuredBuffer<int2> _DepthPyramidMipLevelOffsets;
        float Unity_HDRP_SampleSceneDepth_float(float2 uv, float lod)
        {
            #if defined(REQUIRE_DEPTH_TEXTURE) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
            int2 coord = int2(uv * _ScreenSize.xy);
            int2 mipCoord  = coord.xy >> int(lod);
            int2 mipOffset = _DepthPyramidMipLevelOffsets[int(lod)];
            return LOAD_TEXTURE2D_X(_CameraDepthTexture, mipOffset + mipCoord).r;
            #endif
            return 0.0;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
        {
            float2 delta = UV - Center;
            float radius = length(delta) * 2 * RadialScale;
            float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
            Out = float2(radius, angle);
        }
        
        void Unity_Contrast_float(float3 In, float Contrast, out float3 Out)
        {
            float midpoint = pow(0.5, 2.2);
            Out =  (In - midpoint) * Contrast + midpoint;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float4 uv0;
            float4 uv1;
            float LowFrequencyHeight;
            float3 Displacement;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            WaterDisplacementData displacementData;
            ZERO_INITIALIZE(WaterDisplacementData, displacementData);
            EvaluateWaterDisplacement(IN.ObjectSpacePosition, displacementData);
            float3 _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3 = displacementData.displacement;
            float _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float = displacementData.lowFrequencyHeight;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.uv0 = float4 (0, 0, 0, 0);
            description.uv1 = float4 (0, 0, 0, 0);
            description.LowFrequencyHeight = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float;
            description.Displacement = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Smoothness;
            float3 BaseColor;
            float3 NormalWS;
            float3 LowFrequencyNormalWS;
            float3 RefractedPositionWS;
            float TipThickness;
            float Caustics;
            float Foam;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            WaterAdditionalData waterAdditionalData;
            ZERO_INITIALIZE(WaterAdditionalData, waterAdditionalData);
            EvaluateWaterAdditionalData(IN.uv0.xzy, IN.WorldSpacePosition, IN.WorldSpaceNormal, waterAdditionalData);
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3 = waterAdditionalData.normalWS;
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3 = waterAdditionalData.lowFrequencyNormalWS;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float = waterAdditionalData.surfaceFoam;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float = waterAdditionalData.deepFoam;
            float _HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float = LinearEyeDepth(Unity_HDRP_SampleSceneDepth_float(float4(IN.NDCPosition.xy, 0, 0).xy, float(0)), _ZBufferParams);
            float4 _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4 = IN.ScreenPosition;
            float _Split_edf17f69cb8548e982b55e2142415f35_R_1_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[0];
            float _Split_edf17f69cb8548e982b55e2142415f35_G_2_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[1];
            float _Split_edf17f69cb8548e982b55e2142415f35_B_3_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[2];
            float _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[3];
            float _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float;
            Unity_Subtract_float(_HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float, _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float, _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float);
            float _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float;
            Unity_OneMinus_float(_Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float, _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float);
            float _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float;
            Unity_Saturate_float(_OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float, _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float);
            float _Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_b70d02d8fd164d758d904904b216e5f3_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_b70d02d8fd164d758d904904b216e5f3_A_4_Float = 0;
            float2 _Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2 = float2(_Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float, _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float);
            float _Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float = float(700);
            float2 _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2, (_Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float.xx), _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2);
            float2 _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2 = float2(float(0), float(-0.04));
            float2 _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2;
            Unity_PolarCoordinates_float(_Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2, _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2, float(1), float(1), _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2);
            float3 _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3;
            Unity_Contrast_float((float3(_PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2, 0.0)), float(10), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3);
            float3 _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float.xxx), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3, _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3);
            FoamData foamData;
            ZERO_INITIALIZE(FoamData, foamData);
            EvaluateFoamData(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float, (_Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3).x, IN.uv0.xzy, foamData);
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float = foamData.smoothness;
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float = foamData.foamValue;
            float3 refractedPos;
            float2 distordedNDC;
            float3 absorptionTint;
            ComputeWaterRefractionParams(IN.WorldSpacePosition, float4(IN.NDCPosition.xy, 0, 0).xy, IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, IN.FaceSign, false, _WaterUpDirection.xyz, _MaxRefractionDistance, _WaterExtinction.xyz, refractedPos, distordedNDC, absorptionTint);
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3 = refractedPos;
            float2 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2 = distordedNDC;
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3 = absorptionTint;
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float;
            Unity_Multiply_float_float(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float, 0.25, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float3 _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3 = EvaluateScatteringColor(IN.uv0.xzy, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float, _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float = EvaluateTipThickness(IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float);
            float _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float = EvaluateSimulationCaustics(_EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, abs(dot(IN.WorldSpacePosition - _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, _WaterUpDirection.xyz)), _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2);
            surface.Smoothness = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float;
            surface.BaseColor = _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3;
            surface.NormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3;
            surface.LowFrequencyNormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3;
            surface.RefractedPositionWS = _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3;
            surface.TipThickness = _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float;
            surface.Caustics = _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float;
            surface.Foam = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            // The only parameters that can be requested are the position, normal and time parameters
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
            output.ObjectSpaceNormal =                          input.normalOS;
            output.WorldSpaceNormal =                           TransformObjectToWorldNormal(input.normalOS);
        
            return output;
        }
        
        void PackWaterVertexData(VertexDescription vertex, out float4 uv0, out float4 uv1)
        {
        #if defined(SHADER_STAGE_VERTEX) && defined(TESSELLATION_ON)
            uv0 = float4(vertex.Displacement, 1.0);
            uv1 = float4(vertex.Position, 1.0);
        #else
            uv0.xy = vertex.Position.xz;
            uv0.z = vertex.Displacement.y;
            uv0.w = length(vertex.Displacement.xz);
        
            if (_GridSize.x >= 0)
                uv1.xyz = TransformObjectToWorld(vertex.Position + vertex.Displacement);
            uv1.w = vertex.LowFrequencyHeight;
        #endif
        }
        
        #if defined(TESSELLATION_ON)
            #define VaryingsMeshType VaryingsMeshToDS
        #else
            #define VaryingsMeshType VaryingsMeshToPS
        #endif
        
        // Modifications should probably be replicated to ApplyTessellationModification
        void ApplyMeshModification(AttributesMesh input, float3 timeParameters, inout VaryingsMeshType varyings, out VertexDescription vertexDescription)
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
        
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
            vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        
            // Backward compatibility with old graphs
        
            // Custom interpolators
            
        }
        
        #undef VaryingsMeshType
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.texCoord1.xyz;
            output.positionPixel =              input.positionCS.xy; // NOTE: this is not actually in clip space, it is the VPOS pixel coordinate value
            output.tangentToWorld =             GetLocalFrame(input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord1 =                  input.texCoord1;
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            output.WorldSpaceNormal =                           normalize(input.tangentToWorld[2].xyz);
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
            output.WorldSpaceViewDirection =                    normalize(viewWS);
            output.WorldSpacePosition =                         input.positionRWS;
            output.AbsoluteWorldSpacePosition =                 GetAbsolutePositionWS(input.positionRWS);
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
        
        #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x < 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #else
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x > 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 =                                        input.texCoord0;
            output.uv1 =                                        input.texCoord1;
            output.FaceSign =                                   input.isFrontFace;
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void ApplyDecalToSurfaceDataNoNormal(DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData);
        
        void ApplyDecalAndGetNormal(FragInputs fragInputs, PositionInputs posInput, SurfaceDescription surfaceDescription,
            inout SurfaceData surfaceData)
        {
            float3 doubleSidedConstants = GetDoubleSidedConstants();
        
        #ifdef DECAL_NORMAL_BLENDING
            // SG nodes don't ouptut surface gradients, so if decals require surf grad blending, we have to convert
            // the normal to gradient before applying the decal. We then have to resolve the gradient back to world space
            float3 normalTS;
        
        
            normalTS = SurfaceGradientFromPerturbedNormal(fragInputs.tangentToWorld[2],
            surfaceDescription.NormalWS);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        
            GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        #else
            // normal delivered to master node
            GetNormalWS_SrcWS(fragInputs, surfaceDescription.NormalWS, surfaceData.normalWS, doubleSidedConstants);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                // Both uses and modifies 'surfaceData.normalWS'.
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        #endif
        }
        void ApplyDecalToSurfaceData(DecalSurfaceData decalSurfaceData, float3 vtxNormal, inout SurfaceData surfaceData)
        {
            // using alpha compositing https://developer.nvidia.com/gpugems/GPUGems3/gpugems3_ch23.html
            float decalFoam = Luminance(decalSurfaceData.baseColor.xyz);
            surfaceData.foam = surfaceData.foam * decalSurfaceData.baseColor.w + decalFoam;
        
            // Always test the normal as we can have decompression artifact
            if (decalSurfaceData.normalWS.w < 1.0)
            {
                // Re-evaluate the surface gradient of the simulation normal
                float3 normalSG = SurfaceGradientFromPerturbedNormal(vtxNormal, surfaceData.normalWS);
                // Add the contribution of the decals to the normal
                normalSG += SurfaceGradientFromVolumeGradient(vtxNormal, decalSurfaceData.normalWS.xyz);
                // Move back to world space
                surfaceData.normalWS.xyz = SurfaceGradientResolveNormal(vtxNormal, normalSG);
            }
        
            surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
        }
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            surfaceData.baseColor =                 surfaceDescription.BaseColor;
        
            surfaceData.normalWS =                  surfaceDescription.NormalWS;
            surfaceData.lowFrequencyNormalWS =      surfaceDescription.LowFrequencyNormalWS;
        
            surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
            surfaceData.foam =                      surfaceDescription.Foam;
        
            surfaceData.tipThickness =              surfaceDescription.TipThickness;
            surfaceData.caustics =                  surfaceDescription.Caustics;
            surfaceData.refractedPositionWS =       surfaceDescription.RefractedPositionWS;
        
            bentNormalWS = float3(0, 1, 0);
        
            #if HAVE_DECALS
                if (_EnableDecals)
                {
                    float alpha = 1.0;
                    // Both uses and modifies 'surfaceData.normalWS'.
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, _WaterRenderingLayer, alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
                }
            #endif
        
            // Kill the scattering and the refraction based on where foam is perceived
            surfaceData.baseColor *= (1 - saturate(surfaceData.foam));
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                }
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Water/Shaders/ShaderPassWaterGBuffer.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "WaterGBufferTessellation"
            Tags
            {
                "LightMode" = "WaterGBufferTessellation"
            }
        
            // Render State
            Cull [_CullWaterMask]
        ZTest LEqual
        ZWrite On
        Stencil
        {
        ReadMask [_StencilWaterReadMaskGBuffer]
        WriteMask [_StencilWaterWriteMaskGBuffer]
        Ref [_StencilWaterRefGBuffer]
        CompFront Equal
        FailFront Keep
        PassFront Replace
        CompBack Equal
        FailBack Keep
        PassBack Replace
        }
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma instancing_options procedural:SetupInstanceID
        #pragma hull Hull
        #pragma domain Domain
        
            // Keywords
            #pragma multi_compile WATER_ONE_BAND WATER_TWO_BANDS WATER_THREE_BANDS
        #pragma multi_compile _ WATER_LOCAL_CURRENT
        #pragma multi_compile WATER_DECAL_PARTIAL WATER_DECAL_COMPLETE
        #pragma multi_compile _ PROCEDURAL_INSTANCING_ON
        #pragma multi_compile _ STEREO_INSTANCING_ON
        #pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
            // GraphKeywords: <None>
        
            // Defines
            #define SHADERPASS SHADERPASS_GBUFFER
        #define SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        #define HAS_REFRACTION 1
        #define WATER_SURFACE_GBUFFER 1
        #define DECAL_SURFACE_GRADIENT 1
        #define USE_CLUSTERED_LIGHTLIST 1
        #define PUNCTUAL_SHADOW_LOW
        #define DIRECTIONAL_SHADOW_LOW
        #define AREA_SHADOW_MEDIUM
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        #define TESSELLATION_ON 1
        #define HAVE_TESSELLATION_MODIFICATION 1
        #define WATER_DISPLACEMENT 1
        #define SUPPORT_GLOBAL_MIP_BIAS 1
        #define REQUIRE_DEPTH_TEXTURE
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl" // Required before including properties as it defines UNITY_TEXTURE_STREAMING_DEBUG_VARS
            // Always include Shader Graph version
            // Always include last to avoid double macros
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD1
        
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
            // Define when IsFontFaceNode is included in ShaderGraph
            #define VARYINGS_NEED_CULLFACE
        
        
            #ifdef TESSELLATION_ON
            // World and normal are always available
                #define VARYINGS_DS_NEED_TANGENT
            #define VARYINGS_DS_NEED_TEXCOORD0
            #define VARYINGS_DS_NEED_TEXCOORD1
                        	#endif
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _EmissionColor;
        CBUFFER_END
        
        
        // Object and Global properties
        float _StencilWaterRefGBuffer;
        float _StencilWaterWriteMaskGBuffer;
        float _StencilWaterReadMaskGBuffer;
        float _CullWaterMask;
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Water/Water.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
            // GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToDS
        {
             float3 positionRWS;
             float tessellationFactor;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 WorldSpaceViewDirection;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 ScreenPosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float4 uv1;
             float FaceSign;
        };
        struct PackedVaryingsMeshToDS
        {
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 packed_positionRWS_tessellationFactor : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToDS PackVaryingsMeshToDS (VaryingsMeshToDS input)
        {
            PackedVaryingsMeshToDS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToDS, output);
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.packed_positionRWS_tessellationFactor.xyz = input.positionRWS;
            output.packed_positionRWS_tessellationFactor.w = input.tessellationFactor;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToDS UnpackVaryingsMeshToDS (PackedVaryingsMeshToDS input)
        {
            VaryingsMeshToDS output;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.positionRWS = input.packed_positionRWS_tessellationFactor.xyz;
            output.tessellationFactor = input.packed_positionRWS_tessellationFactor.w;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        StructuredBuffer<int2> _DepthPyramidMipLevelOffsets;
        float Unity_HDRP_SampleSceneDepth_float(float2 uv, float lod)
        {
            #if defined(REQUIRE_DEPTH_TEXTURE) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
            int2 coord = int2(uv * _ScreenSize.xy);
            int2 mipCoord  = coord.xy >> int(lod);
            int2 mipOffset = _DepthPyramidMipLevelOffsets[int(lod)];
            return LOAD_TEXTURE2D_X(_CameraDepthTexture, mipOffset + mipCoord).r;
            #endif
            return 0.0;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
        {
            float2 delta = UV - Center;
            float radius = length(delta) * 2 * RadialScale;
            float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
            Out = float2(radius, angle);
        }
        
        void Unity_Contrast_float(float3 In, float Contrast, out float3 Out)
        {
            float midpoint = pow(0.5, 2.2);
            Out =  (In - midpoint) * Contrast + midpoint;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float4 uv0;
            float4 uv1;
            float LowFrequencyHeight;
            float3 Displacement;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            WaterDisplacementData displacementData;
            ZERO_INITIALIZE(WaterDisplacementData, displacementData);
            EvaluateWaterDisplacement(IN.ObjectSpacePosition, displacementData);
            float3 _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3 = displacementData.displacement;
            float _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float = displacementData.lowFrequencyHeight;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.uv0 = float4 (0, 0, 0, 0);
            description.uv1 = float4 (0, 0, 0, 0);
            description.LowFrequencyHeight = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float;
            description.Displacement = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Smoothness;
            float3 BaseColor;
            float3 NormalWS;
            float3 LowFrequencyNormalWS;
            float3 RefractedPositionWS;
            float TipThickness;
            float Caustics;
            float Foam;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            WaterAdditionalData waterAdditionalData;
            ZERO_INITIALIZE(WaterAdditionalData, waterAdditionalData);
            EvaluateWaterAdditionalData(IN.uv0.xzy, IN.WorldSpacePosition, IN.WorldSpaceNormal, waterAdditionalData);
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3 = waterAdditionalData.normalWS;
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3 = waterAdditionalData.lowFrequencyNormalWS;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float = waterAdditionalData.surfaceFoam;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float = waterAdditionalData.deepFoam;
            float _HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float = LinearEyeDepth(Unity_HDRP_SampleSceneDepth_float(float4(IN.NDCPosition.xy, 0, 0).xy, float(0)), _ZBufferParams);
            float4 _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4 = IN.ScreenPosition;
            float _Split_edf17f69cb8548e982b55e2142415f35_R_1_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[0];
            float _Split_edf17f69cb8548e982b55e2142415f35_G_2_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[1];
            float _Split_edf17f69cb8548e982b55e2142415f35_B_3_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[2];
            float _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[3];
            float _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float;
            Unity_Subtract_float(_HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float, _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float, _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float);
            float _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float;
            Unity_OneMinus_float(_Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float, _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float);
            float _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float;
            Unity_Saturate_float(_OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float, _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float);
            float _Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_b70d02d8fd164d758d904904b216e5f3_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_b70d02d8fd164d758d904904b216e5f3_A_4_Float = 0;
            float2 _Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2 = float2(_Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float, _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float);
            float _Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float = float(700);
            float2 _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2, (_Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float.xx), _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2);
            float2 _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2 = float2(float(0), float(-0.04));
            float2 _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2;
            Unity_PolarCoordinates_float(_Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2, _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2, float(1), float(1), _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2);
            float3 _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3;
            Unity_Contrast_float((float3(_PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2, 0.0)), float(10), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3);
            float3 _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float.xxx), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3, _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3);
            FoamData foamData;
            ZERO_INITIALIZE(FoamData, foamData);
            EvaluateFoamData(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float, (_Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3).x, IN.uv0.xzy, foamData);
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float = foamData.smoothness;
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float = foamData.foamValue;
            float3 refractedPos;
            float2 distordedNDC;
            float3 absorptionTint;
            ComputeWaterRefractionParams(IN.WorldSpacePosition, float4(IN.NDCPosition.xy, 0, 0).xy, IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, IN.FaceSign, false, _WaterUpDirection.xyz, _MaxRefractionDistance, _WaterExtinction.xyz, refractedPos, distordedNDC, absorptionTint);
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3 = refractedPos;
            float2 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2 = distordedNDC;
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3 = absorptionTint;
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float;
            Unity_Multiply_float_float(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float, 0.25, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float3 _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3 = EvaluateScatteringColor(IN.uv0.xzy, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float, _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float = EvaluateTipThickness(IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float);
            float _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float = EvaluateSimulationCaustics(_EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, abs(dot(IN.WorldSpacePosition - _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, _WaterUpDirection.xyz)), _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2);
            surface.Smoothness = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float;
            surface.BaseColor = _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3;
            surface.NormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3;
            surface.LowFrequencyNormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3;
            surface.RefractedPositionWS = _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3;
            surface.TipThickness = _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float;
            surface.Caustics = _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float;
            surface.Foam = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            // The only parameters that can be requested are the position, normal and time parameters
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
            output.ObjectSpaceNormal =                          input.normalOS;
            output.WorldSpaceNormal =                           TransformObjectToWorldNormal(input.normalOS);
        
            return output;
        }
        
        void PackWaterVertexData(VertexDescription vertex, out float4 uv0, out float4 uv1)
        {
        #if defined(SHADER_STAGE_VERTEX) && defined(TESSELLATION_ON)
            uv0 = float4(vertex.Displacement, 1.0);
            uv1 = float4(vertex.Position, 1.0);
        #else
            uv0.xy = vertex.Position.xz;
            uv0.z = vertex.Displacement.y;
            uv0.w = length(vertex.Displacement.xz);
        
            if (_GridSize.x >= 0)
                uv1.xyz = TransformObjectToWorld(vertex.Position + vertex.Displacement);
            uv1.w = vertex.LowFrequencyHeight;
        #endif
        }
        
        #if defined(TESSELLATION_ON)
            #define VaryingsMeshType VaryingsMeshToDS
        #else
            #define VaryingsMeshType VaryingsMeshToPS
        #endif
        
        // Modifications should probably be replicated to ApplyTessellationModification
        void ApplyMeshModification(AttributesMesh input, float3 timeParameters, inout VaryingsMeshType varyings, out VertexDescription vertexDescription)
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
        
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
            vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        
            // Backward compatibility with old graphs
        
            // Custom interpolators
            
        }
        
        #undef VaryingsMeshType
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.texCoord1.xyz;
            output.positionPixel =              input.positionCS.xy; // NOTE: this is not actually in clip space, it is the VPOS pixel coordinate value
            output.tangentToWorld =             GetLocalFrame(input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord1 =                  input.texCoord1;
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            #ifdef TESSELLATION_ON
        
        // TODO: We should generate this struct like all the other varying struct
        VaryingsMeshToDS InterpolateWithBaryCoordsMeshToDS(VaryingsMeshToDS input0, VaryingsMeshToDS input1, VaryingsMeshToDS input2, float3 baryCoords)
        {
            VaryingsMeshToDS output;
        
            UNITY_TRANSFER_INSTANCE_ID(input0, output);
        
            // The set of values that need to interpolated is fixed
            TESSELLATION_INTERPOLATE_BARY(positionRWS, baryCoords);
            TESSELLATION_INTERPOLATE_BARY(normalWS, baryCoords);
            TESSELLATION_INTERPOLATE_BARY(texCoord0, baryCoords);
            TESSELLATION_INTERPOLATE_BARY(texCoord1, baryCoords);
        
            // Pass-Through for custom interpolator
            
        
            return output;
        }
        
        VertexDescriptionInputs VaryingsMeshToDSToVertexDescriptionInputs(VaryingsMeshToDS input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            // texcoord1 contains the pre displacement object space position
            // normal is marked WS but it's actually in object space :(
            output.ObjectSpacePosition = input.texCoord1.xyz;
            output.WorldSpacePosition  = TransformObjectToWorld(input.texCoord1.xyz);
            output.ObjectSpaceNormal   = input.normalWS;
            output.WorldSpaceNormal    = TransformObjectToWorldNormal(input.normalWS);
        
        
            return output;
        }
        
        // tessellationFactors
        // x - 1->2 edge
        // y - 2->0 edge
        // z - 0->1 edge
        // w - inside tessellation factor
        void ApplyTessellationModification(VaryingsMeshToDS input, float3 timeParameters, inout VaryingsMeshToPS output, out VertexDescription vertexDescription)
        {
            // HACK: As there is no specific tessellation stage for now in shadergraph, we reuse the vertex description mechanism.
            // It mean we store TessellationFactor inside vertex description causing extra read on both vertex and hull stage, but unused parameters are optimize out by the shader compiler, so no impact.
            VertexDescriptionInputs vertexDescriptionInputs = VaryingsMeshToDSToVertexDescriptionInputs(input);
        
            // Override time parameters with used one (This is required to correctly handle motion vector for tessellation animation based on time)
        
            // evaluate vertex graph
            vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        
            // Backward compatibility with old graphs
        
            // Custom interpolators
            
        }
        
        #endif // TESSELLATION_ON
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            output.WorldSpaceNormal =                           normalize(input.tangentToWorld[2].xyz);
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
            output.WorldSpaceViewDirection =                    normalize(viewWS);
            output.WorldSpacePosition =                         input.positionRWS;
            output.AbsoluteWorldSpacePosition =                 GetAbsolutePositionWS(input.positionRWS);
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
        
        #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x < 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #else
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x > 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 =                                        input.texCoord0;
            output.uv1 =                                        input.texCoord1;
            output.FaceSign =                                   input.isFrontFace;
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void ApplyDecalToSurfaceDataNoNormal(DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData);
        
        void ApplyDecalAndGetNormal(FragInputs fragInputs, PositionInputs posInput, SurfaceDescription surfaceDescription,
            inout SurfaceData surfaceData)
        {
            float3 doubleSidedConstants = GetDoubleSidedConstants();
        
        #ifdef DECAL_NORMAL_BLENDING
            // SG nodes don't ouptut surface gradients, so if decals require surf grad blending, we have to convert
            // the normal to gradient before applying the decal. We then have to resolve the gradient back to world space
            float3 normalTS;
        
        
            normalTS = SurfaceGradientFromPerturbedNormal(fragInputs.tangentToWorld[2],
            surfaceDescription.NormalWS);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        
            GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        #else
            // normal delivered to master node
            GetNormalWS_SrcWS(fragInputs, surfaceDescription.NormalWS, surfaceData.normalWS, doubleSidedConstants);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                // Both uses and modifies 'surfaceData.normalWS'.
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        #endif
        }
        void ApplyDecalToSurfaceData(DecalSurfaceData decalSurfaceData, float3 vtxNormal, inout SurfaceData surfaceData)
        {
            // using alpha compositing https://developer.nvidia.com/gpugems/GPUGems3/gpugems3_ch23.html
            float decalFoam = Luminance(decalSurfaceData.baseColor.xyz);
            surfaceData.foam = surfaceData.foam * decalSurfaceData.baseColor.w + decalFoam;
        
            // Always test the normal as we can have decompression artifact
            if (decalSurfaceData.normalWS.w < 1.0)
            {
                // Re-evaluate the surface gradient of the simulation normal
                float3 normalSG = SurfaceGradientFromPerturbedNormal(vtxNormal, surfaceData.normalWS);
                // Add the contribution of the decals to the normal
                normalSG += SurfaceGradientFromVolumeGradient(vtxNormal, decalSurfaceData.normalWS.xyz);
                // Move back to world space
                surfaceData.normalWS.xyz = SurfaceGradientResolveNormal(vtxNormal, normalSG);
            }
        
            surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
        }
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            surfaceData.baseColor =                 surfaceDescription.BaseColor;
        
            surfaceData.normalWS =                  surfaceDescription.NormalWS;
            surfaceData.lowFrequencyNormalWS =      surfaceDescription.LowFrequencyNormalWS;
        
            surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
            surfaceData.foam =                      surfaceDescription.Foam;
        
            surfaceData.tipThickness =              surfaceDescription.TipThickness;
            surfaceData.caustics =                  surfaceDescription.Caustics;
            surfaceData.refractedPositionWS =       surfaceDescription.RefractedPositionWS;
        
            bentNormalWS = float3(0, 1, 0);
        
            #if HAVE_DECALS
                if (_EnableDecals)
                {
                    float alpha = 1.0;
                    // Both uses and modifies 'surfaceData.normalWS'.
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, _WaterRenderingLayer, alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
                }
            #endif
        
            // Kill the scattering and the refraction based on where foam is perceived
            surfaceData.baseColor *= (1 - saturate(surfaceData.foam));
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                }
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Water/Shaders/ShaderPassWaterGBuffer.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "LowRes"
            Tags
            {
                "LightMode" = "LowRes"
            }
        
            // Render State
            Cull [_CullWaterMask]
        ZTest LEqual
        ZWrite On
        Stencil
        {
        ReadMask [_StencilWaterReadMaskGBuffer]
        WriteMask [_StencilWaterWriteMaskGBuffer]
        Ref [_StencilWaterRefGBuffer]
        CompFront Equal
        FailFront Keep
        PassFront Replace
        CompBack Equal
        FailBack Keep
        PassBack Replace
        }
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma instancing_options procedural:SetupInstanceID
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            // Defines
            #define SHADERPASS SHADERPASS_GBUFFER
        #define SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        #define HAS_REFRACTION 1
        #define WATER_SURFACE_GBUFFER 1
        #define DECAL_SURFACE_GRADIENT 1
        #define USE_CLUSTERED_LIGHTLIST 1
        #define PUNCTUAL_SHADOW_LOW
        #define DIRECTIONAL_SHADOW_LOW
        #define AREA_SHADOW_MEDIUM
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        #define SUPPORT_GLOBAL_MIP_BIAS 1
        #define REQUIRE_DEPTH_TEXTURE
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl" // Required before including properties as it defines UNITY_TEXTURE_STREAMING_DEBUG_VARS
            // Always include Shader Graph version
            // Always include last to avoid double macros
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD1
        
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
            // Define when IsFontFaceNode is included in ShaderGraph
            #define VARYINGS_NEED_CULLFACE
        
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _EmissionColor;
        CBUFFER_END
        
        
        // Object and Global properties
        float _StencilWaterRefGBuffer;
        float _StencilWaterWriteMaskGBuffer;
        float _StencilWaterReadMaskGBuffer;
        float _CullWaterMask;
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Water/Water.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
            // GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 WorldSpaceViewDirection;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 ScreenPosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float4 uv1;
             float FaceSign;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        StructuredBuffer<int2> _DepthPyramidMipLevelOffsets;
        float Unity_HDRP_SampleSceneDepth_float(float2 uv, float lod)
        {
            #if defined(REQUIRE_DEPTH_TEXTURE) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
            int2 coord = int2(uv * _ScreenSize.xy);
            int2 mipCoord  = coord.xy >> int(lod);
            int2 mipOffset = _DepthPyramidMipLevelOffsets[int(lod)];
            return LOAD_TEXTURE2D_X(_CameraDepthTexture, mipOffset + mipCoord).r;
            #endif
            return 0.0;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
        {
            float2 delta = UV - Center;
            float radius = length(delta) * 2 * RadialScale;
            float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
            Out = float2(radius, angle);
        }
        
        void Unity_Contrast_float(float3 In, float Contrast, out float3 Out)
        {
            float midpoint = pow(0.5, 2.2);
            Out =  (In - midpoint) * Contrast + midpoint;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float4 uv0;
            float4 uv1;
            float LowFrequencyHeight;
            float3 Displacement;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            WaterDisplacementData displacementData;
            ZERO_INITIALIZE(WaterDisplacementData, displacementData);
            EvaluateWaterDisplacement(IN.ObjectSpacePosition, displacementData);
            float3 _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3 = displacementData.displacement;
            float _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float = displacementData.lowFrequencyHeight;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.uv0 = float4 (0, 0, 0, 0);
            description.uv1 = float4 (0, 0, 0, 0);
            description.LowFrequencyHeight = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float;
            description.Displacement = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Smoothness;
            float3 BaseColor;
            float3 NormalWS;
            float3 LowFrequencyNormalWS;
            float3 RefractedPositionWS;
            float TipThickness;
            float Caustics;
            float Foam;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            WaterAdditionalData waterAdditionalData;
            ZERO_INITIALIZE(WaterAdditionalData, waterAdditionalData);
            EvaluateWaterAdditionalData(IN.uv0.xzy, IN.WorldSpacePosition, IN.WorldSpaceNormal, waterAdditionalData);
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3 = waterAdditionalData.normalWS;
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3 = waterAdditionalData.lowFrequencyNormalWS;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float = waterAdditionalData.surfaceFoam;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float = waterAdditionalData.deepFoam;
            float _HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float = LinearEyeDepth(Unity_HDRP_SampleSceneDepth_float(float4(IN.NDCPosition.xy, 0, 0).xy, float(0)), _ZBufferParams);
            float4 _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4 = IN.ScreenPosition;
            float _Split_edf17f69cb8548e982b55e2142415f35_R_1_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[0];
            float _Split_edf17f69cb8548e982b55e2142415f35_G_2_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[1];
            float _Split_edf17f69cb8548e982b55e2142415f35_B_3_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[2];
            float _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[3];
            float _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float;
            Unity_Subtract_float(_HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float, _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float, _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float);
            float _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float;
            Unity_OneMinus_float(_Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float, _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float);
            float _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float;
            Unity_Saturate_float(_OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float, _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float);
            float _Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_b70d02d8fd164d758d904904b216e5f3_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_b70d02d8fd164d758d904904b216e5f3_A_4_Float = 0;
            float2 _Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2 = float2(_Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float, _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float);
            float _Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float = float(700);
            float2 _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2, (_Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float.xx), _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2);
            float2 _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2 = float2(float(0), float(-0.04));
            float2 _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2;
            Unity_PolarCoordinates_float(_Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2, _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2, float(1), float(1), _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2);
            float3 _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3;
            Unity_Contrast_float((float3(_PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2, 0.0)), float(10), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3);
            float3 _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float.xxx), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3, _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3);
            FoamData foamData;
            ZERO_INITIALIZE(FoamData, foamData);
            EvaluateFoamData(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float, (_Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3).x, IN.uv0.xzy, foamData);
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float = foamData.smoothness;
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float = foamData.foamValue;
            float3 refractedPos;
            float2 distordedNDC;
            float3 absorptionTint;
            ComputeWaterRefractionParams(IN.WorldSpacePosition, float4(IN.NDCPosition.xy, 0, 0).xy, IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, IN.FaceSign, false, _WaterUpDirection.xyz, _MaxRefractionDistance, _WaterExtinction.xyz, refractedPos, distordedNDC, absorptionTint);
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3 = refractedPos;
            float2 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2 = distordedNDC;
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3 = absorptionTint;
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float;
            Unity_Multiply_float_float(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float, 0.25, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float3 _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3 = EvaluateScatteringColor(IN.uv0.xzy, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float, _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float = EvaluateTipThickness(IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float);
            float _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float = EvaluateSimulationCaustics(_EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, abs(dot(IN.WorldSpacePosition - _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, _WaterUpDirection.xyz)), _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2);
            surface.Smoothness = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float;
            surface.BaseColor = _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3;
            surface.NormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3;
            surface.LowFrequencyNormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3;
            surface.RefractedPositionWS = _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3;
            surface.TipThickness = _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float;
            surface.Caustics = _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float;
            surface.Foam = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            // The only parameters that can be requested are the position, normal and time parameters
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
            output.ObjectSpaceNormal =                          input.normalOS;
            output.WorldSpaceNormal =                           TransformObjectToWorldNormal(input.normalOS);
        
            return output;
        }
        
        void PackWaterVertexData(VertexDescription vertex, out float4 uv0, out float4 uv1)
        {
        #if defined(SHADER_STAGE_VERTEX) && defined(TESSELLATION_ON)
            uv0 = float4(vertex.Displacement, 1.0);
            uv1 = float4(vertex.Position, 1.0);
        #else
            uv0.xy = vertex.Position.xz;
            uv0.z = vertex.Displacement.y;
            uv0.w = length(vertex.Displacement.xz);
        
            if (_GridSize.x >= 0)
                uv1.xyz = TransformObjectToWorld(vertex.Position + vertex.Displacement);
            uv1.w = vertex.LowFrequencyHeight;
        #endif
        }
        
        #if defined(TESSELLATION_ON)
            #define VaryingsMeshType VaryingsMeshToDS
        #else
            #define VaryingsMeshType VaryingsMeshToPS
        #endif
        
        // Modifications should probably be replicated to ApplyTessellationModification
        void ApplyMeshModification(AttributesMesh input, float3 timeParameters, inout VaryingsMeshType varyings, out VertexDescription vertexDescription)
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
        
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
            vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        
            // Backward compatibility with old graphs
        
            // Custom interpolators
            
        }
        
        #undef VaryingsMeshType
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.texCoord1.xyz;
            output.positionPixel =              input.positionCS.xy; // NOTE: this is not actually in clip space, it is the VPOS pixel coordinate value
            output.tangentToWorld =             GetLocalFrame(input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord1 =                  input.texCoord1;
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            output.WorldSpaceNormal =                           normalize(input.tangentToWorld[2].xyz);
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
            output.WorldSpaceViewDirection =                    normalize(viewWS);
            output.WorldSpacePosition =                         input.positionRWS;
            output.AbsoluteWorldSpacePosition =                 GetAbsolutePositionWS(input.positionRWS);
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
        
        #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x < 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #else
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x > 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 =                                        input.texCoord0;
            output.uv1 =                                        input.texCoord1;
            output.FaceSign =                                   input.isFrontFace;
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void ApplyDecalToSurfaceDataNoNormal(DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData);
        
        void ApplyDecalAndGetNormal(FragInputs fragInputs, PositionInputs posInput, SurfaceDescription surfaceDescription,
            inout SurfaceData surfaceData)
        {
            float3 doubleSidedConstants = GetDoubleSidedConstants();
        
        #ifdef DECAL_NORMAL_BLENDING
            // SG nodes don't ouptut surface gradients, so if decals require surf grad blending, we have to convert
            // the normal to gradient before applying the decal. We then have to resolve the gradient back to world space
            float3 normalTS;
        
        
            normalTS = SurfaceGradientFromPerturbedNormal(fragInputs.tangentToWorld[2],
            surfaceDescription.NormalWS);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        
            GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        #else
            // normal delivered to master node
            GetNormalWS_SrcWS(fragInputs, surfaceDescription.NormalWS, surfaceData.normalWS, doubleSidedConstants);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                // Both uses and modifies 'surfaceData.normalWS'.
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        #endif
        }
        void ApplyDecalToSurfaceData(DecalSurfaceData decalSurfaceData, float3 vtxNormal, inout SurfaceData surfaceData)
        {
            // using alpha compositing https://developer.nvidia.com/gpugems/GPUGems3/gpugems3_ch23.html
            float decalFoam = Luminance(decalSurfaceData.baseColor.xyz);
            surfaceData.foam = surfaceData.foam * decalSurfaceData.baseColor.w + decalFoam;
        
            // Always test the normal as we can have decompression artifact
            if (decalSurfaceData.normalWS.w < 1.0)
            {
                // Re-evaluate the surface gradient of the simulation normal
                float3 normalSG = SurfaceGradientFromPerturbedNormal(vtxNormal, surfaceData.normalWS);
                // Add the contribution of the decals to the normal
                normalSG += SurfaceGradientFromVolumeGradient(vtxNormal, decalSurfaceData.normalWS.xyz);
                // Move back to world space
                surfaceData.normalWS.xyz = SurfaceGradientResolveNormal(vtxNormal, normalSG);
            }
        
            surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
        }
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            surfaceData.baseColor =                 surfaceDescription.BaseColor;
        
            surfaceData.normalWS =                  surfaceDescription.NormalWS;
            surfaceData.lowFrequencyNormalWS =      surfaceDescription.LowFrequencyNormalWS;
        
            surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
            surfaceData.foam =                      surfaceDescription.Foam;
        
            surfaceData.tipThickness =              surfaceDescription.TipThickness;
            surfaceData.caustics =                  surfaceDescription.Caustics;
            surfaceData.refractedPositionWS =       surfaceDescription.RefractedPositionWS;
        
            bentNormalWS = float3(0, 1, 0);
        
            #if HAVE_DECALS
                if (_EnableDecals)
                {
                    float alpha = 1.0;
                    // Both uses and modifies 'surfaceData.normalWS'.
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, _WaterRenderingLayer, alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
                }
            #endif
        
            // Kill the scattering and the refraction based on where foam is perceived
            surfaceData.baseColor *= (1 - saturate(surfaceData.foam));
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                }
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Water/Shaders/ShaderPassWaterGBuffer.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "WaterMaskTessellation"
            Tags
            {
                "LightMode" = "WaterMaskTessellation"
            }
        
            // Render State
            Cull [_CullWaterMask]
        ZTest LEqual
        ZWrite On
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma instancing_options procedural:SetupInstanceID
        #pragma hull Hull
        #pragma domain Domain
        
            // Keywords
            #pragma multi_compile WATER_ONE_BAND WATER_TWO_BANDS WATER_THREE_BANDS
        #pragma multi_compile _ WATER_LOCAL_CURRENT
        #pragma multi_compile WATER_DECAL_PARTIAL WATER_DECAL_COMPLETE
        #pragma multi_compile _ PROCEDURAL_INSTANCING_ON
        #pragma multi_compile _ STEREO_INSTANCING_ON
        #pragma multi_compile _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define SHADERPASS SHADERPASS_WATER_MASK
        #define SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        #define HAS_LIGHTLOOP 1
        #define PUNCTUAL_SHADOW_LOW
        #define DIRECTIONAL_SHADOW_LOW
        #define AREA_SHADOW_MEDIUM
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        #define TESSELLATION_ON 1
        #define HAVE_TESSELLATION_MODIFICATION 1
        #define WATER_DISPLACEMENT 1
        #define SUPPORT_GLOBAL_MIP_BIAS 1
        #define REQUIRE_DEPTH_TEXTURE
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl" // Required before including properties as it defines UNITY_TEXTURE_STREAMING_DEBUG_VARS
            // Always include Shader Graph version
            // Always include last to avoid double macros
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD1
        
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
            // Define when IsFontFaceNode is included in ShaderGraph
            #define VARYINGS_NEED_CULLFACE
        
        
            #ifdef TESSELLATION_ON
            // World and normal are always available
                #define VARYINGS_DS_NEED_TANGENT
            #define VARYINGS_DS_NEED_TEXCOORD0
            #define VARYINGS_DS_NEED_TEXCOORD1
                        	#endif
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _EmissionColor;
        CBUFFER_END
        
        
        // Object and Global properties
        float _StencilWaterRefGBuffer;
        float _StencilWaterWriteMaskGBuffer;
        float _StencilWaterReadMaskGBuffer;
        float _CullWaterMask;
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Water/Water.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
            // GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToDS
        {
             float3 positionRWS;
             float tessellationFactor;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 WorldSpaceViewDirection;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 ScreenPosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float4 uv1;
             float FaceSign;
        };
        struct PackedVaryingsMeshToDS
        {
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 packed_positionRWS_tessellationFactor : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToDS PackVaryingsMeshToDS (VaryingsMeshToDS input)
        {
            PackedVaryingsMeshToDS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToDS, output);
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.packed_positionRWS_tessellationFactor.xyz = input.positionRWS;
            output.packed_positionRWS_tessellationFactor.w = input.tessellationFactor;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToDS UnpackVaryingsMeshToDS (PackedVaryingsMeshToDS input)
        {
            VaryingsMeshToDS output;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.positionRWS = input.packed_positionRWS_tessellationFactor.xyz;
            output.tessellationFactor = input.packed_positionRWS_tessellationFactor.w;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_DS_NEED_INSTANCEID) || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        StructuredBuffer<int2> _DepthPyramidMipLevelOffsets;
        float Unity_HDRP_SampleSceneDepth_float(float2 uv, float lod)
        {
            #if defined(REQUIRE_DEPTH_TEXTURE) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
            int2 coord = int2(uv * _ScreenSize.xy);
            int2 mipCoord  = coord.xy >> int(lod);
            int2 mipOffset = _DepthPyramidMipLevelOffsets[int(lod)];
            return LOAD_TEXTURE2D_X(_CameraDepthTexture, mipOffset + mipCoord).r;
            #endif
            return 0.0;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
        {
            float2 delta = UV - Center;
            float radius = length(delta) * 2 * RadialScale;
            float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
            Out = float2(radius, angle);
        }
        
        void Unity_Contrast_float(float3 In, float Contrast, out float3 Out)
        {
            float midpoint = pow(0.5, 2.2);
            Out =  (In - midpoint) * Contrast + midpoint;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float4 uv0;
            float4 uv1;
            float LowFrequencyHeight;
            float3 Displacement;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            WaterDisplacementData displacementData;
            ZERO_INITIALIZE(WaterDisplacementData, displacementData);
            EvaluateWaterDisplacement(IN.ObjectSpacePosition, displacementData);
            float3 _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3 = displacementData.displacement;
            float _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float = displacementData.lowFrequencyHeight;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.uv0 = float4 (0, 0, 0, 0);
            description.uv1 = float4 (0, 0, 0, 0);
            description.LowFrequencyHeight = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float;
            description.Displacement = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Smoothness;
            float3 BaseColor;
            float3 NormalWS;
            float3 LowFrequencyNormalWS;
            float3 RefractedPositionWS;
            float TipThickness;
            float Caustics;
            float Foam;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            WaterAdditionalData waterAdditionalData;
            ZERO_INITIALIZE(WaterAdditionalData, waterAdditionalData);
            EvaluateWaterAdditionalData(IN.uv0.xzy, IN.WorldSpacePosition, IN.WorldSpaceNormal, waterAdditionalData);
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3 = waterAdditionalData.normalWS;
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3 = waterAdditionalData.lowFrequencyNormalWS;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float = waterAdditionalData.surfaceFoam;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float = waterAdditionalData.deepFoam;
            float _HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float = LinearEyeDepth(Unity_HDRP_SampleSceneDepth_float(float4(IN.NDCPosition.xy, 0, 0).xy, float(0)), _ZBufferParams);
            float4 _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4 = IN.ScreenPosition;
            float _Split_edf17f69cb8548e982b55e2142415f35_R_1_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[0];
            float _Split_edf17f69cb8548e982b55e2142415f35_G_2_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[1];
            float _Split_edf17f69cb8548e982b55e2142415f35_B_3_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[2];
            float _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[3];
            float _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float;
            Unity_Subtract_float(_HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float, _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float, _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float);
            float _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float;
            Unity_OneMinus_float(_Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float, _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float);
            float _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float;
            Unity_Saturate_float(_OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float, _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float);
            float _Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_b70d02d8fd164d758d904904b216e5f3_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_b70d02d8fd164d758d904904b216e5f3_A_4_Float = 0;
            float2 _Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2 = float2(_Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float, _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float);
            float _Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float = float(700);
            float2 _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2, (_Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float.xx), _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2);
            float2 _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2 = float2(float(0), float(-0.04));
            float2 _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2;
            Unity_PolarCoordinates_float(_Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2, _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2, float(1), float(1), _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2);
            float3 _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3;
            Unity_Contrast_float((float3(_PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2, 0.0)), float(10), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3);
            float3 _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float.xxx), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3, _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3);
            FoamData foamData;
            ZERO_INITIALIZE(FoamData, foamData);
            EvaluateFoamData(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float, (_Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3).x, IN.uv0.xzy, foamData);
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float = foamData.smoothness;
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float = foamData.foamValue;
            float3 refractedPos;
            float2 distordedNDC;
            float3 absorptionTint;
            ComputeWaterRefractionParams(IN.WorldSpacePosition, float4(IN.NDCPosition.xy, 0, 0).xy, IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, IN.FaceSign, false, _WaterUpDirection.xyz, _MaxRefractionDistance, _WaterExtinction.xyz, refractedPos, distordedNDC, absorptionTint);
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3 = refractedPos;
            float2 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2 = distordedNDC;
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3 = absorptionTint;
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float;
            Unity_Multiply_float_float(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float, 0.25, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float3 _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3 = EvaluateScatteringColor(IN.uv0.xzy, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float, _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float = EvaluateTipThickness(IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float);
            float _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float = EvaluateSimulationCaustics(_EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, abs(dot(IN.WorldSpacePosition - _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, _WaterUpDirection.xyz)), _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2);
            surface.Smoothness = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float;
            surface.BaseColor = _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3;
            surface.NormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3;
            surface.LowFrequencyNormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3;
            surface.RefractedPositionWS = _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3;
            surface.TipThickness = _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float;
            surface.Caustics = _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float;
            surface.Foam = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            // The only parameters that can be requested are the position, normal and time parameters
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
            output.ObjectSpaceNormal =                          input.normalOS;
            output.WorldSpaceNormal =                           TransformObjectToWorldNormal(input.normalOS);
        
            return output;
        }
        
        void PackWaterVertexData(VertexDescription vertex, out float4 uv0, out float4 uv1)
        {
        #if defined(SHADER_STAGE_VERTEX) && defined(TESSELLATION_ON)
            uv0 = float4(vertex.Displacement, 1.0);
            uv1 = float4(vertex.Position, 1.0);
        #else
            uv0.xy = vertex.Position.xz;
            uv0.z = vertex.Displacement.y;
            uv0.w = length(vertex.Displacement.xz);
        
            if (_GridSize.x >= 0)
                uv1.xyz = TransformObjectToWorld(vertex.Position + vertex.Displacement);
            uv1.w = vertex.LowFrequencyHeight;
        #endif
        }
        
        #if defined(TESSELLATION_ON)
            #define VaryingsMeshType VaryingsMeshToDS
        #else
            #define VaryingsMeshType VaryingsMeshToPS
        #endif
        
        // Modifications should probably be replicated to ApplyTessellationModification
        void ApplyMeshModification(AttributesMesh input, float3 timeParameters, inout VaryingsMeshType varyings, out VertexDescription vertexDescription)
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
        
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
            vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        
            // Backward compatibility with old graphs
        
            // Custom interpolators
            
        }
        
        #undef VaryingsMeshType
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.texCoord1.xyz;
            output.positionPixel =              input.positionCS.xy; // NOTE: this is not actually in clip space, it is the VPOS pixel coordinate value
            output.tangentToWorld =             GetLocalFrame(input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord1 =                  input.texCoord1;
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            #ifdef TESSELLATION_ON
        
        // TODO: We should generate this struct like all the other varying struct
        VaryingsMeshToDS InterpolateWithBaryCoordsMeshToDS(VaryingsMeshToDS input0, VaryingsMeshToDS input1, VaryingsMeshToDS input2, float3 baryCoords)
        {
            VaryingsMeshToDS output;
        
            UNITY_TRANSFER_INSTANCE_ID(input0, output);
        
            // The set of values that need to interpolated is fixed
            TESSELLATION_INTERPOLATE_BARY(positionRWS, baryCoords);
            TESSELLATION_INTERPOLATE_BARY(normalWS, baryCoords);
            TESSELLATION_INTERPOLATE_BARY(texCoord0, baryCoords);
            TESSELLATION_INTERPOLATE_BARY(texCoord1, baryCoords);
        
            // Pass-Through for custom interpolator
            
        
            return output;
        }
        
        VertexDescriptionInputs VaryingsMeshToDSToVertexDescriptionInputs(VaryingsMeshToDS input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            // texcoord1 contains the pre displacement object space position
            // normal is marked WS but it's actually in object space :(
            output.ObjectSpacePosition = input.texCoord1.xyz;
            output.WorldSpacePosition  = TransformObjectToWorld(input.texCoord1.xyz);
            output.ObjectSpaceNormal   = input.normalWS;
            output.WorldSpaceNormal    = TransformObjectToWorldNormal(input.normalWS);
        
        
            return output;
        }
        
        // tessellationFactors
        // x - 1->2 edge
        // y - 2->0 edge
        // z - 0->1 edge
        // w - inside tessellation factor
        void ApplyTessellationModification(VaryingsMeshToDS input, float3 timeParameters, inout VaryingsMeshToPS output, out VertexDescription vertexDescription)
        {
            // HACK: As there is no specific tessellation stage for now in shadergraph, we reuse the vertex description mechanism.
            // It mean we store TessellationFactor inside vertex description causing extra read on both vertex and hull stage, but unused parameters are optimize out by the shader compiler, so no impact.
            VertexDescriptionInputs vertexDescriptionInputs = VaryingsMeshToDSToVertexDescriptionInputs(input);
        
            // Override time parameters with used one (This is required to correctly handle motion vector for tessellation animation based on time)
        
            // evaluate vertex graph
            vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        
            // Backward compatibility with old graphs
        
            // Custom interpolators
            
        }
        
        #endif // TESSELLATION_ON
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            output.WorldSpaceNormal =                           normalize(input.tangentToWorld[2].xyz);
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
            output.WorldSpaceViewDirection =                    normalize(viewWS);
            output.WorldSpacePosition =                         input.positionRWS;
            output.AbsoluteWorldSpacePosition =                 GetAbsolutePositionWS(input.positionRWS);
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
        
        #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x < 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #else
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x > 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 =                                        input.texCoord0;
            output.uv1 =                                        input.texCoord1;
            output.FaceSign =                                   input.isFrontFace;
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void ApplyDecalToSurfaceDataNoNormal(DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData);
        
        void ApplyDecalAndGetNormal(FragInputs fragInputs, PositionInputs posInput, SurfaceDescription surfaceDescription,
            inout SurfaceData surfaceData)
        {
            float3 doubleSidedConstants = GetDoubleSidedConstants();
        
        #ifdef DECAL_NORMAL_BLENDING
            // SG nodes don't ouptut surface gradients, so if decals require surf grad blending, we have to convert
            // the normal to gradient before applying the decal. We then have to resolve the gradient back to world space
            float3 normalTS;
        
        
            normalTS = SurfaceGradientFromPerturbedNormal(fragInputs.tangentToWorld[2],
            surfaceDescription.NormalWS);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        
            GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        #else
            // normal delivered to master node
            GetNormalWS_SrcWS(fragInputs, surfaceDescription.NormalWS, surfaceData.normalWS, doubleSidedConstants);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                // Both uses and modifies 'surfaceData.normalWS'.
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        #endif
        }
        void ApplyDecalToSurfaceData(DecalSurfaceData decalSurfaceData, float3 vtxNormal, inout SurfaceData surfaceData)
        {
            // using alpha compositing https://developer.nvidia.com/gpugems/GPUGems3/gpugems3_ch23.html
            float decalFoam = Luminance(decalSurfaceData.baseColor.xyz);
            surfaceData.foam = surfaceData.foam * decalSurfaceData.baseColor.w + decalFoam;
        
            // Always test the normal as we can have decompression artifact
            if (decalSurfaceData.normalWS.w < 1.0)
            {
                // Re-evaluate the surface gradient of the simulation normal
                float3 normalSG = SurfaceGradientFromPerturbedNormal(vtxNormal, surfaceData.normalWS);
                // Add the contribution of the decals to the normal
                normalSG += SurfaceGradientFromVolumeGradient(vtxNormal, decalSurfaceData.normalWS.xyz);
                // Move back to world space
                surfaceData.normalWS.xyz = SurfaceGradientResolveNormal(vtxNormal, normalSG);
            }
        
            surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
        }
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            surfaceData.baseColor =                 surfaceDescription.BaseColor;
        
            surfaceData.normalWS =                  surfaceDescription.NormalWS;
            surfaceData.lowFrequencyNormalWS =      surfaceDescription.LowFrequencyNormalWS;
        
            surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
            surfaceData.foam =                      surfaceDescription.Foam;
        
            surfaceData.tipThickness =              surfaceDescription.TipThickness;
            surfaceData.caustics =                  surfaceDescription.Caustics;
            surfaceData.refractedPositionWS =       surfaceDescription.RefractedPositionWS;
        
            bentNormalWS = float3(0, 1, 0);
        
            #if HAVE_DECALS
                if (_EnableDecals)
                {
                    float alpha = 1.0;
                    // Both uses and modifies 'surfaceData.normalWS'.
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, _WaterRenderingLayer, alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
                }
            #endif
        
            // Kill the scattering and the refraction based on where foam is perceived
            surfaceData.baseColor *= (1 - saturate(surfaceData.foam));
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                }
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Water/Shaders/ShaderPassWaterMask.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "WaterMaskLowRes"
            Tags
            {
                "LightMode" = "WaterMaskLowRes"
            }
        
            // Render State
            Cull [_CullWaterMask]
        ZTest LEqual
        ZWrite On
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma instancing_options procedural:SetupInstanceID
        
            // Keywords
            #pragma multi_compile WATER_ONE_BAND WATER_TWO_BANDS WATER_THREE_BANDS
        #pragma multi_compile _ WATER_LOCAL_CURRENT
        #pragma multi_compile WATER_DECAL_PARTIAL WATER_DECAL_COMPLETE
        #pragma multi_compile _ PROCEDURAL_INSTANCING_ON
        #pragma multi_compile _ STEREO_INSTANCING_ON
        #pragma multi_compile _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define SHADERPASS SHADERPASS_WATER_MASK
        #define SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        #define HAS_LIGHTLOOP 1
        #define PUNCTUAL_SHADOW_LOW
        #define DIRECTIONAL_SHADOW_LOW
        #define AREA_SHADOW_MEDIUM
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        #define SUPPORT_GLOBAL_MIP_BIAS 1
        #define REQUIRE_DEPTH_TEXTURE
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl" // Required before including properties as it defines UNITY_TEXTURE_STREAMING_DEBUG_VARS
            // Always include Shader Graph version
            // Always include last to avoid double macros
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD1
        
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
            // Define when IsFontFaceNode is included in ShaderGraph
            #define VARYINGS_NEED_CULLFACE
        
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _EmissionColor;
        CBUFFER_END
        
        
        // Object and Global properties
        float _StencilWaterRefGBuffer;
        float _StencilWaterWriteMaskGBuffer;
        float _StencilWaterReadMaskGBuffer;
        float _CullWaterMask;
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Water/Water.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
            // GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 WorldSpaceViewDirection;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 ScreenPosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float4 uv1;
             float FaceSign;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        StructuredBuffer<int2> _DepthPyramidMipLevelOffsets;
        float Unity_HDRP_SampleSceneDepth_float(float2 uv, float lod)
        {
            #if defined(REQUIRE_DEPTH_TEXTURE) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
            int2 coord = int2(uv * _ScreenSize.xy);
            int2 mipCoord  = coord.xy >> int(lod);
            int2 mipOffset = _DepthPyramidMipLevelOffsets[int(lod)];
            return LOAD_TEXTURE2D_X(_CameraDepthTexture, mipOffset + mipCoord).r;
            #endif
            return 0.0;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
        {
            float2 delta = UV - Center;
            float radius = length(delta) * 2 * RadialScale;
            float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
            Out = float2(radius, angle);
        }
        
        void Unity_Contrast_float(float3 In, float Contrast, out float3 Out)
        {
            float midpoint = pow(0.5, 2.2);
            Out =  (In - midpoint) * Contrast + midpoint;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float4 uv0;
            float4 uv1;
            float LowFrequencyHeight;
            float3 Displacement;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            WaterDisplacementData displacementData;
            ZERO_INITIALIZE(WaterDisplacementData, displacementData);
            EvaluateWaterDisplacement(IN.ObjectSpacePosition, displacementData);
            float3 _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3 = displacementData.displacement;
            float _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float = displacementData.lowFrequencyHeight;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.uv0 = float4 (0, 0, 0, 0);
            description.uv1 = float4 (0, 0, 0, 0);
            description.LowFrequencyHeight = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_LowFrequencyHeight_2_Float;
            description.Displacement = _EvaluateWaterDisplacement_6a534b9292cd492592fbd7761d895c1f_Displacement_1_Vector3;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Smoothness;
            float3 BaseColor;
            float3 NormalWS;
            float3 LowFrequencyNormalWS;
            float3 RefractedPositionWS;
            float TipThickness;
            float Caustics;
            float Foam;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            WaterAdditionalData waterAdditionalData;
            ZERO_INITIALIZE(WaterAdditionalData, waterAdditionalData);
            EvaluateWaterAdditionalData(IN.uv0.xzy, IN.WorldSpacePosition, IN.WorldSpaceNormal, waterAdditionalData);
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3 = waterAdditionalData.normalWS;
            float3 _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3 = waterAdditionalData.lowFrequencyNormalWS;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float = waterAdditionalData.surfaceFoam;
            float _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float = waterAdditionalData.deepFoam;
            float _HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float = LinearEyeDepth(Unity_HDRP_SampleSceneDepth_float(float4(IN.NDCPosition.xy, 0, 0).xy, float(0)), _ZBufferParams);
            float4 _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4 = IN.ScreenPosition;
            float _Split_edf17f69cb8548e982b55e2142415f35_R_1_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[0];
            float _Split_edf17f69cb8548e982b55e2142415f35_G_2_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[1];
            float _Split_edf17f69cb8548e982b55e2142415f35_B_3_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[2];
            float _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float = _ScreenPosition_dd531ffb982d4382b6330d480f31594d_Out_0_Vector4[3];
            float _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float;
            Unity_Subtract_float(_HDSceneDepth_8a1f82d76e17444abde60e12d4113999_Output_2_Float, _Split_edf17f69cb8548e982b55e2142415f35_A_4_Float, _Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float);
            float _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float;
            Unity_OneMinus_float(_Subtract_3be6ba1b513042c1bef2c09d1abc957f_Out_2_Float, _OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float);
            float _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float;
            Unity_Saturate_float(_OneMinus_45d220f66a794d76a67f8f887971edf0_Out_1_Float, _Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float);
            float _Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float = IN.AbsoluteWorldSpacePosition[0];
            float _Split_b70d02d8fd164d758d904904b216e5f3_G_2_Float = IN.AbsoluteWorldSpacePosition[1];
            float _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float = IN.AbsoluteWorldSpacePosition[2];
            float _Split_b70d02d8fd164d758d904904b216e5f3_A_4_Float = 0;
            float2 _Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2 = float2(_Split_b70d02d8fd164d758d904904b216e5f3_R_1_Float, _Split_b70d02d8fd164d758d904904b216e5f3_B_3_Float);
            float _Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float = float(700);
            float2 _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_eb61deaa8fe444b59a6675db23d856d2_Out_0_Vector2, (_Float_1329e720489d4cdc81e36e70f77d9ef3_Out_0_Float.xx), _Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2);
            float2 _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2 = float2(float(0), float(-0.04));
            float2 _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2;
            Unity_PolarCoordinates_float(_Divide_9fb08d86368c4306ad3a69f62d0e77ee_Out_2_Vector2, _Vector2_78d8442fa2534378a1dc5904bcba9303_Out_0_Vector2, float(1), float(1), _PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2);
            float3 _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3;
            Unity_Contrast_float((float3(_PolarCoordinates_1e9dc507872f432bb86fbe8009c239a5_Out_4_Vector2, 0.0)), float(10), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3);
            float3 _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3;
            Unity_Multiply_float3_float3((_Saturate_dffa0d3c26ff4fefae12acec43625565_Out_1_Float.xxx), _Contrast_046c713423384c4da35821388b57adfe_Out_2_Vector3, _Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3);
            FoamData foamData;
            ZERO_INITIALIZE(FoamData, foamData);
            EvaluateFoamData(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_SurfaceFoam_4_Float, (_Multiply_99ae16a3653d4eca8f71e4d0640a83d5_Out_2_Vector3).x, IN.uv0.xzy, foamData);
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float = foamData.smoothness;
            float _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float = foamData.foamValue;
            float3 refractedPos;
            float2 distordedNDC;
            float3 absorptionTint;
            ComputeWaterRefractionParams(IN.WorldSpacePosition, float4(IN.NDCPosition.xy, 0, 0).xy, IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, IN.FaceSign, false, _WaterUpDirection.xyz, _MaxRefractionDistance, _WaterExtinction.xyz, refractedPos, distordedNDC, absorptionTint);
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3 = refractedPos;
            float2 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2 = distordedNDC;
            float3 _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3 = absorptionTint;
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float;
            Unity_Multiply_float_float(_EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_DeepFoam_3_Float, 0.25, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float3 _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3 = EvaluateScatteringColor(IN.uv0.xzy, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_LowFrequencyHeight_0_Float, _UnpackWaterData_26bed83b685447bfbc0a740d805642b8_HorizontalDisplacement_1_Float, _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_AbsorptionTint_4_Vector3, _Multiply_e086d1e347b9443cb8cdd6d4039dac25_Out_2_Float);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float = saturate(IN.uv1.w);
            float _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_HorizontalDisplacement_1_Float = IN.uv0.w;
            float _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float = EvaluateTipThickness(IN.WorldSpaceViewDirection, _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3, _UnpackWaterData_0aac2a1598d244f1a4818b981d77066b_LowFrequencyHeight_0_Float);
            float _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float = EvaluateSimulationCaustics(_EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, abs(dot(IN.WorldSpacePosition - _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3, _WaterUpDirection.xyz)), _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_DistordedWaterNDC_3_Vector2);
            surface.Smoothness = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Smoothness_3_Float;
            surface.BaseColor = _EvaluateScatteringColorWater_7d43f8c41de740e0ac5872903f2da806_BaseColor_4_Vector3;
            surface.NormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_NormalWS_1_Vector3;
            surface.LowFrequencyNormalWS = _EvaluateSimulationAdditionalDataWater_b7095c617e634f0096cd6fe24e3ca89e_LowFrequencyNormalWS_2_Vector3;
            surface.RefractedPositionWS = _EvaluateRefractionDataWater_88156e2a85a843d5a75f511607a09514_RefractedPositionWS_2_Vector3;
            surface.TipThickness = _EvaluateTipThicknessWater_d8c7563c22924321bf2ef984385ff501_TipThickness_2_Float;
            surface.Caustics = _EvaluateSimulationCausticsWater_4f423c7ac0384c73833f32dee09d87c9_Caustics_2_Float;
            surface.Foam = _EvaluateFoamDataWater_5b95bcf112934e958733d07e1e0b395c_Foam_2_Float;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            // The only parameters that can be requested are the position, normal and time parameters
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
            output.ObjectSpaceNormal =                          input.normalOS;
            output.WorldSpaceNormal =                           TransformObjectToWorldNormal(input.normalOS);
        
            return output;
        }
        
        void PackWaterVertexData(VertexDescription vertex, out float4 uv0, out float4 uv1)
        {
        #if defined(SHADER_STAGE_VERTEX) && defined(TESSELLATION_ON)
            uv0 = float4(vertex.Displacement, 1.0);
            uv1 = float4(vertex.Position, 1.0);
        #else
            uv0.xy = vertex.Position.xz;
            uv0.z = vertex.Displacement.y;
            uv0.w = length(vertex.Displacement.xz);
        
            if (_GridSize.x >= 0)
                uv1.xyz = TransformObjectToWorld(vertex.Position + vertex.Displacement);
            uv1.w = vertex.LowFrequencyHeight;
        #endif
        }
        
        #if defined(TESSELLATION_ON)
            #define VaryingsMeshType VaryingsMeshToDS
        #else
            #define VaryingsMeshType VaryingsMeshToPS
        #endif
        
        // Modifications should probably be replicated to ApplyTessellationModification
        void ApplyMeshModification(AttributesMesh input, float3 timeParameters, inout VaryingsMeshType varyings, out VertexDescription vertexDescription)
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
        
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
            vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        
            // Backward compatibility with old graphs
        
            // Custom interpolators
            
        }
        
        #undef VaryingsMeshType
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.texCoord1.xyz;
            output.positionPixel =              input.positionCS.xy; // NOTE: this is not actually in clip space, it is the VPOS pixel coordinate value
            output.tangentToWorld =             GetLocalFrame(input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord1 =                  input.texCoord1;
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            output.WorldSpaceNormal =                           normalize(input.tangentToWorld[2].xyz);
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
            output.WorldSpaceViewDirection =                    normalize(viewWS);
            output.WorldSpacePosition =                         input.positionRWS;
            output.AbsoluteWorldSpacePosition =                 GetAbsolutePositionWS(input.positionRWS);
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
        
        #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x < 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #else
            output.PixelPosition = float2(input.positionPixel.x, (_ProjectionParams.x > 0) ? (_ScreenParams.y - input.positionPixel.y) : input.positionPixel.y);
        #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 =                                        input.texCoord0;
            output.uv1 =                                        input.texCoord1;
            output.FaceSign =                                   input.isFrontFace;
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void ApplyDecalToSurfaceDataNoNormal(DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData);
        
        void ApplyDecalAndGetNormal(FragInputs fragInputs, PositionInputs posInput, SurfaceDescription surfaceDescription,
            inout SurfaceData surfaceData)
        {
            float3 doubleSidedConstants = GetDoubleSidedConstants();
        
        #ifdef DECAL_NORMAL_BLENDING
            // SG nodes don't ouptut surface gradients, so if decals require surf grad blending, we have to convert
            // the normal to gradient before applying the decal. We then have to resolve the gradient back to world space
            float3 normalTS;
        
        
            normalTS = SurfaceGradientFromPerturbedNormal(fragInputs.tangentToWorld[2],
            surfaceDescription.NormalWS);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        
            GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        #else
            // normal delivered to master node
            GetNormalWS_SrcWS(fragInputs, surfaceDescription.NormalWS, surfaceData.normalWS, doubleSidedConstants);
        
            #if HAVE_DECALS
            if (_EnableDecals)
            {
                float alpha = 1.0;
        
                // Both uses and modifies 'surfaceData.normalWS'.
                DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
            }
            #endif
        #endif
        }
        void ApplyDecalToSurfaceData(DecalSurfaceData decalSurfaceData, float3 vtxNormal, inout SurfaceData surfaceData)
        {
            // using alpha compositing https://developer.nvidia.com/gpugems/GPUGems3/gpugems3_ch23.html
            float decalFoam = Luminance(decalSurfaceData.baseColor.xyz);
            surfaceData.foam = surfaceData.foam * decalSurfaceData.baseColor.w + decalFoam;
        
            // Always test the normal as we can have decompression artifact
            if (decalSurfaceData.normalWS.w < 1.0)
            {
                // Re-evaluate the surface gradient of the simulation normal
                float3 normalSG = SurfaceGradientFromPerturbedNormal(vtxNormal, surfaceData.normalWS);
                // Add the contribution of the decals to the normal
                normalSG += SurfaceGradientFromVolumeGradient(vtxNormal, decalSurfaceData.normalWS.xyz);
                // Move back to world space
                surfaceData.normalWS.xyz = SurfaceGradientResolveNormal(vtxNormal, normalSG);
            }
        
            surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
        }
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            surfaceData.baseColor =                 surfaceDescription.BaseColor;
        
            surfaceData.normalWS =                  surfaceDescription.NormalWS;
            surfaceData.lowFrequencyNormalWS =      surfaceDescription.LowFrequencyNormalWS;
        
            surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
            surfaceData.foam =                      surfaceDescription.Foam;
        
            surfaceData.tipThickness =              surfaceDescription.TipThickness;
            surfaceData.caustics =                  surfaceDescription.Caustics;
            surfaceData.refractedPositionWS =       surfaceDescription.RefractedPositionWS;
        
            bentNormalWS = float3(0, 1, 0);
        
            #if HAVE_DECALS
                if (_EnableDecals)
                {
                    float alpha = 1.0;
                    // Both uses and modifies 'surfaceData.normalWS'.
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, _WaterRenderingLayer, alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, fragInputs.tangentToWorld[2], surfaceData);
                }
            #endif
        
            // Kill the scattering and the refraction based on where foam is perceived
            surfaceData.baseColor *= (1 - saturate(surfaceData.foam));
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                }
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Water/Shaders/ShaderPassWaterMask.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "Rendering.HighDefinition.LightingShaderGraphGUI" "UnityEngine.Rendering.HighDefinition.HDRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}