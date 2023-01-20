using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Button : MonoBehaviour
{
    public InteractableParent obj;

    private void OnMouseDown()
    {
        obj.ToggleSomethingButton(obj.gameObject);

        obj.DoSomethingButton();
    }
}
