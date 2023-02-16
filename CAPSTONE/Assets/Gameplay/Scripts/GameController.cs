using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Level[] levels;

    int[][] levelAndSection; // this will keep track of what current level and section I'm on, if level is -1, then i'm in the hub?

    public _Light micLight;

    public delegate void FunctionAfterDelay();

    Level currentLevel;



    public void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // each levels has .section number?
        //print(levels[0].sections.Length);
    }

    public void StartNewLevel(int i)
    {
        // instead of just activate this thing, run the animation by the trigger name and because of that in the animation, it will have the event in the animation
        // but for now lets just say fuck it

        // idk just making stuff up at this point
        if (currentLevel == null)
        {
            levels[i].sections[0].SetActive(true); // this starts the first puzzle in the list // do this actually lol

            currentLevel = levels[i];
        }
        //print("start new level");
        Tesseract.instance.animator.SetTrigger("MoveToProject");
        // so the puzzle runs now, very good, that should mean that it knows when it won now too
    }

    // does this mean I need to keep track of where I am in the puzzle list?
    public void GoToNextSection() // maybe I can phrase this as "puzzle done"
    {
        //print("go to next section more than once?");
        StartCoroutine(DelayAndThenFunction(ChangeSection, 2f));
    }

    void ChangeSection()
    {
        //print("how many times does this run?");
        // OOO wait loop through the levels[] and then the sections[] if a section is active, set the next one to be active and this one off, if there is no next one, then run a level done function
        foreach (Level l in levels) // for each level is levels? shouldn't it just be for this section? we'll we don't know which section is currently going
        {
            for (int i = 0; i < l.sections.Length; i++)
            {
                //print(i);
                GameObject s = l.sections[i];

                if (s.activeInHierarchy)
                {
                    // turn it off and set the next one to on, then break I imagine
                    if (i + 1 < l.sections.Length)
                    {
                        // meaning there is another one in the list of sections
                        s.SetActive(false); // how do I move this code to something with no input?
                        l.sections[i + 1].SetActive(true);

                       // print("changing sections");
                        //print("This puzzle name is: " + s.name + " and the next puzzle name is: " + l.sections[i+1]);
                        //print("turn on new section");
                        return; // AAAH this might not break all the way out of the loop, maybe that's the issue i feel like level done must still run after some time in the loop
                    }
                    else
                    {
                        //print("level done");
                        LevelDone();
                        return;
                    }
                    
                }
            }
            //print("is any of this running?");
        }
        //print("is even this running?");
    }

    void TurnOffFinalPuzzle()
    {
        foreach (Level l in levels)
        {
            foreach (GameObject s in l.sections)
            {
                if (s.activeInHierarchy)
                {
                    //print(s.name + " turning this off");
                    s.SetActive(false);
                    //print(l.noiseSignal.layer1Tex);
                    //print(l.noiseSignal);
                    currentLevel = null;
                    Tesseract.instance.animator.SetTrigger("ReturnToHub");
                    return;
                }
            }
        }
    }

    public void LevelDone()
    {
        //print("level done!");
        // when the level is done, close all the puzzles
        // I could make it so that the light goes off here too and I have a cleaner list of references
        micLight.TriggerLight(); // trigger goes for 2 seconds, lets do a 2 second delay before we leave all the puzzles
        FreqScanObject.instance.ChangeSignal(currentLevel.noiseSignal); // when we set signal lets just send this levels' freqscan class
        // I need to just turn all puzzles off? or relly just the one actully. lets get a direct reference to it instead of looping? // lets loop for now
        StartCoroutine(DelayAndThenFunction(TurnOffFinalPuzzle, 2f));

         // this should also happen in the game controller probably
        // do this though after the light is done recording
    }

    public IEnumerator DelayAndThenFunction(FunctionAfterDelay function, float delayTime) // wonder if I can put this in its own script somewhere, could be a static function or something
    {
        //print("how many times does the delay happen?");
        yield return new WaitForSeconds(delayTime);
        //print("and what happens when I put this down here?");
        function();
    }
}
