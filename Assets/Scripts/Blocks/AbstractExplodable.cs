using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class of explodable gameobject (Blocks, Scene props).
/// </summary>
public abstract class AbstractExplodable : MonoBehaviour
{
    /// <summary>
    /// This variable is used to prevent infinite loops with objects exploading each other forever
    /// </summary>
    protected bool WasExploded = false;

    /// <summary>
    /// Implementation of actual explosion method. AVOID Calling this method directly. Use Explode() inteads
    /// </summary>
    protected abstract void ExplodeImplementation();

    /// <summary>
    /// Main method for starting explosion. Always use this method instead Explode() directly 
    /// </summary>
    /// <param name="delayInSeconds">time in seconds before explosion</param>
    public void Explode(float delayInSeconds)
    {
        if (WasExploded) return;

        if (delayInSeconds == 0)
        {
            ExplodeInstantly();
        }
        else
        {
            StartCoroutine(ExplodeAfterTime(delayInSeconds));
        }
    }

    /// <summary>
    /// Instantly executes explode method
    /// </summary>
    public void ExplodeInstantly()
    {
        WasExploded = true;

        ExplodeImplementation();
    }

    /// <summary>
    /// Starts explosion on every gameobjects that has collider and script that inherits from this class
    /// </summary>
    /// <param name="radius">Radius of sphere to activate explosions in</param>
    /// <param name="minExplosionDelay">minimum delay before explosion</param>
    /// <param name="maxExplosionDelay">maximum delay befor explosion</param>
    protected void ExplodeAllNearExploadables(float radius, float minExplosionDelay, float maxExplosionDelay)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            if (gameObject.Equals(collider.gameObject)) continue;

            List<AbstractExplodable> explodableList = ClassTypeUtil.GetScriptWithClassTypeFromGameobject<AbstractExplodable>(collider.gameObject);

            foreach (AbstractExplodable explodable in explodableList)
            {
                if (maxExplosionDelay == 0)
                {
                    explodable.ExplodeInstantly();
                }
                else
                {
                    explodable.Explode(Random.Range(minExplosionDelay, maxExplosionDelay));
                }
            }
        }
    }

    private IEnumerator ExplodeAfterTime(float seconds)
    {
        WasExploded = true;

        yield return new WaitForSeconds(seconds);

        ExplodeImplementation();
    }
}
