using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// This class contains Rigidbody AddExplosionForce() alternative, but for Rigidbody2D
/// </summary>
public static class Rigidbody2DExtensions
{
    private static readonly float randomForceVariation = 0.10f;

    /// <summary>
    /// Calculates and simulates explosion force applied to given rigidbody2D with given explosion data
    /// </summary>
    /// <param name="rigidbody2D">rigidbody that force will applied to</param>
    /// <param name="explosionForce">force of explosion (at its center)</param>
    /// <param name="explosionPosition">world position of explosion source</param>
    /// <param name="explosionRadius">radius of explosion</param>
    /// <param name="upwardsModifier">how much force vector should be modified in upwards direction</param>
    /// <param name="mode">Force mode of explosion force, default is ForceMode2D.Force</param>
    public static void AddExplosionForce(this Rigidbody2D rigidbody2D, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier, ForceMode2D mode = ForceMode2D.Force)
    {
        if (explosionRadius <= 0f) Debug.LogError("Invalid explosionRadius! " + explosionRadius);

        List<Collider2D> attachedColliders = new();
        rigidbody2D.GetAttachedColliders(attachedColliders);
        Bounds bounds = CalculateBounds(attachedColliders, rigidbody2D);

        // Get the closest point on the rigidbody2D for calculating the explosion distance.
        Vector2 closestPoint = bounds.ClosestPoint(explosionPosition);

        bool isOverlappingExplosion = attachedColliders.Count > 0 && closestPoint == explosionPosition;

        // Get the force position.
        var upwardsExplosionPosition = explosionPosition + Vector2.down * upwardsModifier;
        var forcePosition = (Vector2)bounds.ClosestPoint(upwardsExplosionPosition);
        if (forcePosition == upwardsExplosionPosition)
        {
            forcePosition = rigidbody2D.worldCenterOfMass;
        }

        Vector2 force = CalculateForce(closestPoint, explosionPosition, explosionRadius, explosionForce, upwardsExplosionPosition, forcePosition, isOverlappingExplosion);

        rigidbody2D.AddForceAtPosition(force, forcePosition, mode);
    }

    private static Vector2 CalculateForce(Vector2 closestPoint, Vector2 explosionPosition, float explosionRadius, float explosionForce, Vector2 upwardsExplosionPosition, Vector2 forcePosition, bool isOverlappingExplosion) {
        float forceDistanceWearoff = CalculateExplosionForceWearoff(closestPoint, explosionPosition, explosionRadius);

        Vector2 forceDirection = CalculateForceDirection(forcePosition, upwardsExplosionPosition, isOverlappingExplosion);

        return CalculateFinalForceValue(randomForceVariation, forceDirection, forceDistanceWearoff, explosionForce);
    }

    private static Vector2 CalculateForceDirection(Vector2 forcePosition, Vector2 upwardsExplosionPosition, bool isOverlapping)
    {
        Vector2 forceDirection = forcePosition - upwardsExplosionPosition;

        if (forceDirection.sqrMagnitude <= float.Epsilon)
        {
            forceDirection = Vector2.up;    // Default the force direction to up.
        }
        else if (!isOverlapping)
        {
            forceDirection.Normalize();     // Only normalize the force direction if we aren't overlapping with the explosion.
        }

        return forceDirection;
    }

    private static Vector2 CalculateFinalForceValue(float randomForceVariation, Vector2 forceDirection, float forceWearoff, float explosionForce)
    {
        float randomForceMultiplier = 1 + Random.Range(-randomForceVariation, randomForceVariation);

        return explosionForce * forceWearoff * forceDirection * randomForceMultiplier;
    }

    private static Bounds CalculateBounds(List<Collider2D> attachedColliders, Rigidbody2D rigidbody2D)
    {
        Bounds bounds = new Bounds();

        if (attachedColliders.Count == 0)
        {
            bounds = new Bounds(rigidbody2D.worldCenterOfMass, Vector3.zero);
        }
        else
        {
            foreach (Collider2D collider in attachedColliders)
            {
                bounds.Encapsulate(collider.bounds);
            }
        }

        return bounds;
    }

    private static float CalculateExplosionForceWearoff(Vector2 closestPoint, Vector2 explosionPosition, float explosionRadius)
    {
        // Get the explosion distance.
        var explosionDistance = (closestPoint - explosionPosition).magnitude;

        // If the explosion distance is further than the explosion radius, explosion force is 0
        if (explosionDistance > explosionRadius)
        {
            return 0f;
        }

        return 1 - explosionDistance / explosionRadius;
    }
}
