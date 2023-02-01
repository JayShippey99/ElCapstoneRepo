using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorBeamBrain : MonoBehaviour
{
    public static ProjectorBeamBrain instance;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        transform.RotateAroundLocal(Vector3.forward, Time.deltaTime / 2);
    }

}
