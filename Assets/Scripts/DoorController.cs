using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    MainController mainController;
    public string sceneId;
    // Start is called before the first frame update
    void Start()
    {
        mainController = GameObject.Find("MainController").GetComponent<MainController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Uncomment the following method to enable collision detectio

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            mainController.OpenScene(sceneId);
        }
    }


    // void onColliderEnter2d(Collider2D collider)
    // {
    //     if(collider.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Door opened");
    //     }
    // }

    // void OnColliderEnter2d(Collider2D collider)
    // {
    //     if(collider.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Door opened");
    //     }
    // }
}
