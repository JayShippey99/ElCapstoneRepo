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

    static public SideManager instance;

    public Transform frontSide, backSide, rightSide, leftSide, topSide, bottomSide;
    Transform[] tsides;
    SideBrain[] psides;

    [HideInInspector]
    public Transform closestSide;

    [HideInInspector]
    public bool projecting;
    [HideInInspector]
    public bool focused;

    // this is utter chaos but I think its good for now
    public PlantPuzzle currentlyActivePuzzle;

    // I wonder if this is where we need to do our game controller code now?

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


        tsides = new Transform[]
        {
            frontSide, backSide, rightSide, leftSide, topSide, bottomSide
        };

        psides = new SideBrain[]
        {
            frontSide.GetComponent<SideBrain>(),
            backSide.GetComponent<SideBrain>(),
            rightSide.GetComponent<SideBrain>(),
            leftSide.GetComponent<SideBrain>(),
            topSide.GetComponent<SideBrain>(),
            bottomSide.GetComponent<SideBrain>()
        };
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
        projecting = on;

        if (projecting)
        {
            if (focused)
            {
                TurnOnFocusedSide();
            }
            else
            {
                TurnOnAll();
            }
        }
        else
        {
            TurnOffAll();
        }
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        // focus or up focus
        focused = !focused;

        if (projecting)
        {
            if (focused)
            {   
                TurnOnFocusedSide();
            }
            else
            {
                TurnOnAll();
            }
        }
    }

    public override void ChangeSomethingDial(float f) // i need to make it so that when I touch the dial while its focused it unfocuses, the issue here is that I need to know if the dials are being touched cause of me or not, like are they resetting
    {
        // i can ask though if the value of f is a distance from 0
        float n = Mathf.Abs(f) - 30;

        print(n);

        // this code is supposed to unfocus the cube when you move the dials, the goal is to make it so that you need to move the dials a certain amount before things happen

        if (Mathf.Abs(n) > .5f)
        {
            if (Tesseract.instance != null)
            {
                if (!Tesseract.instance.needsToUpright)
                {
                    if (focused)
                    {
                        focused = false;

                        if (projecting)
                        {
                            TurnOnAll(); // ugh okay this is not going to work
                                         // this is probably what is turning everything back on
                        }
                    }
                }
            }
        }
    }

    // in here is the first step I think. In here I'm focusing on just one side, but what I also need to do is just have one puzzle be the one that it reacts to.

    void TurnOffAll()
    {
        foreach (SideBrain pb in psides)
        {
            pb.SetState(false);
        }
    }

    void TurnOnAll()
    {
        foreach (SideBrain pb in psides)
        {
            pb.SetState(true);
        }
    }

    void TurnOnFocusedSide()
    {
        foreach (SideBrain pb in psides)
        {
            if (closestSide != pb.transform) pb.SetState(false);
            else
            {

                foreach (PlantPuzzle pp in pb.puzzles) // very gross but this is how we can set the currently active puzzle
                {
                    if (pp.gameObject.activeInHierarchy) currentlyActivePuzzle = pp;
                }

                pb.SetState(true);
            }
        }
    }

    void SetClosestSide()
    {
        float shortestDistance = 9999;
        closestSide = null;

        Vector3 cameraPos = Camera.main.transform.position;

        foreach (Transform side in tsides)
        {
            if (Vector3.Distance(side.position, cameraPos) < shortestDistance)
            {
                closestSide = side;
                shortestDistance = Vector3.Distance(side.position, cameraPos);
            }
        }
    }

    public Vector3 GetRotationToClosestSide()
    {
        SetClosestSide();

        // activate the trigger for the puzzle. we might be straying away from the game controller lowkey
        switch (closestSide.name)
        {
            case "FrontPlane":
                // print("front");
                return Vector3.forward;
            case "BackPlane":
                //print("back");
                return Vector3.back;
            case "RightPlane":
                //print("right");
                return Vector3.right;
            case "LeftPlane":
                // print("left");
                return Vector3.left;
            case "TopPlane":
                // print("top");
                return Vector3.up;
            case "BottomPlane":
                // print("bottom");
                return Vector3.down;
        }

        return Vector3.zero;
    }
}