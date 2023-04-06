using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Slider slider;
    public bool controlMusic;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        print(GameController.instance.musicVolume);
        print(GameController.instance.sfxVolume);
    }
    private void OnEnable()
    {
        print("HELLOOOO");
        print(GameController.instance.musicVolume);
        print(GameController.instance.sfxVolume);

        // get the global volume for this slider
        if (controlMusic) slider.value = GameController.instance.musicVolume;
        else slider.value = GameController.instance.sfxVolume;
    }

    public void ChangeVolume()
    {
        print("HELLOO");
        float db = slider.value * 90 - 80;

        print(db);

        if (controlMusic) GameController.instance.ChangeBusVolume(true, slider.value, DecibelToLinear(db));
        else GameController.instance.ChangeBusVolume(false, slider.value, DecibelToLinear(db));
    }

    float DecibelToLinear(float db)
    {
        float linear = Mathf.Pow(10f, db / 20f);
        return linear;
    }
}
