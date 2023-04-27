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

    public Animator animator;

    public ParticleSystem particles;

    public float speed; // turn speed
    float turnRightAmount, turnUpAmount;

    [HideInInspector]
    public bool needsToUpright; // while it needs to upright itself, turn off the responding to the dials

    Quaternion startRot, targetRot;
    public float uprightSpeed;

    [HideInInspector]
    public Vector3 rotateTo;

    float uprightTimer;

    public SideManager sm;
    [HideInInspector]
    public Transform closestSide;

    [HideInInspector]
    public bool focused; // we need to know WHICH side its focused on too I feel like

    bool playingSoundLR, playingSoundUD;
    FMOD.Studio.EventInstance turnSoundLR;
    FMOD.Studio.EventInstance turnSoundUD;

    public Material mat;

    public RoomParticles spaceExplosion; // as psychotic as this is to keep this here it'l be the easiest to mange


    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        turnSoundLR = FMODUnity.RuntimeManager.CreateInstance("event:/TurningSoundsLR");
        turnSoundUD = FMODUnity.RuntimeManager.CreateInstance("event:/TurningSoundsUD");
        //print(animator);
    }

    private void Update()
    {

        // UGHHHH so as soon as the animator is up, then the scale goes back down to 0
        // okay so its whatever the value is in the editor
        // but what sucks is that idk of a good way I can change that

        
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

            if (uprightTimer >= 1)
            {
                // send out the focus function to the full input controller?
                FullInputController.instance.PuzzleReady();
                needsToUpright = false;
            }
        }
        
    }

    /*
    public void SetScaleOfAnimator(float size)
    {
        animator.transform.localScale = Vector3.one * size;
    }
    */

    public void ManualUpright(Vector3 rTo, Side s)
    {
        needsToUpright = true;
        startRot = transform.rotation;
        uprightTimer = 0;
        rotateTo = rTo;

        sm.closestSide = s.projection.transform;
        GameController.instance.focusedSide = s;
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        // something in here is where the mterial gets lost weirdly
        // Regardless of which button is doing it, the tesseract will upright
        needsToUpright = true;
        startRot = transform.rotation;
        uprightTimer = 0;

        rotateTo = sm.GetRotationToClosestSide();
        FullInputController.instance.Uprighting(); // something in here
        
    }

    public override void SetSwitch(bool on)
    {
        //projecting = on;
        // if true, set trigger one way, if false set trigger for reverse? no, there's only the one
        animator.enabled = true;
    }

    void TurnRight()
    {
        transform.RotateAroundLocal(Vector3.up, turnRightAmount * speed * Time.deltaTime);
    }

    void TurnUp()
    {
        transform.RotateAroundLocal(Vector3.right, turnUpAmount * speed * Time.deltaTime);
    }

    void StartTurnSound(float f, bool LR)
    {
        if (LR)
        {
            if (playingSoundLR == false && Time.time > .1)
            {
                //print("playing???");
                playingSoundLR = true;
                turnSoundLR.start();
            }
        }
        else
        {
            if (playingSoundUD == false && Time.time > .1)
            {
                //print("playing???");
                playingSoundUD = true;
                turnSoundUD.start();
            }
        }
    }

    public void TesseractLeave()
    {
        animator.SetTrigger("Leave"); // leave can actually contain the particles? nah I like the setup that I have, plus that means they can stay loaded in for next time
        // here 
        // this means
        // okay lets just say that the leave is where the star field gets brought in from
    }

    public void ExplodeStars()
    {
        spaceExplosion.Explode();
    }

    public void RemoveBrightness(float time)
    {
        StartCoroutine(cRemoveBrightness(time));
    }

    IEnumerator cRemoveBrightness(float time)
    {
        float progress = 1;

        while (progress > 0)
        {
            progress -= Time.deltaTime / time;

            mat.SetFloat("_FlashAmount", progress * .65f); // progress starts at 1, 1 * .65 is .65, goes down to 0, nice

            yield return null;
        }

        mat.SetFloat("_FlashAmount", 0);
    }

    void StopTurnSound(bool LR)
    {
        if (LR)
        {
            if (playingSoundLR == true)
            {
                turnSoundLR.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                playingSoundLR = false;
            }
        }
        else
        {
            if (playingSoundUD == true)
            {
                turnSoundUD.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                playingSoundUD = false;
            }
        }
    }

    public override void ChangeSomethingDial(float f) // i gotta redo the movement code woof
    {

        
       // print("hello???");

        // if I set unfocus here I think that'll break things right?

        if (f < 0)
        {
            turnRightAmount = f + 30;

            TurnRight();
            if (Mathf.Abs(turnRightAmount) > 1f)
            {
                StartTurnSound(turnRightAmount, true);
                turnSoundLR.setParameterByName("TurnAmountLR", Mathf.Abs(turnRightAmount) / 20f);
            }
            else
            {
               
                StopTurnSound(true);
                turnRightAmount = 0;
            }
        }

        if (f > 0)
        {
            turnUpAmount = f - 30;
            //turnSound.setParameterByName("TurnAmount", turnUpAmount / 20);
            TurnUp();
            if (Mathf.Abs(turnUpAmount) > 1f)
            {
                StartTurnSound(turnUpAmount, false);
                turnSoundUD.setParameterByName("TurnAmountUD", Mathf.Abs(turnUpAmount) / 20f);
            }
            else
            {
                StopTurnSound(false);
                turnUpAmount = 0;
            }
        }
    }

}
