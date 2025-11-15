using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // You can set this in the inspector
    public int maxHealth = 3;
    private int currentHealth;

    // We'll add the hurt animation trigger here
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Get the Animator component on THIS player object
        animator = GetComponent<Animator>();
    }

    // Fixed the syntax error here (removed the 'S')
    public void TakeDamage(int damage)
    {
        // --- THIS IS THE HURT MECHANIC FROM YOUR REQUEST 'a' ---

        if (animator != null)
        {
            // Play the player's hurt animation
            animator.SetTrigger("isHurt"); 
        }
        else
        {
            Debug.LogWarning("Player is missing an Animator component!");
        }

        // --- THIS IS THE LOG MESSAGE FROM YOUR REQUEST 'a' ---
        Debug.Log("Player is hurt");

        currentHealth -= damage;
        Debug.Log($"Player health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // For now, we'll just log it.
        // You could later add: Destroy(gameObject) or load a game over screen
        Debug.Log("Player has died.");
    }
}