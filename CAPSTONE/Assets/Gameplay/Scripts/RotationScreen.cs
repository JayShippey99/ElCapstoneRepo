using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScreen : InteractableParent
{
    // oh my fucking goddd I need the screens to match the puzzles
    // eugh
    // i'll save that for another day, seems fun to figure out though

    public static RotationScreen instance;
    public Transform indicator;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        PlantPuzzle pp = GameController.instance.currentPuzzle;

        if (theButton.name == "TurnLeft")
        {
            // make it go around the circle
            // I need an angle, and I need to increment and decrement that angle
            // and for placement I need to * the angle by the local scale proper axes
            indicator.Rotate(Vector3.forward, -45);
        }
        else
        {
            indicator.Rotate(Vector3.forward, 45);
        }

        if (pp!= null)
        {
            if (pp.HasPlacedBranches()) pp.ClearPuzzle();
        }
    }

    public void AdjustToPuzzle() // okay now when do we call this? we call it when we focus in on a puzzle
    {

        // this will get the root's rotation, and use it to determine how the indicator should be rotated
        // could it just be reset the rotation and add the (int).z of the rotation?
        PlantPuzzle pp = GameController.instance.currentPuzzle;

        //print("we are trying to adjust to puzzle");

        if (pp != null)
        {
            //print("there is a puzzle " + pp.GetRootRotation());
            Restart();
            indicator.Rotate(Vector3.forward, -pp.GetRootRotation());
        }
    }

    public override void LeverPulled()
    {
        //print("lever puller???");
        Restart();
    }

    public void Restart()
    {
        indicator.localRotation = Quaternion.Euler(Vector3.right * 90);
    }
}
