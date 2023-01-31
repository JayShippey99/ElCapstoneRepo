using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    // the dot NEEDS to fade in and out a little, that looks so damn cool
    Sprite on, off;

    public void SetOn(bool on) // ALL of this code feels so bad lmfao
    {
        // we can get the visuals from the resources, cool
        this.on = Resources.Load<Sprite>("UniversalAssets/FullDot"); // I don't want to do this every time, i'd rather just load it all from the start I imagine rihgt?
        off = Resources.Load<Sprite>("UniversalAssets/EmptyDot");

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
