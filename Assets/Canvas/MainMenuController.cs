using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        Debug.Log("Start game...");
        canvas.SetActive(false);
        GameObject.Find("MainController").GetComponent<MainController>().StartGame();
    }

    public void ShowCredits()
    {
        Debug.Log("Show credits...");
    }
}
