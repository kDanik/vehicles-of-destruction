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

    [Tooltip("Collider tracker on area effector of rocket in active state (pushes objects away from rocket base)")]
    [SerializeField]
    private CollisionTracker rocketAreaEffectorCollisionTracker;

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

            AddRocketForces();

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

        rocketAreaEffectorCollisionTracker.gameObject.SetActive(true);
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

    private void AddRocketForces()
    {
        float force = GetRandomSpeedMultiplier() * speed;

        rigidbodyBuffer.AddRelativeForce(force * Vector2.left);
        if (Random.Range(1, 4) == 1) AddRocketAreaEffectorForce(force);
    }

    /// <summary>
    /// Area effector for this rocket. It check the collider list from collision tracker and adds force to object in it.
    /// Only adds force to object, that are directly colliding by this trail (and not behind some other object).
    /// </summary>
    /// <param name="force">Force (without direction) that should be used by this area effector</param>
    private void AddRocketAreaEffectorForce(float force)
    {
        foreach (Collider2D collider in rocketAreaEffectorCollisionTracker.GetColliders())
        {
            if (collider.gameObject.layer.Equals(Physics.IgnoreRaycastLayer)) return;

            Vector3 direction = collider.gameObject.transform.position - rocketAreaEffectorCollisionTracker.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(rocketAreaEffectorCollisionTracker.transform.position, direction);

            if (collider.Equals(hit.collider) && collider.attachedRigidbody != null)
            {
                collider.attachedRigidbody.AddForce(force * Random.Range(0.05f, 0.4f) * direction, ForceMode2D.Impulse);
            }
        }
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
