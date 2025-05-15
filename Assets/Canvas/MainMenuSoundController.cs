using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundController : MonoBehaviour
{
    public AudioClip BTN_HOVER_clip;
    public AudioClip BTN_CLICK_clip;
    public AudioClip BTN_PLAY_clip;
    public AudioSource audioSource;

    public void OnMouseClick()
    {
        audioSource.PlayOneShot(BTN_CLICK_clip);
    }

    public void OnMouseHover()
    {
        audioSource.PlayOneShot(BTN_HOVER_clip);
    }

    public void OnPlay()
    {
        audioSource.PlayOneShot(BTN_PLAY_clip);
    }
}
