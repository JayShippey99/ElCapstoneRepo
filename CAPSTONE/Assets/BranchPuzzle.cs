using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPuzzle : MonoBehaviour
{
    public string levelCode;
    public List<PlantCondition> ends, branches;
    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
    /*
    private void ReadCode()
    {
        char[] cStr = levelCode.ToCharArray();

        try
        {
            for (int i = 0; i < cStr.Length; i ++)
            {
                switch (cStr[i])
                {
                    case '|':
                        if (cStr[i - 1] == 1) branches.Add(new BranchCondition());
                        else throw new System.Exception();
                        break;
                    case '.':
                        if (cStr[i - 1] != 3) ends.Add(new EndCondition());
                        else throw new InvalidCodeException();
                        break;
                    default:
                        break;
                }
            }
        }
        catch (InvalidCodeException ex)
        {
            Debug.Log("INVALID CODE AAA");
        }
    }
    */
}
