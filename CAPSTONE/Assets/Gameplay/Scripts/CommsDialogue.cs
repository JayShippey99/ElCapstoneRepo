using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsDialogue : MonoBehaviour
{
    // I need a way to know if each thing has been done before or not
    static public CommsDialogue instance;


    public CommsBrain cb;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void WarnHowClose()
    {
        cb.SendMessage("BE VERY CAREFUL ____! We have never seen the Tesseract get this close before. We're not sure what it means.");
    }

    public void Introduction()
    {
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        cb.SendMessage("Hello, welcome to area 51. We're sitting you down today to be in charge of some VERY important business.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("In front of you is... well... its a tesseract and that's all we know really.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("Okay that's not totally true, we've done a lot of testing prior you being here and we've learned quite a lot.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("First thing we've learned is that this tesseract is made up entirely of frequencies, but ones that are so dense they've become visible.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("That being said this tesseract REACTS to frequencies just as well. Sound frequencies, as far as we know.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("Our testing has shown that sending different patterns of signals causes the tesseract to almost... communicate with us.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("We believe that what we have here is the bridge to an alien lifeform. We're hoping you can help us understand.");
    }

    public void Outro()
    {
        StartCoroutine(Out());
    }

    IEnumerator Out()
    {
        cb.SendMessage("Beep boop we are the aliens.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("Congratulations on getting to us.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("This tesseract we sent to Earth was our last attempt at saving our lifeform.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("We are under attack.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("We may have control of the fourth dimension, but that is no match for the lifeform that is attacking us.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("You see, we cannot fight back. We are harmless.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("Humans on the other hand...");
        yield return new WaitForSeconds(6);
        cb.SendMessage("You humans know nothing but anger.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("You know our solar system, our planet, and you now have the knowledge to harness space to get to us.");
        yield return new WaitForSeconds(6);
        cb.SendMessage("Come save us.");
    }
}
