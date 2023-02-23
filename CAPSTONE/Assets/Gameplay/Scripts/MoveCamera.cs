using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    Vector3 startLook;
    bool look;
    public float lookSpeed;
    float lookTime;

    Vector3 targetLook;

    bool shaking;
    float shakeTime, stayShakingTime, shakeIntensity;
    float shakeCounter;

    Vector3 startLocation;

    public AnimationCurve shakeCurve;

    public float testIntensity, testTime, testStayTime;

    private void Start()
    {
        startLocation = transform.position;
    }

    private void Update()
    {
        if (look)
        {
            if (lookTime < 1) 
            { 
                lookTime += Time.deltaTime * lookSpeed;

                transform.rotation = Quaternion.Slerp(Quaternion.Euler(startLook), Quaternion.Euler(targetLook), lookTime);
            }
            else
            {
                look = false;
            }
        }

        if (shaking)
        {
            if (shakeCounter < 1)
            {
                if (shakeCounter > .5 && stayShakingTime > 0) // holy shit this went so well imma fuckin genius
                {
                    stayShakingTime -= Time.deltaTime;
                }
                else
                {
                    shakeCounter += Time.deltaTime * shakeTime; // if I'm working with an animation curve, maybe this isn't the best idea. we might need like a speed variable?
                }

                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

                transform.position = startLocation + randomOffset * shakeCurve.Evaluate(shakeCounter) * shakeIntensity;

                //print("shaking " + randomOffset + " " + shakeCurve.Evaluate(shakeCounter) + " " + shakeIntensity);
            }
            else
            {
                shaking = false;
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("Space");
            ShakeCamera(testIntensity, testTime, testStayTime);
        }
        */
    }

    public void LookThisWay(float yRot)
    {
        if (look == false)
        {
            lookTime = 0;
            look = true;
            startLook = transform.rotation.eulerAngles;
            targetLook = new Vector3(0, yRot, 0);
        }
    }

    public void ShakeCamera(float intensity, float time, float holdTime) // holdTime will be used to say when it reaches the max intensity, how long will it stay like that before it goes down again
    {
        // if for some reason we have two things shaking at once, we want the last one to get its full runtime

        //print("Shake");

        shakeTime = time;
        shakeIntensity = intensity;
        stayShakingTime = holdTime;

        shaking = true;
        shakeCounter = 0;
    }
}
