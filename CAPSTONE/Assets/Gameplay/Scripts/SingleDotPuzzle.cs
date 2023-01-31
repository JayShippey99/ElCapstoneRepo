using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDotPuzzle : MonoBehaviour, IDotPuzzle
{
    // This code will control the first dot puzzle, and hopefully, it will give me some insight on a script for more puzzles
    // What will this script ask for?
    // it will ask if the input is more than one character long
    // needs to be an instance which fucking sucks lmfao
    // maybe I can make a dot puzzle parent script or something
    // loop through or something
    // oohhhh you know what we can run a loop for all the puzzles that run a "input has been sent" trigger that's a cool idea
    // I feel like there's no way i'm making a consistent script for everything

    public static SingleDotPuzzle instance;
    public Dot dot;



    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GetInput(string t)
    {
        //print("AM I STILL RUNNING??");
        if (t.Length >= 1)
        {
            // set the correct child to on
            dot.SetOn(true);
            GameController.instance.GoToNextSection(); // that's the win condition, A WIN CONDITION parent function
            // I also need a pause first
        }
    }

    public bool CheckIfSolved()
    {
        throw new System.NotImplementedException();
    }

    public bool IsMatchingReferenceGrid(Transform rg)
    {
        throw new System.NotImplementedException();
    }
}
