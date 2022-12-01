using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInnerCubePuzzle : MonoBehaviour
{
    // in HERE we'll do the gross stuff about all if thens maybe
    // in here will be the tesseract.moveindirection stuff
    // I'll be honest, maybe we scrap the foward and backward stuff 4 types of chars for 4 directions, why complicate it more, I think the amount of times that character is in the string will tell how much it goes
    // I feel like the cube should move back if the position isn't met in one move


    public MovementPuzzle tesseract; // I'm starting to lose it lmao

    string lastFrameInput;
    string currentFrameInput;

    SubmitToBoard stb;

    void Start()
    {
        stb = GetComponent<SubmitToBoard>(); // this works because its on the same object
        currentFrameInput = stb.text;
    }

    // Update is called once per frame
    void Update()
    {
        // maybe here we can ask the main script if the input changes, this will keep the main one even cleaner
        // actually, this doesn't make
        /*
        currentFrameInput = stb.text;

        // i could set up a boolean in the update in the submit script so that each time it submits for one frame it pops off and then we can ask for tht?
        if (currentFrameInput != lastFrameInput) // this doesn't make sense cause then that means that we can't put in the same thing twice and have it move, i'd love to send out a signal whenever a new thing is sent
        {
            CheckConditions(stb.text);
        }

        lastFrameInput = currentFrameInput;
        */
    }

    public void CheckConditions(string str)
    {
        // run stuff using the conditions script, we need to know how many times a symbol, big letter, lowercase, and number are used
        // what kind of code do we need for that
        // we need 4 new functions, check frequency of number, sym, big, and little

        //tesseract.MoveInnerCube("up");

        //print("hello");
        // save last place here
        tesseract.SetPreviousPositions();

        if (Conditions.HasNumber(str)) tesseract.MoveInnerCube("left", Conditions.HowManyNumbers(str));
        if (Conditions.HasUpper(str)) tesseract.MoveInnerCube("up", Conditions.HowManyUpperCase(str));
        if (Conditions.HasLower(str)) tesseract.MoveInnerCube("down", Conditions.HowManyLowerCase(str));
        if (Conditions.HasSymbol(str)) tesseract.MoveInnerCube("right", Conditions.HowManySymbols(str));

        // check if the last place is right
        tesseract.IsInnerCubeInside();

    }
}
