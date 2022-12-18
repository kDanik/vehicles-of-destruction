using UnityEngine;

/// <summary>
/// This class contains methods for calculating impulse of 2D colisions.
/// </summary>
public static class CollisionImpulseCalculator
{
    /// <summary>
    /// This method should do the same that Collision.impulse does, but for Collision2D.
    /// Collision2D doesn't have same functionality that Collision (3D) offers.
    /// </summary>
    /// <param name="collision">use Collision2D created by OnCollisionEnter2D</param>
    /// <returns>total impulse of the collision</returns>
    public static float CalculateTotalImpulse(Collision2D collision2D)
    {
        float totalImpulse = 0;
        
        ContactPoint2D[] contacts = new ContactPoint2D[collision2D.contactCount];
        collision2D.GetContacts(contacts);

        foreach (ContactPoint2D contact in contacts)
        {
            totalImpulse += contact.normalImpulse;
        }

        return totalImpulse;
    }
}
