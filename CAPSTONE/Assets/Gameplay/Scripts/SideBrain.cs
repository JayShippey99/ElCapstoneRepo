using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBrain : InteractableParent
{
    public Material m;

    ParticleSystem particles;

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

    private void Start()
    {
        //if (instance == null) instance = this;
        //else Destroy(gameObject);

        particles = transform.GetChild(0).GetComponent<ParticleSystem>();
        particles.Stop();

        //m.SetFloat("_Gradient1", 0f);
        m.SetFloat("_Visibility", 0f);
        //m.SetFloat("_FlickerAmount", 0f);

        puzzles = new PlantPuzzle[puzzleHouser.childCount - 1];

        for (int i = 0; i < puzzleHouser.childCount - 1; i++)
        {
            puzzles[i] = puzzleHouser.GetChild(i).GetComponent<PlantPuzzle>();
        }

        alienIntel = puzzleHouser.GetChild(puzzleHouser.childCount - 1).gameObject;
    }

    private void Update()
    {
        //print(m.GetFloat("_FlickerAmount"));

        if (!idle)
        {
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

    public void SetState(bool on)
    {
        print("turning on");
        isOn = on;
        idle = false;
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
        m.SetFloat("_FlickerAmount", flickerAmount);
    }



}
