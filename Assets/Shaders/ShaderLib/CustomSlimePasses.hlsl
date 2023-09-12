#pragma once

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "CustomUtils.hlsl"

#if defined(LOD_FADE_CROSSFADE)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#endif

float3 _LightDirection;
float3 _LightPosition;

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normal       : NORMAL;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS   : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

struct VaryingsPosNormal
{
    float4 positionCS   : SV_POSITION;
    half3 normalWS     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

float4 GetShadowPositionHClip(float3 positionOS, float3 normalOS)
{
    float3 positionWS = TransformObjectToWorld(positionOS);
    float3 normalWS = TransformObjectToWorldNormal(normalOS);

    #if _CASTING_PUNCTUAL_LIGHT_SHADOW
        float3 lightDirectionWS = normalize(_LightPosition - positionWS);
    #else
        float3 lightDirectionWS = _LightDirection;
    #endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

    #if UNITY_REVERSED_Z
        positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #else
        positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #endif

    return positionCS;
}

/*
=======  Vertex Shaders =======
*/

Varyings SlimeVertex(Attributes IN)
{
    Varyings OUT;
    UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
    
    float4 posOS = IN.positionOS;
    #ifdef _ENABLE_SLIME_SHAKE
        posOS.xyz = SlimeUpDownShake(posOS.xyz, _ShakeSpeed, _ShakeAmount);
    #endif
    
    OUT.positionCS = TransformObjectToHClip(posOS.xyz);
    return OUT;
}

Varyings SlimeShadowVertex(Attributes IN)
{
    Varyings OUT;
    UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
    
    float4 posOS = IN.positionOS;
    #ifdef _ENABLE_SLIME_SHAKE
        posOS.xyz = SlimeUpDownShake(posOS.xyz, _ShakeSpeed, _ShakeAmount);
    #endif
    
    OUT.positionCS = GetShadowPositionHClip(posOS.xyz, IN.normal);
    return OUT;
}

VaryingsPosNormal SlimeDepthNormalVertex(Attributes IN)
{
    VaryingsPosNormal OUT = (VaryingsPosNormal)0;
    UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
    
    OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
    OUT.normalWS = half3(TransformObjectToWorldNormal(IN.normal));
    return OUT;
}

/*
=======  Fragment Shaders =======
*/

half SlimeDepthOnlyFragment(Varyings IN): SV_TARGET
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    
    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(IN.positionCS);
    #endif

    return IN.positionCS.z;
}

half4 SlimeShadowFragment(Varyings IN) : SV_TARGET
{
    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(IN.positionCS);
    #endif

    return 0;
}

void SlimeDepthNormalsFragment(VaryingsPosNormal IN, out half4 outNormalWS : SV_Target0)
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    #ifdef LOD_FADE_CROSSFADE
        LODFadeCrossFade(IN.positionCS);
    #endif

    float3 normalWS = IN.normalWS;
    outNormalWS = half4(NormalizeNormalPerPixel(normalWS), 0.0);
    
}

