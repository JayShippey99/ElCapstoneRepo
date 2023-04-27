using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlink : MonoBehaviour
{
    public void AddMachine()
    {
        GameController.instance.AddMachine();
    }

    public void GoBackIntoRoom()
    {
        // start a long ass coroutine that just lasts for the seconds until the song ends
        // what else needs to happen here? well, we can add the paper to the desk
        GameController.instance.RoomFinalState();
        // we also have to.. wait

        // we need to trigger the tesseract animation too right?
    }

    public void TesseractLeave()
    {
        Tesseract.instance.TesseractLeave();
    }

    public void PlayEndSong()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/EndMusic");
    }
}
