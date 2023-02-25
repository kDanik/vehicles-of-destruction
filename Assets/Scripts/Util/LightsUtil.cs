using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Utility class for different light effects
/// </summary>
public class LightsUtil
{
    /// <summary>
    /// Fade light blinking effect (looped)
    /// </summary>
    /// <param name="light">light to which effect should be applied</param>
    /// <param name="targetIntensityOn">Target max intensity of light</param>
    /// <param name="secondsBetweenStates">wait time between states, how much off and on state should be</param>
    /// <param name="intensityChangePerInterval">how much per tick will light intensity change</param>
    /// <param name="intervalSeconds">how slow light intensity will change</param>
    /// <returns></returns>
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

    /// <summary>
    /// Slowly turnes light off / reaches 0 light instensity
    /// </summary>
    /// <param name="light">Light2D component that should be changed</param>
    /// <param name="intensityChangePerInterval">how much per tick will light intensity decrease</param>
    /// <param name="intervalSeconds">how slow light intensity will change</param>
    /// <returns></returns>
    public static IEnumerator FadeLightOff(Light2D light, float intensityChangePerInterval = 0.05f, float intervalSeconds = 0.05f)
    {
        while (light.intensity > 0)
        {
            yield return new WaitForSeconds(intervalSeconds);
            light.intensity -= intensityChangePerInterval;
        }

        light.intensity = 0f;
    }

    /// <summary>
    /// Slowly turnes light on / reaches target light instensity
    /// </summary>
    /// <param name="light">Light2D component that should be changed</param>
    /// <param name="targetIntensity">target intensity / final intensity</param>
    /// <param name="intensityChangePerInterval">how much per tick will light intensity increase</param>
    /// <param name="intervalSeconds">how slow light intensity will change</param>
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
