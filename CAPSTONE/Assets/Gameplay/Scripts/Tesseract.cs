using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Tesseract : MonoBehaviour
{
    // okay I'm trying to get it so that the progress shows up and adds to the grid for this puzzle too

    // I want to come up with a good system for going from puzzle to puzzle
    // Maybe I can make puzzle paths as lists or something
    // so that you activate the one, and then it goes into the chain and then from there once one is solved it just adds onto that num

    static public Tesseract instance;

    // get reference to material with script?
    public Material cubeMaterial;

    [HideInInspector]
    public Animator animator;

    public GameObject projectorScreen, projectorBeam;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        animator = GetComponent<Animator>();
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
        TurnOffProjectorStuff();
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    private void OnDisable()
    {        
        Color newColor;

        if (ColorUtility.TryParseHtmlString("#56e5ff", out newColor))
        {
            cubeMaterial.SetColor("_Color", newColor);
        }
    }

    public void Flash() // how do we want to do this, do
    {
        StartCoroutine(FlashFade());
    }

    IEnumerator FlashFade() // What I could do is make it so that I'm not affecting the flash of the whole thing but instead just use an image with the same material on, and make tht flash, but wait the material is universal oops
    {
        float progress = .1f; // i wonder if i can know the upper limit to the slider

        while (progress > 0)
        {
            cubeMaterial.SetFloat("_FlashAmount", progress);

            progress -= Time.deltaTime * .5f;

            yield return null;
        }
    }

    public void ChangeColor(string str)
    {
        string strWithHash = "#" + str;
        Color newColor;

        if (ColorUtility.TryParseHtmlString(strWithHash, out newColor))
        {
            cubeMaterial.SetColor("_Color", newColor);
        }
    } 
    public void MakeSound()
    {
        StartCoroutine(MakeSoundTimer());
    }

    IEnumerator MakeSoundTimer()
    {
        yield return new WaitForSeconds(1);
        Oscillator.instance.On("AAAA");
    }
}
