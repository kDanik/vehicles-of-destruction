using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour
{
    private HashSet<Collider2D> colliders = new();

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
