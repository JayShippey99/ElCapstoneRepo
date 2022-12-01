using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    // wait if we're doing it from here, do we even need the other script?
    // we just need the transform, the prefab

    // notes go from 0 to 600, how do I inscribe both the melody and the rhythm in one oh duh, cause its 16 long, i just indicate the length by adding it more than once
    // so when the thing submits
    // wait i wanna print the value of every key if possible
    // 0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\|;:'"/?.>,<
    // maybe we could limit the symbols too?
    // it would be awesome if we got puzzles going that used brackets to apply things and yada yadda
    public Transform pianoRollTransform;
    public GameObject notePrefab;

    char[] fullCharList;

    float xDiv; // the number to * i by
    float yDiv; // the number to * j by

    void Start()
    { 

        
        string half1 = @"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\|;:'";//     "/?.>,<
        string half2 = "\"/?.>,<";

        string fullString = half1 + half2;

        fullCharList = fullString.ToCharArray();

        xDiv = 1200 / 16; // full x / 16
        yDiv = 600 / fullCharList.Length - 1; // minus 1 for last index, this means when we put in the last index, it'll be 600

        // DUH i can just use exactly what I've done right here and make a dictionary out of it and then use those numbers to scale between 0 and 600 ahhhhh that's fucking awesome
        // Like i can make a dictionary out of these??
        // i wonder if inputting something with quotes actually is bad or something
        // so lets say I make a dictionary, then what how do I use that, I'm basically just getting i with that WAIT
        // I wanna grab the string, and for each
        // this puzzle might be really nice to guide what the player can do and the values of these keys
        // its weird to think that it might be at the start of the game though


         //print(note);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearNotes() // if I do this first try I'm a fucking god
    {
        // get all children and kill them!
        for (int i = pianoRollTransform.childCount - 1; i >= 0; i--) // wait this isn't the right child
        {
            Destroy(pianoRollTransform.GetChild(i).gameObject);
        }
    }

    public void MakeNotes(string t) // we will do this from the main script, and then for each char in t, instantiate it but like based on the index of that letter
    {
        ClearNotes();

        char[] chars = t.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            for (int j = 0; j < fullCharList.Length; j++)
            {
                if (chars[i] == fullCharList[j])
                {
                    print(chars[i] + ":" + i + "  " + fullCharList[j] + ":" + j); // so i think this is kinda gonna work, its gonna be jank // dont we wanna do something with the fact that its 600/count and not 600/last index

                    Vector3 offset = new Vector3(i * xDiv, j * yDiv);

                    GameObject note = Instantiate(notePrefab, pianoRollTransform.position + offset, Quaternion.identity); // we need to add proper offsets
                    note.transform.parent = pianoRollTransform;
                }
            }
        }


        for (int i = 0; i < fullCharList.Length; i++) // i can write some code to print all these in order later, or I wonder if I can use like a txt file to just set all this information how I want
        {
            //print(((byte)fullCharList[i]) + " " + fullCharList[i]);
        }
    }
}
