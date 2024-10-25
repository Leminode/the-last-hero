using UnityEngine;

public class LeverController : MonoBehaviour
{
    public Sprite leverLeftSprite;   // Sprite for the left position
    public Sprite leverMiddleSprite; // Sprite for the middle position
    public Sprite leverRightSprite;  // Sprite for the right position

    private SpriteRenderer spriteRenderer;  // Reference to the Sprite Renderer
    private int leverState = 1;  // 0 = left, 1 = middle, 2 = right

    private void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial state to middle
        spriteRenderer.sprite = leverMiddleSprite;
    }

    private void Update()
    {
        // Detect right mouse click (Mouse1)
        if (Input.GetMouseButtonDown(1))  // Right click only
        {
            // Get the mouse position in world coordinates
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the mouse is over the lever using Raycast2D
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Change the lever state if the player clicks on the lever with right click
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

        // Debug message to check the state
        Debug.Log("Lever state: " + leverState);
    }
}
