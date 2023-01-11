using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCounter : MonoBehaviour
{
    // Making this its own script so I keep my main script clean
    static public InputCounter instance;

    [HideInInspector]
    public int count;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
