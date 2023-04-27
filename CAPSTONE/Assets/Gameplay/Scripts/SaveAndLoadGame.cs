using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class SaveAndLoadGame
{
    static public int unlockedSides = 0;
    static public string dialogueSection = "Intro";
    static public float musicVolume = .8f;
    static public float sfxVolume = .8f;
    static public int addedPapers = 0;
    static public int addedMachines = 0;
    static public float tesseractSize = 0;
    static public int[] puzzleForSides = new int[] { 0, 0, 0, 0, 0, 0 };
    static public int unlockedParticles = 2;
    static public int lightsOn = 0;
    static public int tutorialStep = 0;
    static public int gameDone = 0; // what this will do is trump everything on the loading and just set everything to its max
    static public int canDoEndParticles = 0;

    // okay so if I load with a dialogue section, I know exactly what to load everything with
    // if I don't have a dialogue section, that means I'm solving puzzles or something. so that means that I can use the most open side to determine how to load everything
    // this will be super manual and might require a lot of reworking
    // is there a better way to do this

    // so what happened, i closd down opened up, the arrow was in the wrong spot, because of this, it kept iterating at the wrong step
    // why did this happen
    // it started with exactly nothing which is the weird part


    static public void Save() 
    {
        //Debug.Log("save game");
        // save number of unlocked sides
        //Debug.Log("sides unlocked " + unlockedSides);
        PlayerPrefs.SetInt("UnlockedSides", unlockedSides);
        //Debug.Log("sides unlocked " + unlockedSides);

        // save dialogue section
        //Debug.Log("saveing dialogue with " + dialogueSection);
        PlayerPrefs.SetString("DialogueSection", dialogueSection);

        // save sound
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);

        // save added papers
        PlayerPrefs.SetInt("AddedPapers", addedPapers);

        // save machines added
        PlayerPrefs.SetInt("AddedMachines", addedMachines);

        // set tesseract size for animation purposes
        //Debug.Log("saving size as " + tesseractSize);
        PlayerPrefs.SetFloat("TesseractSize", tesseractSize);

        // set which puzzle is activate for each side
        //Debug.Log("puzzles unlocked " + puzzleForSides[0]);
        PlayerPrefs.SetInt("Side1Puzzle", puzzleForSides[0]);
        PlayerPrefs.SetInt("Side2Puzzle", puzzleForSides[1]);
        PlayerPrefs.SetInt("Side3Puzzle", puzzleForSides[2]);
        PlayerPrefs.SetInt("Side4Puzzle", puzzleForSides[3]);
        PlayerPrefs.SetInt("Side5Puzzle", puzzleForSides[4]);
        PlayerPrefs.SetInt("Side6Puzzle", puzzleForSides[5]);

        // set unlocked particles
        PlayerPrefs.SetInt("UnlockedParticles", unlockedParticles);

        // save lights on
        //Debug.Log(lightsOn);
        PlayerPrefs.SetInt("LightsOn", lightsOn);
        //PlayerPrefs.SetInt("LightsOn2", lightsOn);

        PlayerPrefs.SetInt("TutorialStep", tutorialStep);

        PlayerPrefs.SetInt("GameDone", gameDone);

        PlayerPrefs.SetInt("CanDoEndParticles", canDoEndParticles);
    }

    static public void Load() // I wonder if load should take in some things? like game controller and stuff just to make typing easier?
    {
        //Debug.Log("Load game");

        GameController gc = GameController.instance;

        // load unlocked sides
        unlockedSides = PlayerPrefs.GetInt("UnlockedSides");
        //Debug.Log("unlocked sides " + unlockedSides);
        for (int i = 0; i < PlayerPrefs.GetInt("UnlockedSides"); i++)
        {
            gc.sides[i].projection.unlocked = true;
            gc.sides[i].projection.SetStateTo(1);
            gc.sides[i].projection.SetIdle(false);
            gc.sides[i].projection.SetProgress(0);

            //gc.sides[i].projection.Unlock();
        }

        // load the dialogue section
        dialogueSection = PlayerPrefs.GetString("DialogueSection");
        gc.cb.StartWithSection(PlayerPrefs.GetString("DialogueSection"));

        // load the sound settings
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        gc.SetMusicVol(musicVolume);
        gc.SetSFXVol(sfxVolume);

        // load added papers
        addedPapers = PlayerPrefs.GetInt("AddedPapers");
        if (dialogueSection == "tutorial2") addedPapers = 0;
        if (dialogueSection == "side1") addedPapers = 1;
        if (dialogueSection == "side2") addedPapers = 2;
        if (dialogueSection == "side3") addedPapers = 3;
        if (dialogueSection == "side4") addedPapers = 4;
        if (dialogueSection == "side5") addedPapers = 5;
        if (dialogueSection == "side6") addedPapers = 6; // side 6 is AFTER we oslve the 6th aka ALL sides // we may need ot come back later and add to this
        for (int i = 0; i < addedPapers; i++)
        {
            gc.corkboard.papersList[i].SetActive(true);
        }

        // load added machines
        addedMachines = PlayerPrefs.GetInt("AddedMachines");
        for (int i = 0; i < addedMachines; i++)
        {
            gc.machines[i].SetActive(true);
        }

        // load tesseract size for anim purposes
        tesseractSize = PlayerPrefs.GetFloat("TesseractSize"); // if the tesseract size is 1, we also need to turn on the particles
        //Debug.Log("load with a size of " + tesseractSize);
        if (tesseractSize != 0 && gameDone == 0)
        {
            gc.tesseract.animator.SetTrigger("ScaleUp");
        }
        //gc.tesseract.animator.enabled = true; // we have to set the size FIRST before the animator turns on
        //Debug.Log(gc.tesseract.animator.transform.localScale);

        // load active puzzles for each side
        puzzleForSides = new int[] { PlayerPrefs.GetInt("Side1Puzzle"), PlayerPrefs.GetInt("Side2Puzzle"), PlayerPrefs.GetInt("Side3Puzzle"), PlayerPrefs.GetInt("Side4Puzzle"), PlayerPrefs.GetInt("Side5Puzzle"), PlayerPrefs.GetInt("Side6Puzzle") };
        //puzzleForSides = new int[] { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < 6; i++)
        {
            if (gc.sides[i].puzzles.Count > puzzleForSides[i]) gc.sides[i].puzzles[puzzleForSides[i]].gameObject.SetActive(true);
            else
            {
                gc.sides[i].intel.SetActive(true);
                gc.sides[i].done = true;
            }
        }
        

        // load unlocked particles
        unlockedParticles = PlayerPrefs.GetInt("UnlockedParticles");
        gc.unlockedParticles = unlockedParticles;

        // load lights on
        lightsOn = PlayerPrefs.GetInt("LightsOn");
        //Debug.Log(lightsOn + " light on vlue" + " player pref value is" + PlayerPrefs.GetInt("LightsOn"));
        if (lightsOn == 1)
        {
            //Debug.Log("lights on from load?");
            gc.TurnOnLightsInRoom();
        }
        else gc.TurnOffLightsInRoom();

        // load which step
        tutorialStep = PlayerPrefs.GetInt("TutorialStep");
        if (dialogueSection == "tutorial") tutorialStep = 1;
        if (dialogueSection == "tutorial2") tutorialStep = 4;
        if (dialogueSection == "rotateMachine") tutorialStep = 12;
        if (dialogueSection == "spreadMachine") tutorialStep = 14;
        gc.tutorialArrows[tutorialStep].SetActive(true); // the last step is a none again so this is fine

        gameDone = PlayerPrefs.GetInt("GameDone");
        // if game is done, then turn on the final stuff in the room, set the end particles to scale 1 and on, and don't let the tesseract grow
        if (gameDone == 1)
        {
            gc.finalRoomMess.SetActive(true);
            gc.tesseract.spaceExplosion.gameObject.SetActive(true);
            gc.tesseract.spaceExplosion.transform.localScale = Vector3.one * .4f;
        }

        canDoEndParticles = PlayerPrefs.GetInt("CanDoEndParticles");
        if (canDoEndParticles == 1) gc.allSidesCompleted = true;
    }

    static public void ResetGame()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("resetting game");
        // reset number of unlocked sides
        unlockedSides = 0;

        // reset the dialogue section
        dialogueSection = "Intro";

        // reset sound stats
        musicVolume = .8f;
        sfxVolume = .8f;

        // reset added papers
        addedPapers = 0;

        // reset added machines
        addedMachines = 0;

        // reset tesseract sizes
        tesseractSize = 0;

        // reset which puzzles are showing
        puzzleForSides = new int[] { 0, 0, 0, 0, 0, 0 };

        // reset unlocked particles
        unlockedParticles = 2;

        // reset lights on
        lightsOn = 0;

        // rset tut
        tutorialStep = 0;

        gameDone = 0;

        canDoEndParticles = 0;

        Save();
    }

    static public void IncreaseUnlockedSides()
    {
        unlockedSides++;
        if (unlockedSides > 6) Debug.LogError("Too many sides have been unlocked");
        Save();
    }

    static public void IncreaseAddedPapers()
    {
        Debug.Log("increasing papers " + (addedPapers + 1));
        addedPapers++;
        Save();
    }

    static public void IncreaseAddedMachines()
    {
        addedMachines++;
        Save();
    }

    static public void IncreaseUnlockedParticles()
    {
        unlockedParticles++;
        Save();
    }

    static public void UpdateCurrentDialogueSection(string section)
    {
        //Debug.Log("updating dialogue with" + section);
        dialogueSection = section;
        Save();
    }

    static public void ChangeMusicVolume(float volume)
    {
        Debug.Log("changing music " + volume);
        musicVolume = volume;
        Save();
    }

    static public void ChangeSFXVolume(float volume)
    {
        Debug.Log("changing sfx " + volume);
        sfxVolume = volume;
        Save();
    }

    static public void UpdateTesseractSize(float size)
    {
        //Debug.Log("changing size to " + size);
        tesseractSize = size;
        Save();
    }

    static public void IncreasePuzzleForSide(int side) // since it starts with 0, then each time we CHANGE to a new puzzle we can do this
    {
        puzzleForSides[side]++;
        Save();
    }

    static public void UpdateTurnOnLights(bool on)
    {
        if (on) lightsOn = 1;
        else lightsOn = 0;
        Save();
    }

    static public void IncreaseTutorialStep()
    {
        tutorialStep++;
        Debug.Log("updated tut step is " + tutorialStep);
        Save();
    }

    static public void UpdateGameCompletion(int done) // 0 for no 1 for yes
    {
        gameDone = 1;
        Save();
    }

    static public void UpdateCanSendEndParticle(int yes)
    {
        canDoEndParticles = yes;
        Save();
    }
}
