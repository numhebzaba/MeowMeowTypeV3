using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundFxVolumeManager : MonoBehaviour
{
    [SerializeField] private Slider BgMusicSlider;
    [SerializeField] private Slider FxSlider;
    [SerializeField] private Slider KeyboardFxSlider;

    [SerializeField] private AudioMixerManager audioMixer;
    [SerializeField] private DataVolumeOS DataVolume;

    private void Start()
    {
        BgMusicSlider.value = DataVolume.GetBgVolume();
        FxSlider.value = DataVolume.GetFxVolum();
        KeyboardFxSlider.value = DataVolume.GetKeyboardFxVolume();

        audioMixer.GetVolume(BgMusicSlider.value, FxSlider.value,KeyboardFxSlider.value);

        BgMusicSlider.onValueChanged.AddListener(OnBgSliderValueChanged);
        FxSlider.onValueChanged.AddListener(OnFxSliderValueChanged);
        KeyboardFxSlider.onValueChanged.AddListener(OnKeyboardBulueChanged);
    }

    private void OnBgSliderValueChanged(float volume)
    {
        DataVolume.ChanegBgVolume(volume);
        audioMixer.SetBgVolume(volume);
        audioMixer.GetVolume(BgMusicSlider.value, FxSlider.value, KeyboardFxSlider.value);
    }

    private void OnFxSliderValueChanged(float value)
    {
        DataVolume.ChangeFxVolume(value);
        audioMixer.SetFxVolume(value);
        audioMixer.GetVolume(BgMusicSlider.value, FxSlider.value, KeyboardFxSlider.value);
    }

    private void OnKeyboardBulueChanged(float value)
    {
        DataVolume.ChangeKeyboardFxVolume(value);
        audioMixer.SetKeyboardVolume(value);
        audioMixer.GetVolume(BgMusicSlider.value, FxSlider.value, KeyboardFxSlider.value);
    }

}
