using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

    // if you are focused and looking at the intel, there are no lights on

    public SideManager sm;
    public GameObject laser;
    LaserBrain lb;

    bool laserOn;
    float laserTimer = .25f;

    Tesseract t;

    int lightsOnNum;

    PlantPuzzle currentPuzzle;

    private void Start()
    {
        t = GameController.instance.tesseract;

        lb = laser.GetComponent<LaserBrain>();
    }

    private void Update()
    {
        currentPuzzle = GameController.instance.currentPuzzle;

        if (readingInput)
        {
            //print("hello?");
            readTimer += Time.deltaTime;

            if (readTimer >= timeForEachInput)
            {
                //print("does this ever go?");
                readTimer = 0;

                //print(output[currentIndex]);
                if (t.focused)
                {
                    //FMODUnity
                    RuntimeManager.PlayOneShot("event:/LaserFire");
                    //print("enable");
                    lb.SetColor(output[currentIndex]);
                    EnableLaser();
                    //print("should be doing the noise");

                    if (currentPuzzle != null) currentPuzzle.MakeBranches(output[currentIndex].ToString());
                }
                else
                {
                    sm.FlickerSides();
                    RuntimeManager.PlayOneShot("event:/LaserFire");
                    lb.SetColor(output[currentIndex]);
                    EnableLaser();
                    //print("should be doing the noise");
                }

                if (currentIndex + 1 < output.Length) currentIndex++;
                else
                {

                    //print("is this possibly a one time thing?");
                    DoneReadingInput();
                    return;
                }

                
            }
        }

        if (t.projecting) // ah okay so this is controlling how many lights are on and runs all the time
        {
            if (t.focused)
            {
                // when you're focused and projecting
                TurnAllLightsOff();
                if (currentPuzzle != null) // this needs to be here though
                {
                    lightsOnNum = currentPuzzle.emptyBranches.Count;
                    if (currentPuzzle.emptyBranches.Count == 0) lightsOnNum = 1;
                    TurnLightsOnForEmpties(lightsOnNum);
                }
            }
            else
            {
                // when you're projecting but not focused, set all lights to on and lightonnum to 8

                foreach (_Light l in lights)
                {
                    l.SetLight(true);
                }

                lightsOnNum = 8; // it seems like this part of the code is running when I hit the focus button?
            }
        }
        else
        {
            TurnAllLightsOff();
        }

        if (laserOn)
        {
            //print("should be on");
            if (laserTimer > 0) laserTimer -= Time.deltaTime;
            else
            {
                //print("turn off");
                DisableLaser();
            }
        }
    }

    void EnableLaser()
    {
        laser.SetActive(true);
        laserOn = true;
        laserTimer = .25f;
    }

    void DisableLaser()
    {
        laser.SetActive(false);
        laserOn = false;
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

        lightsOnNum = 0;
    }

    public override void DoSomethingButton(GameObject theButton) // the button click is only once, we then need it to start reading
    {
        // omg if there are no lights on this breaks

        if (!readingInput)
        {

            output = "";

            for (int i = 0; i < lightsOnNum; i++) // when you click the send button, you ask how many lights are on and only get that length of output
            {
                output = output + inputs[i].tmp.text;
            }

            //print(output);

            foreach (BinaryInputBrain bib in inputs) // you clear the inputs back to displaying 0s
            {
                bib.Clear();
            }

            // technically I don't really want any of this to run yet
            if (lightsOnNum > 0)
            {
                readingInput = true;
                currentIndex = 0;
            }
        }

    }

    void DoneReadingInput()
    {
        readingInput = false;
        if (currentPuzzle != null) currentPuzzle.UpdateEmptyBranches();
    }

}
