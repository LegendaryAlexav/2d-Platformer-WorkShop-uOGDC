using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_CatchUp : MonoBehaviour
{
    private float horizontal; // Horizontal movement -1 to 1
    public float speed = 6f; // Speed of the player
    public float jumpForce = 14f; // How high the player can jump
    public bool grounded; // Check if the player is grounded
    private int isFacingRight = 1; // Check if the player is facing the right direction, 1 == true, -1 == false

    // [SerializeField]: allows the developer to view the variable in the inspector while keeping it private
    [SerializeField] private Rigidbody2D rb; // This is the physics body of the player
    [SerializeField] private BoxCollider2D groundCheck; // This is the collider to check if the player is touching the ground
    [SerializeField] private LayerMask groundLayer; // Defines to the code what layer is the ground
    [SerializeField] private Animator animator; // Accesses the Animator for the player.

    // Update runs on frames, good for most things
    private void Update() { 
        // Runs the Methods
        AllPlayerInputs();
        CheckFlip();
        UpdateAnimator();
    }

    // FixedUpdate runs on a timer, good for physics.
    private void FixedUpdate() {
        // Moves the player depending on the horizontal input and speed
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    // Send the variables to the animator
    private void UpdateAnimator() {
        animator.SetBool("IsGrounded", grounded);
        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetFloat("Vertical", Mathf.Abs(rb.velocity.y));
        animator.SetFloat("IsFacingRight", isFacingRight);
    }
    #region - Checks -

    //Check if the box colliders is colliding with the ground layer.
    private bool IsGrounded() {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer).Length > 0;
        return grounded;
    }
    
    // Updates the players idle facing positions
    private void CheckFlip() {
        if (isFacingRight > 0 && horizontal < 0f || isFacingRight < 0 && horizontal > 0f) {
            isFacingRight = isFacingRight * -1;
        }
    }

    #endregion

    #region - Inputs -

    // Runs all the player's inputs
    void AllPlayerInputs() {
        // Access Unity's input manager and converst left and right to -1 and 1 respectivally
        horizontal = Input.GetAxisRaw("Horizontal");
        
        // Check if the player is grounded and when they press the jump buttons
        if (IsGrounded() && Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Slows down the velocity.y so that if you hold the jump button longer it makes you jump higher
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    #endregion
}
