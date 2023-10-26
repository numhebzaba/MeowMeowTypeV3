using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataVolumeOs")]
public class DataVolumeOS : ScriptableObject
{
    [SerializeField] private float BgVolume;
    [SerializeField] private float FxVolume;

    public void ChanegBgVolume(float volume)
    {
        BgVolume = volume;
    }

    public void ChangeFxVolume(float volume)
    {
        FxVolume = volume;
    }

    public float GetBgVolume()
    {
        return BgVolume;
    }

    public float GetFxVolum()
    {
        return FxVolume;

    }

}
