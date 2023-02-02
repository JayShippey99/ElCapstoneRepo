using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorBrain : MonoBehaviour
{
    public static ProjectorBrain instance;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

   
    public void OpenProjector()
    {

    }

    public void CloseProjector()
    {

    }
}
