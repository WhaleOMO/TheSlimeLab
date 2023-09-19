#pragma once

/**
 * \brief Map a value from range(min1, max1) to range(min2, max2)
 * \return remapped value
 */
float map(float value, float min1, float max1, float min2, float max2)
{
    // Convert the current value to a percentage
    const float perc = (value - min1) / (max1 - min1);
    // Do the same operation backwards
    const float val = perc * (max2 - min2) + min2;
    return val;
}

/**
 * \brief Average value of v1~4
 * \return average value
 */
half avg4(half v1, half v2, half v3, half v4)
{
    return (v1+v2+v3+v4) * 0.25;
}

half pow2(half value)
{
    return value * value;
}

/**
 * \brief Shake Slime up and down with sine wave
 * \param objectPos object space vertex position, should be from -1 to 1
 * \param shakeSpeed the frequency of shaking
 * \param shakeAmount the intensity of shaking
 * \return 
 */
float3 SlimeUpDownShake(float3 objectPos, float shakeSpeed, float shakeAmount)
{
    const float x = _Time.y * shakeSpeed;
    const float shakeMask = pow(map(objectPos.y, -1, 1, 0, 1), 2.5);
    const float shake = shakeAmount * shakeMask * abs(sin(x));
    return objectPos + float3(0,shake,0);
}

/**
 * \brief A fast translucency shading approach based on
 *        "Approximating Translucency for a Fast, Cheap and Convincing Subsurface-Scattering Look" by Colin Barre-Brisebois at GDC 2011
 * \param lightDir light direction (point towards light)
 * \param lightColor light color
 * \param albedo base color of object
 * \param viewDir viewing direction
 * \param normal world space normal
 * \param thickness local space object thickness, use fresnel as an approximation
 * \param shadow shadow value, 0 being in shadow
 * \return a additional color that should be added to the shading(diffuse) result
 */
half3 FastTranslucency(half3 lightDir, half3 lightColor, half3 albedo, half3 viewDir, half3 normal, half thickness, half shadow)
{
    const half3 halfNL = normalize(normalize(lightDir) + normal * 0.5);
    const half HdotV = pow(saturate(dot(viewDir, -halfNL)), 10) * 3;
    half3 translucentColor = shadow * (HdotV + 0.4) * albedo * lightColor * saturate(thickness);
    return translucentColor;
}