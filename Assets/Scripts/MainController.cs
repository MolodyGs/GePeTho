using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool openScenesAdditive;
    public GameObject playerClone;
    public GameObject playerReference;
    Animator playerCloneAnimator;
    public string lastSceneId;
    public bool isTheMainMenu;
    public GameObject door;

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

        door = GameObject.Find("door");
    }

    private async Task LoadScene(string nombre)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nombre, LoadSceneMode.Additive);
        while (!op.isDone)
            await Task.Yield();
    }

    public async void OpenScene(string sceneId)
    {
        CloseScenes();
        Debug.Log("Abriendo escena: " + sceneId);
        await LoadScene(sceneId + "_design");
        await LoadScene(sceneId + "_art");
        lastSceneId = sceneId;
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
}
