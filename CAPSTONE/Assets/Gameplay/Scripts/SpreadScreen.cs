using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadScreen : InteractableParent
{
    static public SpreadScreen instance;

    public _Switch switcher;

    public Sprite spreadOn, spreadOff;
    public SpriteRenderer screen;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public override void SetSwitch(bool on)
    {
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
    }

    // what I really don't love is that it needs to flip a physical switch. I think I may change it to a button maybe
    // but for now lets just keep it the same
    public void AdjustToPuzzle()
    {
        // get the spread of the puzzle
        PlantPuzzle pp = GameController.instance.currentPuzzle;

        if (pp != null)
        {
            if (pp.GetBranchSpread() == 1) Restart();
            else
            {
                TurnOn();
            }
        }
    }

    public override void LeverPulled()
    {
        Restart();
    }

    void TurnOn()
    {
        switcher.isOn = true;
        screen.sprite = spreadOn;
    }
    
    void TurnOff()
    {
        switcher.isOn = false;
        screen.sprite = spreadOff;
    }

    public void Restart() // restart will be called from lever pulled
    {
        TurnOff();
    }


}
