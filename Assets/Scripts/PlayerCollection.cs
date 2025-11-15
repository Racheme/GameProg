using UnityEngine;

public class PlayerCollection : MonoBehaviour
{
    // This variable will store our collection count
    public int cropCount = 0;

    // This is a public function that other scripts (like the crop) can call
    public void AddCrop()
    {
        // 1. Add 1 to the count
        cropCount++;

        // 2. Print the log message to the console
        // The '$' allows us to insert the variable {cropCount} directly
        Debug.Log($"Crop harvested: {cropCount}");
    }
}