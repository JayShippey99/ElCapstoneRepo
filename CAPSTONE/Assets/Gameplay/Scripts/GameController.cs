using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // i might need a way to let each puzzle know where in the list is falls
    // how to go to the next one is really interesting
    
    // okay for now I'm just gonna set it up so that it recognizes abc
    public static GameController instance;

    public Level[] levels;

    int[][] levelAndSection; // this will keep track of what current level and section I'm on, if level is -1, then i'm in the hub?

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

        levels[i].sections[0].SetActive(true); // this starts the first puzzle in the list

        // so the puzzle runs now, very good, that should mean that it knows when it won now too
    }

    // does this mean I need to keep track of where I am in the puzzle list?
    public void GoToNextSection()
    {
        // OOO wait loop through the levels[] and then the sections[] if a section is active, set the next one to be active and this one off, if there is no next one, then run a level done function
        foreach (Level l in levels)
        {
            for (int i = 0; i < l.sections.Length; i++)
            {
                GameObject s = l.sections[i];

                if (s.activeInHierarchy)
                {
                    // turn it off and set the next one to on, then break I imagine
                    if (i + 1 < l.sections.Length)
                    {
                        // meaning there is another one in the list of sections
                        s.SetActive(false);
                        l.sections[i + 1].SetActive(true);

                        /*
                        if (l.sections[i + 1].CompareTag("BranchPuzzle")) // we can maybe do different things for different level tags, not totally elegant but it gets it done I suppose
                        {
                            // do something
                            // what do we want to do again?
                            // we want to send a thing to the branch parent
                            // we should just send that object to the branch brain parent?
                            // this is how we set the puzzle?
                            // okay I'm just lost then, i'd rather not have a script on all of these puzzles, especially for how many there are. I guess this works.
                            //PlantPuzzle.instance.currentLevel = l.sections[i + 1];
                        }
                        */
                        
                        break;
                    }
                    else
                    {
                        LevelDone();
                    }

                }
            }
        }

    }

    public void LevelDone()
    {
        print("LEVEL DONE");
    }
}
