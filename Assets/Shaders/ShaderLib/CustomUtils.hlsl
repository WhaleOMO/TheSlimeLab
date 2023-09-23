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