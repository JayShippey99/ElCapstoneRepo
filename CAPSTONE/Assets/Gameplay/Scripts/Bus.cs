using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour
{
    FMOD.Studio.Bus busMusic;
    FMOD.Studio.Bus busSFX;

    [Range(-80f, 10f)]
    public float busMusicVolume;
    [Range(-80f, 10f)]
    public float busSFXVolume;
    void Start()
    {
        busMusic = FMODUnity.RuntimeManager.GetBus("bus:/BusMusic");
        busSFX = FMODUnity.RuntimeManager.GetBus("bus:/BusSFX");
    }

    // Update is called once per frame
    void Update()
    {
        busMusic.setVolume(DecibelToLinear(busMusicVolume));
        busSFX.setVolume(DecibelToLinear(busSFXVolume));
    }

    float DecibelToLinear(float db)
    {
        float linear = Mathf.Pow(10f, db / 20f);
        return linear;
    }
}
