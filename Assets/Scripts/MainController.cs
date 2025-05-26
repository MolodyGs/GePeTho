using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
  // For editor testing
  public bool openScenesAdditive;
  public bool isTheMainMenu;

  // Introduction level variables
  public GameObject playerClone;
  Animator playerCloneAnimator;

  // Player variables
  public GameObject playerPrefab;
  public GameObject playerPositionReference;
  public GameObject player;

  // Variables for loading scenes 
  public string lastSceneId;

  // Main camera variables
  public GameObject cameraPivotReference;
  public GameObject mainCameraPivot;
  public GameObject mainCameraBlackBackground;
  public Animator mainCameraBlackBackgroundAnimator;

  // Puzzle controller reference
  public PuzzleController puzzleController;

  async void Start()
  {
    UnloadAllScenes();

    await LoadScene("MAIN_SOUND");
    if (isTheMainMenu) await LoadScene("MAIN_MENU");

    if (openScenesAdditive)
    {
      await LoadScene(lastSceneId + "_design");
      await LoadScene(lastSceneId + "_art");
      StartCoroutine(Wait(lastSceneId, () =>
      {
        puzzleController.SetInitialState();
      }));
    }

    if (GetPlayerPositionReference())
    {
      player = Instantiate(playerPrefab, playerPositionReference.transform.position, Quaternion.identity);
    }

    if (isTheMainMenu)
    {
      player.SetActive(false);

      // Gepetho Clone
      playerClone = GameObject.Find("gepetho_sleep");
      Debug.Log("Player clone found: " + playerClone);
      playerCloneAnimator = playerClone.GetComponent<Animator>();

      await LoadScene("MAIN_MENU");
    }

    GetPlayerPositionReference();

    LoadCamera(lastSceneId);
  }

  private void LoadCamera(string sceneId)
  {
    cameraPivotReference = GameObject.Find(sceneId + "_camera_reference");

    if (cameraPivotReference == null)
    {
      Debug.LogError("Camera reference not found!!! " + sceneId + "_cameraReference");
      return;
    }

    Debug.Log("Camera reference Found! " + sceneId + "_cameraReference");

    mainCameraPivot.transform.position = new Vector3(cameraPivotReference.transform.position.x, cameraPivotReference.transform.position.y, cameraPivotReference.transform.position.z);
  }

  private async Task LoadScene(string nombre)
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

  private async Task CloseScenes(string scene)
  {
    await CloseScene(scene + "_design");
    await CloseScene(scene + "_art");
  }

  public async void OpenScene(string sceneId)
  {

    // block the player movement
    player.GetComponent<Conditions>().blockMovement = true;

    // active a black background
    mainCameraBlackBackgroundAnimator.SetBool("fade", true);

    // load the new scene
    await LoadScene(sceneId + "_design");
    await LoadScene(sceneId + "_art");

    StartCoroutine(Wait(sceneId, async () =>
    {

      // closing current scenes
      await CloseScenes(lastSceneId);

      if (GetPlayerPositionReference())
      {
        player.transform.position = new Vector3(playerPositionReference.transform.position.x, playerPositionReference.transform.position.y, playerPositionReference.transform.position.z);
        player.GetComponent<Conditions>().blockMovement = false;
      }

      // change the camera position by the camera reference
      LoadCamera(sceneId);

      // disable the black background
      mainCameraBlackBackgroundAnimator.SetBool("fade", false);

      // get the scene reference
      lastSceneId = sceneId;

      // Set the default state of the puzzle controller
      puzzleController.SetInitialState();
    }, true));
  }

  public void StartGame()
  {
    StartCoroutine(WakingUpAnimation());
    puzzleController.ForcePuzzleComplete();
  }

  IEnumerator WakingUpAnimation()
  {
    yield return new WaitForSeconds(2f);
    playerCloneAnimator.SetBool("isWaking", true);
    yield return new WaitForSeconds(1.666f);
    player.SetActive(true);
    playerClone.SetActive(false);
    player.GetComponent<Conditions>().blockMovement = false;
  }

  void UnloadAllScenes()
  {
    Scene activeScene = SceneManager.GetActiveScene();

    for (int i = 0; i < SceneManager.sceneCount; i++)
    {
      Scene scene = SceneManager.GetSceneAt(i);

      if (scene != activeScene && scene.isLoaded)
      {
        SceneManager.UnloadSceneAsync(scene);
      }
    }
  }

  IEnumerator Wait(string sceneId, Action action, bool preWait = false)
  {

    if(preWait) yield return new WaitForSeconds(1.5f);

    bool designSceneLoaded = false;
    bool artSceneLoaded = false;
    while (true)
    {
      yield return new WaitForSeconds(0.1f);

      Debug.Log("Checking scene...");
      for (int i = 0; i < SceneManager.sceneCount; i++)
      {

        if (SceneManager.GetSceneAt(i).name == sceneId + "_design")
        {
          designSceneLoaded = true;
          continue;
        }

        if (SceneManager.GetSceneAt(i).name == sceneId + "_art")
        {
          artSceneLoaded = true;
          continue;
        }
      }

      if (designSceneLoaded && artSceneLoaded)
      {
        Debug.Log("Scenes " + sceneId + "_design and " + sceneId + "_art loaded");
        break;
      }
      designSceneLoaded = false;
      artSceneLoaded = false;
    }

    action();
  }

  IEnumerator WaitAnimation(string stateName)
  {
    // Esperar hasta que comience el estado
    while (!mainCameraBlackBackgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
      yield return null;

    // Esperar hasta que termine el estado
    while (mainCameraBlackBackgroundAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
      yield return null;

    Debug.Log("Animación terminada");
  }

  private bool GetPlayerPositionReference()
  {
    Debug.Log("Loading player position...");
    playerPositionReference = GameObject.Find("player_position_reference");
    Debug.Log("The player position found: " + playerPositionReference);
    if (playerPositionReference == null)
    {
      Debug.LogError("Player position reference not found!!! " + lastSceneId);
      return false;
    }
    Debug.Log("Player position loaded !!!");
    playerPositionReference.GetComponent<SpriteRenderer>().enabled = false;
    return true;
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
}