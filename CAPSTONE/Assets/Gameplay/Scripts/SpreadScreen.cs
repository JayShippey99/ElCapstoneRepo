using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadScreen : InteractableParent
{
    static public SpreadScreen instance;

    public _Switch switcher;

    public Sprite spreadNorm, spreadWide, spreadWidest;
    public SpriteRenderer screen;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public override void SetSwitch(bool on)
    {
        /*
        if (on)
        {
            // change graphic
            screen.sprite = spreadOn;
            if (GameController.instance.currentPuzzle != null)
            {
                if (GameController.instance.currentPuzzle.HasPlacedBranches()) GameController.instance.currentPuzzle.ClearPuzzle();
                
                GameController.instance.currentPuzzle.SpreadBranches(true);
            }
        }
        else
        {
            screen.sprite = spreadOff;
            if (GameController.instance.currentPuzzle != null)
            {
                if (GameController.instance.currentPuzzle.HasPlacedBranches()) GameController.instance.currentPuzzle.ClearPuzzle();

                GameController.instance.currentPuzzle.SpreadBranches(false);
            }
        }
        */
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        if (theButton.name == "Normal")
        {
            // set the spread to normal
            if (GameController.instance.currentPuzzle != null)
            {
                //if (GameController.instance.currentPuzzle.HasPlacedBranches()) GameController.instance.currentPuzzle.ClearPuzzle();

                GameController.instance.currentPuzzle.SpreadBranches(1);
            }

            SetSpread(1);
        }

        if (theButton.name == "Wide")
        {
            // set the spread to 180 degrees
            if (GameController.instance.currentPuzzle != null)
            {
                //if (GameController.instance.currentPuzzle.HasPlacedBranches()) GameController.instance.currentPuzzle.ClearPuzzle();

                GameController.instance.currentPuzzle.SpreadBranches(2);
            }

            SetSpread(2);
        }

        if (theButton.name == "Widest")
        {
            // set the spread to crazy number
            if (GameController.instance.currentPuzzle != null)
            {
                //if (GameController.instance.currentPuzzle.HasPlacedBranches()) GameController.instance.currentPuzzle.ClearPuzzle();

                GameController.instance.currentPuzzle.SpreadBranches(3);
            }

            SetSpread(3);
        }
    }

    // what I really don't love is that it needs to flip a physical switch. I think I may change it to a button maybe
    // but for now lets just keep it the same
    public void AdjustToPuzzle()
    {
        // get the spread of the puzzle
        PlantPuzzle pp = GameController.instance.currentPuzzle;

        if (pp != null)
        {
            SetSpread(pp.GetBranchSpread());
        }
    }

    public override void LeverPulled()
    {
        Restart();
    }

    public void SetSpread(int spreadAmount)
    {
        //switcher.isOn = true;
        if (spreadAmount == 1) screen.sprite = spreadNorm;
        if (spreadAmount == 2) screen.sprite = spreadWide;
        if (spreadAmount == 3) screen.sprite = spreadWidest;
    }

    public void Restart() // restart will be called from lever pulled
    {
        SetSpread(1);
    }


}
