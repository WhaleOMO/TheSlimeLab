#pragma once

#include "CustomUtils.hlsl"

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

Varyings OutlineVertex(Attributes IN)
{
    Varyings OUT;
    UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
    
    float4 posOS = IN.positionOS;
    // Todo: outline in view space?
    posOS.xyz += IN.normal.xyz * _OutlineWidth;
        
    #ifdef _ENABLE_SLIME_SHAKE
        posOS.xyz = SlimeUpDownShake(posOS.xyz, _ShakeSpeed, _ShakeAmount);
    #endif
    
    OUT.positionCS = TransformObjectToHClip(posOS.xyz);
    return OUT;
}

half4 OutlineFragment(): SV_TARGET
{
    return half4(_OutlineColor.rgb, 1.0);
}