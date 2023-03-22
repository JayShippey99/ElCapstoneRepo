using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using System.Reflection;
using System;
using UnityEngine.Events;
using ZSerializer;

public class GameController : MonoBehaviour
{
    // so now that the game is linear // the game will be linear BUT I think we could have splits at some points so that both sides help each other be solved
    // I think I need a function for start intro, start tutorial, start sideX premessage, end,  // do we even need the exit codes? yeah I think it'll just be an easy thing to hook up with our system

    // gamecontroller will have a runGameCode function, it'll be a big ass switch statament? orrr? maybe instead its, no. swtich statment cause idk if we could run functions through a class[] thing
    // how does the tutroial play into this?



    // I need an easy way to trigger gameEvents // game events are the intro, between sides, wait these all mostly sound like dialogue stuff. within those i can send calls to here to do thigns not dialogue related
    // stuff like adding to the corkboard
    // the cork board could be its own class
    // it could store all the paper game objects
    
    // what if I do more than one thing with a gameCode?

    // I need a way to say add these papers when the player isn't looking
    // so I need a function that takes in an array of gameobjects to turn on when you're not looking

    // THE DIALOGUE has the game codes
    // so I just need a function for each time I need to change something about the world
    // adding new papers
    // turning on new sides
    // activating arrows for the tutorial

    // what does the switch do now? // i think the switch will instead run an animation to spin some things, turn some lights on, and make the tesseract appear in front of the player
    // omg how tf do we animate a whole scene? if i want to animate a whole scene I'll instead just need to get all the animators of pieces within the scene and trigger the same trigger in each

    // when does the first cutscene stop, after the tesseract becomes shwon, so intro sequence basically, lets make an event that happens at the end of the intro animation that starts the next dialogue, and turns cutscene to off
    // end introSeq event

    public static GameController instance;

    public Side frontSide, backSide, rightSide, leftSide, topSide, bottomSide;
    [HideInInspector]
    public Side[] sides;

    [HideInInspector]
    public Side focusedSide;
    [HideInInspector]
    public PlantPuzzle currentPuzzle;
    // maybe what we can do is just recieve a signal if a plant puzzle was solved or not

    public float fizzleDelay;
    public delegate void FunctionAfterDelay();

    public Animator finalStickNote;

    public GameObject mainMenu, hudMenu;
    public GameObject endMenu;

    [HideInInspector]
    public bool allSidesCompleted;

    public Tesseract tesseract;

    [HideInInspector]
    public bool cutsceneHappening;

    PlantPuzzle thisPuzzle, nextPuzzle; // this is just for the delay and change


    public CommsBrain cb;
    
    // I think I may need to make a bank of all the dialogue I've given and stuff
    [HideInInspector]
    public bool tutorial;
    [Header("Tutorial")]
    public GameObject[] tutorialArrows;

    public GameEvent[] gameEvents; // i'd like there to be a gameCode + a function name that gets run
    // select function from drop down?

    [Header("CorkboardPapers")]
    public Corkboard corkboard;
    [HideInInspector]
    public bool readingPaper;

    [Header("TestingShorcuts")]
    public bool cutscene;
    public bool playIntroSound;

    //public bool shortCutToFirstPuzzle;

    [HideInInspector]
    public int dialogueSectionNum;

    public Vector3 introShakeStats;

    [Header("ObjectsInTheRoom")]
    public Light[] roomLights;
    public Material lightsMaterial;

    //int testCounter = 0;

    public void Awake()
    {
        print("DOes the awake run again??");
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

        //cutscene = true;
        //print(tesseract.animator);
        TurnOffLightsInRoom();

        ZSerialize.LoadScene();
        //print(testCounter);

    }

