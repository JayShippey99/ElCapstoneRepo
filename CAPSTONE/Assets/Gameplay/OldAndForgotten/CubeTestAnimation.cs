using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTestAnimation : MonoBehaviour
{
    // Okay this is really awesome that the animations do blend together just based on their first keyframes

    // Another thing to know is that in a blend tree if all animations have the same threshold then they all run at the same time

    public Animator anim;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("Left");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetTrigger("Normal");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Right");
        }
    }
}
