using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle1MiniGridBrain : MonoBehaviour
{
    static public Puzzle1MiniGridBrain instance;

    // this is now the brain for the whole puzzle
    public string desiredInput;

    // i need a good way to get on and off images
    public Sprite on, off;

    Image[] miniDots; // i need to get all the children first

    Image[] bigDots;

    bool solved;
    float winDelay = 3;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        var c1 = transform.GetChild(0);

        int cc = c1.childCount;

        miniDots = new Image[cc];

        for (int i = 0; i < cc; i++)
        {
            // print each name
            // lets turn all of the children on now
            miniDots[i] = c1.GetChild(i).GetComponent<Image>(); // for each child in the panel, aka each grid space, get the image component, fill miniDots bascially

            if (ShouldTurnOn(c1.GetChild(i).name)) miniDots[i].sprite = on;

            //miniDots[i].sprite = on;
        }



        var c2 = transform.GetChild(1);

        bigDots = new Image[cc];

        for (int i = 0; i < cc; i++)
        {
            // print each name
            // lets turn all of the children on now
            bigDots[i] = c2.GetChild(i).GetComponent<Image>(); // for each child in the panel, aka each grid space, get the image component, fill dots bascially

            //if (ShouldTurnOn(c.GetChild(i).name)) dots[i].sprite = on;

            //dots[i].sprite = on;

        }
    }

    bool ShouldTurnOn(string name)
    {
        if (desiredInput.Contains(name)) return true;

        return false;
    }

    bool IsPuzzleSolved()
    {
        for (int i = 0; i < miniDots.Length; i ++)
        {
            if (miniDots[i].sprite != bigDots[i].sprite)
            {
                return false;
            }
        }
        return true;
    }

    void ResetDots()
    {
        foreach (var dot in bigDots)
        {
            dot.sprite = off;
        }
    }

    public void TurnOnDots(string input)
    {
        ResetDots();
        foreach (var dot in bigDots)
        {
            if (input.Contains(dot.name)) dot.sprite = on;
        }

        if (IsPuzzleSolved()) solved = true;
    }

    void Update()
    {
        if (solved)
        {
            winDelay -= Time.deltaTime;
        }

        if (winDelay <= 0)
        {
            // instead of closing the puzzle, do that i guess, but also, spawn the next puzzle in the sequence
            // do this tomorrow, I should go
            //Tesseract.instance.CloseABCPuzzle(); // this function kills this script
            //gameObject.SetActive(false);

            // so instead of going back to the tesseract I need instead to get the game conrtoller now
            // could I make a next in the sequence function?
            GameController.instance.GoToNextSection();

            // okay so the issue now is getting the plant puzzles to work with the current system, cause I'm only making one visible at a time
        }
    }

}
