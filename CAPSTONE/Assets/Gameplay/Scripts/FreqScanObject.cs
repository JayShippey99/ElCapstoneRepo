using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer))]
public class FreqScanObject : InteractableParent // has two funcitons, onup, and changesomething
{
    // okay so what i need to do here is make events or something, like I can't just link up all the specific functions directly to this cause then it doesn't work versitilley (spelling champ)
    // I need to just be able to read the behavior of the knob from a script
    // maybe I can add a script for knob reader and then inherit that here, and it just reads values and frames and stuff?

    // I need to make a system where I can take a knob and give it an object (or script) and have that knob affect specific variables in that script
    // for the light switch, I just run a toggle function which takes the game object and toggle's its activeness
    // for the dial I need to get a script and chnge a variable in it
    // maybe this does work backwards where I create the knob but have the object look at it?
    // I wonder if I can assign events or functions in the inspector

    public static FreqScanObject instance;

    MeshRenderer mr;

    public FreqScanClass[] signals;

    float scanFrequency; // 

    public bool isHighlightingSomething;

    int currentSignal = 0;

    public Transform documentList;

    public GameObject intelPrefab;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        mr = GetComponent<MeshRenderer>();

        mr.material.SetTexture("_Layer1Tex", signals[0].layer1Tex.texture);
        mr.material.SetTexture("_Layer2Tex", signals[0].layer2Tex.texture);
        mr.material.SetTexture("_Layer3Tex", signals[0].layer3Tex.texture);

        mr.material.SetFloat("_Layer1Level", signals[0].layer1Level);
        mr.material.SetFloat("_Layer2Level", signals[0].layer2Level);
        mr.material.SetFloat("_Layer3Level", signals[0].layer3Level);
    }
    public void ChangeSignal(int i)
    {
        currentSignal = i;

        mr.material.SetTexture("_Layer1Tex", signals[i].layer1Tex.texture);
        mr.material.SetTexture("_Layer2Tex", signals[i].layer2Tex.texture);
        mr.material.SetTexture("_Layer3Tex", signals[i].layer3Tex.texture);

        mr.material.SetFloat("_Layer1Level", signals[i].layer1Level);
        mr.material.SetFloat("_Layer2Level", signals[i].layer2Level);
        mr.material.SetFloat("_Layer3Level", signals[i].layer3Level);
    }

    public override void ChangeSomethingDial(float f) // I need to make it so that when I let go, if a thing is on an image, 
    {
        // so amazingly this shit runs, so that's awesome but idk if its sending the value properly
        scanFrequency = f;
        mr.material.SetFloat("_ScanAmount", scanFrequency);
    }

    public override void ToggleSomethingSwitch(GameObject obj) // it works lets goooo
    {
        obj.SetActive(!obj.activeInHierarchy);
    }

    public override void DoSomethingButton()
    {
        //print("I LET GO");
        // if scanfrequency is 

        int i = currentSignal;

        float freq = Mathf.Round(scanFrequency * 10.0f) * 0.1f; // rounds to .x

        float layer1 = Mathf.Round(signals[i].layer1Level * 10.0f) * 0.1f;
        float layer2 = Mathf.Round(signals[i].layer2Level * 10.0f) * 0.1f;
        float layer3 = Mathf.Round(signals[i].layer3Level * 10.0f) * 0.1f;

        print(freq + " " + layer1 + " " + layer2 + " " + layer3);
        //print(freq); // wait no lmfao rounding like this isn't good

        if (freq == layer1) // is this even what I want? yeah it is I guess, whatever image I'm on, I gotta send that, and I can have many images at different places
        {
            if (!signals[i].layer1Added)
            {
                print("1");
                //document.sprite = signals[i].layer1Tex; // ayo? // niceee so instead now if changing the material I need to add a new thing. lemme just scrap the tablet as it and start raw
                AddImage(signals[i].layer1Tex);
                // I could add a bool for each image
                signals[i].layer1Added = true;
            }
        }

        if (freq == layer2)
        {
            if (!signals[i].layer2Added)
            {
                print("2");
                //document.sprite = signals[i].layer2Tex;
                AddImage(signals[i].layer2Tex);
                signals[i].layer2Added = true;
            }
        }

        if (freq == layer3)
        {
            if (!signals[i].layer3Added)
            {
                print("3");
                //document.sprite = signals[i].layer3Tex;
                AddImage(signals[i].layer3Tex);
                signals[i].layer3Added = true;
            }
        }
    }

    // instead of just lifting off

    public void AddImage(Sprite image) // we need to check if its already been added I'm realizing // how would I do that?
    {
        GameObject newDocument = Instantiate(intelPrefab, documentList);

        if (newDocument.TryGetComponent<DocumentItem>(out DocumentItem item)) // if it exists, name it item and do stuff with it, super sick
        {
            item.SetImage(image);
        }
    }

}

