using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class GameController : MonoBehaviour
{
    // I want the controller now to be in charge of knowing what puzzles go to which side
    // i'm mainly doing this so that I can keep track of the game state easier, like if all things are completed or not
    // it'll also help keep track of button presses for pausing and such

    // okay I might need to rethink this, I think that I'm gonna need to shove A TON of code into this one script and it might just not even be cleaner in the end is my concern
    // I feel like I'm gonna start having EVERYThing move through this script which would just get clunky


    public static GameController instance;

    /*
    public Level[] levels;

    int[][] levelAndSection; // this will keep track of what current level and section I'm on, if level is -1, then i'm in the hub?

    public _Light micLight;


    Level currentLevel;
    */

    public Side frontSide, backSide, rightSide, leftSide, topSide, bottomSide;
    [HideInInspector]
    public Side[] sides;

    [HideInInspector]
    public Side focusedSide;
    [HideInInspector]
    public PlantPuzzle currentPuzzle;
    // maybe what we can do is just recieve a signal if a plant puzzle was solved or not

    public delegate void FunctionAfterDelay();

    public Animator finalStickNote;

    public GameObject mainMenu;
    public GameObject endMenu;

    [HideInInspector]
    public bool allSidesCompleted;

    public Tesseract tesseract;

    [HideInInspector]
    public bool cutsceneHappening;

    public void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // each levels has .section number?
        //print(levels[0].sections.Length);

        sides = new Side[]
        {
            frontSide, backSide, rightSide, leftSide, topSide, bottomSide
        };

        foreach (Side s in sides) // foreach side
        {
            Transform pp = s.puzzleList.transform;
            foreach (Transform child in pp) // go through all the children in the parent and make them the puzzles
            {
                //print("adding to puzzle list");
                s.puzzles.Add(child.GetComponent<PlantPuzzle>()); // now we have assigned the puzzles for each side
            }

            //print(s.puzzles.Count + " this is how many puzzles I just added");
        }

        //print(tesseract.animator);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // bring up menu again
            mainMenu.SetActive(true);
        }
    }


    public void StartNewLevel(int i)
    {
        /*
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
        */
    }

    // does this mean I need to keep track of where I am in the puzzle list?
    
    /*
    public void GoToNextSection() // maybe I can phrase this as "puzzle done"
    {
        //print("go to next section more than once?");
        StartCoroutine(DelayAndThenFunction(ChangeSection, 2f));
    }
    */
    

    public void GoToNextPuzzle() // do we want to send in which side? or should we already know that?
    {
        //print(focusedSide.puzzles.Count); // the count is fucking 0??
        Side s = focusedSide; // i wonder if this is being set how we want
        // turn off current puzzle, turn on the next puzzle
        for (int i = 0; i < s.puzzles.Count - 1; i++) // puzzles will be the right length to only check for the puzzles
        {
            //print("does this run?");
            if (s.puzzles[i].gameObject.activeInHierarchy)
            {
                //print("what about this?");
                s.puzzles[i].gameObject.SetActive(false);
                s.puzzles[i + 1].gameObject.SetActive(true);
                // well fuck now we also need to tell the side manager what's the current puzzle
                //SideManager.instance.currentlyActivePuzzle = s.puzzles[i + 1]; // this will need to change
                currentPuzzle = s.puzzles[i + 1];

                // okay so the issue with this is that puzzles can't contain the alien image in the list
                return;
            }
        }

        LevelDone(s);
    }

    void ChangeSection()
    {
        /*
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
        */
    }

    public void ShowEndScreen()
    {
        endMenu.SetActive(true);
    }

    void StartEndSequence()
    {
        // rotate back to front again

        tesseract.ManualUpright(Vector3.forward, frontSide);

        foreach (Side s in sides)
        {
            s.projection.SetState(false);
        }

        finalStickNote.SetTrigger("Release");
        //print(tesseract.animator);
        tesseract.animator.SetTrigger("FinalSequence");
    }

    void CheckIfAllSidesAreComplete()
    { 
        
        foreach (Side s in sides)
        {
            if (!s.done) return;
        }
        
        

        print("ALL SIDE ARE COMPLETED YOU DID ITTT");
        allSidesCompleted = true;
        StartEndSequence();
    }

    public void LevelDone(Side s)
    {
        // if its the last puzzle then this will run
        //print("ALL PUZZLES FOR THIS SIDE ARE DONE");
        // here is where we would run the sequence for solving the side
        // and here is where we would show the constellation

        s.puzzles[s.puzzles.Count - 1].gameObject.SetActive(false); // what does this do?
        currentPuzzle = null;
        s.intel.SetActive(true); // lowkey with the way that I did these sticky notes, I might as well have just done the same thing for theese
        s.done = true;
        s.stickyNote.SetTrigger("Release");
        RuntimeManager.PlayOneShot(s.sideSolvedSoundPath);

        CheckIfAllSidesAreComplete();
    }

    public IEnumerator DelayAndThenFunction(FunctionAfterDelay function, float delayTime) // wonder if I can put this in its own script somewhere, could be a static function or something
    {
        //print("how many times does the delay happen?");
        yield return new WaitForSeconds(delayTime);
        //print("and what happens when I put this down here?");
        function();
    }

    public void QuitGame()
    {
        // save all stats here, but also save periodically
        Application.Quit();
    }
}


[System.Serializable]
public class Side
{
    public SideBrain projection;
    public GameObject puzzleList;
    [HideInInspector]
    public List<PlantPuzzle> puzzles;
    [HideInInspector]
    public PlantPuzzle currentPuzzle; // for each side there will be a current puzzle being solved
    public GameObject intel; // maybe this doesn't need to be in the list?
    public Animator stickyNote;
    [HideInInspector]
    public bool done;
    public string sideSolvedSoundPath;
}