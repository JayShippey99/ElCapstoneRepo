using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBrain : InteractableParent
{
    public Material projM, bhM;

    public ParticleSystem particles;

    [HideInInspector]
    public bool unlocked;

    float pbOn = .53f;
    float pOn = 1f;

    [HideInInspector]
    public bool isOn;
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

    // lets actually do it so that when the puzzles are solved we just turn on a game object and that on start plays a show image animation

    GameObject alienIntel;

    public Animator stickyNote;

    const int off = 0; // not unlocked or activated
    const int on = 1; // unlocked, not activated
    const int activated = 2; // unlocked and big
    const int deactivated = 3; // going back to just on
    int state = 0;

    public float maxVis;

    public bool refreshMaterials;

    private void Start()
    {
        //print("when does this happen"); // okay so I need a way to make it so that they unlock when they need on load

        puzzles = new PlantPuzzle[puzzleHouser.childCount];

        for (int i = 0; i < puzzleHouser.childCount; i++)
        {
            puzzles[i] = puzzleHouser.GetChild(i).GetComponent<PlantPuzzle>(); // is this why???
        }

        alienIntel = puzzleHouser.GetChild(puzzleHouser.childCount - 1).gameObject;

        if (refreshMaterials) RefreshMaterials();
    }

    void RefreshMaterials()
    {
        // set all their value that change back to 0
        particles.Stop();
        projM.SetFloat("_Visibility", 0f);
        projM.SetFloat("_ProjectionAlphaIntensity", 0f);
        bhM.SetFloat("_Visibility", 0f);
    }

    private void Update() // so the sides have an on, off, and activated state now. if off, all the stats are 0 and the particles are off, if its unlocked, then the visibility goes up to 1, if its activated, then it brings up the projectionAlpha and then also turns on the particle when necessary
    {
        //print(m.GetFloat("_FlickerAmount"));

        if (!idle)
        {
            //print("is update running");
            if (state == on)
            {
                Unlocking();
            }

            if (state == activated)
            {
                Activating();
            }

            if (state == deactivated)
            {
                Deactivating();
            }
        }
    }

    void Unlocking()
    {
        //print("are we unlocking?");
        if (progress < 1)
        {
            progress += Time.deltaTime * speedOn;

            projM.SetFloat("_Visibility", ac1.Evaluate(progress));
            bhM.SetFloat("_Visibility", ac2.Evaluate(progress));
        }
        else
        {
            projM.SetFloat("_Visibility", 1);
            bhM.SetFloat("_Visibility", 1);
            idle = true;
        }
    }

    void Activating()
    {
        if (progress < 1)
        {
            progress += Time.deltaTime * speedOn;

            projM.SetFloat("_ProjectionAlphaIntensity", ac1.Evaluate(progress) * .1f); // .1f cause that's just the max brightness we want that thing
        }
        else
        {
            projM.SetFloat("_ProjectionAlphaIntensity", .1f);
            idle = true;
            particles.Play();
        }
    }

    void Deactivating()
    {
        if (progress > 0)
        {
            progress -= Time.deltaTime * speedOff;

            projM.SetFloat("_ProjectionAlphaIntensity", ac2.Evaluate(progress) * .1f);

            //print("deactivating " + ac2.Evaluate(progress) * .1f);
        }
        else
        {
            projM.SetFloat("_ProjectionAlphaIntensity", 0);
            idle = true;
            particles.Stop();
        }
    }

    public void Unlock()
    {
        // this line looks weird but we won't refresh if we load up a side, but then for eerything else they get reset
        refreshMaterials = false;
        //print("do we unlock?");

        // this could cue up an animtion or something
        unlocked = true;
        state = on;
        idle = false;
        progress = 0;

        // do we want unlock to mess with which puzzle is activated? probably not
    }

    public void SetState(bool on) // set state means show or hide, has nothing to do with being focused, BUT if its being hidden, then it'll shrink
    {
        //print("turning on");
        if (unlocked)
        {
            if (on)
            {
                //print("activate");
                state = activated;
                progress = 0;
            }
            else
            {
                state = deactivated;
                progress = 1;
            }


            idle = false;
        }
    }

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
        projM.SetFloat("_FlickerAmount", flickerAmount);
    }



}
