using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A1_Puzzle : MonoBehaviour
{

    public static A1_Puzzle instance;

    Sprite on, off;
    Image[] codeDots;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        on = Resources.Load<Sprite>("UniversalAssets/FullDot");
        off = Resources.Load<Sprite>("UniversalAssets/EmptyDot");

        codeDots = new Image[transform.childCount];
        print(codeDots.Length);
        for (int i = 0; i < transform.childCount; i ++)
        {
            codeDots[i] = transform.GetChild(i).GetComponent<Image>();
        }

        
    }

    public void CheckConditions(string t)
    {
        // here we will loop through all the children and see if the string fits their need
        // we need to know what each condition for each thing is. hell, since this is just simple, I can pretty much just ask if string contains a thing, change this things sprite, don't need a lot of connection code
        if (Conditions.HasLetter(t))
        {
            codeDots[0].sprite = on;
            //print("letter");
        }
        else
        {
            codeDots[0].sprite = off;
            //print("no letter");
        }

        if (Conditions.HasNumber(t)) codeDots[1].sprite = on;
        else codeDots[1].sprite = off;

        if (Conditions.HasSymbol(t)) codeDots[2].sprite = on;
        else codeDots[2].sprite = off;

        CheckIfDone();
    }

    void CheckIfDone()
    {
        for (int i = 0; i < codeDots.Length; i ++)
        {
            if (codeDots[i].sprite == off) return;
        }

        print("PUZZLE IS DONE");
    }
}
