using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Light : InteractableParent
{
    [Tooltip("Time it stays on, after time, it turns off. If 0 or less, then it acts as a toggle")]
    public float onTime;

    [Tooltip("How many times does the light blink. If desired effect is a toggle, set the on time to 0")]
    [Range(1, 5)] // kinda jank but it works
    public int blinkAmount;

    [Tooltip("If there is more than one blink, how much time passes between those blinks")]
    public float offTime;

    [Tooltip("Color of the actual light surface")]
    public Color physicalColor;

    [Tooltip("Color of the emitted light")]
    public Color lightColor;

    //public bool startOn;

    float toggleTimer; // time before it toggles again
    float timer; // since I can't progress the time in the middle of a frame I gotta count it this way
    int blinkCounter;

    // get ref to material so that it can change individually
    MeshRenderer lMat;

    public void Awake()
    {
        lMat = GetComponent<MeshRenderer>();

        lMat.material.SetColor("_PhysicalLightColor", physicalColor);
        lMat.material.SetColor("_EmissiveLightColor", lightColor);
        //if (startOn) TurnOn();
    }

    // maybe I can give it some stats like the knob,
    // it'll have the color of the light, the color of the emissive, the length that it turns on for (if 0, then function as a toggle), how many times it blinks if it has a on and off rate and also the time between blinks
    
    private void Update()
    {
        //print(onTime + " " + blinkCounter + " "  +toggleTimer);

        if (onTime > 0 && blinkCounter < blinkAmount && toggleTimer > timer) // we actually do need this last condition because we need a way to trigger it on a command since bc will start at 0
        {
            timer += Time.deltaTime;
            
            //print("running updates!" + onTime + " " + blinkCounter + " " + toggleTimer);

            // ohh okay I think I get it, time can't increase in the middle of a frame so the first condition will always be off before it can trigger again. no biggie, need to use deltatime

            if (timer > toggleTimer)
            {
                //print("running this part too");
                Toggle(); // after the toggle, but it should still add up though

                if (IsOn()) // if its on,
                {
                    toggleTimer += onTime;
                }
                else
                {
                    blinkCounter++;
                    toggleTimer += offTime;
                }
            }
        }
    }
   

    public override void DoSomethingButton(GameObject theButton) // this is what happen if a button is hooked up to it, we need a function for each thing that could be hooked up to it
    {
        Toggle();
    }

    // but the main issue was that i can't have this going every frame
    // 
    public void SetLight(bool on)
    {
        if (on)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    // trigger light will happen from only a button? what? hm
    public override void TriggerLight() // trigger light because it could just be as simple as turning on, but it could also be like a double blink // do i even need this as an override? yes for the quick function running stuff
    {
        // i want trigger light to be something that just gets called
        // and depending on the light settings it will react in different ways
        // so far I got it set up right for blinking I believe

        if (onTime <= 0) // If its a toggle light
        {
            Toggle();
        }
        else // otherwise
        {
            //print("not a toggle light");
            TurnOn(); // start the blink cycle. turn it on and reset all variables
            blinkCounter = 0;
            timer = 0;
            toggleTimer = onTime; // when the time reaches the time + the amount of time it should be on
        }
    }

    void Toggle()
    {
        if (IsOn())
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
    void TurnOff()
    {
        lMat.material.SetInt("_On", 0);
    }

    void TurnOn()
    {
        lMat.material.SetInt("_On", 1);
    }


    bool IsOn()
    {
        int i = lMat.material.GetInt("_On");
        return i == 1 ? true : false; // if 1, true, if not 1, false
    }
}
