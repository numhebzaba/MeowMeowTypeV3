using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundFXManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip ButtonAudioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClickButton()
    {
        audioSource.PlayOneShot(ButtonAudioClip);
    }

}
