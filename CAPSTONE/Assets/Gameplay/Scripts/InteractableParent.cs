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

    // I don't need light specific things you dummy the light will just be the light, SO we don't use light specific ones but we keep the parent child scripts and use a different function

    public virtual void ChangeSomethingDial(float f)
    {

    }

    public virtual void ToggleLight()
    {

    }

    public virtual void TriggerLight()
    {

    }
}
