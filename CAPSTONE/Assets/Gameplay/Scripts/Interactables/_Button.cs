using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class _Button : MonoBehaviour
{
    public InteractableParent[] objs;

    public Animator anim;
    private void OnMouseDown()
    {
        if (GameController.instance.cutscene == false)
        {
            foreach (InteractableParent ip in objs)
            {
                ip.ToggleSomethingButton(ip.gameObject);
                ip.DoSomethingButton(this.gameObject);


            }

            RuntimeManager.PlayOneShot("event:/ButtonPress");

            //print("button");
            anim.SetTrigger("Press");

        }
    }
    }
