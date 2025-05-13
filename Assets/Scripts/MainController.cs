using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool openScenesAdditive = false;

    public GameObject playerShadow;
    public GameObject playerReference;
    Animator playerShadowAnimator;
    public string lastSceneId = "scen_intro";

    // Start is called before the first frame update
    void Start()
    {
        // playerReference = GameObject.Find("gepetho_reference");
        // playerReference.SetActive(false);
        // playerShadow = GameObject.Find("gepetho_shadow");
        // playerShadowAnimator = playerShadow.GetComponent<Animator>();

        if (openScenesAdditive)
        {
            SceneManager.LoadScene(lastSceneId + "_design", LoadSceneMode.Additive);
            SceneManager.LoadScene(lastSceneId + "_art", LoadSceneMode.Additive);
        }
        // StartCoroutine(WakingUpAnimation());
    }

    public void OpenScene(string sceneId)
    {
        if (openScenesAdditive)
        {
            CloseScenes();
            SceneManager.LoadScene(sceneId, LoadSceneMode.Additive);
            SceneManager.LoadScene(sceneId + "_design", LoadSceneMode.Additive);
            // SceneManager.LoadScene(sceneId + "_design", LoadSceneMode.Additive);
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

    IEnumerator WakingUpAnimation()
    {
        yield return new WaitForSeconds(5f);
        playerShadowAnimator.SetBool("isWaking", true);
        yield return new WaitForSeconds(1.666f);
        playerReference.SetActive(true);
        playerShadow.SetActive(false);
        playerReference.GetComponent<Conditions>().blockMovement = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
