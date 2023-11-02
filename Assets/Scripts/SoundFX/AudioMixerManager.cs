using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;

    public void SetBgVolume(float volume)
    {
        Mixer.SetFloat("BG", Mathf.Log10(volume)*20);
    }

    public void SetFxVolume(float volume)
    {
        Mixer.SetFloat("UI", Mathf.Log10(volume) * 20);
    }

    public void SetKeyboardVolume(float volume)
    {
        Mixer.SetFloat("KeyboardFx", Mathf.Log10(volume) * 20);
    }

    public void GetVolume(float Bgvolume, float FxVolume,float KeyboardVolume)
    {
        Mixer.SetFloat("BG", Mathf.Log10(Bgvolume) * 20);
        Mixer.SetFloat("UI", Mathf.Log10(FxVolume) * 20);
        Mixer.SetFloat("KeyboardFx", Mathf.Log10(KeyboardVolume) * 20);
    }

}
