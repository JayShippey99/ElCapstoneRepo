using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BinaryInputBrain : InteractableParent
{
    [HideInInspector]
    public TextMeshProUGUI tmp;
    
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        Clear();
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        string t = tmp.text;
        if (t == "0")
        {
            tmp.text = "1";
        }
        else if (t == "1")
        {
            tmp.text = "2";
        }
        else if (t == "2")
        {
            tmp.text = "3";
        }
        else if (t == "3")
        {
            tmp.text = "0";
        }
    }

    public void Clear()
    {
        tmp.text = "0";
    }
}
