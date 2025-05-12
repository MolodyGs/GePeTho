using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool openScenesAdditive = false;

    public GameObject playerShadow;
    public GameObject playerReference;
    Animator playerShadowAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerReference = GameObject.Find("gepetho_reference");
        playerReference.SetActive(false);
        playerShadow = GameObject.Find("gepetho_shadow");
        playerShadowAnimator = playerShadow.GetComponent<Animator>();

        if (openScenesAdditive)
        {
            SceneManager.LoadScene("intro_scen_design", LoadSceneMode.Additive);
            SceneManager.LoadScene("intro_scen_art", LoadSceneMode.Additive);
        }
        StartCoroutine(WakingUpAnimation());
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
