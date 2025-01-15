using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpForce = 16f;
    public bool grounded;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");

        if(IsGrounded() && Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            // Slows down the velocity.y so that if you hold the jump button longer it makes you jump higher
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        CheckFlip();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded() {
        //Check if the box colliders is colliding with the ground layer.
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer).Length > 0;
        return grounded;
    }

    private void CheckFlip() {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
        }
    }

}
