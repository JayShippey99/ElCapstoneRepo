using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorBeamBrain : MonoBehaviour
{

    // so do we want to have one material with things changed?

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
        transform.RotateAroundLocal(transform.up, Time.deltaTime / 2);
    }
}
