using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Button : MonoBehaviour
{
    public InteractableParent[] objs;

    private void OnMouseDown()
    {
        foreach(InteractableParent ip in objs)
        {
            ip.ToggleSomethingButton(ip.gameObject);
            ip.DoSomethingButton();
        }
    }
}
