using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour
{
  public float jumpForce;
  public float fallMultiplier = 2.5f;

  private Rigidbody2D rb;
  private bool isGrounded = false;
  bool gravityModified = false;
  bool jump = false;
  bool falling = false;
  private float originalGravityScale;
  Animator animator;

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
      // rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      isGrounded = false;
      jump = true;
      animator.SetBool("grounded", false);
      animator.SetBool("jump", true);
    }

    // if (!Input.GetKey(KeyCode.Space) && !isGrounded)
    // {
    //   rb.velocity += 10 * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
    // }
  }

  void FixedUpdate()
  {

    if (GetComponent<Conditions>().blockMovement) return;

    if (jump)
    {
      rb.velocity += new Vector2(0, jumpForce);
      jump = false;
    }

    if (rb.velocity.y <= 0)
    {
      if (!isGrounded)
      {
        falling = true;
        animator.SetBool("falling", true);
      }
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
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    isGrounded = true;
    falling = false;
    animator.SetBool("grounded", true);
    animator.SetBool("falling", false);
    animator.SetBool("jump", false);
  }

  bool OnAir()
  {
    return !isGrounded || falling;
  }
}
