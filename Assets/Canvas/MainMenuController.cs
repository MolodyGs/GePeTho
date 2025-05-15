using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject canvas;
    public MainMenuSoundController mainMenuSoundController;

    public void StartGame()
    {
        mainMenuSoundController.OnPlay();
        Debug.Log("Start game...");
        canvas.SetActive(false);
        GameObject.Find("MainController").GetComponent<MainController>().StartGame();
    }

    public void ShowCredits()
    {
        mainMenuSoundController.OnMouseClick();
        Debug.Log("Show credits...");
    }
}
