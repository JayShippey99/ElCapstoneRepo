using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnableDisable : InteractableParent
{
    public override void ToggleSomethingSwitch(GameObject obj)
    {
        obj.SetActive(!obj.activeInHierarchy);
    }
}
