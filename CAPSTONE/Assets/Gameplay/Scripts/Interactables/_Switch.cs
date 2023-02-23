using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Switch : MonoBehaviour
{
    // Start is called before the first frame update
    public InteractableParent[] objs; // I'm gonna add multiple parents

    bool isOn;

    public GameObject flipper;

    public float speed;
    float progress = 0;

    public AnimationCurve ac;

    private void Update()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        if (isOn)
        {
            if (progress < 1)
            {
                progress += Time.deltaTime * speed;
            }
        }
        else
        {
            if (progress > 0)
            {
                progress -= Time.deltaTime * speed;
            }
        }

        flipper.transform.rotation = Quaternion.Euler(rot.x + ac.Evaluate(progress), rot.y, rot.z);
    }

    private void OnMouseUpAsButton()
    {
        //print("clicking");

        isOn = !isOn;


        foreach (InteractableParent ip in objs)
        {
            ip.SetSwitch(isOn);
        }
    }

    // okay well actually, how do we wanna do this? does this break things? basically, where do we run the function to turn things on and off, like, can we call stuff from a script thats deactivated?
}
