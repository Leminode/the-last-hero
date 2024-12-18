using UnityEngine;
using UnityEngine.Events;

public class LeverController : MonoBehaviour
{
    public Sprite leverLeftSprite;   // Sprite for the left position
    public Sprite leverMiddleSprite; // Sprite for the middle position
    public Sprite leverRightSprite;  // Sprite for the right position

    private SpriteRenderer spriteRenderer;  // Reference to the Sprite Renderer
    private int leverState = 1;  // 0 = left, 1 = middle, 2 = right

    public float activationRange = 2f;  // Range within which the player can activate the lever
    private Transform playerTransform;  // Reference to the player's transform

    public UnityEvent<int> leverStateChanged;
    
    private void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial state to middle
        spriteRenderer.sprite = leverMiddleSprite;

        // Find the player by tag (make sure the player has the tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        // Check if the player is within activation range
        if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= activationRange)
        {
            // Detect if the 'E' key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeLeverState();
            }
        }
    }

    // This method changes the lever state in a cycle: left -> middle -> right -> left
    private void ChangeLeverState()
    {
        // Change the state of the lever
        leverState = (leverState + 1) % 3;  // Cycle between 0, 1, and 2

        // Update the sprite based on the new lever state
        switch (leverState)
        {
            case 0:
                spriteRenderer.sprite = leverLeftSprite;  // Set to left sprite
                break;
            case 1:
                spriteRenderer.sprite = leverMiddleSprite;  // Set to middle sprite
                break;
            case 2:
                spriteRenderer.sprite = leverRightSprite;  // Set to right sprite
                break;
        }
        
        leverStateChanged.Invoke(leverState);

        // Debug message to check the state
        Debug.Log("Lever state: " + leverState);
    }
}
