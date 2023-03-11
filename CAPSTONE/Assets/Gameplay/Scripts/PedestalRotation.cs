using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalRotation : InteractableParent
{
    // so we need an overrid function for getting dial input
    // the code will be a lot like the tesseract
    // it'll always be turning, but will go back to normal as the dial does
    // i need the override function
    // that function needs to turn the dials numbers into a rotation speed
    // I just want these scripts to the dials accordingly, so if the value is positive it'll rotate in the position direction
    // also, I need a rotation speed variable
    // so, dials aren't moving, pedestal isn't moving either
    // when the dials are chgned, we run the override, the override gets the numbers and does math accordingly so that it knows what speed to rotate at, in the update funciton it'll just run
    // a rotate function rotate will get the speed and add onto the current rotation
    // the math will be like assuming that its the -50 to -10 dial, equal it to -20 and 20, and then divide that by 20 and then multiply it by the rotation speed
    // I think I can just go at this one without planning it too much

    public float rotationSpeed;
    float spinAmount;

    public AnimationCurve ac;

    // Update is called once per frame
    void Update()
    {
        Spin();
    }
    
    void Spin()
    {
        transform.RotateAroundLocal(Vector3.up, spinAmount * Time.deltaTime);
    }

    public override void ChangeSomethingDial(float f)
    {
        if (f < 0)
        {
            spinAmount = (f + 30) / 20 * rotationSpeed;
        }

        if (f > 0)
        {
            spinAmount = (f - 30) / 20 * rotationSpeed;
        }
    }

    // I might need to make an animtion for the intro rotaton, butuse animation curve and just trigger that to go insted I mean
    public void IntroAnim()
    {
        // we need a way to change speed easily here too
    }
}
