using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool openScenesAdditive = false;
    public GameObject playerClone;
    public GameObject playerReference;
    Animator playerCloneAnimator;
    public string lastSceneId;
    public bool isTheMainMenu;

    async void Start()
    {
        if (openScenesAdditive)
        {
            await CargarEscenaAsync(lastSceneId + "_design");
            await CargarEscenaAsync(lastSceneId + "_art");
            await CargarEscenaAsync("scen_mainSound");
            await CargarEscenaAsync("scen_mainMenu");
        }

        if (isTheMainMenu)
        {
            // Gepetho Reference
            playerReference = GameObject.Find("gepetho_reference");
            playerReference.SetActive(false);

            // Gepetho Clone
            playerClone = GameObject.Find("gepetho_clone");
            playerCloneAnimator = playerClone.GetComponent<Animator>();
        }
    }

    private async Task CargarEscenaAsync(string nombre)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nombre, LoadSceneMode.Additive);
        while (!op.isDone)
            await Task.Yield();
    }

    // private IEnumerator LoadingSceneAsync(string nombre, System.Action callback = null)
    // {
    //     Debug.Log("Cargando escena: " + nombre);
    //     yield return new WaitForSeconds(0.5f);
    //     {
    //         AsyncOperation operacion = SceneManager.LoadSceneAsync(nombre, );
    //         while (!operacion.isDone)
    //         {
    //             yield return null;
    //         }
    //         Debug.Log("Escena cargada. Continuar aquí.");
    //     }
    //     callback?.Invoke();
    // }

    public void OpenScene(string sceneId)
    {
        // if (openScenesAdditive)
        // {
        CloseScenes();
        // SceneManager.LoadScene(sceneId, LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneId + "_design", LoadSceneMode.Additive);
        // }
        // else
        // {
        //     SceneManager.LoadScene(sceneId);
        // }
        lastSceneId = sceneId;
    }

    public void CloseScenes()
    {
        if (openScenesAdditive)
        {
            SceneManager.UnloadSceneAsync(lastSceneId + "_design");
            SceneManager.UnloadSceneAsync(lastSceneId + "_art");
        }
    }

    public void StartGame()
    {
        StartCoroutine(WakingUpAnimation());
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
}
