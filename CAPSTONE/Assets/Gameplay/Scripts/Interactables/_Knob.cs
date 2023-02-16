using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class _Knob : InteractableParent
{
    // im getting lost in these numbers

    // hm okay this is a little weird, how do I make it so tht when I let go I check if the signal is at a good point.
    // do I access the scan object's variables?
    // or would it be freq scan class?
    // I guess if I want to change specific variables would I need to get a class reference?

    public InteractableParent[] objs;

    [Tooltip("A percent, 0 is left, 1 is right")]
    public float startAngle; // start angle will be clamped within turn amount later, but it will be in degrees, 0 is up

    [Tooltip("Left right rotation allowed, in degrees, 0 is up")]
    public float turnAmount;

    [Tooltip("1 is very fast")]
    public float turnSpeed; // this needs to be in terms of accurte rotations, will screen size matter?? well no, cause I'd be shocked if I couldn't go from one end to the other, but one screen might have it happen quicker

    [Tooltip("When dial is farthest left")]
    public float minValue; // how should this be calculated

    [Tooltip("When dial is farthest right")]
    public float maxValue;


    bool isHeld;
    float initialHoldOffset;
    Quaternion originalRotation;

    // how do I want to tackle this. I have code that rotates each frame, I instead want code that knows how far thigns are from the origin and uses that to place the rotatoin

    // for how far to one side or the other I'll add onto the initial setting
    // doesn't matter even if I let go, I should just save how far its been turned to one way or the other
    // maybe the rotation angle dictates things?
    // 

    float totalOffset; // this will continue to be added onto. so like if I hold and grab, its 0 to start, if I move to the right 10 and let go, its now 10, when I grab on again, my offset is I were to drag 10 to the left would be -10 still but i'd add that to 10
    // so in a way is this oldoffset?
    // this is the stupidest way to do this
    float value;

    // now how do I make it so that I can just expose a specific variable from a script to be hooked up to something else
    // wonder if i could make like a specific class for it or something, for now lets keep it simple


    // I NEED TO MAKE IT SO YOU CAN'T CLICK WHILE ITS GOING BACK TO NORMAL

    bool returnToNeutral = false;
    float returnTime;
    public float returnSpeed;
    float startOffset;

    void Start()
    {
        /*
        changeDialFunction = obj.GetType().GetMethod("ChangeSomething");
        letGoFunction = obj.GetType().GetMethod("OnUp");
        */

        //print(changeDialFunction + " this is loading method");

        originalRotation = transform.rotation;

        // x * 180 - 90

        totalOffset = (startAngle * (turnAmount * 2)) - turnAmount; // .5 * 90 is 45, not 0?  0 * 90 = -90,  1 * 90 = 90, .5 * 90 = 0

        // I don't get this lmfao
        // 0 * 90 * 2 = 0 - 90 // is -90, but its at the right side when its like this

        transform.rotation = originalRotation;
        transform.RotateAroundLocal(transform.forward, Mathf.Deg2Rad * totalOffset);

        value = Remap(totalOffset, -turnAmount, turnAmount, minValue, maxValue);

        foreach (InteractableParent ip in objs)
        {
            ip.ChangeSomethingDial(value);
        }

    }

    public static float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    // Update is called once per frame
    void Update()
    {
        //print(returnToNeutral);
        // I need to grab and drag, 0 + 10 drag. I've gone 10 away from the center point
        if (!returnToNeutral)
        {
            //print("am i not returning to normal");
            if (isHeld)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    isHeld = false;

                    //obj.OnUp();
                }
                else
                {
                    Vector2 mp = Input.mousePosition;
                    totalOffset = (mp.x - initialHoldOffset) * turnSpeed;
                    totalOffset = Mathf.Clamp(totalOffset, -turnAmount, turnAmount);


                    transform.rotation = originalRotation;
                    transform.RotateAroundLocal(transform.forward, Mathf.Deg2Rad * totalOffset);

                    //value = 
                    value = Remap(totalOffset, -turnAmount, turnAmount, minValue, maxValue);

                    foreach (InteractableParent ip in objs)
                    {
                        ip.ChangeSomethingDial(value);
                    }
                }
            }
        }
        else
        {
            //print("or am i returning to normal");
            if (returnTime < 1)
            {
                returnTime += Time.deltaTime * returnSpeed;

                totalOffset = Mathf.Lerp(startOffset, (startAngle * (turnAmount * 2)) - turnAmount, returnTime);

                    //totalOffset = (startAngle * (turnAmount * 2)) - turnAmount;

                totalOffset = Mathf.Clamp(totalOffset, -turnAmount, turnAmount);


                transform.rotation = originalRotation;
                transform.RotateAroundLocal(transform.forward, Mathf.Deg2Rad * totalOffset);

                //value = 
                value = Remap(totalOffset, -turnAmount, turnAmount, minValue, maxValue);

                foreach (InteractableParent ip in objs)
                {
                    ip.ChangeSomethingDial(value);
                }
            }
            else returnToNeutral = false;
        }
    }


    public override void DoSomethingButton(GameObject theButton) // LMFAO sick
    {
        //print("does this get run for some reason?");
        returnToNeutral = true;
        returnTime = 0;
        startOffset = totalOffset;
    }

    private void OnMouseDown()
    {
        if (!returnToNeutral)
        {
            isHeld = true;
            initialHoldOffset = Input.mousePosition.x - (totalOffset / turnSpeed); // - totalOffset // the inital hold offset always puts me back at the middle // okay my hold offset is the mouse position, and then in the function I'm getting the difference btween mouse position and mouse position which is 0 until I move again
        }
    }
}