    void ShortCutToFirstPuzzle()
    {
        IntroSequence();
        StartSide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) ShortCutToFirstPuzzle();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // bring up menu again
            mainMenu.SetActive(true);
            hudMenu.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //currentPuzzle.ClearPuzzle();
        }
        
        if (corkboard.needsToAdd)
        {
            if (corkboard.CanAdd()) corkboard.AddPapers();
        }

        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //testCounter++;
            print("saving from controller");
            ZSerialize.SaveScene();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            StartSide();
        }
        
    }

   

    public void KillCube()
    {
        Destroy(tesseract.gameObject);
    }

    public void GoToNextPuzzle() // This is the puzzle solved function basically
    {
        //print(focusedSide.puzzles.Count); // the count is fucking 0??
        Side s = focusedSide; // i wonder if this is being set how we want
        
        // turn off current puzzle, turn on the next puzzle
        for (int i = 0; i < s.puzzles.Count - 1; i++) // puzzles will be the right length to only check for the puzzles
        {
            //print("does this run?");
            if (s.puzzles[i].gameObject.activeInHierarchy)
            {
                // when we do this we need to delay instead

                thisPuzzle = s.puzzles[i];
                nextPuzzle = s.puzzles[i + 1];

                // CHANGE PUZZLE
                KillCurrentPuzzle(thisPuzzle.transform);
                StartCoroutine(DelayAndThenFunction(StartNextPuzzle, fizzleDelay)); // you can place after the puzzle is solved
                // could the puzzle be broken after its done?

                // okay so the issue with this is that puzzles can't contain the alien image in the list
                return;
            }
        }
        
        //s.puzzles[0].gameObject.SetActive(false);

        LevelDone(s);

        // I think I need it to unfocus when I solve it?
    }

    void KillCurrentPuzzle(Transform puzzle)
    {
        foreach (Transform child in puzzle)
        {
            if (!child.CompareTag("BranchPuzzleCondition"))
            {
                BranchInitializer bi = child.GetComponent<BranchInitializer>();
                EndInitializer ei = child.GetComponent<EndInitializer>();

                //print("does this run?");
                if (bi != null)
                {
                    //print("how about this one");
                    StopCoroutine(bi.Fizzle(-1));
                    StartCoroutine(bi.Fizzle(-1)); // awful line of code //Destroy(transform.GetChild(i).gameObject);
                }
                else if (ei != null)
                {
                    //print("also checking this one");
                    StopCoroutine(ei.Fizzle(-1));
                    StartCoroutine(ei.Fizzle(-1)); // awful line of code //Destroy(transform.GetChild(i).gameObject);
                }
            }
            else
            {
                PlantCondition pc = child.GetComponent<PlantCondition>();
                print("does this run?");
                if (pc != null)
                {
                    StopCoroutine(pc.Fizzle(-1));
                    StartCoroutine(pc.Fizzle(-1));
                }
            }
        }
    }

    void StartNextPuzzle()
    {
        PlantPuzzle p1 = thisPuzzle;
        PlantPuzzle p2 = nextPuzzle;

        //print("what about this?");
        p1.gameObject.SetActive(false);
        p2.gameObject.SetActive(true);
        // well fuck now we also need to tell the side manager what's the current puzzle
        //SideManager.instance.currentlyActivePuzzle = s.puzzles[i + 1]; // this will need to change
        currentPuzzle = p2;
    }

    void ForceCameraForward()
    {
        MoveCamera.instance.LookThisWay(0);
        hudMenu.transform.Find("LookCork").gameObject.SetActive(true);
        hudMenu.transform.Find("LookBack").gameObject.SetActive(false);
    }

    public void IntroSequence()
    {
        if (playIntroSound) RuntimeManager.PlayOneShot("event:/IntroStartUp");


        ForceCameraForward();
        MoveCamera.instance.ShakeCamera(introShakeStats.x, introShakeStats.y, introShakeStats.z);
        tesseract.animator.enabled = true;
        cutscene = true;
        // initial click turns on main lights
        // with the extra clicks, the screens will turn on one by one
        // radars on the side grow big bright particles
        // before they reach the max, the pedestal will begin to rotate fast
        // they shoot it at the center on the drop
        // the tesseract starts out pure white
        // tesesract grows and twitches in different rotations
        // radar particles shrink as this goes on
        // tesseract loses its whiteness when it starts the power down
        // the pedestal slows down
        // it'll spin a little out of this
    }

    public void EndIntroSeq()
    {
        cb.StartDialogueChunk("roomStarted");
        cutscene = false;
    }

    public void ShowEndScreen()
    {
        endMenu.SetActive(true);
    }

    public void StartEndSequence()
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
        //StartEndSequence();
    }

    public void LevelDone(Side s)
    {
        // when the whole side is solved

        s.puzzles[s.puzzles.Count - 1].gameObject.SetActive(false); // what does this do?
        currentPuzzle = null;
        s.intel.SetActive(true); // lowkey with the way that I did these sticky notes, I might as well have just done the same thing for theese
        s.done = true;
        s.stickyNote.SetTrigger("Release");
        RuntimeManager.PlayOneShot(s.sideSolvedSoundPath);

        RunGameEvent(s.exitCode);
        // need to get s exitCode here and send it to the runevent funciton

        CheckIfAllSidesAreComplete();
    }

    public IEnumerator DelayAndThenFunction(FunctionAfterDelay function, float delayTime) // wonder if I can put this in its own script somewhere, could be a static function or something
    {
        //print("how many times does the delay happen?");
        yield return new WaitForSeconds(delayTime);
        //print("and what happens when I put this down here?");
        function();
    }

    public void RunGameEvent(string gameCode)
    {
        foreach (GameEvent ge in gameEvents)
        {
            if (ge.triggerCode == gameCode)
            {
                ge.function.Invoke();
                return;
            }
        }
    }

    public void SetPapersToAdd()
    {
        corkboard.SetPapersToAdd();
    }

    // I definitely need a tutorial list of gameobjects
    public void ChangeTutorialStep() // 
    {
        for (int i = 0; i < tutorialArrows.Length; i ++)
        {
            if (i == tutorialArrows.Length - 1)
            {
                //print("not doing it right " + i + " " + (tutorialArrows.Length - 1));
                tutorialArrows[i].SetActive(false);
            }
            else if (tutorialArrows[i].activeInHierarchy)
            {
                //print("doign the thing");
                tutorialArrows[i].SetActive(false);
                tutorialArrows[i + 1].SetActive(true);
                return;
            }
        }
    }

    void StartIntro()
    {
        cb.StartDialogueSection(cb.dialogueSections[0]);
    }

    public void StartTutorial()
    {
        tutorialArrows[0].SetActive(true);
    }

    public void StartSide() // AKA unlock side
    {
        SideManager.instance.Unfocus();
        for (int i = 0; i < sides.Length; i++)
        {
            if (sides[i].projection.isOn == false)
            {
                sides[i].projection.SetState(true);
                sides[i].projection.unlocked = true;
                return;
            }
        }
    }

    public void EndSide()
    {
        SideManager.instance.Unfocus(); // I'm gonna do it at the beginning and end of a section just to make sure it's working
        cb.StartDialogueSection(cb.dialogueSections[dialogueSectionNum]);
    }


    public void QuitGame()
    {
        // save all stats here, but also save periodically
        Application.Quit();
    }

    void TurnOffLightsInRoom()
    {
        foreach (Light l in roomLights)
        {
            l.gameObject.SetActive(false);
        }
        lightsMaterial.SetFloat("_FlickerAmount", 1);
    }

    public void TurnOnLightsInRoom()
    {
        foreach (Light l in roomLights)
        {
            l.gameObject.SetActive(true);
        }
        lightsMaterial.SetFloat("_FlickerAmount", 0);
    }
}

[System.Serializable]
public class GameEvent
{
    public string triggerCode;
    public UnityEvent function;
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
    public string exitCode;
    [HideInInspector]
    public bool done;
    public string sideSolvedSoundPath;
}

[System.Serializable]
public class Corkboard
{
    public GameObject[] papersList;
    [HideInInspector]
    public List<GameObject> papersToAdd;
    [HideInInspector]
    public bool needsToAdd;

    public bool CanAdd()
    {
        if (Camera.main.transform.rotation.eulerAngles.y == 0) return true;
        else return false;
    }

    public void SetPapersToAdd()
    {
        needsToAdd = true;

        foreach (GameObject paper in papersList)
        {
            if (paper.activeInHierarchy == false) // it feels sketchy now that i'm looking at it again
            {
                papersToAdd.Add(paper);
                return;
            }
        }
    }

    public void AddPapers()
    {
        foreach (GameObject paper in papersToAdd)
        {
            paper.SetActive(true);
        }

        needsToAdd = false;

        papersToAdd.Clear();
    }
}

public class Tutorial // part of me is thinking that for each step in the tutorial, no wait, I can just do something twice if I need
{
    // each section of the tutorial needs a gameCode
}

