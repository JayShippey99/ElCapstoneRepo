using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Hologram : MonoBehaviour
{
    MeshRenderer mr;

    public Texture shownImage;
    void Start()
    {
        mr = GetComponent<MeshRenderer>();

        mr.material.SetTexture("_ShownImage", shownImage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
