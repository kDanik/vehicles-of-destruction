using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks unique colliders that entered trigger collider of this object
/// </summary>
public class CollisionTracker : MonoBehaviour
{
    private readonly HashSet<Collider2D> colliders = new();

    /// <summary>
    /// Returns HashSet with all colliders, that entered trigger collider of this object
    /// </summary>
    public HashSet<Collider2D> GetColliders() { return colliders; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        colliders.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        colliders.Remove(other);
    }
}
