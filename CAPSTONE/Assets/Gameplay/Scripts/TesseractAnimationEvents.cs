using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesseractAnimationEvents : MonoBehaviour
{
    public void EndIntroSeq()
    {
        GameController.instance.EndIntroSeq();
    }

    public void ShowEndScreen()
    {
        GameController.instance.ShowEndScreen();
    }

    public void TurnOnLightsInRoom()
    {
        GameController.instance.TurnOnLightsInRoom();
    }

    public void SetCubeBrightness(float brightness) // maybe I can set up a function so that i have a start and an end number and maybe the time to make that change?
    {
        brightness = Mathf.Clamp(brightness, 0, .65f);
        Tesseract.instance.mat.SetFloat("_FlashAmount", brightness);
    }

    public void RemoveBrightness(float time)
    {
        Tesseract.instance.RemoveBrightness(time);
    }
}
