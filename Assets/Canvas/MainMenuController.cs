using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
  public GameObject canvas;
  public MainMenuSoundController mainMenuSoundController;

  private void Start()
  {
    PlayerController.Instance.GetPlayer().GetComponent<Animator>().SetBool("sleeping", true);
    PlayerController.Instance.SetPlayerLightOff();
  }

  public void StartGame()
  {
    Debug.Log("Start game...");
    mainMenuSoundController.OnPlay();
    StartCoroutine(WakingUpAnimation());
    PuzzleController.Instance.ForcePuzzleComplete();
    canvas.SetActive(false);
    PlayerController.Instance.SetPlayerLightOn();
  }

  public void ShowCredits()
  {
    mainMenuSoundController.OnMouseClick();
    Debug.Log("Show credits...");
  }

  IEnumerator WakingUpAnimation()
  {
    PlayerController.Instance.GetPlayer().GetComponent<Animator>().SetBool("sleeping", false);
    yield return new WaitForSeconds(2f);
    PlayerController.Instance.BlockMovement(false);
  }
}
