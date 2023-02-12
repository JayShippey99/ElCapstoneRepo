using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Tesseract : InteractableParent
{
    // okay I'm trying to get it so that the progress shows up and adds to the grid for this puzzle too

    // I want to come up with a good system for going from puzzle to puzzle
    // Maybe I can make puzzle paths as lists or something
    // so that you activate the one, and then it goes into the chain and then from there once one is solved it just adds onto that num

    static public Tesseract instance;

    [HideInInspector]
    public Animator animator;

    public GameObject projectorScreen, projectorBeam;

    public float speed; // turn speed
    float turnRightAmount, turnUpAmount;
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        TurnRight();
        TurnUp();
    }



    // i need to make it so that it only goes through once all the colliders have a stone in them

    // Update is called once per frame
    public void TurnOnProjectorStuff()
    {
        projectorScreen.SetActive(true);
        projectorBeam.SetActive(true);
    }

    public void TurnOffProjectorStuff()
    {
        projectorScreen.SetActive(false);
        projectorBeam.SetActive(false);
    }

    public void ReturnToHubState()
    {
        //print("remove all puzzles, change animation to floating in place again");
        //TurnOffProjectorStuff();
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    void TurnRight()
    {
        transform.RotateAroundLocal(Vector3.up, turnRightAmount * speed * Time.deltaTime);
    }

    void TurnUp()
    {
        transform.RotateAroundLocal(Vector3.right, turnUpAmount * speed * Time.deltaTime);
    }

    public override void ChangeSomethingDial(float f) // i gotta redo the movement code woof
    {
        if (f < 0)
        {
            turnRightAmount = f + 30;
        }

        if (f > 0)
        {
            turnUpAmount = f - 30;
        }
    }

}
