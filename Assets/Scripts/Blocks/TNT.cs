using UnityEngine;

public class TNT : AbstractExplodable
{
    [SerializeField]
    private float forceToExplode;

    [SerializeField]
    [Tooltip("Object that will be created on explosion")]
    private GameObject explosionPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        float totalImpulse = CollisionImpulseCalculator.CalculateTotalImpulse(collision);

        if (totalImpulse >= forceToExplode) Explode(0);
    }

    protected override void ExplodeImplementation(float explosionForce, float explosionRadius, float explosionUpwardModifier)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

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
