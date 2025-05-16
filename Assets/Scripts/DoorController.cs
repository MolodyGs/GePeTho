using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    MainController mainController;
    Animator animator;
    public string sceneId;
    public bool isOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        mainController = GameObject.Find("MainController").GetComponent<MainController>();
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isOpened);
    }

    // Uncomment the following method to enable collision detectio
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && isOpened)
        {
            mainController.OpenScene(sceneId);
        }
    }

    public void OpenDoor()
    {
        isOpened = true;
        animator.SetBool("isOpen", true);
        Debug.Log("Door opened");
    } 
}
