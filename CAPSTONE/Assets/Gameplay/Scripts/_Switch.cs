using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Switch : MonoBehaviour
{
    // Start is called before the first frame update
    public InteractableParent[] objs; // I'm gonna add multiple parents

    private void OnMouseDown() // I wonder if I could use delegates again to just say run whatever function
    {
        foreach(InteractableParent ip in objs)
        {
            ip.ToggleSomethingSwitch(ip.gameObject);
        }
    }

    // okay well actually, how do we wanna do this? does this break things? basically, where do we run the function to turn things on and off, like, can we call stuff from a script thats deactivated?
}
