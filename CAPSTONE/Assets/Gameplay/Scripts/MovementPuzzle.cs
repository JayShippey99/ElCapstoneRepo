using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPuzzle : MonoBehaviour
{
    public Transform[] smallCubePoints; // Do I want to do this with code?? I feel like if I have to do it with animations
    Vector3[] smallCubeCurrentPoints = new Vector3[8]; // ugh this is so ugly
    Vector3[] smallPrevPoints = new Vector3[8];
    // there HAS to be some middle man I can cut out but maybe i'll worry about it for later
    // I'd need like a MEGA web of stuff, actually yeah, I'd want to make it with code
    public Transform[] bigCubePoints; // this is so gross
    int movingCubePuzzleStep = 0;
    void Start()
    {
        for (int i = 0; i < smallCubePoints.Length; i++)
        {
            smallCubeCurrentPoints[i] = smallCubePoints[i].localPosition;
            smallPrevPoints[i] = smallCubeCurrentPoints[i];
        }
        MoveOutCube(); // maybe we COULD move this all to a new script, and then based on whether that script it on or not, we do things
        // ah i think its cause the stufff below breaks, and breaks out of this funciton
    }


    // to get the target point for the inner cube i need to do some math relative to the 0th point of the big cube, which I feel like is .5 .5 .5 and maybe one of them is negative
    public void IsInnerCubeInside()
    {
        //print((bigCubePoints[0].localPosition + new Vector3(.5f, -.5f, .5f)));

        if (smallCubeCurrentPoints[0] == (bigCubePoints[0].localPosition + new Vector3(.5f, -.5f, .5f)))
        {
            StartCoroutine(WaitForNextPuzzle());
        }
        else
        {
            //print("should be here");
            StartCoroutine(WaitToReturnToPreviousPosition());
        }
    }

    IEnumerator WaitToReturnToPreviousPosition()
    {
        yield return new WaitForSeconds(.5f);
        MoveBack();
        // we should record how it moved, or no maybe we record the previous points? 
    }

    IEnumerator WaitForNextPuzzle() // my constant use of coroutines scares me lol, well, it fucking works though
    {
        yield return new WaitForSeconds(.5f);

        MoveOutCube();
    }

    void MoveOutCube() // maybe this will be the puzzle step? should I add extra scripts for this too?? for now lets not worry about the puzzle number
    {
        //print("move");
        foreach (var point in bigCubePoints)
        {
            switch (movingCubePuzzleStep)
            {
                case 0: // this will mean we start the puzzle
                    point.localPosition += Vector3.up * 2; // i DO need to do *2 at least for this right??
                    //print("should be doing this");
                    break;
                case 1:
                    point.localPosition += (Vector3.down * 2 + Vector3.left * 2);
                    break;
                case 2:
                    point.localPosition += (Vector3.right * 4 + Vector3.up * 2);
                    break;
            }
            // I don't think we need to do the *2 here because the smaller one needs to move twice as far, the big one doesn't
        }

        movingCubePuzzleStep++;

        if (movingCubePuzzleStep > 3) print("MOVING PUZZLE COMPLETE");
    }

    void MoveBack()
    {
        //print("back??");
        for (int i = 0; i < smallCubePoints.Length; i++)
        {
            smallCubePoints[i].localPosition = smallPrevPoints[i];
        }
    }

    public void SetPreviousPositions()
    {
        for (int i = 0; i < smallCubePoints.Length; i++)
        {
            smallPrevPoints[i] = smallCubePoints[i].localPosition;
        }
    }

    void MoveInDirection(Vector3 dir) // i don't understand, I'm setting the previous position, and then changing the current position, omg is it because its a tranform so it always updates?
    {
        //print(smallPrevPoints[0]); // wait the fact that its the same thing twice, isn't that good?

        for (int i = 0; i < smallCubePoints.Length; i++)
        {
            smallCubePoints[i].localPosition += dir;
            smallCubeCurrentPoints[i] = smallCubePoints[i].localPosition; // this is so insanely dumb
        }
        //IsInnerCubeInside();
    }

    // new thing is that once we move the inner cube to the right spot we need to move the outer cube too
    // I like the idea though that if you get it wrong you move back, but this means we need to set up a way to know that the thing is in the right position, that could be a little hairy, although, 
    // WAIT okay one thing to note is that all I need to know is the position of one of the points, if that's in the right spot, so are the rest
    // I first need to know when its in the right spot before I move to the next spot
    // storing all this is gonna be gross if I do it in this script, ew I then need a way to move the big cube around
    public void MoveInnerCube(string dir, int intensity) // This whole function is very redundant but oh well, could maybe change later, but lets leave it for now
    {
        //print("ayo" + dir + intensity);
        //transform.position += Vector3.right * 10; // literally this should not be running

        intensity *= 2;
        switch (dir)
        {
            case "up":
                MoveInDirection(Vector3.up * intensity); // intensity times 2?
                break;
            case "down":
                MoveInDirection(Vector3.down * intensity);
                break;
            case "left":
                MoveInDirection(Vector3.left * intensity);
                break;
            case "right":
                MoveInDirection(Vector3.right * intensity);
                break;
            case "back":
                MoveInDirection(Vector3.back * intensity);
                break;
            case "forward":
                MoveInDirection(Vector3.forward * intensity);
                break;
        }
    }

}
