using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SondFxkeyboardManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip[] KeyboardAudio;

    public void PlyerAudioKeyboardIsTrue()
    {
        audioSource.PlayOneShot(KeyboardAudio[0]);
    }

    public void PlyerAudioKeyboardIsFalse()
    {
        audioSource.PlayOneShot(KeyboardAudio[1]);
    }
}
