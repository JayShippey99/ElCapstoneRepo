using FMODUnity;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // end puzzle stuff
    [HideInInspector]
    public Side[] endPuzzleSideOrder; // we set this in the start
    [HideInInspector]
    public string[] endPuzzleSideStringOrder;
    [HideInInspector]
    public int endPuzzleGuessNumber; // this will start at 0, when we make a guess, we ask if the focused side == endpuzzSO[0] and then if its the same increase the guessNumber, but if its wrong we reset


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

    [Header("Machines to add")]
    public GameObject[] machines;
    public Animator screenDarken;

    [Header("ObjectsInTheRoom")]
    public Light[] roomLights;
    public Material lightsMaterial;
    public _Switch switchForSpread;
    public Animator radarAnim;

    [Header("MusicStuff")]
    FMOD.Studio.Bus busMusic;
    FMOD.Studio.Bus busSFX;

    // i need these to start off as .8 by default
    float musicVolume = .8f; // so these will need a translation one way or another. I think it might be best to save it as . so it starts at .8. and our things go from -80 to 10, so its really -90 to 0 which means .8 of -90 is? -89.2 maybe that'll lineit up right? no, that's not right
    float sfxVolume = .8f;

    [Header("Saving and Loading")]
    public bool loadOnStart;

    [HideInInspector]
    public int unlockedParticles = 2; // unlock side1, then we need to add a new particle, add new particle event

    //int testCounter = 0;

    public float GetMusicVol() { return musicVolume; }
    public float GetSfxVol() { return sfxVolume; }
    public void SetMusicVol(float vol) { musicVolume = vol; }
    public void SetSFXVol(float vol) { sfxVolume = vol; }
    
    public void Awake()
    {
        //print("DOes the awake run again??");
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // each levels has .section number?
        //print(levels[0].sections.Length);

        sides = new Side[]
        {
            frontSide, backSide, rightSide, leftSide, topSide, bottomSide
        };

        endPuzzleSideOrder = new Side[] { topSide, leftSide, backSide, rightSide, frontSide, bottomSide };
        endPuzzleSideStringOrder = new string[] { "0", "1", "2", "3", "0", "1" };

        print("probably not working");

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
        //TurnOffLightsInRoom(); // holy fuck its YOU

        busMusic = FMODUnity.RuntimeManager.GetBus("bus:/BusMusic");
        busSFX = FMODUnity.RuntimeManager.GetBus("bus:/BusSFX");

        if (loadOnStart) SaveAndLoadGame.Load();
    }

    public bool IsPuzzleReal()
    {
        return currentPuzzle != null;
    }

    void ShortCutToFirstPuzzle()
    {
        IntroSequence();
        StartSide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) ShortCutToFirstPuzzle();

        /*
        if (Input.GetKeyDown(KeyCode.D))
        {
            dialogueSectionNum++;
            cb.StartDialogueSection(cb.dialogueSections[dialogueSectionNum]);
            print(dialogueSectionNum);
        }
        */

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // bring up menu again
            mainMenu.SetActive(true);
            hudMenu.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SaveAndLoadGame.ResetGame();
        }
        
        if (corkboard.needsToAdd)
        {
            if (corkboard.CanAdd()) corkboard.AddPapers();
        }

        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveAndLoadGame.Save();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            print("pressing n");
            StartSide();
            //FullInputController.instance.PuzzleReady();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AddNewParticle();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            // I want to do a shortcut to the very end, 
            // so I need each side to be "completed", I could use the logic from the load and save thing
            // solving the last side of the tesseract needs to like override the power of the room and let the inputs send for the intel
            // for starters, we need to make it go to the intel of each side

            SaveAndLoadGame.puzzleForSides = new int[] { 0, 0, 0, 0, 0, 0 };
            SaveAndLoadGame.unlockedSides = 6; // I swear to god though tthat doing 6 woudl break thing

            for (int i = 0; i < sides.Length; i++)
            {
                Side s = sides[i];

                s.projection.Unlock(); // why am i not saving the unlocked sides?

                

                foreach (PlantPuzzle puzzle in s.puzzles)
                {
                    puzzle.gameObject.SetActive(false); // what does this do? // turns the last puzzle child false, gotcha
                //print("puzzle count for side " + i + " is " + s.puzzles.Count);
                    SaveAndLoadGame.IncreasePuzzleForSide(GetIndexOfFocusedSide(s)); // omfg duhhh 
                }

                currentPuzzle = null;
                s.intel.SetActive(true); // lowkey with the way that I did these sticky notes, I might as well have just done the same thing for theese
                s.done = true;
                //RuntimeManager.PlayOneShot(s.sideSolvedSoundPath);

                // need to get s exitCode here and send it to the runevent funciton
            }

            SaveAndLoadGame.Save();

            CheckIfAllSidesAreComplete();



        }

    }

    public void KillCube()
    {
        Destroy(tesseract.gameObject);
    }

    int GetIndexOfFocusedSide(Side side) // am i sending a side that doesn't exist or what
    {
        for (int i = 0; i < sides.Length; i++)
        {
            if (side == sides[i]) return i;
        }

        print("MEGA OOPSIE");
        return -1;
    }    

    public void GoToNextPuzzle() // This is the puzzle solved function basically
    {
        //print(focusedSide.puzzles.Count); // the count is fucking 0??
        Side s = focusedSide; // i wonder if this is being set how we want
        
        // turn off current puzzle, turn on the next puzzle
        for (int i = 0; i < s.puzzles.Count - 1; i++) // puzzles will be the right length to only check for the puzzles
        {
            //print("loop for puzzles count");
            //print("does this run?");
            if (s.puzzles[i].gameObject.activeInHierarchy)
            {
                // when we do this we need to delay instead

                thisPuzzle = s.puzzles[i]; // duh this puzzle never gets set
                nextPuzzle = s.puzzles[i + 1];

                //print("change puzzle!");
                SaveAndLoadGame.IncreasePuzzleForSide(GetIndexOfFocusedSide(s));
                
                // CHANGE PUZZLE
                // CHANGE PUZZLE
                KillCurrentPuzzle(thisPuzzle.transform);
                StartCoroutine(DelayAndThenFunction(StartNextPuzzle, fizzleDelay)); // you can place after the puzzle is solved
                // could the puzzle be broken after its done?

                // okay so the issue with this is that puzzles can't contain the alien image in the list
                return;
            }
        }

        //print("does this run???? " + s);
        StartCoroutine(LevelDone(s));
        //print("does this run49018983");

        // I think I need it to unfocus when I solve it?
    }

    void KillCurrentPuzzle(Transform puzzle)
    {
        if (RotationScreen.instance != null) RotationScreen.instance.Restart();

        if (SpreadScreen.instance != null) SpreadScreen.instance.Restart();

        foreach (Transform child in puzzle)
        {
            //print(child.name);
            if (!child.CompareTag("BranchPuzzleCondition"))
            {
                BranchInitializer bi = child.GetComponent<BranchInitializer>();
                EndInitializer ei = child.GetComponent<EndInitializer>();

                //print("d? ");
                if (bi != null) // this doesn't run
                {
                    //print("how about this one");
                    StopCoroutine(bi.Fizzle(-1));
                    StartCoroutine(bi.Fizzle(-1)); // awful line of code //Destroy(transform.GetChild(i).gameObject);
                }
                else if (ei != null) // and this doesn't run
                {
                    //print("also checking this one");
                    StopCoroutine(ei.Fizzle(-1));
                    StartCoroutine(ei.Fizzle(-1)); // awful line of code //Destroy(transform.GetChild(i).gameObject);
                }
            }
            else if (child.GetComponent<PlantCondition>() != null)
            {
                //print("kill the conditions?");
                PlantCondition pc = child.GetComponent<PlantCondition>();
                StopCoroutine(pc.Fizzle(-1));
                StartCoroutine(pc.Fizzle(-1));
            }
            else
            {
                //print("kill anything else?");
                Destroy(child.gameObject);
            }

        }

        //DelayAndThenFunction(KillThisPuzzle, fizzleDelay);
    }

    void KillThisPuzzle()
    {
        thisPuzzle.gameObject.SetActive(false);
    }

    public void ChangeBusVolume(bool changeMusic, float sliderV, float db)
    {
        if (changeMusic)
        {
            musicVolume = sliderV;
            busMusic.setVolume(db);
            SaveAndLoadGame.ChangeMusicVolume(musicVolume);
            //print(musicVolume + " musvol");
        }
        else
        {
            //print("changing sfx bus volume " + sliderV);
            sfxVolume = sliderV;
            busSFX.setVolume(db);
            SaveAndLoadGame.ChangeSFXVolume(sfxVolume);
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

        radarAnim.SetTrigger("Intro");

        ForceCameraForward();
        MoveCamera.instance.ShakeCamera(introShakeStats.x, introShakeStats.y, introShakeStats.z);
        tesseract.animator.SetTrigger("Intro");
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
        cb.StartDialogueSection("roomStarted");
        cutscene = false;
        SaveAndLoadGame.UpdateTesseractSize(1);
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
            if (!s.done)
            {
                print(s.projection.name + " is what's not done");
                return;
            }
        }
        
        //StartEndSequence();
        AllSidesAreComplete();
    }

    void AllSidesAreComplete()
    {
        print("ALL SIDE ARE COMPLETED YOU DID ITTT");
        allSidesCompleted = true;
        // now what happens
        // without adding all the effects, maybe we should have a function that starts up the new functionality for the last task
        // what is the new functionality for the last task?
        // when you focus on the intel, you're able to shoot at it this time, you'll be able to just shoot 1 particle at it
        // for now it'll print "particle recieved"
        // i also need an order to be done correctly so I need a list of conditions somewhere, or like a list of steps, its 6 steps, each step has a certain amount of conditions
        // conditions include, root rotation, spread amount, and particle type, these conditions need to be a value of each side, and so then the side itself can check if all the conditions were met, then we can add a yes to the list?
        // but wait, we also need it to be in the correct order
        // hmm maybe unlocking new things involves a little version of each of these puzzles
        // I need a way to allow an input when all sides are completed

    }

    public IEnumerator LevelDone(Side s)
    {
        // when the whole side is solved

        print("killing");
        if (thisPuzzle == null) thisPuzzle = s.puzzles[s.puzzles.Count - 1];
        KillCurrentPuzzle(thisPuzzle.transform);

        yield return new WaitForSeconds(fizzleDelay);

        print("after fizzle delay");

        //StartCoroutine(DelayAndThenFunction(StartNextPuzzle, fizzleDelay));

        SaveAndLoadGame.IncreasePuzzleForSide(GetIndexOfFocusedSide(focusedSide));

        s.puzzleList.gameObject.SetActive(false);
        //s.puzzles[s.puzzles.Count - 1].gameObject.SetActive(false); // what does this do? // turns the last puzzle child false, gotcha
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

    public void AddNewParticle()
    {
        // well no, cause we can have a dialogue trigger this, this is okay

        unlockedParticles++;
        if (unlockedParticles == 3) cb.StartDialogueSection("endParticle"); // is this even what I want? // shouldn't end particle just go into the list of dialogue sections nums?
        if (unlockedParticles == 4) cb.StartDialogueSection("repeatParticle"); // is this even what I want? // shouldn't end particle just go into the list of dialogue sections nums?

        SaveAndLoadGame.IncreaseUnlockedParticles();
    }

    public void AddMachineSequence()
    {
        // run an animation 
        screenDarken.SetTrigger("Blink");
    }

    public void AddMachine()
    {


        foreach (GameObject g in machines)
        {
            if (g.activeInHierarchy == false)
            {
                SaveAndLoadGame.IncreaseAddedMachines();

                g.SetActive(true);

                // lowkey what I think I might do also is make it so that based on g, I start a different new dialogue section or something
                if (g.name == "RotateMachine")
                {
                    ChangeTutorialStep();
                    cb.StartDialogueSection("rotateMachine");
                }
                if (g.name == "SpreadMachine")
                {
                    ChangeTutorialStep();
                    cb.StartDialogueSection("spreadMachine");
                }

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
                // so if I have 3 steps, and the last one is n empty one, on the last one, i just turn that one off, but then my index stays as 2, which means if I load up i'll just 
                // this won't happen actually and if it does I think that it won't break
            }
            else if (tutorialArrows[i].activeInHierarchy)
            {
                //print("doign the thing");
                tutorialArrows[i].SetActive(false);
                tutorialArrows[i + 1].SetActive(true);

                SaveAndLoadGame.IncreaseTutorialStep();
                return;
            }
        }
    }

    public void StartTutorial()
    {


        foreach (GameObject g in tutorialArrows)
        {
            g.SetActive(false);
        }

        SaveAndLoadGame.IncreaseTutorialStep();

        tutorialArrows[1].SetActive(true);
        cb.StartDialogueSection("tutorial");
    }

    public void StartSide() // AKA unlock side
    {
        //print("from start side");
        SideManager.instance.Unfocus();
        tesseract.animator.enabled = true;

        for (int i = 0; i < sides.Length; i++)
        {
            if (sides[i].projection.unlocked == false) // ohhh so we're just already asking if its on or not or somehting
            {
                sides[i].projection.Unlock();
                SaveAndLoadGame.IncreaseUnlockedSides(); // eh I don't like that I'm doing the math here but, oh my god yeah it'll be so much smoother if we don't do it this way
                //sides[i].projection.SetState(true); // huh so this is the only way we set things to true? for the first time?
                //sides[i].projection.unlocked = true;
                return;
            }
        }
    }

    public void EndSide(int num)
    {
        //print("from end side");
        //SideManager.instance.Unfocus(); // I'm gonna do it at the beginning and end of a section just to make sure it's working
        //cb.StartDialogueSection(cb.dialogueSections[dialogueSectionNum]); // okay this isn't adding up correctly anymore
        //cutscene = false; // this might change
        // ohhhh dialogue section number isn't being saved so it always resets

        switch(num)
        {

            case 1:
                cb.StartDialogueSection("side1");
                break;
            case 2:
                cb.StartDialogueSection("side2");
                break;
            case 3:
                cb.StartDialogueSection("side3");
                break;
            case 4:
                cb.StartDialogueSection("side4");
                break;
            case 5:
                cb.StartDialogueSection("side5");
                break;
            case 6:
                cb.StartDialogueSection("side6");
                break;
        }

    }

    public void QuitGame()
    {
        // save all stats here, but also save periodically
        print(SaveAndLoadGame.lightsOn + " last call are lights on");
        Application.Quit();

        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void TurnOffLightsInRoom()
    {
        foreach (Light l in roomLights)
        {
            l.gameObject.SetActive(false);
        }
        lightsMaterial.SetFloat("_FlickerAmount", 1);

        SaveAndLoadGame.UpdateTurnOnLights(false);
    }

    public void TurnOnLightsInRoom()
    {
        print("turn on lights!");
        foreach (Light l in roomLights)
        {
            l.gameObject.SetActive(true);
        }
        lightsMaterial.SetFloat("_FlickerAmount", 0);

        SaveAndLoadGame.UpdateTurnOnLights(true);
    }
        
    public void ResetGame()
    {
        SaveAndLoadGame.ResetGame();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        // fade to black or something
        // reset all stats
        // reopen level?
    }

    public void UnlockFirstSide() // we'll need this functoin so that we can unlcock the first side, wait and then run the dialogeu
    {
        StartSide();
        StartCoroutine(DelayAndThenFunction(StartTutorial2, 2)); // you can place after the puzzle is solved
    }

    public void StartTutorial2()
    {
        cb.StartDialogueSection("tutorial2");
        // where to put the code that say if your dialogue section is tutorial, then set restart the tutorial steps, and if you start with tutorial2, load with nother number
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
        return Camera.main.transform.rotation.eulerAngles.y == 0;
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

        SaveAndLoadGame.IncreaseAddedPapers();
    }
}

[System.Serializable]
public class FinalPuzzleCondition
{
    public Side side;
    public string condition; // will be 0, 1, 2 or, 3
}

public class Tutorial // part of me is thinking that for each step in the tutorial, no wait, I can just do something twice if I need
{
    // each section of the tutorial needs a gameCode
}

