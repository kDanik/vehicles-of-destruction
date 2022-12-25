using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MineProp : AbstractExplodable
{
    [SerializeField]
    [Tooltip("Object that will be created on explosion")]
    private GameObject explosionPrefab;

    [SerializeField]
    private Light2D mineLight;

    [SerializeField]
    private float lightMinIntensity;

    [SerializeField]
    private float lightMaxIntensity;

    private bool isLightsCoroutineRunning = false;

    private void Awake()
    {
        if (!isLightsCoroutineRunning)
        {
            isLightsCoroutineRunning = true;
            StartCoroutine(LightsUtil.FadeLightBlinkLoop(mineLight, 1f, 0.01f));
        }
    }

    private void OnEnable()
    {
        if (!isLightsCoroutineRunning)
        {
            isLightsCoroutineRunning = true;
            StartCoroutine(LightsUtil.FadeLightBlinkLoop(mineLight, 1f, 0.01f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explode(0.2f);
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
