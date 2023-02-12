using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DropPuzzle : MonoBehaviour
{
    // from the pens, we can ask if the tesseract has the droppuzzle script enabled, and if it does this can do things
    // when a pen is picked up, and this works since only one can be picked up at a time, start a bool, when its released and is falling grow a percent bar, 
    // maybe it shakes and goes away when its gone too far
    // ohh wait what if it goes beep beep beep at a key note and then when it drops it goes bwoOP
    // OR maybe we use the cube again, maybe lets do this progress bar first and then we can decide to frame it that way later

    // we change the fill amount for our panel
    // how do we want our win condition to work now? when progress is like .9 to 1 and nothing is dropping, you win?
    // I want it to shake when its short too
    // maybe its okay though
    // well that's one puzzle down I guess, not bad

    public GameObject progressBarCanvas;
    public AnimationCurve shakeCurve;
    Vector3 originalPosition;
    public Image progressBar;
    public DeskObject[] pens; // we can loop through these and if any have the dropped condition, then we do this
    [HideInInspector]
    public bool somethingIsDropping = false;
    void Start()
    {
        originalPosition = progressBarCanvas.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        somethingIsDropping = false;
        foreach (var pen in pens)
        {
            if (pen.isDropped) // I'm confused its only going for one frame but they're dropping for longer?
            {
                somethingIsDropping = true;
                print("HEHE YUP");
            }
        }

        if (somethingIsDropping)
        {
            if (progressBar.fillAmount >= 1)
            {
                progressBar.fillAmount = 0;
                StartCoroutine(Shake());
            }
            progressBar.fillAmount += Time.deltaTime * 1.2f;
        }
        else
        {
            if (progressBar.fillAmount >= .95) print("GRAVITATIONAL FORCE LEARNED");
            progressBar.fillAmount = 0; // else means if nothing is dropping
        }
    }

    IEnumerator Shake()
    {
        float progress = 0;
        while (progress < 1)
        {
            progressBar.fillAmount = 0;
            // move the x value back and forth // we'll set it by doing og position + the curve number
            progressBarCanvas.transform.position = originalPosition + Vector3.right * shakeCurve.Evaluate(progress); // this is weird as hell lmfao
            progress += Time.deltaTime * 4;
            yield return null;
        }
    }

}
