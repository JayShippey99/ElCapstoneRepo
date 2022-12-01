using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsDialogue : MonoBehaviour
{
    // I need a way to know if each thing has been done before or not

    public CommsBrain cb;
    public void WarnHowClose()
    {
        cb.SendMessage("BE VERY CAREFUL ____! We have never seen the Tesseract get this close before. We're not sure what it means.");
    }
}
