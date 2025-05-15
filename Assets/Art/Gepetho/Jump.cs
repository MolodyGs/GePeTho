using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour
{
  public float jumpForce;
  public float fallMultiplier = 2.5f;

  private Rigidbody2D rb;
  private bool isGrounded = true;
  bool gravityModified = false;
  bool jump = false;
  bool falling = false;
  private float originalGravityScale;
  Animator animator;
  float lastVelocityY;
  float currentVelocityY;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    originalGravityScale = rb.gravityScale;
    animator = GetComponent<Animator>();
  }

  void Update()
  {
    if (GetComponent<Conditions>().blockMovement) return;

    if (Input.GetKeyDown(KeyCode.Space) && !OnAir())
    {
      isGrounded = false;
      jump = true;
      animator.SetBool("grounded", false);
      animator.SetBool("jump", true);
      Debug.Log("Jumping");
    }
  }

  void FixedUpdate()
  {

    if (GetComponent<Conditions>().blockMovement) return;

    if (jump)
    {
      rb.velocity += new Vector2(0, jumpForce);
      jump = false;
    }

    currentVelocityY = rb.velocity.y;

    if (lastVelocityY > 0 && currentVelocityY <= 0)
    {
      if (!falling)
      {
        falling = true;
        isGrounded = false;
        animator.SetBool("falling", true);
        animator.SetBool("grounded", false);
        // Debug.Log("Falling");
      }
    }
    else if (currentVelocityY == 0)
    {
      falling = false;
      isGrounded = true;
      animator.SetBool("falling", false);
      animator.SetBool("jump", false);
      animator.SetBool("grounded", true);
      // Debug.Log("Grounded");
    }

    if (!isGrounded && !Input.GetKey(KeyCode.Space) && !gravityModified)
    {
      rb.gravityScale = originalGravityScale * fallMultiplier;
      gravityModified = true;
    }

    if (gravityModified && !OnAir())
    {
      gravityModified = false;
      rb.gravityScale = originalGravityScale;

    }

    lastVelocityY = currentVelocityY;
  }

  bool OnAir()
  {
    return !isGrounded || falling;
  }
}
