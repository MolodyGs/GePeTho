using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
  public string buttonId;
  PuzzleController puzzleController;
  Animator animator;

  void Start()
  {
    PuzzleController.Instance.SetVarible(buttonId, false);
    animator = GetComponent<Animator>();
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    puzzleController.UpdateVariable(buttonId, true);
    animator.SetBool("active", true);
  }
}
