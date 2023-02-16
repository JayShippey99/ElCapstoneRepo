using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using static ProjectorBeamBrain;

public class Tesseract : InteractableParent
{
    // okay I'm trying to get it so that the progress shows up and adds to the grid for this puzzle too

    // I want to come up with a good system for going from puzzle to puzzle
    // Maybe I can make puzzle paths as lists or something
    // so that you activate the one, and then it goes into the chain and then from there once one is solved it just adds onto that num

    // I'm thinking that to upright the rotation
    // now the next step is to get it to go based on which side is facing the camera. lets hope that it just lines up

    // how do I know which side is facing me. I have the code for it somewhere else

    // now I gotta have it change the dials too oof. maybe I can make the dials have a return to neutral function
    // but then how I get access to those dials, shit lol
    
    // where to put the code that makes only one side show up?


    static public Tesseract instance;

    [HideInInspector]
    public Animator animator;

    public GameObject projectorScreen, projectorBeam;

    public float speed; // turn speed
    float turnRightAmount, turnUpAmount;

    [HideInInspector]
    public bool needsToUpright; // while it needs to upright itself, turn off the responding to the dials

    Quaternion startRot, targetRot;
    public float uprightSpeed;

    Vector3 rotateTo;

    float uprightTimer;

    public SideManager sm;
    [HideInInspector]
    public Transform closestSide;

    [HideInInspector]
    public bool focused; // we need to know WHICH side its focused on too I feel like
    [HideInInspector]
    public bool projecting;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (needsToUpright == false)
        {
            // I now need to make it so that the thing unfocuses the moment that I move the thing around
            TurnRight();
            TurnUp();
        } else
        {
            //print("is this running for some reason?");
            Quaternion rotation = Quaternion.LookRotation(rotateTo); // just use forward for now, change if problems arise

            transform.rotation = Quaternion.Slerp(startRot, rotation, uprightTimer);

            uprightTimer += Time.deltaTime * uprightSpeed;

            if (uprightTimer >= 1) needsToUpright = false;
        }
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        // Regardless of which button is doing it, the tesseract will upright
        needsToUpright = true;
        startRot = transform.rotation;
        uprightTimer = 0;

        rotateTo = sm.GetRotationToClosestSide();
    }

    public override void SetSwitch(bool on)
    {
        projecting = on;
    }

    public void TurnOnProjectorStuff()
    {
        projectorScreen.SetActive(true);
        projectorBeam.SetActive(true);
    }

    public void TurnOffProjectorStuff()
    {
        projectorScreen.SetActive(false);
        projectorBeam.SetActive(false);
    }

    public void ReturnToHubState()
    {
        //print("remove all puzzles, change animation to floating in place again");
        //TurnOffProjectorStuff();
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    void TurnRight()
    {
        transform.RotateAroundLocal(Vector3.up, turnRightAmount * speed * Time.deltaTime);
    }

    void TurnUp()
    {
        transform.RotateAroundLocal(Vector3.right, turnUpAmount * speed * Time.deltaTime);
    }

    public override void ChangeSomethingDial(float f) // i gotta redo the movement code woof
    {
       // print("hello???");

        // if I set unfocus here I think that'll break things right?

        if (f < 0)
        {
            turnRightAmount = f + 30;
        }

        if (f > 0)
        {
            turnUpAmount = f - 30;
        }
    }

}
