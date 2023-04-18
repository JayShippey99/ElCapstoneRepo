using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera instance;

    Vector3 startLook;
    bool look;
    public float lookSpeed;
    float lookTime;

    Vector3 targetLook;

    bool shaking;
    float shakeTime, stayShakingTime, shakeIntensity;
    float shakeCounter;

    Vector3 startLocation;
    Quaternion startRotation;

    Quaternion shakeRotation = new Quaternion();
    Quaternion lookRotation = new Quaternion();

    public AnimationCurve shakeCurve;

    public float testIntensity, testTime, testStayTime;

    public GameObject lookCork, lookBack;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); // okay its a little psychotic cause this would remove the camera but oh well it still works

        startLocation = transform.position;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        //print(transform.rotation);



        if (look)
        {
            if (lookTime < 1) 
            { 
                lookTime += Time.deltaTime * lookSpeed;

                lookRotation = Quaternion.Slerp(Quaternion.Euler(startLook), Quaternion.Euler(targetLook), lookTime);
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

                //transform.position = startLocation + randomOffset * shakeCurve.Evaluate(shakeCounter) * shakeIntensity;

                shakeRotation = Quaternion.Euler((randomOffset * shakeCurve.Evaluate(shakeCounter) * shakeIntensity)); // i need the rotation to go -1 to 1 in the x and y * shakeIntensity
                // so wait if I want the player to be forced to look at the tesseract, how do I rotate with that in mind?
                // yeah this won't work

                //print("shaking " + randomOffset + " " + shakeCurve.Evaluate(shakeCounter) + " " + shakeIntensity);
            }
            else
            {
                shaking = false;
            }
        }

        // so, if camera is not looking anywhere and not shaking, the rotation doesn't get touched, 

        // when there is a cutscene, set the cork look to true and the other one to false
        
        if (look && shaking)
        {
            transform.rotation = lookRotation * shakeRotation;
        }
        else if (shaking)
        {
            transform.rotation = shakeRotation;
        }
        else if (look)
        {
            transform.rotation = lookRotation;
        }


        //Quaternion newRotation = Quaternion.Euler(lookRotation + shakeRotation);

        //transform.rotation = newRotation;

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("Space");
            ShakeCamera(testIntensity, testTime, testStayTime);
        }
        */

        // i want to take the camera's current rotation and add a shake rotation to it. but this only works since look rotation would be hard setting the rotation again
        // i need a way to hard set rotation again if you're not moving the camera, so if not looking, then set rotation back to normal first? or just 
    }

    public void LookThisWay(float yRot)
    {
        if (look == false && GameController.instance.cutscene == false)
        {
            if (yRot >= 0)
            {
                lookCork.SetActive(true);
                lookBack.SetActive(false);
            }
            else
            {
                lookCork.SetActive(false);
                lookBack.SetActive(true);
            }

            //print("how often is this running");
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
