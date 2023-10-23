using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ButtonAudioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

    }
}
