using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
  public string buttonId;
  Animator animator;

  void Start()
  {
    PuzzleController.Instance.SetVarible(buttonId, false);
    animator = GetComponent<Animator>();
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    PuzzleController.Instance.UpdateVariable(buttonId, true);
    animator.SetBool("active", true);
  }
}
