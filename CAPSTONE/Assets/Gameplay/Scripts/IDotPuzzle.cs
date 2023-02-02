using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDotPuzzle
{
    public void GetInput(string t);

    bool CheckIfSolved();

    bool IsMatchingReferenceGrid(Transform rg);
}
