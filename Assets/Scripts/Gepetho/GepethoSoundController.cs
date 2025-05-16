using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GepethoSoundController : MonoBehaviour
{
    // Walk variables
    public AudioClip[] stepsSfx;
    public AudioSource[] audioSources;
    int audioSourceIndex = 0;

    // Jump variables
    public AudioSource jumpAudioSource;

    public void StepSound()
    {
        audioSources[audioSourceIndex].PlayOneShot(stepsSfx[Random.Range(0, stepsSfx.Length)]);
        audioSourceIndex++;
        if (audioSourceIndex >= audioSources.Length)
        {
            audioSourceIndex = 0;
        }
    }

    public void JumpSound()
    {
        jumpAudioSource.PlayOneShot(stepsSfx[Random.Range(0, stepsSfx.Length)]);
    }
}
