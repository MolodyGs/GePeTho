using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
  // For editor testing
  public bool isTheMainMenu;

  // Player variables
  public string lastSceneId;

  async void Start()
  {

    // Hold black screen 
    SceneController.Instance.HoldBlackScreen();

    // Unload all scenes except "MAIN" scene
    await SceneController.Instance.UnloadAllScenes();

    // Load the main scene
    await SceneController.Instance.LoadScene("MAIN_SOUND");

    // Intantiate the player
    PlayerController.Instance.InstantiatePlayer();

    await SceneController.Instance.OpenInitialLevel();

    PuzzleController.Instance.InitialState();

    // Load the main menu scene if isTheMainMenu is true
    if (isTheMainMenu)
    {
      await SceneController.Instance.LoadScene("MAIN_MENU");
      PlayerController.Instance.BlockMovement();
    }

    // Set the camera position to the camera reference
    CameraController.Instance.LoadCamera(lastSceneId);

    // Animete the black screen to fade out
    SceneController.Instance.HoldBlackScreen(false);
  }
}