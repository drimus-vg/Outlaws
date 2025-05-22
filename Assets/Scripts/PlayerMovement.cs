using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public GameObject objectToSpawn; // Assign the prefab in the Inspector
    public Transform spawnPoint;     // Set the spawn position in the Inspector (can be empty GameObject)
    public float shootCooldown = 1f; // Cooldown duration (in seconds)
    private float lastShotTime = 0f; // Time when the last shot was fired

    public float disableTime = 1f;           // Time to disable player movement
    private float disableTimer = 0f;         // Timer to track the disable duration


    private Animator animator; // Reference to the Animator

    public bool isActive;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (!isActive)
        {
            disableTimer -= Time.deltaTime;
            animator.SetTrigger("Hit");
            // When the timer reaches zero, re-enable the player
            if (disableTimer <= 0f)
            {
                EnablePlayer();
            }
        }
        if (isActive)
        {
            // Input
            movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
            movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrow

            if (Time.time > lastShotTime + shootCooldown)
            {
                animator.SetTrigger("Attack");
            }
            if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + shootCooldown)
            {
                SpawnObject(); // Call the method to spawn the bullet
                lastShotTime = Time.time; // Update the time of the last shot
            }
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetTrigger("MoveUp"); // Trigger MoveUp animation
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetTrigger("MoveDown"); // Trigger MoveDown animation
            }
            else if (Time.time <= lastShotTime + shootCooldown)
            {
                animator.SetTrigger("Idle"); // Trigger Idle or default animation
            }
        }        
    }

    void SpawnObject()
    {
        if (isActive)
        {
            if (objectToSpawn != null && spawnPoint != null)
            {
                // Determine the Z rotation based on input
                float zRotation = 0f;

                if (Input.GetKey(KeyCode.W))
                {
                    zRotation = 45f; // Facing up (default rotation)
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    zRotation = -45f; // Facing down
                }
                else
                {
                    zRotation = 0f; // Facing sideways (example rotation)
                }

                // Create a rotation based on the zRotation value
                Quaternion rotation = Quaternion.Euler(0, 0, zRotation);

                // Instantiate the object with the determined rotation
                Instantiate(objectToSpawn, spawnPoint.position, rotation);
            }
            else
            {
                Debug.LogWarning("Object to Spawn or Spawn Point not set!");
            }
        }        
    }

    void GetHit()
    {
        // Play the "Hit" animation
        animator.SetTrigger("Hit");

        // Disable the player movement and start the timer
        DisablePlayer();
    }

    void DisablePlayer()
    {
        // Set the timer and mark the player as inactive
        disableTimer = disableTime;
        isActive = false;
    }

    void EnablePlayer()
    {
        // Mark the player as active
        isActive = true;
    }


    void FixedUpdate()
    {
        if (isActive)
        {
            // Movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with a bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GetHit(); // Trigger the hit effect
            Destroy(collision.gameObject);
        }
    }
}