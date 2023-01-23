using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletMovement : MonoBehaviour
{
    // Start is called before the first frame update
    // need origin point and end point
    // need click code

    int idle = 0;
    int face = 1; // when its going to move toward your face
    int dock = 2; // when its going to move to the dock
    int desk = 3; // when its going to move to the desk;

    [HideInInspector]
    public int currentState;

    [HideInInspector]
    public bool docked;

    public Vector3 faceLocation;
    public Vector3 faceRotation; // I just just have a reference transform, but eh fuck it
    public Vector3 deskLocation;
    public Vector3 deskRotation;
    Vector3 dockLocation;
    Vector3 dockRotation;

    public GameObject Screen, dockButton, deskButton;

    public float speed = .1f;

    public bool reactToClick;
    void Start()
    {
        currentState = idle;

        docked = true;
        dockLocation = transform.position;
        dockRotation = transform.rotation.eulerAngles;

        Screen.SetActive(false);
    }

    void Update()
    {
        if (reactToClick)
        {
            if (currentState != idle) // honestly I think once you get this system its really nice
            {
                if (currentState == face)
                {
                    //transform.position = showLocation;
                    transform.position = Vector3.Lerp(transform.position, faceLocation, speed); // looks great tbh
                                                                                                //transform.rotation = Quaternion.Euler(showRotation);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(faceRotation), speed);
                    if (Vector3.Distance(transform.position, faceLocation) < .01f)
                    {
                        //print("idle");
                        currentState = idle;
                    }
                }
                else if (currentState == dock)
                {
                    //transform.position = hideLocation;
                    transform.position = Vector3.Lerp(transform.position, dockLocation, speed);
                    //transform.rotation = Quaternion.Euler(hideRotation);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(dockRotation), speed);

                    if (Vector3.Distance(transform.position, dockLocation) < .01f)
                    {
                        //print("idle");
                        currentState = idle;
                        docked = true;
                    }
                }
                else if (currentState == desk)
                {
                    //transform.position = hideLocation;
                    transform.position = Vector3.Lerp(transform.position, deskLocation, speed);
                    //transform.rotation = Quaternion.Euler(hideRotation);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(deskRotation), speed);

                    if (Vector3.Distance(transform.position, deskLocation) < .01f)
                    {
                        //print("idle");
                        currentState = idle;
                    }
                }
            }
        }
    }

    public void Desk()
    {
        
        if (reactToClick)
        {
            //print("desk!");
            if (currentState == idle)
            {
                docked = false;
                //print("desk?");
                GetComponent<BoxCollider>().enabled = true;
                Screen.SetActive(true);
                dockButton.SetActive(false);
                deskButton.SetActive(false);
                currentState = desk;
            }
        }
    }

    // but you can just always click it, I guess this just wouldn't do anything?
    void Show() // different trigger methods so different thigns
    {
        if (reactToClick)
        {
        //print("face!");
            if (currentState == idle) // will change
            {
                docked = false;
                //print("face?");
                GetComponent<BoxCollider>().enabled = false;
                Screen.SetActive(true);
                dockButton.SetActive(true);
                deskButton.SetActive(true);
                currentState = face;
            }
        }
    }

    public void Dock()
    {
        if (reactToClick)
        {
            //print("dock!");
            if (currentState == idle)
            {
                docked = false;
                //print("dock?");
                GetComponent<BoxCollider>().enabled = true;
                Screen.SetActive(false); // this should only happen when the thing is docked actually
                dockButton.SetActive(false);
                deskButton.SetActive(false);
                currentState = dock;
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        if (reactToClick)
        {
            Show();
        }
    }
}
