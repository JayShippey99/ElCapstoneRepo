using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Dot : MonoBehaviour
{
    // the dot NEEDS to fade in and out a little, that looks so damn cool
    Sprite on, off;

    public bool isOn;
    bool prevIsOn;

    private void Update()
    {
        if (!prevIsOn && isOn) 
        {
            SetOn(true);
        }

        if (prevIsOn && !isOn)
        {
            SetOn(false);
        }

        prevIsOn = isOn;
    }

    public void SetOn(bool on) // ALL of this code feels so bad lmfao
    {
        // we can get the visuals from the resources, cool
        this.on = Resources.Load<Sprite>("UniversalAssets/FullDot"); // I don't want to do this every time, i'd rather just load it all from the start I imagine rihgt?
        off = Resources.Load<Sprite>("UniversalAssets/EmptyDot");

        isOn = on;

        if (on)
        {
            
            GetComponent<SpriteRenderer>().sprite = this.on;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = off;
        }

    }
}
