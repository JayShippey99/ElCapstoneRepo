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
        // awake runs literally only the first time it appears

        slider = GetComponent<Slider>();

        print("slider wake");
        //print(GameController.instance.GetMusicVol());
        //print(GameController.instance.GetSfxVol());
        
    }
    private void OnEnable() // enable runs after though
    {
        //print("slider enable");
        print(GameController.instance.GetMusicVol()); // when the slider shows up, we want to copy the game controller variable
        print(GameController.instance.GetSfxVol());

        // get the global volume for this slider
        if (controlMusic)
        {
            print("control music on enable");
            slider.value = GameController.instance.GetMusicVol(); // copy the global volume
            SaveAndLoadGame.ChangeMusicVolume(slider.value); // we always save the variable to the save
        }
        else
        {
            print("control sound on enable");
            slider.value = GameController.instance.GetSfxVol();
            SaveAndLoadGame.ChangeSFXVolume(slider.value);
        }
    }

    public void ChangeVolume() // the value only gets put in the saver when we change it? but if it shows up, OHh wait
    {
        float db = slider.value * 90 - 80;

        print(db + " change slider volume dbs " + slider.value + " change slider volume value");

        GameController.instance.ChangeBusVolume(controlMusic, slider.value, DecibelToLinear(db));
    }

    float DecibelToLinear(float db)
    {
        float linear = Mathf.Pow(10f, db / 20f);
        return linear;
    }
}
