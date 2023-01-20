using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SubmitString : MonoBehaviour
{

    // would like to make it so that I don't need to repeatedly click the box each time

    // we could have something that only appears every x inputs
    // woah maybe the inner cube is what we want to match, oh that's a cool idea

    TMP_InputField input;
    public Tesseract tesseract;
    void Start()
    {
        input = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnSubmit()
    {
        // i'll need to check all the options for patterns
        //Debug.Log(SortAlphabetically(input.text));

        string str = input.text;


        if (str.Length > 0)
        {
            Oscillator.instance.On(str);

            InputHistory.instance.AddInput(str);

            //if (AllSameChar(str)) tesseract.MoveDown();

            if (isHexValue(str)) tesseract.ChangeColor(str.ToLower());
        }
        input.text = "";
    }

    bool AllCapitalLetters(string str)
    {
        return str.All(char.IsUpper);
    }

    bool ContainsLeftArrow(string str)
    {
        return str.Contains("<");
    }
    bool ContainsRightArrow(string str)
    {
        return str.Contains(">");
    }

    bool ContainsDoubleLeftArrow(string str)
    {
        return str.Contains("<<");
    }

    bool ContainsDoubleRightArrow(string str)
    {
        return str.Contains(">>");
    }

    bool AllSameChar(string str)
    {
        // go through string, get the first char, compare everything else
        char[] charstr = str.ToCharArray();
        for (int i = 0; i < charstr.Length; i++)
        {
            if (charstr[0] != charstr[i]) return false;
        }

        return true;
    }

    bool isHexValue(string str) // for now its just a 6 string checker , i guess we can then ask if the string has any spaces, symbols, or anything above F
    {
        
        // lets first get if string is under 6 keys
        if (str.Length != 6) return false;

        print("1");

        string lowerStr = str.ToLower();
        char[] charstr = lowerStr.ToCharArray();
        // if each character is a letter, between A-F
        for (int i = 0; i < charstr.Length; i++)
        {
            char c = charstr[i];
            if (char.IsSymbol(c)) return false;

            print("2");

            if (char.IsLetter(c))
            {
                if (c > 'f') return false;
            }
            print("3");

            if (char.IsNumber(c)) // it breaks here
            {
                if (c > '9') return false;
            }

            print("4");
        }

        //print(lowerStr + " is a hex code");

        return true;
    }

    bool HasNumbers(string str)
    {
        return !str.All(char.IsLetter); // if all letters, return false
    }

    bool AllNumbers(string str)
    {
        return str.All(char.IsDigit);
    }

    bool AllLetters(string str)
    {
        return str.All(char.IsLetter);
    }

    bool AllSymbols(string str) // it'll be intersting if I can find a way to make this work. maybe I could exclude symbols though
    {
        return str.All(char.IsSymbol); // oh my fucking god lmfaooo lets goooo
    }

    bool CheckIfFirstAndLastMatch(string str)
    {
        return (str.First() == str.Last()); // boom lets goooo
    }

    string SortAlphabetically(string str)
    {
        char temp;
        string myStr = str.ToLower();
        char[] charstr = myStr.ToCharArray();
        for (int i = 1; i < charstr.Length; i++)    // for(int i=1;i< str.Length;i++)
        {
            for (int j = 0; j < charstr.Length - 1; j++)  //for(int j=0;j< str.Length-1;j++)
            {
                if (charstr[j] > charstr[j + 1]) // huh weird looks like these all have values to them and we're literally saying if this character has a higher value than this character
                {
                    // i guess we can say here that if this ever triggers the string isn't in alphabetical order
                    temp = charstr[j];
                    charstr[j] = charstr[j + 1];
                    charstr[j + 1] = temp;
                }
            }
        }

        return new string(charstr);
    }

    bool InAlphabeticalOrder(string str) // nice, this works great, what I'm thinking i could do is just make a seperate script that's not a mono behavior that runs all of these functions
    {
        char temp;
        string myStr = str.ToLower();
        char[] charstr = myStr.ToCharArray();
        for (int i = 1; i < charstr.Length; i++)    // for(int i=1;i< str.Length;i++)
        {
            for (int j = 0; j < charstr.Length - 1; j++)  //for(int j=0;j< str.Length-1;j++)
            {
                if (charstr[j] > charstr[j + 1]) // huh weird looks like these all have values to them and we're literally saying if this character has a higher value than this character
                {
                    return false;
                    temp = charstr[j];
                    charstr[j] = charstr[j + 1];
                    charstr[j + 1] = temp;
                }
            }
        }

        return true;
    }

    bool CheckIfAnyRepeatChars(string str) // we didn't use toLower here,
    {
        bool hasRepeating = false;
        Dictionary<char, int> charCount = new Dictionary<char, int>(); // we create a dictionary

        foreach (var character in str) // this is all really similar to what I did on my own fuck yeah
        {
            if (character != ' ')
            {
                if (!charCount.ContainsKey(character))
                {
                    charCount.Add(character, 1);
                }
                else
                {

                    charCount[character]++; // is this adding to the int? that feels liek it shouldn't work
                    // no yeah, that's what it looks like is happening
                    hasRepeating = true;
                }
            }
        }

        if (hasRepeating == true)
        {
            // run additional code to see which and where the letters are repeating
        }

        return hasRepeating;
        
    }

}
