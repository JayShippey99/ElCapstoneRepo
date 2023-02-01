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

    //public FreqScanClass[] signals;
    [HideInInspector]
    public FreqScanClass currentSignal;

    float scanFrequency; // 

    [HideInInspector]
    public bool isHighlightingSomething;

    //int currentSignal = 0;

    public Transform documentList;

    public GameObject intelPrefab;

    public _Light strongSignalLight;
    public _Light signalSentLight;
    public _Light tabletNotDockedLight;
    
    public TabletMovement tablet;

    public float detectionSensitivity;

    public AnimationCurve showNewNoiseCurve;
    float showProgress;

    void Awake()
    {
        
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        
        mr = GetComponent<MeshRenderer>();

        currentSignal = null;

        //print(currentSignal);
        /*
        mr.material.SetTexture("_Layer1Tex", currentSignal.layer1Tex.texture);
        mr.material.SetTexture("_Layer2Tex", currentSignal.layer2Tex.texture);
        mr.material.SetTexture("_Layer3Tex", currentSignal.layer3Tex.texture);

        mr.material.SetFloat("_Layer1Level", currentSignal.layer1Level);
        mr.material.SetFloat("_Layer2Level", currentSignal.layer2Level);
        mr.material.SetFloat("_Layer3Level", currentSignal.layer3Level);
        */
        
    }
    public void ChangeSignal(FreqScanClass newSignal)
    {
        currentSignal = newSignal;

        //print(currentSignal);

        //print(mr);

        // lets just set these things and restart the animstion

        mr.material.SetTexture("_Layer1Tex", currentSignal.layer1Tex.texture);
        mr.material.SetTexture("_Layer2Tex", currentSignal.layer2Tex.texture);
        mr.material.SetTexture("_Layer3Tex", currentSignal.layer3Tex.texture);

        mr.material.SetFloat("_Layer1Level", currentSignal.layer1Level);
        mr.material.SetFloat("_Layer2Level", currentSignal.layer2Level);
        mr.material.SetFloat("_Layer3Level", currentSignal.layer3Level);

        StartCoroutine(ShowSignal());
    }

    IEnumerator ShowSignal()
    {
        float progress = 0;

        while (progress < 1)
        {

            progress += Time.deltaTime;

            mr.material.SetFloat("_OverallVisibility", showNewNoiseCurve.Evaluate(progress));

            yield return null;
        }
    }

    public override void ChangeSomethingDial(float f) // I need to make it so that when I let go, if a thing is on an image, 
    {
        //print(currentSignal);
        if (currentSignal != null)
        {
            // so amazingly this shit runs, so that's awesome but idk if its sending the value properly
            scanFrequency = f;
            mr.material.SetFloat("_ScanAmount", scanFrequency);

            // i need to ask HERE if its on a strong image
            //int i = currentSignal;

            // lets not round lets instead ask if the frequency layer is a distance of .1 or less

            float freq = scanFrequency; // rounds to .x

            float layer1 = currentSignal.layer1Level;
            float layer2 = currentSignal.layer2Level;
            float layer3 = currentSignal.layer3Level;

            //print(freq + " " + layer1 + " " + layer2 + " " + layer3);
            //print(freq); // wait no lmfao rounding like this isn't good

            float layer1Distance = Mathf.Abs(freq - layer1);
            float layer2Distance = Mathf.Abs(freq - layer2);
            float layer3Distance = Mathf.Abs(freq - layer3);

            //print(layer1Distance + " " + layer2Distance + " " + layer3Distance);

            // for the sake of making it clear in the light script. I imagine this may, do I just need a specific to the lights function like set light?

            if (layer1Distance > detectionSensitivity && layer2Distance > detectionSensitivity && layer3Distance > detectionSensitivity) // jesus that was a pain lmao
            {
                strongSignalLight.SetLight(false);
            }
            else
            {
                strongSignalLight.SetLight(true);
            }
        }
    }

    public override void ToggleSomethingSwitch(GameObject obj) // it works lets goooo
    {
        obj.SetActive(!obj.activeInHierarchy);
    }

    public override void DoSomethingButton() // we need to know if the tablet is docked or not
    {
        // in the future we should check if the signal is within a float range instead. that'll make it nice

        // i think we should just ignore EVERYTHing is the tablet isn't docked. now we need a ref which is annoying
        // if any signal is higlighted, then we run this 
        // if the tablet is docked and the button is pressed and there is a strong signal, trigger green light
        // if the tablet is not docked and the button is pressed and there is a strong signl trigger the red light

        //int i = currentSignal;

        if (currentSignal != null)
        {


            float freq = scanFrequency; // rounds to .x

            float layer1 = currentSignal.layer1Level;
            float layer2 = currentSignal.layer2Level;
            float layer3 = currentSignal.layer3Level;

            float layer1Distance = Mathf.Abs(freq - layer1);
            float layer2Distance = Mathf.Abs(freq - layer2);
            float layer3Distance = Mathf.Abs(freq - layer3);

       

            if (layer1Distance < detectionSensitivity) // jesus that was a pain lmao
            {
                // only if the tablet is docked do we worry abotu this
                if (tablet.docked)
                {
                    if (!currentSignal.layer1Added)
                    {
                        //print("1");
                        //document.sprite = signals[i].layer1Tex; // ayo? // niceee so instead now if changing the material I need to add a new thing. lemme just scrap the tablet as it and start raw
                        AddImage(currentSignal.layer1Tex);
                        currentSignal.layer1Added = true;

                        // do green light here, cause it should only be sent once right?
                        signalSentLight.TriggerLight();
                    }
                }
                else
                {
                    // do redlight regardless of whether it was added or not
                    tabletNotDockedLight.TriggerLight();
                }
            }

            if (layer2Distance < detectionSensitivity)
            {
                if (tablet.docked)
                {
                    if (!currentSignal.layer2Added)
                    {
                        //print("2");
                        //document.sprite = signals[i].layer2Tex;
                        AddImage(currentSignal.layer2Tex);
                        currentSignal.layer2Added = true;
                    
                        signalSentLight.TriggerLight();
                    }
                }
                else
                {
                    tabletNotDockedLight.TriggerLight();
                }
            }

            if (layer3Distance < detectionSensitivity)
            {
                if (tablet.docked)
                {
                    if (!currentSignal.layer3Added)
                    {
                        //print("3");
                        //document.sprite = signals[i].layer3Tex;
                        AddImage(currentSignal.layer3Tex);
                        currentSignal.layer3Added = true;

                        signalSentLight.TriggerLight();
                    }
                }
                else
                {
                    tabletNotDockedLight.TriggerLight();
                }
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

