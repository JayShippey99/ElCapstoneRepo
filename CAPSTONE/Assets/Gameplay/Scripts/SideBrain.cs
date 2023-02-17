using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBrain : InteractableParent
{
    public Material m;

    ParticleSystem particles;

    float visLev, prevVisLev;

    float pbOn = .53f;
    float pOn = 1f;

    bool isOn;
    float progress;

    public float speedOn, speedOff;

    public AnimationCurve ac1;
    public AnimationCurve ac2;

    bool idle;

    public Tesseract t;

    public Transform puzzleHouser; // okay so we need to bring back the branch puzzle parent ew
    [HideInInspector]
    public PlantPuzzle[] puzzles;

    // projector brain is slowly turning into just a "side" script

    bool flicker;
    float flickerAmount;
    public float flickerSpeed;

    private void Start()
    {
        //if (instance == null) instance = this;
        //else Destroy(gameObject);

        particles = transform.GetChild(0).GetComponent<ParticleSystem>();
        particles.Stop();

        //m.SetFloat("_Gradient1", 0f);
        m.SetFloat("_Visibility", 0f);
        //m.SetFloat("_FlickerAmount", 0f);

        puzzles = new PlantPuzzle[puzzleHouser.childCount];

        for (int i = 0; i < puzzleHouser.childCount; i ++)
        {
            puzzles[i] = puzzleHouser.GetChild(i).GetComponent<PlantPuzzle>();
        }
    }

    private void Update()
    {
        //print(m.GetFloat("_FlickerAmount"));

        if (!idle) {

            if (isOn)
            {
                //print("do it?");
                //particles.Play();
                if (progress < 1) progress += Time.deltaTime * speedOn;
                else
                {
                    particles.Play();
                    idle = true;
                }
        }
            else
            {
                if (progress > 0) progress -= Time.deltaTime * speedOff;
                else
                {
                    idle = true;
                    particles.Stop();
                }
            }

            //print(ac.Evaluate(progress) * pbOn);
            //print(ac.Evaluate(progress) * pOn);

            progress = Mathf.Clamp(progress, 0, 1);

            m.SetFloat("_Visibility", ac2.Evaluate(progress) * pOn);
        }

        if (flicker)
        {

            flickerAmount = Mathf.Clamp(flickerAmount, 0, 1);

            Flicker();

            if (flickerAmount > 0) flickerAmount -= Time.deltaTime * flickerSpeed;
            else
            {
                flicker = false;
            }
        }
    }

    public void GoToNextPuzzle() // how do I get access to this though from the plant script?
    {
        // turn off current puzzle, turn on the next puzzle
        for (int i = 0; i < puzzles.Length - 1; i++)
        {
            if (puzzles[i].gameObject.activeInHierarchy)
            {
                puzzles[i].gameObject.SetActive(false);
                puzzles[i + 1].gameObject.SetActive(true);
                // well fuck now we also need to tell the side manager what's the current puzzle
                SideManager.instance.currentlyActivePuzzle = puzzles[i + 1];
                return;
            }
        }

        // if its the last puzzle then this will run
        print("ALL PUZZLES FOR THIS SIDE ARE DONE");
        // here is where we would run the sequence for solving the side
        // and here is where we would show the constellation
        puzzles[puzzles.Length - 1].gameObject.SetActive(false);
    }


    public void SetState(bool on)
    {
        isOn = on;
        idle = false;
    }

    // how do we now get it to react to the button press so that it knows to turn on or off
    // okay set switch doesn't ONLY need to come from a switch, we CAN also call it from other things

    // should we make a connection to the button here?
    // and put all these things in the list of the focus button?
    // that means that like each one of them would be asking the 

    public override void DoSomethingButton(GameObject theButton) // This is for focus button
    {
        // when I click the button, I need to focus in on just one side
        // I need to know which side is facing the camera

        // I'm treating this like a toggle button actually

        if (t.focused) // when t is focused, turn everything but the one off
        {
            if (t.closestSide != transform) SetSwitch(false); // if its not facing the camera, turn it off, if its off then its already off, so this is good
        } else
        {
            // this means turn everything back on
            SetSwitch(true);
        }
    }

    public void StartFlicker()
    {
        flicker = true;
        flickerAmount = .2f;
    }

    public void Flicker()
    {
        m.SetFloat("_FlickerAmount", flickerAmount);
    }



}
