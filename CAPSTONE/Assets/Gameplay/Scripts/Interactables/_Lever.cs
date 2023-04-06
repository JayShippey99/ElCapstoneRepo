using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Lever : MonoBehaviour
{
    public InteractableParent[] objs;

    // lever doesn't need any fancy dial stats. I think all i'll need is a pull limit, which I'l make in the inspector for easier testing, and then
    [Tooltip("Pixels the mouse needs to move down from its first grabbing point to trigger")]
    public float pullLimit;
    float initialMouse;
    float mousePull; // mouse pull is the  y difference between the mouses initial grab on screen and what it is currently
    // i need to scale this number so that it correlates that a difference of 0 or less is -90 degrees in the z and at the pull limit its angle is 90
    // I'll need a 0 to 1 situaiton if I want to lerp
    // mousePull(clamped) / pull limit will give me a 0 to 1 result, lowkey could also use an animaition curve
    // but, just doing that * 90 is.. only 0 to 90, so , we do that * 180 - 90 and set that to be the z
    // and we set the rotation by doing the initial rotation but changing the z of it

    int normal = 0;
    int grabbing = 1;
    int returningToNormal = 2;
    int currentState;

    Vector3 ogRot;
    Vector3 startRot;

    float backToNormTime;

    public bool puzzleResetLever;

    void Start()
    {
        ogRot = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == grabbing)
        {
            MatchMousePosition();

            if (Input.GetMouseButtonUp(0))
            {
                currentState = returningToNormal;
                SetStartRotForLerp();
            }
        }
        else if (currentState == returningToNormal)
        {
            ReturnToNormal();
        }
    }

    void MatchMousePosition()
    {
        mousePull = initialMouse - Input.mousePosition.y;
        mousePull = Mathf.Clamp(mousePull, 0, pullLimit) / pullLimit;
        // rotate to mouse
        float angleAmount = (mousePull * 180) - 90;
        angleAmount = Mathf.Clamp(angleAmount, -90, 89);
        transform.localRotation = Quaternion.Euler(ogRot.x, ogRot.y, angleAmount);

        // if your mouse triggers the pull
        if (angleAmount >= 89)
        {
            SetStartRotForLerp();
            LeverPulled();
        }
    }

    void LeverPulled()
    {
        if (puzzleResetLever && GameController.instance.currentPuzzle != null)
        {
            GameController.instance.currentPuzzle.ResetSpread();
            GameController.instance.currentPuzzle.ResetRotation();
            GameController.instance.currentPuzzle.ClearPuzzle(); // this is so wanky lol
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/LeverActivate");
            foreach (InteractableParent ip in objs) // but wait, we really just want to do this for the active puzzle
            {
                ip.LeverPulled();
            }
        }
        

        currentState = returningToNormal;

        foreach (InteractableParent ip in objs)
        {
            ip.LeverPulled();
        }
    }

    void SetStartRotForLerp()
    {
        backToNormTime = 0;
        startRot = transform.localRotation.eulerAngles;
    }

    void ReturnToNormal()
    {
        // lerp back to normal
        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(startRot), Quaternion.Euler(ogRot), backToNormTime);
        backToNormTime += Time.deltaTime;

        if (backToNormTime >= 1) // once its back to normal, like normal rotation
        {
            transform.localRotation = Quaternion.Euler(ogRot);
            currentState = normal;
        }
    }

    private void OnMouseDown()
    {
        if (GameController.instance.cutscene == false)
        {
            if (currentState == normal)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/LeverPickup");
                initialMouse = Input.mousePosition.y;
                mousePull = 0;
                currentState = grabbing;
            }
        }
    }
}
