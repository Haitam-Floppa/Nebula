using UnityEngine;
using UnityEngine.UI;

public class HollowKnightController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;

    private bool isGrounded;
    private bool canDoubleJump;

    public LayerMask groundLayer;

    public Animator animator; // Reference to the Animator component
    public Text characterStateText; // Reference to the UI Text element

    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float doubleJumpForce = 9f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>(); // Assuming the Animator is on the same GameObject

        // Find any Text component in the scene
        characterStateText = FindObjectOfType<Text>();

        // Optional: Set the initial animation state
        SetAnimationState("Idle");
    }

    private void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics2D.IsTouchingLayers(coll, groundLayer);

        // Player Input
        float horizontalInput = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");

        // Handle Movement
        HandleMovement(horizontalInput);

        // Handle Jumping
        HandleJump(jumpInput);

        // Update Animator
        UpdateAnimator(horizontalInput);

        // Update UI Text
        UpdateCharacterStateText();
    }

    private void HandleMovement(float horizontalInput)
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Flip the character sprite based on movement direction
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump(bool jumpInput)
    {
        if (jumpInput)
        {
            if (isGrounded)
            {
                // Regular jump
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                // Double jump
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                canDoubleJump = false;
            }
        }
    }

    private void UpdateAnimator(float horizontalInput)
    {
        float speed = Mathf.Abs(horizontalInput);
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", isGrounded);

        // Additional logic for transitioning between animations if needed
    }

    private void UpdateCharacterStateText()
    {
        float fps = 1f / Time.deltaTime;

        if (characterStateText != null)
        {
            characterStateText.text = $"Grounded: {isGrounded}, Speed: {Mathf.Abs(rb.velocity.x)}, FPS: {fps:F2}";

            // Check for frame drops (optional)
            if (Time.deltaTime > 0.02f) // Adjust the threshold as needed
            {
                characterStateText.text += "\nFrame Drop Detected!";
            }
        }
    }

    private void SetAnimationState(string state)
    {
        // Trigger the specified animation state
        animator.SetTrigger(state);
    }
}