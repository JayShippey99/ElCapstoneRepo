using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesseractAnimationEvents : MonoBehaviour
{
    // well actaully the last thing I reall yneed to do is to have the credits roll... i wonder if I culd just include that in here too?

    public void RollCredits()
    {
        GameController.instance.ShowEndCredits();
    }

    public void SaveGame() // this runs within the animation
    {
        SaveAndLoadGame.UpdateGameCompletion(1);
    }

    public void SuckUp() // turn on stars to generate some and then start the gradient suck
    {
        Tesseract.instance.spaceExplosion.gameObject.SetActive(true);
        EndPuzzleIntensity.instance.Explode();
    }

    public void ExplodeStars()
    {
        Tesseract.instance.ExplodeStars();
    }

    public void EndIntroSeq()
    {
        GameController.instance.EndIntroSeq();
    }

    public void ShowEndScreen()
    {
        GameController.instance.ShowEndCredits();
    }

    public void TurnOnLightsInRoom()
    {
        GameController.instance.TurnOnLightsInRoom();
    }

    public void SetCubeBrightness(float brightness) // maybe I can set up a function so that i have a start and an end number and maybe the time to make that change?
    {
        brightness = Mathf.Clamp(brightness, 0, 1); // .65
        Tesseract.instance.mat.SetFloat("_FlashAmount", brightness);
    }

    public void RemoveBrightness(float time)
    {
        Tesseract.instance.RemoveBrightness(time);
    }
}
