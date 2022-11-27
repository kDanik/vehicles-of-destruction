using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Rocket : AbstractExplodable
{
    [SerializeField]
    [Tooltip("Collision force, from which block will explode")]
    private float forceToExplode;

    [SerializeField]
    [Tooltip("Object that will be created on explosion")]
    private GameObject explosionPrefab;

    [SerializeField]
    [Tooltip("Rocket speed / add force")]
    private int speed = 100;

    [SerializeField]
    [Tooltip("Duration of rocket active phase")]
    private float durationSeconds;

    [SerializeField]
    [Tooltip("Duration before shader (see Rocket shader graph) on rocket is fully active. Only affect the visuals!")]
    private float durationBeforeMaxTemreture;

    [SerializeField]
    [Tooltip("Random rocket speed variationx (from -randomSpeedVariation to randomSpeedVariation)")]
    private float randomSpeedVariation = 0.1f;

    [SerializeField]
    [Tooltip("Particle system for rocket trail in active state")]
    private ParticleSystem rocketTrailParticleSystem;

    [Tooltip("Lightsource of rocket in active state")]
    [SerializeField]
    private Light2D rocketLightSource;

    [Tooltip("Area effector of rocket in active state (pushes objects away from rocket base)")]
    [SerializeField]
    private AreaEffector2D rocketAreaEffector;

    private bool hasFuel = true;
    private float currentDuration;

    private Rigidbody2D rigidbodyBuffer;
    private Material materialBuffer;

    private void Awake()
    {
        rigidbodyBuffer = GetComponent<Rigidbody2D>();
        materialBuffer = GetComponent<Renderer>().material;

        currentDuration = 0;

        if (SceneStateManager.IsPlaymode())
        {
            ActivateRocket();
        }
    }

    private void FixedUpdate()
    {
        if (SceneStateManager.IsEditorMode()) return;

        if (hasFuel)
        {
            currentDuration += Time.deltaTime;

            UpdateShaderColorIntensity();
            rigidbodyBuffer.AddRelativeForce(GetRandomSpeedMultiplier() * speed * Vector2.left);

            if (currentDuration >= durationSeconds)
            {
                hasFuel = false;
                currentDuration = durationBeforeMaxTemreture;
                DeactivateRocketVisuals();
            }
        }
        else
        {
            if (currentDuration <= 0) return;

            currentDuration -= Time.deltaTime;

            UpdateShaderColorIntensity();
        }
    }

    private void ActivateRocket()
    {
        ActivateRocketVisuals();

        rocketAreaEffector.gameObject.SetActive(true);
    }

    private void ActivateRocketVisuals()
    {
        rocketLightSource.gameObject.SetActive(true);
        rocketTrailParticleSystem.gameObject.SetActive(true);
    }

    private void DeactivateRocketVisuals()
    {
        rocketLightSource.gameObject.SetActive(false);
        rocketTrailParticleSystem.Stop();
    }

    private void UpdateShaderColorIntensity()
    {
        if (currentDuration > durationBeforeMaxTemreture)
        {
            materialBuffer.SetFloat("_temperature", 1f);
        }
        else
        {
            materialBuffer.SetFloat("_temperature", currentDuration / durationBeforeMaxTemreture);
        }
    }

    private float GetRandomSpeedMultiplier()
    {
        return 1 + Random.Range(-randomSpeedVariation, randomSpeedVariation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasFuel) return;

        float totalImpulse = CollisionImpulseCalculator.CalculateTotalImpulse(collision);

        if (totalImpulse >= forceToExplode) Explode(0);
    }

    protected override void ExplodeImplementation(float explosionForce, float explosionRadius, float explosionUpwardModifier)
    {
        if (!hasFuel) return;

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
