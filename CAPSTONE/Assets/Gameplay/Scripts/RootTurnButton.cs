using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTurnButton : InteractableParent
{
    public override void DoSomethingButton(GameObject theButton)
    {
        if (GameController.instance.currentPuzzle != null)
        {
            if (theButton.name == "TurnLeft")
            {
                GameController.instance.currentPuzzle.TurnRoot(1);
            }
            else
            {
                GameController.instance.currentPuzzle.TurnRoot(-1);
            }
        }
    }
}
