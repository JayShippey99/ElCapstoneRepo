using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SubmitToBoard : MonoBehaviour
{

    // At this point we may not even need this thing

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

    public _Light radarLight;

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

        if (input.text != "" && !input.readOnly && Input.GetKeyDown(KeyCode.Return))
        {
            //print(input.text);
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

                //input.DeactivateInputField();
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
                // this is when it turns off, this is when we send out the signals
                //print("when does this run");
                if (!string.IsNullOrEmpty(text)) ActivateCallouts(text);
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

        if (!string.IsNullOrEmpty(text)) PlayOscillator(text);

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

        input.ActivateInputField(); // do I ever set it to not interactabel?

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

    void PlayOscillator(string str)
    {
        if (Oscillator.instance != null)
        {
            radarLight.onTime = str.Length * .2f;
            radarLight.TriggerLight(); // i want to be able to set the length of on time
            Oscillator.instance.On(str);
        }
    }

    void ActivateCallouts(string str) // where do I check if it should start the puzzle? // this could probably be a cleaner piece of code too
    {
        //print("when is this running callouts");
        if (PlantPuzzle.instance != null)
        {
            //print("instence is real");
            PlantPuzzle.instance.MakeBranches(str);
        }
        if (GameController.instance != null)
        {
            //if (str == "first") GameController.instance.StartNewLevel(0); // I basically need to just copy the code I have below but relocate it kinda, and find a cleaner system for it
            //if (str == "second") GameController.instance.StartNewLevel(1);
            //if (str == "third") GameController.instance.StartNewLevel(2);

            // maybe later I can have something like "run current level script"
        }

        if (FrequencyList.instance != null) FrequencyList.instance.AddNewChars(str); // technically this could be a slotted variable, but whatever i literally will only have one of these per scene
    }
}
