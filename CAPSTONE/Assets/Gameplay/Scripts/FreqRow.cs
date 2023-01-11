using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreqRow : MonoBehaviour
{
    // i think what I want this script to do is be spawned into the hierarchy, and based on its spawn index, set the char and freq to different things
    public TextMeshProUGUI charText, freqText;

    public Color firstRow, secondRow;
    void Start()
    {
        
        //charText = transform.GetChild(0).GetComponent<TextMeshProUGUI>(); // we need to get this component in the first child, and 
        //freqText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //charText = g

        // i think I need to reset a lot of the transform values here
        // rotation is off, scale is off, I think that's really all the issues
        transform.localRotation = Quaternion.Euler(0, 0, 0); // i literally don't get why 
        transform.localScale = Vector3.one;
    }

    public void Instantiate(char c, int f, bool r) // char, freq, row
    {
        //Debug.Log(c + " value of C"); // so the values seem to be coming in okay
        //print(freqText + " freqText"); 

        //print(charText); // how is charText null

        // set color of this row,
        // set text of char
        // set text of freq;

        // we can set row color based on a true or false or maybe just an int
        if (r) GetComponent<Image>().color = firstRow;
        else GetComponent<Image>().color = secondRow;

        charText.text = c.ToString();
        freqText.text = f.ToString();
    }

    public void ShowChar()
    {
        charText.color = Color.white;
    }
}
