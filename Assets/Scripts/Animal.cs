using UnityEngine;

public class Animal : MonoBehaviour
{
    // This public variable lets you type the sound in the Inspector
    // You can put "MOO" for a cow, "BAA" for a sheep, etc.
    public string animalSound = "MOO";

    // This function runs when another object enters our trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player
        if (other.CompareTag("Player"))
        {
            // It was the player! Print the sound.
            Debug.Log(animalSound);
        }
    }
}