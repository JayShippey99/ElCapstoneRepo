using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    // Start is called before the first frame update
    // need origin point and end point
    // need click code

    int idle = 0;
    int show = 1; // when its going to move toward your face
    int home = 2; // when its going to move to the dock

    [HideInInspector]
    public int currentState;

    [HideInInspector]
    public bool hidden;

    Vector3 showLocation;
    Vector3 showRotation; // I just just have a reference transform, but eh fuck it
    Vector3 hideLocation;
    Vector3 hideRotation;

    // inspect object will now just have face and home positions and rotationsS
    // I'll get the face position by using the camera.main position

    public float speed = .1f;

    public bool reactToClick;
    void Start()
    {
        currentState = idle;

        

        hidden = true;
        hideLocation = transform.position;
        hideRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if (reactToClick)
        {
            if (currentState != idle) // honestly I think once you get this system its really nice
            {
                if (currentState == show)
                {
                    //transform.position = showLocation;
                    transform.position = Vector3.Lerp(transform.position, showLocation, speed); // looks great tbh
                                                                                                //transform.rotation = Quaternion.Euler(showRotation);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(showRotation), speed);
                    if (Vector3.Distance(transform.position, showLocation) < .01f)
                    {
                        //print("idle");
                        currentState = idle;
                        hidden = false;
                    }
                }
                else if (currentState == home)
                {
                    //transform.position = hideLocation;
                    transform.position = Vector3.Lerp(transform.position, hideLocation, speed);
                    //transform.rotation = Quaternion.Euler(hideRotation);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(hideRotation), speed);

                    if (Vector3.Distance(transform.position, hideLocation) < .01f)
                    {
                        //print("idle");
                        currentState = idle;
                        hidden = true;
                    }
                }
            }
        }
    }

    // but you can just always click it, I guess this just wouldn't do anything?
    void Show() // different trigger methods so different thigns
    {
        if (reactToClick && GameController.instance.readingPaper == false)
        {
            //print("face!");
            if (currentState == idle) // will change
            {
                showLocation = Camera.main.transform.position + Camera.main.transform.forward * 1;
                showRotation = Camera.main.transform.rotation.eulerAngles;
                //hidden = false;
                //print("face?");
                //GetComponent<BoxCollider>().enabled = false;
                currentState = show;
            }
            GameController.instance.readingPaper = true;
        }
    }

    public void Hide()
    {
        if (reactToClick)
        {
            //print("dock!");
            if (currentState == idle)
            {
                //hidden = true;
                //print("dock?");
                //GetComponent<BoxCollider>().enabled = true;
                currentState = home;
            }
            GameController.instance.readingPaper = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (reactToClick)
        {
            if (hidden)
            {
                print("show");
                Show();
            }
            else
            {
                print("hide");
                Hide();
            }
        }
    }
}
