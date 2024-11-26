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

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private float targetChangeTimer;

    void Start()
    {
        // Get or add Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Configure Rigidbody2D for smooth movement
        rb.gravityScale = 0; // Disable gravity
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Set initial random target
        SetRandomTarget();
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
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }
}


//public class NPCRandomMovement : MonoBehaviour
//{
//       [Header("Movement Parameters")]
//    [Tooltip("Maximum movement speed")]
//    public float maxSpeed = 10f;

//    [Tooltip("Acceleration force")]
//    public float accelerationForce = 10f;

//    [Tooltip("Time between target changes")]
//    public float targetChangeInterval = 5f;

//    [Header("Movement Boundaries")]
//    [Tooltip("Minimum X boundary")]
//    public float minX = -10f;

//    [Tooltip("Maximum X boundary")]
//    public float maxX = 10f;

//    [Tooltip("Minimum Y boundary")]
//    public float minY = -10f;

//    [Tooltip("Maximum Y boundary")]
//    public float maxY = 10f;

//    [Header("Physics Control")]
//    private Rigidbody2D rb;
//    private Vector3 targetPosition;
//    private float targetChangeTimer;

//    void Start()
//    {
//        // Get or add Rigidbody
//        rb = GetComponent<Rigidbody2D>();


//        if (rb == null)
//        {
//            rb = gameObject.GetComponent<Rigidbody2D>();
//        }


//        if (rb == null)
//        Debug.LogError(rb);

//        // Freeze rotation to prevent unwanted tilting
//        rb.freezeRotation = true;

//        // Set initial random target
//        SetRandomTarget();
//    }

//    void FixedUpdate()
//    {
//        // Update target change timer
//        targetChangeTimer += Time.fixedDeltaTime;

//        // Check if it's time to change target
//        if (targetChangeTimer >= targetChangeInterval)
//        {
//            SetRandomTarget();
//            targetChangeTimer = 0f;
//        }

//        // Move towards the current target
//        MoveTowardsTarget();
//    }

//    void SetRandomTarget()
//    {
//        // Generate a random target within specified boundaries
//        targetPosition = new Vector3(
//            Random.Range(minX, maxX),
//            Random.Range(minY, maxY), // Keep same Y position
//            transform.position.z
//        );
//    }

//    void MoveTowardsTarget()
//    {
//        // Calculate direction to the target
//        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        
//        // Calculate distance to target
//        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

//        // If very close to target, set a new random target
//        if (distanceToTarget < 0.5f)
//        {
//            SetRandomTarget();
//            return;
//        }

//        // Calculate desired velocity
//        Vector3 desiredVelocity = directionToTarget * maxSpeed;

//        // Calculate steering force
//        Vector3 steeringForce = desiredVelocity - rb.velocity;

//        // Apply acceleration based on proximity to target
//        float accelerationMultiplier = Mathf.Clamp01(distanceToTarget / 5f);
        
//        // Apply force for smooth movement
//        rb.AddForce(steeringForce * accelerationForce * accelerationMultiplier, ForceMode.Acceleration);

//        // Add slight drag near destination to prevent overshooting
//        rb.drag = distanceToTarget < 2f ? 5f : 0f;
//    }

//    // Visualize movement boundaries and current target in Scene view
//    void OnDrawGizmosSelected()
//    {
//        // Draw movement boundaries
//        Gizmos.color = Color.green;
//        Gizmos.DrawWireCube(
//            new Vector3((minX + maxX) / 2, (minY + maxY) / 2, transform.position.z), 
//            new Vector3(maxX - minX, maxY - minY, 0.1f)
//        );

//        // Draw current target
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(targetPosition, 0.5f);
//    }
//}