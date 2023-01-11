using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FrequencyList : MonoBehaviour
{
    static public FrequencyList instance;


    public GameObject leftSide, rightSide; // left and right are the columns, we need the row prefab
    public GameObject rowPrefab;

    string characterString = "zxcvbnmasdfghjklqwertyuiop1234567890-=!@#$%^&*()_+";
    char[] listOfChars;

    List<FreqRow> rows = new List<FreqRow>();

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        listOfChars = characterString.ToCharArray();


        // so the issue it that its inheriting way too much, like the scale, rotation, etc, maybe i can just manually set these things??

        for (int i = 0; i < listOfChars.Length; i++)
        {
            FreqRow f = null;

            // for each letter in the array, spawn a prefab
            if (i < listOfChars.Length / 2)
            {
                GameObject g = Instantiate(rowPrefab, leftSide.transform.position, Quaternion.identity);
                //g.transform.parent = leftSide.transform;
                g.transform.SetParent(leftSide.transform);



                f = g.GetComponent<FreqRow>();


                f.Instantiate(listOfChars[i], i * 25, i % 2 == 0);



            }
            else
            {
                GameObject g = Instantiate(rowPrefab, rightSide.transform.position, Quaternion.identity);
                //g.transform.parent = rightSide.transform;
                g.transform.SetParent(rightSide.transform);

                f = g.GetComponent<FreqRow>();

                f.Instantiate(listOfChars[i], i * 25, i % 2 == 0);
            }

            if (f != null) rows.Add(f);
        }

        if (transform.parent.gameObject.activeInHierarchy) transform.parent.gameObject.SetActive(false);
    }

    public void AddNewChars(string t)
    {
        foreach (var row in rows)
        {
            if (t.Contains(row.charText.text)) row.ShowChar();
        }
    }
}
