using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCameraMovement : MonoBehaviour
{
    public Transform puzzles;
    public Transform pivot;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pivot.transform.rotation = Quaternion.Inverse(Tesseract.instance.transform.rotation);
    }
}
