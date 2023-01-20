using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreqScanClass
{
    public Sprite layer1Tex, layer2Tex, layer3Tex;

    [Range(-1, 1)]
    public float layer1Level, layer2Level, layer3Level;

    [HideInInspector]
    public bool layer1Added, layer2Added, layer3Added;
}
