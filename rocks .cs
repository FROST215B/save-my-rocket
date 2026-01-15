using UnityEngine;

public class rocksmoving : MonoBehaviour
{
    // Size range for the rocks
    public float minSize = 0.5f; // Minimum scale of the rock
    public float maxSize = 2.0f; // Maximum scale of the rock
    
    // Speed range for the rocks
    public float minSpeed = 50f; // Minimum movement speed
    public float maxSpeed = 150f; // Maximum movement speed
    public float maxSpinSpeed = 10f; // Maximum rotation speed (torque)
    
    Rigidbody2D rb; // Physics component for movement
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Randomize the rock's size
        float randomSize = Random.Range(minSize, maxSize); // Pick a random size between min and max
        transform.localScale = new Vector3(randomSize, randomSize, 1); // Apply the size to X and Y (Z stays 1 for 2D)
        
        // Get the Rigidbody2D component attached to this rock
        rb = GetComponent<Rigidbody2D>();
        
        // Calculate random speed (inversely proportional to size - smaller rocks move faster)
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize; // Divide by size so small rocks are faster
        
        // Get a random direction for the rock to move
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // Get random point in circle and normalize for consistent direction
        
        // Apply force to make the rock move in that direction
        rb.AddForce(randomDirection * randomSpeed, ForceMode2D.Impulse); // Impulse mode applies instant force
        
        // Make the rock spin randomly
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed); // Random spin (negative = counterclockwise, positive = clockwise)
        rb.AddTorque(randomTorque); // Apply the rotational force
    }
    
    // Update is called once per frame
    void Update()
    {
        // Currently empty - no per-frame updates needed for this rock behavior
    }
}