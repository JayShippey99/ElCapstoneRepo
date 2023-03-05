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
}
