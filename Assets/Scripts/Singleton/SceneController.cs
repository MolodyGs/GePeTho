using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour
{
  public static SceneController Instance { get; private set; }

  public string currentSceneId;
  public Animator mainCameraBlackBackgroundAnimator;
  bool levelIsLoading;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  public async Task OpenLevel(string sceneId)
  {
    string lastSceneId = currentSceneId;
    currentSceneId = sceneId;

    levelIsLoading = true;

    // block the player movement
    PlayerController.Instance.BlockMovement();

    // active a black background
    FadeInBlackScreen();

    // load the new scene
    await LoadScene(sceneId + "_design");
    await LoadScene(sceneId + "_art");

    levelIsLoading = false;

    await Task.Delay(1000);

    await PlayerController.Instance.SetPlayerPosition();

    // change the camera position by the camera reference
    CameraController.Instance.LoadCamera(sceneId);

    // disable the black background
    FadeOutBlackScreen();

    // Set the default state of the puzzle controller
    PuzzleController.Instance.InitialState();

    Debug.Log("Level " + sceneId + " loaded Complete");

    // block the player movement
    PlayerController.Instance.BlockMovement(false);

    if (sceneId != lastSceneId) await CloseScenes(lastSceneId);
  }

  public async Task LoadScene(string nombre)
  {

    // Verify if the scene is already loaded
    if (IsSceneLoaded(nombre)) return;

    AsyncOperation op = SceneManager.LoadSceneAsync(nombre, LoadSceneMode.Additive);
    while (!op.isDone)
    {
      Debug.Log("Loading scene " + nombre + " " + op.progress);
      await Task.Yield();
    }
    Debug.Log("Scene " + nombre + " loaded");
  }

  bool IsSceneLoaded(string sceneName)
  {
    for (int i = 0; i < SceneManager.sceneCount; i++)
    {
      Scene scene = SceneManager.GetSceneAt(i);
      if (scene.name == sceneName && scene.isLoaded)
      {
        return true;
      }
    }
    return false;
  }

  private async Task CloseScenes(string scene)
  {
    await CloseScene(scene + "_design");
    await CloseScene(scene + "_art");
  }

  private async Task CloseScene(string nombre)
  {
    AsyncOperation op = SceneManager.UnloadSceneAsync(nombre);
    while (!op.isDone)
    {
      Debug.Log("Closing scene " + nombre + " " + op.progress);
      await Task.Yield();
    }
    Debug.Log("Scene " + nombre + " closed");
  }

  public Task UnloadAllScenes()
  {
    Scene activeScene = SceneManager.GetActiveScene();

    for (int i = 0; i < SceneManager.sceneCount; i++)
    {
      Scene scene = SceneManager.GetSceneAt(i);

      if (scene != activeScene && scene.isLoaded && scene.name != "MAIN")
      {
        SceneManager.UnloadSceneAsync(scene);
      }
    }
    return Task.CompletedTask;
  }

  public async Task OpenInitialLevel()
  {
    await OpenLevel(currentSceneId);
  }

  public async Task<LevelVariables> GetLevelVariables()
  {
    await WaitUntilLevelIsLoaded();
    Debug.Log("Level loaded 100%: " + currentSceneId);
    return GameObject.Find(currentSceneId + "_LEVEL_VARIABLES").GetComponent<LevelVariables>();
  }

  public async Task WaitUntilLevelIsLoaded()
  {
    int iterations = 0;
    while (levelIsLoading)
    {
      await Task.Delay(100);
      iterations++;

      if (iterations > 100)
      {
        Debug.LogError("Level variables not loaded after 10 seconds, aborting.");
      }
    }
  }

  public void HoldBlackScreen(bool value = true)
  {
    mainCameraBlackBackgroundAnimator.SetBool("hold_black", value);
    mainCameraBlackBackgroundAnimator.SetBool("fade", false);
  }

  public void FadeInBlackScreen()
  {
    mainCameraBlackBackgroundAnimator.SetBool("fade", true);
  }

  public void FadeOutBlackScreen()
  {
    mainCameraBlackBackgroundAnimator.SetBool("fade", false);
  }
}
