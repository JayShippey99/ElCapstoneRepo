using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolChanger : MonoBehaviour
{
    public Sprite[] symbolSprites;

    float nextFrameDelay = 1f; // how do they normally do timers? // RIGHT they take the current time and add the delay onto it and that's the new trigger time

    Image img;
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // i think this just is better as a coroutine lowkey

    public void StartSymbolSwitch(string t) // maybe we send the string through here and then we cycle through the frames on our own // lets trigger this from the submit script
    {

        char[] charA = t.ToCharArray();

        StartCoroutine(SymbolCycle(charA));

        // for each thing in charA, 
    }

    void SwitchSymbols(int n) // n is the symbol to switch to
    {
        if (n != -1) img.sprite = symbolSprites[n];
    }

    IEnumerator SymbolCycle(char[] c)
    {
        // this will cycle through, and each time it needs to, it'll do the switch symbols function
        // for each cycle, see if the symbol is a number or +-/* and if it is, send its associated number
        // if it is not, do nothing
        int counter = 0; // for each symbol in the string
        float progress = 0;

        if (Conditions.IsNumber(c[counter]) || Conditions.IsMathSymbol(c[counter]))
        {
            SwitchSymbols(counter);
            img.color = Color.white;
        }
        else
        {
            img.sprite = null;
            img.color = Color.clear;
        }

        while (counter < c.Length - 1) // loop for each symbol in string // wait, yes
        {
            progress += Time.deltaTime;
            
            if (progress >= nextFrameDelay)
            {
                print("GO");
                progress = 0;
                counter++; // its like counting up more than it should?

                // for each symbol in the string, check if number or +-*/
                // ugh i need not to use counter here
                // OH WAIT I GOT IT
                if (Conditions.IsNumber(c[counter]) || Conditions.IsMathSymbol(c[counter])) // this is so dumb ugh whatever
                {
                    int num = -1;

                    switch (c[counter])
                    {
                        case '0': // dude literally what is this lmfao
                            num = 0;
                            break;
                        case '1':
                            num = 1;
                            break;
                        case '2':
                            num = 2;
                            break;
                        case '3':
                            num = 3;
                            break;
                        case '4':
                            num = 4;
                            break;
                        case '5':
                            num = 5;
                            break;
                        case '6':
                            num = 6;
                            break;
                        case '7':
                            num = 7;
                            break;
                        case '8':
                            num = 8;
                            break;
                        case '9':
                            num = 9;
                            break;
                        case '+':
                            num = 10;
                            break;
                        case '-':
                            num = 11;
                            break;
                        case '*':
                            num = 12;
                            break;
                        case '/':
                            num = 13;
                            break;
                    }
                    SwitchSymbols(num);
                    img.color = Color.white;
                }
                else
                {
                    img.sprite = null;
                    img.color = Color.clear;
                }
            }
            yield return null;
        }

        img.sprite = null;
        img.color = Color.clear;
    }
}
