using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullInputController : InteractableParent
{
    public BinaryInputBrain[] inputs;
    public _Light[] lights;

    string output;

    bool readingInput;
    float readTimer;
    public float timeForEachInput;
    int currentIndex; // counter to get each binary at a time

    // I kind of want to just hook up the interactions now instead of working on the delays and stuff
    // With the input string thing, I needed a instance ref to the puzzles
    // I guess I can still do that??

    // lets put a delay on each thing being sent even though that means it'll only place on the end

    //  I wanna make it so that there's no final blips that give more signals

    public SideManager sm;

    int lightsOnNum;

    private void Update()
    {
        


        if (readingInput)
        {
            //print("hello?");
            readTimer += Time.deltaTime;

            if (readTimer >= timeForEachInput)
            {
                //print("does this ever go?");
                readTimer = 0;
                if (currentIndex + 1 < output.Length) currentIndex++;
                else
                {
                    //print("is this possibly a one time thing?");
                    DoneReadingInput();
                    return;
                }

                print(output[currentIndex]);
                if (sm.focused) sm.currentlyActivePuzzle.MakeBranches(output[currentIndex].ToString());
                else
                {
                    sm.FlickerSides();
                }
            }
        }

        if (sm.projecting)
        {
            if (!sm.focused)
            {
                // do a constant check of which side is toward the camera
                // NO because it only matters when I focus and then when I focus i know

                foreach (_Light l in lights)
                {
                    l.SetLight(true);
                }

                lightsOnNum = 8;
            }
            else
            {
                // here we need to know the empty branch amount of the current puzzle
                TurnAllLightsOff();
                lightsOnNum = sm.currentlyActivePuzzle.emptyBranches.Count;
                if (sm.currentlyActivePuzzle.emptyBranches.Count == 0) lightsOnNum = 1;
                TurnLightsOnForEmpties(lightsOnNum);
            }
        }
        else
        {
            TurnAllLightsOff();
        }
    }

    void TurnLightsOnForEmpties(int n)
    {
        for (int i = 0; i < n; i++)
        {
            lights[i].SetLight(true);
        }
    }

    void TurnAllLightsOff()
    {
        foreach (_Light l in lights)
        {
            l.SetLight(false);
        }
    }

    public override void DoSomethingButton(GameObject theButton) // the button click is only once, we then need it to start reading
    {
        if (!readingInput)
        {
            readingInput = true;

            output = "";

            for (int i = 0; i < lightsOnNum; i++) // I guess we can just hardwire this to 8
            {
                output = output + inputs[i].tmp.text;
            }

            //print(output);

            foreach (BinaryInputBrain bib in inputs)
            {
                bib.Clear();
            }

            currentIndex = 0;

            print(output[currentIndex]);
            if (sm.focused) sm.currentlyActivePuzzle.MakeBranches(output[currentIndex].ToString());
            else // if NOT focused, flicker everything
            {
                sm.FlickerSides();
            }
            //if (sm.focused) sm.currentlyActivePuzzle.MakeBranches(output); // so instead of the instance, maybe we just apply one directly
            // get the currently active puzzle
            // why does this not work after doing it on one side?
        }

    }

    void DoneReadingInput()
    {
        readingInput = false;
        sm.currentlyActivePuzzle.UpdateEmptyBranches();
    }

}
