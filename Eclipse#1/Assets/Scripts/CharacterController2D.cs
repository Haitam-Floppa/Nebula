using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HollowKnightController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;

    private bool isGrounded;
    private bool isDashing = false;
    private bool isJumping = false;

    public LayerMask groundLayer;

    public Animator animator;
    public Text characterStateText;

    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float maxJumpTime = 0.5f; // Adjust this value for maximum jump duration

    private float jumpTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        characterStateText = FindObjectOfType<Text>();
        SetAnimationState("Idle");
    }

    private void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(coll, groundLayer);

        float horizontalInput = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");
        bool dashInput = Input.GetKey(KeyCode.LeftShift);

        HandleMovement(horizontalInput);
        HandleJump(jumpInput);
        HandleDash(dashInput);
        UpdateAnimator(horizontalInput);
        UpdateCharacterStateText();
    }

    private void HandleMovement(float horizontalInput)
    {
        if (!isDashing)
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump(bool jumpInput)
    {
        if (jumpInput && isGrounded)
        {
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        isJumping = true;
        jumpTime = 0f;

        while (Input.GetButton("Jump") && jumpTime < maxJumpTime && isGrounded)
        {
            float jumpMultiplier = Mathf.Lerp(1f, 0.5f, jumpTime / maxJumpTime); // Adjust the second parameter for the desired jump height
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpMultiplier);
            jumpTime += Time.deltaTime;
            yield return null;
        }

        isJumping = false;
    }

    private void HandleDash(bool dashInput)
    {
        if (dashInput && !isDashing && isGrounded)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        while (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashForce, rb.velocity.y);
            yield return null;
        }

        isDashing = false;
        StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
    }

    private void UpdateAnimator(float horizontalInput)
    {
        float speed = Mathf.Abs(horizontalInput);
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsDashing", isDashing);
        animator.SetBool("IsJumping", isJumping);
    }

    private void UpdateCharacterStateText()
    {
        float fps = 1f / Time.deltaTime;

        if (characterStateText != null)
        {
            characterStateText.text = $"Grounded: {isGrounded}, Speed: {Mathf.Abs(rb.velocity.x)}, FPS: {fps:F2}, Jump Time: {jumpTime:F2}";

            if (Time.deltaTime > 0.02f)
            {
                characterStateText.text += "\nFrame Drop Detected!";
            }
        }
    }

    private void SetAnimationState(string state)
    {
        animator.SetTrigger(state);
    }
}
