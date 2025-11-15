using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Crop : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite harvestedSprite; // Drag your 'harvested' sprite here in the Inspector

    private SpriteRenderer spriteRenderer;
    private Collider2D col; // We'll disable this after harvesting
    private bool isHarvested = false;

    void Start()
    {
        // Get the components on this object
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // This function runs automatically when another Collider2D enters our trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If we've already harvested this, do nothing.
        if (isHarvested)
        {
            return;
        }

        // We check if the object that entered has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // It was the player!
            
            // 1. Find the PlayerCollection script on the player object
            PlayerCollection playerCollection = other.GetComponent<PlayerCollection>();

            // 2. Check if the player actually has that script
            if (playerCollection != null)
            {
                // 3. Call the AddCrop() function on the player's script
                playerCollection.AddCrop();

                // --- This is the new part ---
                
                // 4. Mark as harvested so this can't run again
                isHarvested = true;

                // 5. Change the sprite to the harvested version
                if (harvestedSprite != null)
                {
                    spriteRenderer.sprite = harvestedSprite;
                }
                
                // 6. Disable the trigger so the player can't 'collect' it again
                col.enabled = false;
            }
        }
    }
}