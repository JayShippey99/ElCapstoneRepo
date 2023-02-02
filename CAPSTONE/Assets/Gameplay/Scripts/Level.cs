using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public string placeHolderPuzzleTrigger;
    public string placeHolderAnimationVariable; // this could be for the trigger I guess
    public GameObject[] sections; // this will be each individual puzzle that needs to be solved
    [HideInInspector]
    public bool placeHolderCompleted;
    [Header("Outputs for finishing level")]
    public FreqScanClass noiseSignal;
}
