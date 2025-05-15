using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool openScenesAdditive = false;
    public GameObject playerClone;
    public GameObject playerReference;
    Animator playerCloneAnimator;
    public string lastSceneId;

    void Start()
    {
        // Gepetho Reference
        playerReference = GameObject.Find("gepetho_reference");
        playerReference.SetActive(false);

        // Gepetho Clone
        playerClone = GameObject.Find("gepetho_clone");
        playerCloneAnimator = playerClone.GetComponent<Animator>();

        if (openScenesAdditive)
        {
            SceneManager.LoadScene(lastSceneId + "_design", LoadSceneMode.Additive);
            SceneManager.LoadScene(lastSceneId + "_art", LoadSceneMode.Additive);
            SceneManager.LoadScene("scen_mainMenu", LoadSceneMode.Additive);
            SceneManager.LoadScene("scen_mainSound", LoadSceneMode.Additive);
        }
    }

    public void OpenScene(string sceneId)
    {
        if (openScenesAdditive)
        {
            CloseScenes();
            SceneManager.LoadScene(sceneId, LoadSceneMode.Additive);
            SceneManager.LoadScene(sceneId + "_design", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene(sceneId);
        }
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
