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
  public GameObject playerReference;
  Animator playerCloneAnimator;

  // Variables for loading scenes 
  public string lastSceneId;

  // Level door reference
  public GameObject door;

  // Main camera variables
  public GameObject cameraPivotReference;
  public GameObject mainCameraPivot;
  public GameObject mainCameraBlackBackground;
  public Animator mainCameraBlackBackgroundAnimator;

  async void Start()
  {
    UnloadAllScenes();
    if (openScenesAdditive)
    {
      await LoadScene(lastSceneId + "_design");
      await LoadScene(lastSceneId + "_art");
      await LoadScene("scen_mainSound");
    }

    if (isTheMainMenu)
    {
      // Gepetho Reference
      playerReference = GameObject.Find("gepetho_reference");
      playerReference.SetActive(false);

      // Gepetho Clone
      playerClone = GameObject.Find("gepetho_clone");
      playerCloneAnimator = playerClone.GetComponent<Animator>();

      await LoadScene("scen_mainMenu");
    }

    LoadCamera(lastSceneId);

    door = GameObject.Find("door");
  }

  private void LoadCamera(string sceneId)
  {
    cameraPivotReference = GameObject.Find(sceneId + "_cameraReference");

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
    AsyncOperation op = SceneManager.LoadSceneAsync(nombre, LoadSceneMode.Additive);
    while (!op.isDone)
      await Task.Yield();
  }

  public async void OpenScene(string sceneId)
  {
    // active a black background
    mainCameraBlackBackgroundAnimator.SetBool("fade", true);

    // load the new scene
    await LoadScene(sceneId + "_design");
    await LoadScene(sceneId + "_art");

    StartCoroutine(Wait(() =>
    {
      // change the camera position by the camera reference
      LoadCamera(sceneId);

      // disable the black background
      mainCameraBlackBackgroundAnimator.SetBool("fade", false);

      // closing current scenes
      CloseScenes();

      // get the scene reference
      lastSceneId = sceneId;
    }));
  }

  public void CloseScenes()
  {
    Debug.Log("Cerrando escena: " + lastSceneId);
    SceneManager.UnloadSceneAsync(lastSceneId + "_design");
    SceneManager.UnloadSceneAsync(lastSceneId + "_art");
  }

  public void StartGame()
  {
    StartCoroutine(WakingUpAnimation());
    door.GetComponent<DoorController>().OpenDoor();
  }

  IEnumerator WakingUpAnimation()
  {
    yield return new WaitForSeconds(2f);
    playerCloneAnimator.SetBool("isWaking", true);
    yield return new WaitForSeconds(1.666f);
    playerReference.SetActive(true);
    playerClone.SetActive(false);
    playerReference.GetComponent<Conditions>().blockMovement = false;
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

  IEnumerator Wait(Action action)
  {
    yield return new WaitForSeconds(2f);

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
}
