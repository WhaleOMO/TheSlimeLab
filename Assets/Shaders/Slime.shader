Shader "Custom/Slime"
{
    Properties
    {
        [Header(Slime Color)][Space(10)]
        _BaseColor ("Slime Base Color", Color) = (0.5,0.5,0.5,1.0)
        _AmbientColor ("Slime Ambient Color", Color) = (0.1,0.1,0.1,1.0)
        _HighlightColor ("Slime Highlight Color", Color) = (1.0,1.0,1.0,1.0)
        [HDR]_RimColor ("Slime Rim Color", Color) = (0.9, 0.9, 0.9, 1.0)
        
        [Header(Slime Expression)][Space(10)]
        [NoScaleOffset]_ExpressionTex ("Slime Expression", 2D) = "black"{}
        _ExpColorLayer1 ("Expression Layer1 Color", Color) = (1.0,1.0,1.0)
        _ExpColorLayer2 ("Expression Layer2 Color", Color) = (1.0,1.0,1.0)
        _ExpColorLayer3 ("Expression Layer3 Color", Color) = (1.0,1.0,1.0)
        
        [Header(Slime Shake)][Space(10)]
        [Toggle(_ENABLE_SLIME_SHAKE)]_EnableShake ("Slime Shake?", float) = 1
        _ShakeSpeed ("Shake Speed", float) = 8
        _ShakeAmount ("Shake Amount", float) = 0.15
        
        [Header(Slime Outline)][Space(10)]
        _OutlineColor("Slime Outline Color", Color) = (0.0,0.0,0.0,0.0)
        _OutlineWidth("Slime Outline Width", float) = 0.01
    }
    
    SubShader
    {
        Tags { "RenderQueue" = "Geometry" "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_instancing
            #pragma multi_compile _ _ENABLE_SLIME_SHAKE
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "ShaderLib/CustomUtils.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _ShakeSpeed, _ShakeAmount;
                float4 _BaseColor, _AmbientColor, _RimColor, _HighlightColor;
                float3 _ExpColorLayer1, _ExpColorLayer2, _ExpColorLayer3;
                TEXTURE2D(_ExpressionTex);
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normal       : NORMAL;
                float2 texcoord     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float3 posWS        : TEXCOORD2;
                float4 positionHCS  : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };            
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.uv = IN.texcoord;
                float4 posOS = IN.positionOS;
                #ifdef _ENABLE_SLIME_SHAKE
                    posOS.xyz = SlimeUpDownShake(posOS.xyz, _ShakeSpeed, _ShakeAmount);
                #endif
                OUT.posWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normal);
                OUT.positionHCS = TransformObjectToHClip(posOS.xyz);
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                // Data
                const Light mainLight = GetMainLight();
                const float3 posWS = IN.posWS;
                const float3 normal = normalize(IN.normalWS);
                // Diffuse
                const float NdotL = dot(normal, mainLight.direction);
                const float halfLambert = NdotL * 0.5 + 0.5;
                const float lightDirYRemap = map(saturate(mainLight.direction.y), 0, 1, 0, 1.5);
                half3 diffuse = halfLambert * _BaseColor.rgb * lightDirYRemap;
                const half3 ambient = 0.4 * _AmbientColor.rgb;
                // Specular
                const half3 viewDir = normalize(_WorldSpaceCameraPos - posWS);
                const half3 halfDir = normalize(viewDir + mainLight.direction);
                const float NdotH = saturate(dot(halfDir, normal));
                half3 specular = smoothstep(0.96,1.0,NdotH) * _HighlightColor.rgb;
                // Rim
                const float rimIntensity = saturate(dot(half3(0,1,0), mainLight.direction));
                const float fresnel = pow(1.0 - saturate(dot(normal, viewDir)), 1.0 + rimIntensity);
                const half3 rimColor = fresnel * _RimColor.rgb * (0.5 + rimIntensity);
                // Combined Shaded Color
                half3 color = diffuse + ambient + specular;
                color = lerp(color, rimColor, fresnel);
                // Additional Point Lights
                #if defined(_ADDITIONAL_LIGHTS)
                    uint pixelLightCount = GetAdditionalLightsCount();
                    LIGHT_LOOP_BEGIN(pixelLightCount)
                        Light light = GetAdditionalLight(lightIndex, posWS, 1.0);
                        color += saturate(dot(light.direction, normal) * 0.5 + 0.5) * light.color * light.distanceAttenuation;
                    LIGHT_LOOP_END
                #endif
                // Expression
                const float3 expressionLayers = SAMPLE_TEXTURE2D(_ExpressionTex, sampler_LinearClamp, IN.uv).rgb;
                const float expressionMask = max(expressionLayers.b, max(expressionLayers.r, expressionLayers.g));
                const float3 expression = expressionLayers.r * _ExpColorLayer1
                                        + expressionLayers.g * _ExpColorLayer2
                                        + expressionLayers.b * _ExpColorLayer3;
                // Final Lerp
                color = lerp(color, expression, expressionMask);
                return half4(color,1.0);
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "SlimeOutline"
            Tags
            {
                "LightMode" = "SlimeOutline"
            }
            
            Cull Front
            
            HLSLPROGRAM
            #pragma vertex OutlineVertex
            #pragma fragment OutlineFragment
            
            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            
            #pragma multi_compile _ _ENABLE_SLIME_SHAKE
            
            CBUFFER_START(UnityPerMaterial)
                float _ShakeSpeed, _ShakeAmount;
                float _OutlineWidth;
                float3 _OutlineColor;
            CBUFFER_END
            
            #include "ShaderLib/CustomOutline.hlsl"
            ENDHLSL
        }
        
        Pass 
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM
            #pragma target 2.0
            
            // -------------------------------------
            // Shader Stages
            #pragma vertex SlimeShadowVertex
            #pragma fragment SlimeShadowFragment

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            
            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #pragma multi_compile _ _ENABLE_SLIME_SHAKE
            
            CBUFFER_START(UnityPerMaterial)
                float _ShakeSpeed, _ShakeAmount;
            CBUFFER_END
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "ShaderLib/CustomSlimePasses.hlsl"
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex SlimeDepthNormalVertex
            #pragma fragment SlimeDepthNormalsFragment

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            #pragma multi_compile _ _ENABLE_SLIME_SHAKE
            
            CBUFFER_START(UnityPerMaterial)
                float _ShakeSpeed, _ShakeAmount;
            CBUFFER_END

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "ShaderLib/CustomSlimePasses.hlsl"
            ENDHLSL
        }
    }
}
