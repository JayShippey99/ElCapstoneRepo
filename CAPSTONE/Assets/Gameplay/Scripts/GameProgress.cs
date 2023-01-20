using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    // This class will save which puzzles have been solved, if you're starting a new game, if you've finished the game, etc etc
    // it'll also save and load all this stuff

    static public GameProgress instance;

    int newGame = 0, middleGame = 1, endGame = 2; // end game is when the end sequence plays // can't get rid of this script, could be good to reference
    public int currentGameState;

    bool[] puzzles = new bool[2];

    public void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PuzzleSolved(int n)
    {
        puzzles[n] = true;
        SaveGame();

        // check if all puzzles are solved
        if (AreAllPuzzlesSolved()) CommsDialogue.instance.Outro();
    }

    bool AreAllPuzzlesSolved()
    {
        foreach (bool b in puzzles)
        {
            if (b == false) return false;
        }
        
        return true;
    }

    void SaveGame()
    {
        // this will trigger when we finish a puzzle
    }

    void LoadGame()
    {

    }

    public void ResetGame()
    {
        currentGameState = 0;
        CommsDialogue.instance.Introduction();
    }
}
