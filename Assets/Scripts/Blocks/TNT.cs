using UnityEngine;

public class TNT : AbstractExplodable
{
    [SerializeField]
    private float forceToExplode;

    void OnCollisionEnter2D(Collision2D collision)
    {
        float totalImpulse = CollisionImpulseCalculator.CalculateTotalImpulse(collision);

        if (totalImpulse >= forceToExplode) Explode(0);
    }

    protected override void ExplodeImplementation(float explosionForce, float explosionRadius, float explosionUpwardModifier)
    {
        // TODO maybe collision direction (of other object relative to this one) could be used for fancier physics

        ExplodeAllNearExploadables(explosionRadius / 2, 0.1f, 0.4f);

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, explosionRadius))
        {
            if (gameObject.Equals(collider.gameObject)) continue;

            Rigidbody2D rigidbody2D = collider.gameObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                Rigidbody2DExtensions.AddExplosionForce(rigidbody2D, explosionForce, transform.position, explosionRadius, explosionUpwardModifier);
            }

        }

        Destroy(gameObject);
        enabled = false;
    }
}
