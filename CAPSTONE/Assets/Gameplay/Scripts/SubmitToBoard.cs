using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SubmitToBoard : MonoBehaviour
{

    // new idea, instead of working with caret keys and stuff I can just make my own thing, it'll have 16 _ and then the one I'm currently on will be flashing or like hidden or something // genius


   // i need to figure out when I start the first puzzle

    // okay so now I need the second puzzle to start based on something else now. I think i'll have it be based on like an alternating input
    // how am I checking for the first puzzle

    // I'm very confused why pressing the puzzle made things break

    // so its playing thigns based on what's in the last input?
    // gonna comment things out in the oscillator page

    //something to think about later on is that things should be triggered after the input gets read

    static public SubmitToBoard instance;

    TMP_InputField input;

    [HideInInspector]
    public string text;

    public TextMeshProUGUI pastInputs;
    string ogInput;
    List<string> pastInputList = new List<string>(); // when we press up, we will get the last input until we go back to 0
    int historyNumber; // this will go from the list limit, down to 0, but reset when you submit
    // when we submit we add to this list

    //PianoPuzzle pp; // hehe

    bool isReadingInput = false;

    int thisChar;
    int pastChar;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);


        input = GetComponent<TMP_InputField>();
        //pp = GetComponent<PianoPuzzle>();

        input.caretWidth = 0; // ugh so thankful for this lol // this sucks though cause I can't make it a decimal which would be perfect. its like i'd need to make my own thing or idk upscale everything just to match this caret lmfao
    }

    // Update is called once per frame
    void Update()
    {

        if (input.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            Submit(); // Submit, here it is
        }


        if (input.text.Length > input.characterLimit && Oscillator.instance.isOn == false) input.text = input.text.Substring(0, input.characterLimit); // Keep input under x char length

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (historyNumber > 0) historyNumber--;
            //print(pastInputList.Count + " " + historyNumber); // wait both are 0? how?
            input.text = pastInputList[historyNumber]; // its this
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (historyNumber < pastInputList.Count - 1) historyNumber++; // if count is 2 there are 3 indexes 
            input.text = pastInputList[historyNumber];
        }

        if (Oscillator.instance != null) // OHHH its just because its damn on that's what the issue is
        {
            if (Oscillator.instance.isOn && isReadingInput) // so what I need to do is just say yes I'm outputting soething or not
            {

                thisChar = Oscillator.instance.charIndex; // okay wtf so charIndex is not right
                                                          // so this char is counting up as we expect

                input.interactable = false; // we need to disable adding new inputs now
                input.readOnly = true;



                if (thisChar != pastChar) // This makes the currently read letter red, I'd like to activate a puzzle once its read the last thing but that can move
                {
                    string firstHalf = "";
                    string highlightedChar = "";
                    string secondHalf = "";

                    string newFullString;


                    if (thisChar == 0)
                    {
                        firstHalf = "";
                        highlightedChar = "<color=red>" + ogInput.Substring(thisChar, 1) + "</color>";
                        secondHalf = ogInput.Substring(thisChar + 1, ogInput.Length - (thisChar + 1));
                    }
                    else if (thisChar > 0 && thisChar < ogInput.Length - 1)
                    {
                        firstHalf = ogInput.Substring(0, thisChar);
                        highlightedChar = "<color=red>" + ogInput.Substring(thisChar, 1) + "</color>";
                        secondHalf = ogInput.Substring(thisChar + 1, ogInput.Length - (thisChar + 1));
                    }
                    else // if this char is the last char
                    {
                        firstHalf = ogInput.Substring(0, thisChar);
                        highlightedChar = "<color=red>" + ogInput.Substring(thisChar, 1) + "</color>";
                        secondHalf = "";
                    }

                    //print(firstHalf + " " + highlightedChar + " " + secondHalf);

                    newFullString = firstHalf + highlightedChar + secondHalf;

                    input.text = firstHalf + highlightedChar + secondHalf;
                }


                pastChar = thisChar;
            }
        }

        if (Oscillator.instance != null)
        {
            if (isReadingInput && Oscillator.instance.isOn == false) // this is working so far, I just need to make it so that you can't edit it while its playing, but first, lets make it so the chars change color
            {
                isReadingInput = false;
                Clear();
                input.interactable = true;
                input.readOnly = false;
            }
        }
    }

    public void AddToInput(string str)
    {
        input.text = input.text + str;
    }

    public void Backspace()
    {
        input.text = input.text.Substring(0, input.text.Length - 1); // sub string goes crazy
    }

    public void Clear()
    {
        input.text = "";
    }

    public void Submit() // When you submit
    {
        isReadingInput = true;

        text = input.text;

        if (!string.IsNullOrEmpty(text)) ActivateCallouts(text);

        // get pastinputs text
        if (pastInputs.text == "") pastInputs.text = text;
        else pastInputs.text = pastInputs.text + "\n" + text;

        if (GetHowManyLineBreaks(pastInputs.text) > 9) // This will change once input history starts to work
        {
            pastInputs.text = GetRidOfFirstLine(pastInputs.text);
        }

        pastInputList.Add(text);
        historyNumber = pastInputList.Count;


        // this lets me get the first char as a new input
        thisChar = -1;
        pastChar = -1;

        ogInput = input.text;

        input.ActivateInputField();

        //InputCounter.instance.count++; // this can be reworked, for now we'll comment it out
    }

    string GetRidOfFirstLine(string str) // it'll return the new string, since we add the new one first, we'll just take off the first line
    {
        str = str.Remove(0, str.IndexOf('\n') + 1);
        return str;
    }

    int GetHowManyLineBreaks(string str)
    {
        int numLineBreaks = 0;
        numLineBreaks = str.Count(c => c.Equals('\n')) + 1;
        return numLineBreaks;
    }

    void ActivateCallouts(string str) // where do I check if it should start the puzzle? // this could probably be a cleaner piece of code too
    {
        if (PlantPuzzle.instance != null)
        {
            print("instence is real");
            PlantPuzzle.instance.MakeBranches(str);
        }
        if (GameController.instance != null)
        {
           if ((str == "abc")) GameController.instance.StartNewLevel(0); // I basically need to just copy the code I have below but relocate it kinda, and find a cleaner system for it
        }

        if (Oscillator.instance != null) Oscillator.instance.On(str);
        // for each puzzle script, we can activate them here
        if (GetComponent<MoveInnerCubePuzzle>() != null) GetComponent<MoveInnerCubePuzzle>().CheckConditions(str);

        //if (pp != null && pp.enabled) pp.MakeNotes(str);

        if (FrequencyList.instance != null) FrequencyList.instance.AddNewChars(str); // technically this could be a slotted variable, but whatever i literally will only have one of these per scene

        if (Puzzle1MiniGridBrain.instance != null) Puzzle1MiniGridBrain.instance.TurnOnDots(str);

        if (ParticlePuzzle.instance != null) ParticlePuzzle.instance.PushFrequencies(str);

        /*
        // PUZZLE TESTING CHUNK // HERE IT IS LMFAO DUh // again, lets just keep this for now? // modify it though
        if (Tesseract.instance != null)
        {
            if (Conditions.IsInAscendingOrder(str)) Tesseract.instance.StartABCPuzzle(); // aha
            if (Conditions.IsAlternating(str)) Tesseract.instance.StartSoundPuzzle(); //Tesseract.instance.spinX(); // change in a sec;
            // wonder if we could make the pitches match the things I used to put in
        }
        */

        

        if (A1_Puzzle.instance != null) A1_Puzzle.instance.CheckConditions(str);
        // we need to have a callout for the cue for each puzzle start, interesting, should this be here on in the tesseract class, lets do here first
        //print(Conditions.IsInAscendingOrder(str) + " ascending order return");

        if (SingleDotPuzzle.instance != null) SingleDotPuzzle.instance.GetInput(str);
    }
}
