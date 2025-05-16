using System.Collections;
using UnityEngine;

public class Walk : MonoBehaviour
{
  SpriteRenderer spriteRenderer;
  public float speed;
  private Coroutine animationCoroutine;
  Rigidbody2D rb;
  Animator animator;
  public float acceleration = 0.1f;
  public float actualSpeed = 50f;

  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    spriteRenderer.flipX = false;
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  int i = 0;

  void Update()
  {

    if (GetComponent<Conditions>().blockMovement) return;

    if (Input.GetKey(KeyCode.A) && !Input.GetKeyDown(KeyCode.D))
    {
      actualSpeed = Mathf.Min(actualSpeed + acceleration * Time.deltaTime, speed);
      rb.velocity = new Vector2(-actualSpeed, rb.velocity.y);
      Debug.Log(rb.velocity);
      Debug.Log(speed);
      animator.GetBool("jump");
      animator.SetBool("walking", true);
      spriteRenderer.flipX = true;
    }

    if (Input.GetKey(KeyCode.D) && !Input.GetKeyDown(KeyCode.A))
    {
      actualSpeed = Mathf.Min(actualSpeed + acceleration * Time.deltaTime, speed);
      rb.velocity = new Vector2(actualSpeed, rb.velocity.y);
      Debug.Log(rb.velocity);
      Debug.Log(speed);
      animator.GetBool("jump");
      animator.SetBool("walking", true);
      spriteRenderer.flipX = false;
    }

    if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
    {
      rb.velocity = new Vector2(0, rb.velocity.y);
      actualSpeed = 50f;
      animator.SetBool("walking", false);
    }
  }

  void StopAnimation()
  {
    if (animationCoroutine != null)
    {
      StopCoroutine(animationCoroutine);
      animationCoroutine = null;
    }
  }
}
