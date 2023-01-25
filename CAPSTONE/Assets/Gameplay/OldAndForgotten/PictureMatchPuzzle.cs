using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureMatchPuzzle : MonoBehaviour
{
    // I need the list of sprites, and a starting sprite, lets cycle through the sprites first using left and right
    // I'll just make a bool and then have back alwasy be relative to front with that in mind
    // the smartest thing to do for this test is to just actually use a cube

    // this is the biggest piece of defeat I've felt in a while

    // I FUCKING GOT IT LETS GOOOOOO

    // okay so the next step is to now just sync it all up with the input
    public PictureMatchSide[] sides = new PictureMatchSide[6];

    bool spinLeft;
    bool spinRight;
    int spinCounter = 90;

    bool rotLeft;
    bool rotRight;
    int rotCounter = 90;

    private void Update()
    {
        if (spinLeft) // DUH this isn't an update function
        {
            SpinLeft(); // order of operations is bad, i think i'm spinnning it one frame too far

            if (spinCounter > 0) // counter starts at 90
            {
                spinCounter--; // 81

                if (spinCounter == 1) // kapow I was exactly right, this system is clunky as hell though tbh
                {
                    // that means this is running
                    spinCounter = 90;
                    spinLeft = false;
                }
            }
        }

        if (spinRight)
        {
            SpinRight();

            if (spinCounter > 0)
            {
                spinCounter--;

                if (spinCounter == 1)
                {
                    spinCounter = 90;
                    spinRight = false;
                }
            }
        }

        if (rotLeft)
        {
            RotateLeft();

            if (rotCounter > 0)
            {
                rotCounter--;

                if (rotCounter == 1)
                {
                    rotCounter = 90;
                    rotLeft = false;
                }
            }
        }

        if (rotRight)
        {
            RotateRight();

            if (rotCounter > 0)
            {
                rotCounter--;

                if (rotCounter == 1)
                {
                    rotCounter = 90;
                    rotRight = false;
                }
            }
        }
    }

    public void CheckConditions(string t) // should only one thing happen at a time? // this whole script is a train wreck
    {
        /*
        if (Conditions.HasThisChar(t, ">"))
        {
            SpinRight();
        }
        else if (Conditions.HasThisChar(t, "<"))
        {
            SpinLeft();
        }
        else if (Conditions.HasThisChar(t, "q"))
        {
            RotateRight();
        }
        else if (Conditions.HasThisChar(t, "e"))
        {
            RotateLeft();
        }
        
        if (Conditions.HasNumber(t))
        {
            Swap();
        }
        */

    }

    void SpinLeft()
    {
        spinLeft = true;
        transform.RotateAround(transform.position, Vector3.up, -1); // maybe I should have this change over time
    }

    void SpinRight()
    {
        spinRight = true;
        transform.RotateAround(transform.position, Vector3.up, 1);
    }

    void RotateLeft()
    {
        rotLeft = true;
        transform.RotateAround(transform.position, Vector3.forward, -1);
    }

    void RotateRight()
    {
        rotRight = true;
        transform.RotateAround(transform.position, Vector3.forward, 1);
    }

    void Swap()
    {
        foreach (var side in sides)
        {
            side.SwapImages(); // fucking weird ass chain
        }
    }
}
