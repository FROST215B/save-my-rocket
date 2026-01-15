using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class player_script : MonoBehaviour
{
    // Score variables
    private int score = 0; // Current player score
    public float scoreMultiplier = 10f; // How fast the score increases per second
    private float elapsedTime = 0f; // Total time played in current session
    
    // Movement variables
    public float thrustForce = 1f; // How strong the player's thrust is when clicking
    
    // Visual effects and UI references
    public GameObject boosterFlame; // Visual effect that shows when thrusting
    public UIDocument uiDocument; // Reference to the UI system
    public GameObject explosionEffect; // Effect shown when player dies
    private Label scoreLabel; // UI element that displays the score
    private Button restartButton; // Button to restart the game after dying
    public GameObject bounceEffectPrefab; // Effect shown at collision point
    public GameObject borderParent; // Parent object containing border elements

    Rigidbody2D rb; // Physics component for player movement
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Rigidbody2D component attached to this game object
        rb = GetComponent<Rigidbody2D>();
        
        // Set up the UI elements
        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement; // Get the root UI element
            scoreLabel = root.Q<Label>("ScoreLabel"); // Find the score label by name
            restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton"); // Find restart button
            restartButton.style.display = DisplayStyle.None; // Hide restart button at start
            restartButton.clicked += ReloadScene; // Subscribe to button click event
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update score based on time survived
        elapsedTime += Time.deltaTime;  // Add the time since last frame to elapsed time
        Debug.Log("Elapsed time: " + elapsedTime); // Log time to console
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier); // Calculate score (time * multiplier, rounded down)
        Debug.Log("Score: " + score); // Log score to console
        
        // Update the UI to show current score
        if (scoreLabel != null)
        {
            scoreLabel.text = "Score: " + score;
        }

//====================================================================
        // Handle player input and movement
        if (Mouse.current.leftButton.wasPressedThisFrame) // When mouse button is first pressed
        {
            boosterFlame.SetActive(true); // Show the booster flame effect
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame) // When mouse button is released
        {
            boosterFlame.SetActive(false); // Hide the booster flame effect
        }
        
        if (Mouse.current.leftButton.isPressed) // While mouse button is held down
        {
            // Calculate direction from player to mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value); // Convert screen position to world position
            Vector2 direction = (mousePos - transform.position).normalized; // Get direction vector and normalize it

            transform.up = direction; // Rotate the player to face the mouse cursor
            rb.AddForce(direction * thrustForce); // Apply force in that direction to move the player
        }
    }
    
    // Called when this object collides with another 2D collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Player death sequence
        Instantiate(explosionEffect, transform.position, transform.rotation); // Create explosion effect at player position
        Destroy(gameObject); // Destroy the player object
        restartButton.style.display = DisplayStyle.Flex; // Show the restart button
        
        // Create bounce effect at collision point
        Vector2 contactPoint = collision.GetContact(0).point; // Get the exact point where collision occurred
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity); // Create bounce effect
        borderParent.SetActive(false); // Hide the border

        Destroy(bounceEffect, 1f); // Destroy the bounce effect after 1 second
    }
    
    // Restart the game by reloading the current scene
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the active scene
    }
    
}