using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BoardLight : MonoBehaviour
{
    public Color offColor;
    public Color onColor;
    public Color correctColor;
    public Color wrongColor;

    public string charTrigger;
    public string patternTrigger; // now pattern triggers will include, wait should this be an enum? that'd be nice
    public int indexTrigger;


    // wait this high low makes no sense if i can have more than 4 characters
    // how does each of these know which spot to look for? god this one is so stupid

    public enum Triggers
    {
        AllLetters,
        DescendingValue,
        EveryOtherInput,
        AtLeastThree,
        HasCharTrigger,
        HasLetter,
        HasNumber,
        HasSymbol,
        IsThree,
        LessThanCharTrigger,
        MoreThanCharTrigger,
        IsCharTrigger
    };

    public Triggers dropDown;

    Image sr;

    public SubmitToBoard input;
    void Start()
    {
        sr = GetComponent<Image>();
    }

    private void Update()
    {
        string t = input.text;

        if (!string.IsNullOrEmpty(input.text))
        {
            TurnOff();

            switch (dropDown)
            {
                case Triggers.AllLetters:
                    CheckIfAllLetters();
                    break;
                case Triggers.DescendingValue:
                    CheckIfDescendingValues();
                    break;
                case Triggers.EveryOtherInput:
                    CheckIfEveryOtherInput();
                    break;
                case Triggers.AtLeastThree:
                    if (Conditions.IsItThisLong(t, 3) || Conditions.IsLongerThan(t, 3))
                    {
                        TurnOn();
                    }
                    else TurnOff();
                    break;
                case Triggers.HasCharTrigger:
                    if (Conditions.HasThisChar(t, charTrigger))
                    {
                        print("They got me!" + charTrigger);
                        TurnOn();
                    }
                    else
                    {
                        print("Didn't get me!" + charTrigger);
                    }
                    
                    break;
                case Triggers.HasLetter:
                    if (Conditions.HasLetter(t)) TurnOn();
                    break;
                case Triggers.HasNumber:
                    if (Conditions.HasNumber(t)) TurnOn();
                    break;
                case Triggers.HasSymbol:
                    if (Conditions.HasSymbol(t)) TurnOn();
                    break;
                case Triggers.IsThree:
                    if (Conditions.IsItThisLong(t, 3)) TurnOn();
                    break;
                case Triggers.LessThanCharTrigger: // for less than we need a char and a target, I need to give it up for the night I think?
                                                   // find if number at spot in list
                    if (indexTrigger < t.Length)
                    {
                        if (Conditions.IsThisIndexANumber(t, indexTrigger))
                        {

                            if (Conditions.IsLowerValueThan(t[indexTrigger], charTrigger)) TurnWrong();
                        }
                    }
                    break;
                case Triggers.MoreThanCharTrigger:
                        if (indexTrigger < t.Length) // index trigger is 0, t.length is 1, make sure its a number?
                        {
                    if (Conditions.IsThisIndexANumber(t, indexTrigger))
                    {
                            if (Conditions.IsHigherValueThan(t[indexTrigger], charTrigger)) TurnWrong();
                        }
                    }
                    break;
                case Triggers.IsCharTrigger:
                        if (indexTrigger < t.Length)
                        {
                    if (Conditions.IsThisIndexANumber(t, indexTrigger))
                    {
                            if (Conditions.AreValuesMatching(t[indexTrigger], charTrigger)) TurnCorrect();
                        }
                    }
                    break;
            }

            /*
            if (charTrigger != "")
            {
                if (Conditions.HasThisChar(t, charTrigger)) TurnOn();
                else TurnOff();
            }
            */

            if (!string.IsNullOrEmpty(patternTrigger)) // if there is a pattern trigger, and there is, so this will be run, but they should probably be set to false?
            {
                //print("hello");
                // check for certain rules
                TurnOff();
                switch (patternTrigger)
                {
                    case "all letters":
                        CheckIfAllLetters();
                        break;
                    case "descending value":
                        CheckIfDescendingValues();
                        break;
                    case "every other input": // i guess i can do input count but that's a little funky
                        CheckIfEveryOtherInput();
                        break;
                    case "at least three":
                        if (Conditions.IsItThisLong(t, 3) || Conditions.IsLongerThan(t, 3))
                        {
                            TurnOn();
                        }
                        else TurnOff();
                        break;
                    case "has letter":
                        if (Conditions.HasLetter(t)) TurnOn();
                        break;
                    case "has number":
                        if (Conditions.HasNumber(t)) TurnOn();
                        break;
                    case "has symbol":
                        if (Conditions.HasSymbol(t)) TurnOn();
                        break;
                    case "is three":
                        if (Conditions.IsItThisLong(t, 3)) TurnOn();
                        break;
                }
            }
        }
    }

    void CheckIfAllLetters()
    {
        // if yes, do turn on
        // if no, do turn off
        //print("letters");

        if (input.text.All(char.IsLetter))
        {
            TurnOn();
        }
        else TurnOff();
    }
    
    void CheckIfDescendingValues()
    {
        string str = input.text.ToLower(); // debatable if I want this
        char[] charstr = str.ToCharArray();

        bool isDescending = true;

        for (int i = 0; i < charstr.Length - 1; i++) // could i not just go through the list once and 
        {
            if (charstr[i] <= charstr[i + 1]) // why tf is this only doing stuff with the j
            {
                isDescending = false; // this means that one of the things comes after and is bigger, its not descending
            }
        }



        if (isDescending) TurnOn();
        else TurnOff();
    }
    void CheckIfEveryOtherInput()
    {
        //print(InputCounter.instance.count + " " + InputCounter.instance.count % 2);
        if ((InputCounter.instance.count % 2) == 0) // this isn't right
        {
            TurnOn();
        }
        else TurnOff();
    }

    void TurnCorrect()
    {
        sr.color = correctColor;
    }

    void TurnWrong()
    {
        sr.color = wrongColor;
    }

    void TurnOn()
    {
        sr.color = onColor;
    }

    void TurnOff()
    {
        sr.color = offColor;
    }

}
