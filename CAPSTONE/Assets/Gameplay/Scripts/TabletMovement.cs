using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletMovement : MonoBehaviour
{
    // Start is called before the first frame update
    // need origin point and end point
    // need click code

    int idle = 0;
    int show = 1;
    int hide = 2;

    [HideInInspector]
    public int currentState;

    public Vector3 showLocation;
    public Vector3 showRotation;
    Vector3 hideLocation;
    Vector3 hideRotation;

    public GameObject Screen;

    public float speed = .1f;
    void Start()
    {
        currentState = idle;

        hideLocation = transform.position;
        hideRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != idle)
        {
            if (currentState == show)
            {
                //transform.position = showLocation;
                transform.position = Vector3.Lerp(transform.position, showLocation, speed); // looks great tbh
                //transform.rotation = Quaternion.Euler(showRotation);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(showRotation), speed);
                if (transform.position == showLocation) currentState = idle;
            }
            else if (currentState == hide)
            {
                //transform.position = hideLocation;
                transform.position = Vector3.Lerp(transform.position, hideLocation, speed);
                //transform.rotation = Quaternion.Euler(hideRotation);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(hideRotation), speed);
                if (transform.position == hideLocation) currentState = idle;
            }
        }
    }

    // but you can just always click it, I guess this just wouldn't do anything?
    void Show() // different trigger methods so different thigns
    {
        if (currentState == idle) // will change
        {
            GetComponent<BoxCollider>().enabled = false;
            Screen.SetActive(true);
            currentState = show;
        }
    }

    public void Hide()
    {
        if (currentState == idle)
        {
            GetComponent<BoxCollider>().enabled = true;
            Screen.SetActive(false);
            currentState = hide;
        }
    }

    private void OnMouseUpAsButton()
    {
        Show();
    }
}
