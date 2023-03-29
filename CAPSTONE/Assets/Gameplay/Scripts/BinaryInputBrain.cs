using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BinaryInputBrain : InteractableParent
{
    public _Light light;

    [HideInInspector]
    public Image img;

    public Sprite[] particles;
    [HideInInspector]
    public int counter;

    [HideInInspector]
    public bool showing;
    
    void Start()
    {
        img = GetComponent<Image>();
        Hide();
    }

    public override void DoSomethingButton(GameObject theButton)
    {
        counter++;
        if (counter >= particles.Length) counter = 0;

        //print(particles.Length + " " + particles[counter]);
        img.sprite = particles[counter];
    }

    public void Clear()
    {
        counter = 0;
        img.sprite = particles[0];
    }

    public void Show() // What we'll do is just send in which image from the input controller
    {
        showing = true;
        light.SetLight(true);
        img.enabled = true;
    }

    public void ResetIt()
    {
        Clear();
        Show();
    }

    public void Hide()
    {
        showing = false;
        light.SetLight(false);
        img.enabled = false;
    }
}
