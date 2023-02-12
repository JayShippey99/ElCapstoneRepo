using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorBeamBrain : MonoBehaviour
{
    public static ProjectorBeamBrain instance;
    public enum Sides { front, back, top, left, right, bottom};

    public Sides side;

    void Start()
    {
        //if (instance == null) instance = this;
        //else Destroy(gameObject);
    }

    private void Update()
    {
        switch (side)
        {
            case Sides.front:
                transform.RotateAroundLocal(Vector3.forward, Time.deltaTime / 2);
                break;
            case Sides.back:
                transform.RotateAroundLocal(Vector3.forward, Time.deltaTime / 2);
                break;
            case Sides.top:
                transform.RotateAroundLocal(Vector3.up, Time.deltaTime / 2);
                break;
            case Sides.left:
                transform.RotateAroundLocal(Vector3.right, Time.deltaTime / 2);
                break;
            case Sides.right:
                transform.RotateAroundLocal(Vector3.right, Time.deltaTime / 2);
                break;
            case Sides.bottom:
                transform.RotateAroundLocal(Vector3.up, Time.deltaTime / 2);
                break;
        }
    }

}
