using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreqScanClass
{
    public Texture layer1Tex, layer2Tex, layer3Tex;

    [Range(-1, 1)]
    public float layer1Level, layer2Level, layer3Level;
}
