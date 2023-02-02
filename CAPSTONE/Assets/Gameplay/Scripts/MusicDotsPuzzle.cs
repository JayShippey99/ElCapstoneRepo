using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDotsPuzzle : MonoBehaviour
{
    public TouchDot[] dots;

    float winDelay = 3;

    bool solved = false;
    public void CheckIfAllAreCorrect()
    {
        foreach (var dot in dots)
        {
            if (!dot.correct)
            {
                //print(dot.name);
                return;
            }
        }

        //print("PUZZLE IS DONE");
        // I need to add a delay now before it goes back
        solved = true;
        
    }

    private void Update()
    {
        if (solved)
        {
            winDelay -= Time.deltaTime;
            //if (winDelay <= 0) Tesseract.instance.EndSoundPuzzle();
        }
    }

}
