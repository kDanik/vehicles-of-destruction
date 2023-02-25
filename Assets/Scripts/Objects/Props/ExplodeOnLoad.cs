using UnityEngine;

public class ExplodeOnLoad : AbstractExplodable
{
    private void Start()
    {
        ExplodeInstantly();
    }

    protected override void ExplodeImplementation(float explosionForce, float explosionRadius, float explosionUpwardModifier)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, explosionRadius))
        {
            if (gameObject.Equals(collider.gameObject)) continue;

            Rigidbody2D rigidbody2D = collider.gameObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                Rigidbody2DExtensions.AddExplosionForce(rigidbody2D, explosionForce, transform.position, explosionRadius, explosionUpwardModifier);
            }

        }
    }
}
