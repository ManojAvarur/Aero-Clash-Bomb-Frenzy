using UnityEngine;

public class NPCRandomMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [Tooltip("Maximum movement speed")]
    public float maxSpeed = 5f;

    [Tooltip("Acceleration force")]
    public float accelerationForce = 5f;

    [Tooltip("Time between target changes")]
    public float targetChangeInterval = 3f;

    [Header("Movement Boundaries")]
    [Tooltip("Minimum X boundary")]
    public float minX = -10f;

    [Tooltip("Maximum X boundary")]
    public float maxX = 10f;

    [Tooltip("Minimum Y boundary")]
    public float minY = -10f;

    [Tooltip("Maximum Y boundary")]
    public float maxY = 10f;

    [Header("Movement Settings")]
    [Tooltip("Should movement be completely random or smoother")]
    public bool smoothMovement = true;

    [Header("Bullet Controller")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float minfireRate = 0.3f; // How often the player can shoot
    [SerializeField] private float maxfireRate = 0.7f; // How often the player can shoot
    private float nextFireTime = 0f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private float targetChangeTimer;
    private SpriteRenderer spriteRenderer;
    System.Random random;

    void Start()
    {
        // Get or add Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        

        // Configure Rigidbody2D for smooth movement
        rb.gravityScale = 0; // Disable gravity
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Set initial random target
        SetRandomTarget();

        spriteRenderer.color = HealthDataStore.getInstance(gameObject.name).getHealthColor();

        random = new();
    }

    void FixedUpdate()
    {
        // Update target change timer
        targetChangeTimer += Time.fixedDeltaTime;

        // Check if it's time to change target
        if (targetChangeTimer >= targetChangeInterval)
        {
            SetRandomTarget();
            targetChangeTimer = 0f;
        }

        // Move towards the current target
        MoveTowardsTarget();
        FireBullet();

        spriteRenderer.color = HealthDataStore.getInstance(gameObject.name).getHealthColor();
    }

    void SetRandomTarget()
    {
        // Generate a random target within specified boundaries
        targetPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }

    void MoveTowardsTarget()
    {
        // Calculate direction to the target
        Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;

        // Calculate distance to target
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        // If very close to target, set a new random target
        if (distanceToTarget < 0.5f)
        {
            SetRandomTarget();
            return;
        }

        if (smoothMovement)
        {
            // Smooth physics-based movement
            Vector2 desiredVelocity = directionToTarget * maxSpeed;
            Vector2 steeringForce = desiredVelocity - rb.velocity;

            float accelerationMultiplier = Mathf.Clamp01(distanceToTarget / 5f);
            rb.AddForce(steeringForce * accelerationForce * accelerationMultiplier, ForceMode2D.Force);
        }
        else
        {
            // Direct, less physics-based movement
            rb.velocity = directionToTarget * maxSpeed;
        }
    }

    // Visualize movement boundaries in Scene view
    void OnDrawGizmosSelected()
    {
        // Draw movement boundaries
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0),
            new Vector3(maxX - minX, maxY - minY, 0.1f)
        );

        // Draw current target
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }

    void FireBullet()
    {
        if (Time.time <= 2 || Time.time < nextFireTime)
        {
            return;
        }

        Vector3 spawnPosition = gameObject.transform.position - new Vector3(0, .7f, 0);

        GameObject newBullet = Instantiate(bullet, spawnPosition, gameObject.transform.rotation);
        newBullet.name = gameObject.name + "'s Bullet";

        Bullet bulletClass = newBullet.GetComponent<Bullet>();

        bulletClass.setBulletFrom(gameObject.name);
        bulletClass.setBulletDirection(Vector2.up);
        newBullet.SetActive(true);

        nextFireTime = Time.time + 1f / (float) System.Math.Round(random.NextDouble() * (maxfireRate - minfireRate) + minfireRate, 1);
    }
}