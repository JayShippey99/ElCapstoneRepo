using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideManager : InteractableParent
{
    // okay so I need to:
    // while not focused, and projected, have all 8 inputs always lit up
    // if not projected, no lights are lit up
    // while not focused, all the lights are lit up
    // while focused, I need to know the side facing the camera, and get how many empty branches are open, and send that to the lights

    // I'll need a reference to the input thing with a list of lights
    // i'll get the number and then use that with a for loop

    // Is there a place where I can know if all sides are completed

    static public SideManager instance;

    //public Transform frontSide, backSide, rightSide, leftSide, topSide, bottomSide; // but i feel like its smarter to have them segmented out like this?
    Side[] sides; // just usgni the sides of the game controller we get all of these
    Transform[] tsides;
    SideBrain[] psides;

    // for now i don't need to worry about the proejction material
    // i just need to make sure that these new sides unlock properly and all tht?

    [HideInInspector]
    public Transform closestSide;

    // this is utter chaos but I think its good for now
    //public PlantPuzzle currentlyActivePuzzle;

    Tesseract t;

    // I wonder if this is where we need to do our game controller code now?

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        t = GameController.instance.tesseract;

        sides = new Side[6];

        for (int i = 0; i < 6; i++)
        {
            sides[i] = GameController.instance.sides[i];
        }

        psides = new SideBrain[6];

        for (int i = 0; i < 6; i++)
        {
            psides[i] = sides[i].projection;
        }

        tsides = new Transform[6];

        for (int i = 0; i < 6; i ++)
        {
            tsides[i] = sides[i].projection.transform;
        }

    }

    public void FlickerSides()
    {
        foreach (SideBrain pb in psides)
        {
            pb.StartFlicker();
        }
    }

    public override void SetSwitch(bool on)
    {
        
        // here's where we turn on the stuff again

        if (t.focused)
        {
            TurnOnFocusedSide();
        }
        else
        {
            print("from set switch");
            Unfocus();
        }
        
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        // focus or up focus
        t.focused = !t.focused;

        if (t.focused)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SideActivate");
            TurnOnFocusedSide();
        }
        else
        {
            print("from do something button");

            Unfocus();
        }
    }

    public override void ChangeSomethingDial(float f) // i need to make it so that when I touch the dial while its focused it unfocuses, the issue here is that I need to know if the dials are being touched cause of me or not, like are they resetting
    {
        // i can ask though if the value of f is a distance from 0
        float n = Mathf.Abs(f) - 30;

        //print(n);

        // this code is supposed to unfocus the cube when you move the dials, the goal is to make it so that you need to move the dials a certain amount before things happen

        if (Mathf.Abs(n) > .5f)
        {
            if (!t.needsToUpright)
            {
                if (t.focused)
                {
                    print("from change dial");
                    Unfocus();
                    t.focused = false;
                }
            }
        }
    }

    // in here is the first step I think. In here I'm focusing on just one side, but what I also need to do is just have one puzzle be the one that it reacts to.

    void TurnOffAll() // I don't really feel like we'll need this anymore?
    {
        foreach (SideBrain pb in psides)
        {
            pb.SetState(false);
        }
    }

    public void Unfocus() // turns on all sides that are unlocked
    {

        foreach (SideBrain pb in psides)
        {
            if (pb.unlocked)
            {
                pb.SetState(true);
            }
        }

        GameController.instance.currentPuzzle = null;

        // when unfocusing also run the full input controller function for show all or hide all depending on what the bool is
        FullInputController.instance.SidesUnfocus();

        print("what? " + t.focused);
        if (t.focused) t.animator.SetTrigger("Return"); // making this already trigger means that when i get to this path it'll just go automatically
        
    }

    void TurnOnFocusedSide()
    {
        // right, all that it comes down to is setting the currently active PUZZZLE and then turning on that side, so we can maybe call these functions from the game controller? at some point we need to get the side brain in the gc scripts

        GameController.instance.currentPuzzle = null;
        
        foreach (SideBrain pb in psides)
        {
            //print("1");
            if (pb.unlocked)
            {
                //print("2");
                if (closestSide != pb.transform)
                {
                //print("3");
                    pb.SetState(false);
                }
                else
                {
                //print("4");
                    foreach (PlantPuzzle pp in pb.puzzles) // very gross but this is how we can set the currently active puzzle
                    {
                        //print("does the error happen here?");
                        if (pp.gameObject.activeInHierarchy) GameController.instance.currentPuzzle = pp; // this should never run, which I think is good
                    }
                    
                    if (pb.name == "FrontFace") t.animator.SetTrigger("Side1");
                    if (pb.name == "BackFace") t.animator.SetTrigger("Side2");
                    if (pb.name == "RightFace") t.animator.SetTrigger("Side3");
                    if (pb.name == "LeftFace") t.animator.SetTrigger("Side4");
                    if (pb.name == "TopFace") t.animator.SetTrigger("Side5");
                    if (pb.name == "BottomFace") t.animator.SetTrigger("Side6");

                    pb.SetState(true);
                }
            }
        }

        
        if (RotationScreen.instance != null)
        {
            RotationScreen.instance.AdjustToPuzzle();
        }
        if (SpreadScreen.instance != null)
        {
            SpreadScreen.instance.AdjustToPuzzle();
        }
        
    }

    void SetClosestSide()
    {
        float shortestDistance = 9999;
        closestSide = null;
        GameController.instance.focusedSide = null;

        Vector3 cameraPos = Camera.main.transform.position;

        for (int i = 0; i < sides.Length; i ++)
        {
            if (Vector3.Distance(tsides[i].position, cameraPos) < shortestDistance)
            {
                GameController.instance.focusedSide = sides[i];
                closestSide = tsides[i];
                shortestDistance = Vector3.Distance(tsides[i].position, cameraPos);
            }
        }
        // this can't work anymore
        //GameController.instance.focusedSide = closestSide;
    }

    public Vector3 GetRotationToClosestSide()
    {
        SetClosestSide();

        Vector3 sideVector = Vector3.zero;

        // get the dot product? no. wait just normalize the position of hte closest side? and return that?
        sideVector = closestSide.position.normalized;

        // activate the trigger for the puzzle. we might be straying away from the game controller lowkey
        switch (closestSide.name)
        {
            case "FrontFace":
                // print("front");
                return Vector3.forward;
            case "BackFace":
                //print("back");
                return Vector3.back;
            case "RightFace":
                //print("right");
                return Vector3.right;
            case "LeftFace":
                // print("left");
                return Vector3.left;
            case "TopFace":
                // print("top");
                return Vector3.up;
            case "BottomFace":
                // print("bottom");
                return Vector3.down;
        }

        return sideVector;
    }
}