using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class YourDialogueBrain : MonoBehaviour
{
    // i feel like this guy needs a reference to the comms brain so that all the other text reacts too

    public Animator anim;
    public TextMeshProUGUI text;
    [HideInInspector]
    public string exitCode;
    [HideInInspector]
    public string gameCode;
    [HideInInspector]
    public CommsBrain cb;
    [HideInInspector]
    public bool sent;

    public void Exit()
    {
        if (!sent)
        {
            print("exit you");
            anim.SetTrigger("Exit");
        }
    }

    public void Send()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/YourDialogue");

        sent = true;
        print("send");
        anim.SetTrigger("Send");
        // here we have to send a notification back up to make everything else exit?
        // yeah this just means // OH WAIT this is also important for triggering which dialogue section to run next
        // how do we use exit code now?
        print("this is exitCode " + exitCode);
        cb.EndDialogueChunk(exitCode);
        GameController.instance.RunGameEvent(gameCode); // activate gameEvent?
    }

    public void KillSelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
