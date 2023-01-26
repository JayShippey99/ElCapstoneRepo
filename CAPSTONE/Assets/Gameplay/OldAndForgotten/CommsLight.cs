using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsLight : MonoBehaviour
{
    // so while flash count is less than 2, turn on, then wait, we wait until the timer is above time + ontime


    Light light;

    float on = 0.1f, off = .01f;

    float onTime = .1f, offTime = .3f; // on for .2 seconds, then off for .4 seconds
    float timer = 0;

    bool isOn;

    int flashAmount = 2; // flash twice aka turn on twice, then once its off again, reset the counter
    int flashCounter = 3;
    void Start()
    {
        light = GetComponent<Light>();
    }

    public void Update()
    {
        if (flashCounter < flashAmount)
        {
            if (Time.time >= timer)
            {
                //print("do it");
                if (isOn) TurnOff();
                else TurnOn();
            }
        }
    }

    public void Flash()
    {
        //print("YES");
        flashCounter = 0;
        TurnOn();
    }

    public void TurnOn()
    {
        
        isOn = true;
        light.enabled = true;
        //light.intensity = on;
        timer = Time.time + onTime;
    }

    public void TurnOff()
    {
        
        isOn = false;
        light.enabled = false;
        //light.intensity = off;
        timer = Time.time + offTime;
        flashCounter++;
    }
}
