using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnableDisable : InteractableParent
{
    public override void ToggleSomethingButton(GameObject obj)
    {
        obj.SetActive(!obj.activeInHierarchy);
    }
}
