using UnityEngine;

public class EnemySkeletonAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float roamSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float roamRadius = 4f;

    [Header("Detection Settings")]
    public float chaseDistance = 3f;
    public float attackDistance = 1.0f;
    public float attackCooldown = 1f;

    [Header("Roam State (NEW)")]
    [Tooltip("The minimum time to idle before moving again.")]
    public float minIdleTime = 1.0f;
    [Tooltip("The maximum time to idle before moving again.")]
    public float maxIdleTime = 3.0f;
    [Tooltip("The minimum time to move before idling again.")]
    public float minMoveTime = 2.0f;
    [Tooltip("The maximum time to move before idling again.")]
    public float maxMoveTime = 4.0f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private Vector2 roamPoint;
    private float lastAttackTime;
    private bool attacking = false;

    // --- NEW ROAM STATE VARIABLES ---
    private enum RoamState { Idling, Moving }
    private RoamState currentRoamState = RoamState.Idling;
    private float roamStateTimer; // This timer will count down for both idling and moving
    // ---

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start by idling for a bit
        SetNewRoamState(RoamState.Idling);
    }

    void FixedUpdate()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        // If we are in the middle of an attack animation, do nothing.
        if (attacking) return;

        // --- STATE LOGIC ---
        // Decide what to do *based on distance*

        if (dist < chaseDistance && dist > attackDistance)
        {
            // --- CHASE STATE ---
            animator.SetBool("isMoving", true);
            ChasePlayer();
        }
        else if (dist <= attackDistance)
        {
            // --- ATTACK STATE ---
            animator.SetBool("isMoving", false);
            AttackPlayer();
        }
        else
        {
            // --- ROAM STATE ---
            RoamAround(); // This function is now much smarter!
        }
    }

    // --------------------
    // ROAM (HEAVILY UPDATED)
    // --------------------
    void RoamAround()
    {
        // Count down the timer for our current state
        roamStateTimer -= Time.fixedDeltaTime;

        if (currentRoamState == RoamState.Idling)
        {
            // --- IDLING LOGIC ---
            animator.SetBool("isMoving", false);
            rb.linearVelocity = Vector2.zero; // <-- UPDATED LINE: Stop movement

            if (roamStateTimer <= 0)
            {
                // Time to move!
                SetNewRoamState(RoamState.Moving);
            }
        }
        else // (currentRoamState == RoamState.Moving)
        {
            // --- MOVING LOGIC ---
            animator.SetBool("isMoving", true);
            MoveTowards(roamPoint, roamSpeed);

            // Check if we've reached our point OR our time is up
            if (roamStateTimer <= 0 || Vector2.Distance(transform.position, roamPoint) < 0.5f)
            {
                // Time to idle!
                SetNewRoamState(RoamState.Idling);
            }
        }
    }

    void SetNewRoamState(RoamState newState)
    {
        currentRoamState = newState;

        if (newState == RoamState.Idling)
        {
            // Set a random idle timer
            roamStateTimer = Random.Range(minIdleTime, maxIdleTime);
        }
        else // (newState == RoamState.Moving)
        {
            // Set a random move timer
            roamStateTimer = Random.Range(minMoveTime, maxMoveTime);
            // Pick a new point to move to
            PickNewRoamPoint();
        }
    }

    void PickNewRoamPoint()
    {
        roamPoint = (Vector2)transform.position + Random.insideUnitCircle * roamRadius;
    }

    // --------------------
    // CHASE
    // --------------------
    void ChasePlayer()
    {
        // When we chase, reset the roam state to idling
        // so it idles first when the player gets away.
        SetNewRoamState(RoamState.Idling);
        MoveTowards(player.position, chaseSpeed);
    }

    // --------------------
    // ATTACK
    // --------------------
    void AttackPlayer()
    {
        rb.linearVelocity = Vector2.zero; // <-- UPDATED LINE: freeze movement

        if (Time.time > lastAttackTime + attackCooldown)
        {
            attacking = true;
            animator.SetTrigger("isAttacking");
            lastAttackTime = Time.time;
        }
    }

    // Called by Animation Event
    public void DealDamage()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackDistance + 0.2f)
        {
            // Find the PlayerHealth script on the player and call TakeDamage
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    // Called at end of attack animation (add animation event)
    public void EndAttack()
    {
        attacking = false;
    }

    // --------------------
    // MOVE HANDLER (KINEMATIC)
    // --------------------
    void MoveTowards(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        rb.linearVelocity = dir * speed; // <-- UPDATED LINE: Use velocity

        // Flip sprite
        if (dir.x > 0.1f)
        {
            Vector3 original = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(original.x), original.y, original.z);
        }
        else if (dir.x < -0.1f)
        {
            Vector3 original = transform.localScale;
            transform.localScale = new Vector3(-Mathf.Abs(original.x), original.y, original.z);
        }
    }
}