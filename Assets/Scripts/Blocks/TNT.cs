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

    protected override void ExplodeImplementation()
    {
        // TODO maybe collision direction (of other object relative to this one) could be used for fancier physics

        ExplodeAllNearExploadables(10, 0.1f, 0.4f);

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 10))
        {
            if (gameObject.Equals(collider.gameObject)) continue;



            Rigidbody2D rigidbody2D = collider.gameObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                Rigidbody2DExtensions.AddExplosionForce(rigidbody2D, 3000f, transform.position, 10f, 0.2f);
            }

        }

        Destroy(gameObject);
        enabled = false;
    }
}
