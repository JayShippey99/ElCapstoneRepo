using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeTypesPuzzle : MonoBehaviour, IDotPuzzle
{
    public static ThreeTypesPuzzle instance; // yeah there's no way I wanna be doing this for each and every script

    public Transform interactiveGrid;

    List<Dot> children = new List<Dot>();
    public bool CheckIfSolved()
    {
        foreach (Dot d in children)
        {
            if (!d.isOn) return false;
        }

        return true;
    }

    public void GetInput(string t)
    {
        if (Conditions.HasLetter(t)) children[0].SetOn(true);
        else children[0].SetOn(false);
        if (Conditions.HasNumber(t)) children[1].SetOn(true);
        else children[1].SetOn(false);
        if (Conditions.HasSymbol(t)) children[2].SetOn(true);
        else children[2].SetOn(false);

        if (CheckIfSolved()) GameController.instance.GoToNextSection();
    }

    public bool IsMatchingReferenceGrid(Transform rg)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        foreach (Transform child in interactiveGrid.transform)
        {
            children.Add(child.GetComponent<Dot>());
        }
    }
}
