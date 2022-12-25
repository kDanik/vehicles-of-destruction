using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsUtil
{
    public static IEnumerator FadeLightBlinkLoop(Light2D light, float targetIntensityOn, float secondsBetweenStates, float intensityChangePerInterval = 0.05f, float intervalSeconds = 0.05f)
    {
        while (true)
        {
            yield return FadeLightOff(light, intensityChangePerInterval, intervalSeconds);
            yield return new WaitForSeconds(secondsBetweenStates);
            yield return FadeLightOn(light, targetIntensityOn, intensityChangePerInterval, intervalSeconds);
            yield return new WaitForSeconds(secondsBetweenStates);
        }
    }

    public static IEnumerator FadeLightOff(Light2D light, float intensityChangePerInterval = 0.05f, float intervalSeconds = 0.05f)
    {
        while (light.intensity > 0)
        {
            yield return new WaitForSeconds(intervalSeconds);
            light.intensity -= intensityChangePerInterval;
        }

        light.intensity = 0f;
    }

    public static IEnumerator FadeLightOn(Light2D light, float targetIntensity, float intensityChangePerInterval = 0.05f, float intervalSeconds = 0.05f)
    {
        while (light.intensity < targetIntensity)
        {
            yield return new WaitForSeconds(intervalSeconds);
            light.intensity += intensityChangePerInterval;
        }

        light.intensity = targetIntensity;
    }
}
