using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle1BigGridBrain : MonoBehaviour
{
    static public Puzzle1BigGridBrain instance;

    public Sprite on, off; // still need these

    Image[] dots; // i need to get all the children first

    // now we need a way to get the puzzle to be known when its solved

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


        var c = transform.GetChild(0);

        int cc = c.childCount;

        dots = new Image[cc];

        for (int i = 0; i < cc; i++)
        {
            // print each name
            // lets turn all of the children on now
            dots[i] = c.GetChild(i).GetComponent<Image>(); // for each child in the panel, aka each grid space, get the image component, fill dots bascially

            //if (ShouldTurnOn(c.GetChild(i).name)) dots[i].sprite = on;

            //dots[i].sprite = on;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IsPuzzleSolved()
    {
        // when is the puzzle solved? the puzzle is solved when all of the dots are matching up


        return false;
    }

    void ResetDots()
    {
        foreach (var dot in dots)
        {
            dot.sprite = off;
        }
    }

    public void TurnOnDots(string input)
    {
        ResetDots();

        foreach (var dot in dots)
        {
            if (input.Contains(dot.name)) dot.sprite = on;
        }
    }
}
