using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableParent : MonoBehaviour
{

    // kinda hacky script, works though

    public virtual void ToggleSomethingButton(GameObject obj) // swithc wont be the only thing to toggle
    {

    }

    public virtual void ToggleSomethingSwitch(GameObject obj)
    {
        //obj.SetActive(!obj.activeInHierarchy); // this might lag things though weirdly though // weirdly I want to leave this empty? I think I'm gonna always want to override these functions
        // I guess i could have here code that flips the 
    }

    public virtual void DoSomethingButton()
    {

    }


    public virtual void ChangeSomethingDial(float f)
    {

    }
}
