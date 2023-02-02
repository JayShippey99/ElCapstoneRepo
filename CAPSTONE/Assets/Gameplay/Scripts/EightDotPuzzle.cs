using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightDotPuzzle : MonoBehaviour, IDotPuzzle
{
    public static EightDotPuzzle instance;

    public GameObject interactiveGrid;

    List<Dot> children = new List<Dot>();

    public Transform referenceGrid;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


        foreach (Transform child in interactiveGrid.transform)
        {
            //print(child.name + " " + child.gameObject.GetComponent<Dot>());
            children.Add(child.gameObject.GetComponent<Dot>());
        }

        
    }

    public void GetInput(string t)
    {
        // 
        foreach (Dot d in children)
        {
            d.SetOn(false);
        }

        for (int i = 0; i < t.Length; i ++)
        {
            children[i].SetOn(true);
        }

        if (CheckIfSolved()) GameController.instance.GoToNextSection();
        //print("Does this go more than once???");
    }

    public bool CheckIfSolved()
    {
        Transform rg = referenceGrid;
        if (interactiveGrid.transform.childCount != rg.childCount)
        {
            Debug.LogWarning("Interactive Grid and Reference Grid do not have matching amounts of dots");
        }
        else
        {
            // check if matching
            return IsMatchingReferenceGrid(rg);
        }

        return true;
    }

    public bool IsMatchingReferenceGrid(Transform rg)
    {
        //print("checking ref grid");
        for (int i = 0; i < rg.childCount; i++)
        {
            //print(rg.GetChild(i).GetComponent<Dot>().isOn + " " + rg.GetChild(i).gameObject.name + " " + interactiveGrid.transform.GetChild(i).GetComponent<Dot>().isOn + " " + interactiveGrid.transform.GetChild(i).name);
            if (rg.GetChild(i).GetComponent<Dot>().isOn != interactiveGrid.transform.GetChild(i).GetComponent<Dot>().isOn) return false; // good lord
        }

        return true;
    }
}
